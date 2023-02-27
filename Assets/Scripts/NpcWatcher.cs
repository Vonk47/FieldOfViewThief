using Game.Mechanics.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Controllers.NPC
{
    public class NpcWatcher : NpcController
    {
        private const string Confused = "IsConfused";

        private Coroutine _watchCoroutine;
        private bool _targetSeen;
        private float _seenValue;
        [SerializeField] private float _maxSeenValue = 3f;
        private bool _finalSeen;


        protected override void Update()
        {
            base.Update();

            if (_finalSeen)
                return;

            if (_seenValue >= _maxSeenValue)
            {
                TriggerSeen();
                return;
            }

            if (_targetSeen)
            {
                Debug.Log("seen target" + _seenValue);
                _seenValue += Time.deltaTime;
                _alertIcon.fillAmount = _seenValue / _maxSeenValue;
                return;
            }

            if (_seenValue > 0)
                _seenValue -= Time.deltaTime;

            _alertIcon.fillAmount = _seenValue / _maxSeenValue;
        }

        protected override void Start()
        {
            base.Start();
            SubscribeMethods();
        }

        private void SubscribeMethods()
        {
            _fieldOfView.OnSeeTarget += VoidOnSeeTarget;
            _fieldOfView.OnUnseenTarget += VoidOnUnseeTarget;
        }

        private void UnsubscribeMethods()
        {
            _fieldOfView.OnSeeTarget -= VoidOnSeeTarget;
            _fieldOfView.OnUnseenTarget -= VoidOnUnseeTarget;
        }

        private IEnumerator WatchTarget(Transform target)
        {
            while (true)
            {
                transform.LookAt(target);
                yield return null;
            }
        }

        private void TriggerSeen()
        {
            _finalSeen = true;
            _animator.SetTrigger(Confused);
            GameController.Trigger_Wasted();
        }

        protected override void VoidOnSeeTarget(Transform target)
        {
            base.VoidOnSeeTarget(target);
            _targetSeen = true;
        }

        protected override void VoidOnUnseeTarget()
        {
            Debug.Log("target unseen");
            _targetSeen = false;
        }

        private void OnDestroy()
        {
            UnsubscribeMethods();
        }

    }
}
