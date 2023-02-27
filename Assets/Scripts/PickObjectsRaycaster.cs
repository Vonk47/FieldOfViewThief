using Game.Mechanics.Controllers;
using Game.Mechanics.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Game.Core
{
    public class PickObjectsRaycaster : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private LayerMask _mask;
        private MoveController _controller;
        private PlayerController _player;

        public void Setup(MoveController controller, PlayerController player)
        {
            _controller = controller;
            _controller.OnPress += HitObject;
            _player = player;
        }

        private void OnDestroy()
        {
            _controller.OnPress -= HitObject;
        }

        private void HitObject()
        {
            var ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hitData;

            if (Physics.Raycast(ray, out hitData, 10, _mask))
            {
                Debug.Log("pickable hit " + " Distance of object " + Vector3.Distance(_player.transform.position, hitData.point) + " OBJECT HIT" + hitData.transform.name);

                if (Vector3.Distance(_player.transform.position, hitData.point) > 2)
                    _player.MoveToPickable(hitData.point);
                else
                    hitData.transform.GetComponent<PickableObject>().PickObject();
            }
        }
    }
}
