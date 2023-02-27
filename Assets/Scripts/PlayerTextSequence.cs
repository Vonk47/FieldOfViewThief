using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Core
{
    [RequireComponent(typeof(TMP_Text))]
    public class PlayerTextSequence : MonoBehaviour
    {
        [SerializeField] private Transform _lookAtObject;
        [SerializeField] private string[] _playerLines;
        private TMP_Text _text;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            transform.LookAt(_lookAtObject);
         //   transform.eulerAngles = Vector3.Scale(transform.eulerAngles, -Vector3.one);
        }

        public void TriggerLine(int id)
        {
            _text.text = _playerLines[id];
            StartCoroutine(TurnTextOff());
        }

        public IEnumerator TurnTextOff()
        {
            yield return new WaitForSecondsRealtime(5);
            _text.text = string.Empty;
        }
    }
}
