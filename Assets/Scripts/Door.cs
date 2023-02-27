using Game.Core.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Core
{
    [RequireComponent(typeof(NavMeshObstacle))]
    public class Door : MonoBehaviour
    {
        [SerializeField] private NavMeshObstacle _obstacle;
        [SerializeField] private Transform _pivot;
        [SerializeField] private float _targetAngle = 90;
        [SerializeField] private float _animationTime = 1;
        [Header("Optional")]
        [SerializeField] private DitheringObjectListTrigger _ditheringObjectListTrigger;

        public void OpenDoor()
        {
            Debug.Log("door Opened!");
            _obstacle.size = Vector3.Scale(_obstacle.size, new Vector3(0.5f, 0.5f, 0.5f));
            StartCoroutine(OpenDoorAnimation());
            if (_ditheringObjectListTrigger is not null)
                _ditheringObjectListTrigger.OnActionTriggered();
        }


        private IEnumerator OpenDoorAnimation()
        {
            float currentAngle = _pivot.eulerAngles.y;
            float currentTime = 0;
            float tempAngle;
            while (currentTime < _animationTime)
            {
                Debug.Log("opening the door ");
                tempAngle = Mathf.Lerp(currentAngle, _targetAngle, currentTime / _animationTime);
                _pivot.eulerAngles = new Vector3(_pivot.eulerAngles.x, tempAngle, _pivot.eulerAngles.z);
                currentTime += Time.unscaledDeltaTime;
                yield return null;
            }
        }
    }
}
