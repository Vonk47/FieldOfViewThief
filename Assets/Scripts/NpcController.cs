using Game.Core;
using Game.Mechanics.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Game.Controllers.NPC
{
    public class NpcController : MonoBehaviour
    {
        protected const string Idle = "Idle";

        [SerializeField] protected FieldOfView _fieldOfView;
        [SerializeField] protected Animator _animator;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Vector3[] _patroulingPoints;
        [SerializeField] protected Image _alertIcon;

        private int _currentPoint = 0;

        [SerializeField] float _waitTime = 5f;
        private float _currentWaitTime = 0;

        protected virtual void Start()
        {
            _animator.applyRootMotion = true;
            _navMeshAgent.updatePosition = false;
            if (_patroulingPoints.Length > 1)
                StartMove();
        }

        protected virtual void VoidOnSeeTarget(Transform target)
        {

        }

        protected virtual void VoidOnUnseeTarget()
        {

        }

        protected virtual void Update()
        {
            SynchronizeAnimatorAndAgent();
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
            _animator.SetBool("isWalking", shouldMove);
            _animator.SetFloat("VelocityX", velocity.x);
            _animator.SetFloat("VelocityY", velocity.y);
        }

        private IEnumerator WaitForMoveCoroutine()
        {

            while (_currentWaitTime < _waitTime)
            {
                _currentWaitTime += Time.deltaTime;
                yield return null;
            }

            _currentWaitTime = 0f;

            if (_currentPoint + 1 >= _patroulingPoints.Length)
                _currentPoint = 0;
            else
                _currentPoint++;

            Move(_currentPoint);
        }

        private IEnumerator CheckNavmeshReachedDestination()
        {
            while (!_navMeshAgent.ReachedDestinationOrGaveUp())
            {
                yield return null;
            }
            StartCoroutine(WaitForMoveCoroutine());
        }

        private void StartMove()
        {
            Move(0);
            StartCoroutine(CheckNavmeshReachedDestination());
        }

        private void Move(int point)
        {
            if (!_navMeshAgent.hasPath)
            {
                _navMeshAgent.SetDestination(_patroulingPoints[point]);
                _animator.SetBool("isWalking", true);
                StartCoroutine(CheckNavmeshReachedDestination());
            }

        }
    }
}
