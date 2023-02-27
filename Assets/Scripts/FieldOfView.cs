using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Game.Core
{
    public class FieldOfView : MonoBehaviour
    {
        [SerializeField] private float _viewRadius;
        [Range(0, 360)]
        [SerializeField] private float _viewAngle;

        [SerializeField] private LayerMask _targetMask;
        [SerializeField] private LayerMask _obstructionMask;

        public event Action<Transform> OnSeeTarget;
        public event Action OnUnseenTarget;

        private HashSet<Transform> _seenTargets = new HashSet<Transform>();

        bool _hasTarget { get { return _seenTargets.Count > 0; } }


        private void Start()
        {
            StartCoroutine(FOVRoutine());
        }

        private IEnumerator FOVRoutine()
        {
            WaitForSeconds wait = new WaitForSeconds(0.2f);

            while (true)
            {
                yield return wait;
                FieldOfViewCheck();
            }
        }

        private void FieldOfViewCheck()
        {
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, _viewRadius, _targetMask);
            IEnumerable<Transform> targets = rangeChecks.Select(t => t.transform).ToArray();

            if (_hasTarget && _seenTargets.Intersect(targets).ToHashSet().Count == 0)
                OnUnseenTarget?.Invoke();

            _seenTargets = _seenTargets.Intersect(targets).ToHashSet();

            if (rangeChecks.Length != 0)
            {
                foreach (var target in targets)
                {
                    Vector3 directionToTarget = (target.position - transform.position).normalized;

                    if (Vector3.Angle(transform.forward, directionToTarget) < _viewAngle / 2)
                    {
                        float distanceToTarget = Vector3.Distance(transform.position, target.position);
                        bool targetSpoted = !Physics.Raycast(transform.position, directionToTarget, distanceToTarget, _obstructionMask);
                        if (targetSpoted && !_seenTargets.Contains(target))
                        {
                            _seenTargets.Add(target);
                            OnSeeTarget?.Invoke(target);
                        }
                        else if (!targetSpoted && _seenTargets.Contains(target))
                            RemoveTargetAndFireEvent(target);
                    }
                    else if (_seenTargets.Contains(target))
                        RemoveTargetAndFireEvent(target);
                }
            }

        }

        private void RemoveTargetAndFireEvent(Transform target)
        {
            _seenTargets.Remove(target);
            if (!_hasTarget)
            {
                OnUnseenTarget?.Invoke();
            }
        }


        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            FieldOfView fov = this;
            Handles.color = Color.white;
            Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov._viewRadius);

            Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov._viewAngle / 2);
            Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov._viewAngle / 2);

            Handles.color = Color.yellow;
            Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov._viewRadius);
            Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov._viewRadius);

            foreach (var target in _seenTargets)
            {
                Handles.color = Color.green;
                Handles.DrawLine(fov.transform.position, target.position);
            }
#endif
        }

        private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
        {
            angleInDegrees += eulerY;

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}

