using Game.Core;
using Game.Mechanics.Levels;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Game.Mechanics.Visuals
{
    public class GameDirector : MonoBehaviour
    {
        [SerializeField] private Volume _volume;
        [SerializeField] private float _timeToChange = 3f;
        [SerializeField] private float _speedToChange = 0.05f;
        [SerializeField] private Level _level;
        [SerializeField] private PlayerTextSequence _playerText;

        private void Start()
        {
            _playerText.TriggerLine(0);
            _level.PickableRegistered += OnPickableRegistered;
        }

        private void OnPickableRegistered(PickableObject pickableObject)
        {
            int dialogueId = pickableObject switch
            {
                PickableObjectKey => 1,
                PickableObjectValuable => UnityEngine.Random.Range(3, 6),
                _ => throw new ArgumentException("No valid range found")
            };

            dialogueId = _level.EverythingPicked ? 6 : dialogueId;
            _playerText.TriggerLine(dialogueId);
        }

        public void ChangeVignette()
        {
            StartCoroutine(RunVignetteChange(0.55f, 0.45f));
        }


        private IEnumerator RunVignetteChange(float intensityValue, float smoothnessValue)
        {
            yield return new WaitForSecondsRealtime(5f);
            float time = 0f;
            if (_volume.profile.TryGet(out Vignette vignette))
            {
                float intensityStartValue = vignette.intensity.value;
                float smoothnessStartValue = vignette.smoothness.value;
                while (time < _timeToChange)
                {
                    vignette.intensity.value = Mathf.Lerp(intensityStartValue, intensityValue, time / _timeToChange);
                    vignette.smoothness.value = Mathf.Lerp(smoothnessStartValue, smoothnessValue, time / _timeToChange);
                    time += _speedToChange;
                    yield return new WaitForSecondsRealtime(0.02f);
                }
                vignette.intensity.value = intensityValue;
                vignette.smoothness.value = smoothnessValue;
            }
            else
            {
                yield break;
            }
        }
    }
}
