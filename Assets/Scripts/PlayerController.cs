using Game.Mechanics.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Game.Mechanics.Controllers
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Animator _animator;
        private MoveController _moveController;
        private string _animatorBool = "isWalking";
        Coroutine checkNavmeshCoroutine;
        UserState _currentState = UserState.Walking;

        [SerializeField] private float _speed = 1.4f;

        void Start()
        {
            _animator.applyRootMotion = true;
            _navMeshAgent.updatePosition = false;
        }

        public void Setup(MoveController controller)
        {
            _moveController = controller;
            _moveController.OnPress += Move;
        }

        public void ChangeState()
        {
            _animator.SetBool(_animatorBool, false);
            _currentState = _currentState == UserState.Walking ? UserState.Crouching : UserState.Walking;

            _animatorBool = _currentState switch
            {
                UserState.Walking => "isWalking",
                UserState.Crouching => "IsCrouching",
                _ => throw new ArgumentException(" none available range")
            };

        }

        private void Update()
        {
            SynchronizeAnimatorAndAgent();
        }

        private void OnDestroy()
        {
            _moveController.OnPress -= Move;
        }

        private void OnAnimatorMove()
        {
            transform.position = _navMeshAgent.nextPosition;
        }

        private void SynchronizeAnimatorAndAgent()
        {
            Vector2 velocity = Vector2.zero;
            Vector2 groundDeltaPosition = Vector2.zero;
            Vector3 worldDeltaPosition = _navMeshAgent.nextPosition - transform.position;
            worldDeltaPosition.y = 0;
            groundDeltaPosition.x = Vector3.Dot(transform.right, worldDeltaPosition);
            groundDeltaPosition.y = Vector3.Dot(transform.forward, worldDeltaPosition);
            velocity = (Time.deltaTime > 1e-5f) ? groundDeltaPosition / Time.deltaTime : Vector2.zero;
            bool shouldMove = velocity.magnitude > 0.025f && _navMeshAgent.remainingDistance > _navMeshAgent.radius;
            _animator.SetBool(_animatorBool, shouldMove);
            _animator.SetFloat("VelocityX", velocity.x);
            _animator.SetFloat("VelocityY", velocity.y);
        }

        private void Move()
        {
            if (checkNavmeshCoroutine != null)
            {
                StopCoroutine(checkNavmeshCoroutine);
            }

            var ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hitData;
            if (Physics.Raycast(ray, out hitData, 100, _layerMask))
            {
                var destinationPoint = hitData.point;
                Debug.Log(hitData.point);
                _navMeshAgent.SetDestination(destinationPoint);
                _animator.SetBool(_animatorBool, true);
            }

        }

        public void MoveToPickable(Vector3 destination)
        {
            _navMeshAgent.SetDestination(destination);
        }
    }

    public enum UserState
    {
        Walking,
        Crouching
    }

}
