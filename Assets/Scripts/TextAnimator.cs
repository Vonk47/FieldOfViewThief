using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.Animators
{
    public class TextAnimator : MonoBehaviour
    {
        [SerializeField] private TMP_Text _currentText;

        private Coroutine _dialogueAnimation;

        public void SetTmpText(TMP_Text currentText)
        {
            _currentText = currentText;
        }

        public void RunDialogAnimation(string text)
        {
            while (!gameObject.activeInHierarchy)
            {

            }
            if (_dialogueAnimation is not null)
            {
                StopCoroutine(_dialogueAnimation);
            }
            _dialogueAnimation = StartCoroutine(RunTextAnimation(text));
        }

        private IEnumerator RunTextAnimation(string text)
        {
            if (_currentText == null)
                yield break;
            StringBuilder textStr = new StringBuilder(string.Empty);
            int count = 0;
            while (textStr.Length < text.Length)
            {
                textStr.Append(text[count]);
                _currentText.text = textStr.ToString();
                count++;
                yield return new WaitForSecondsRealtime(0.04f);
            }
            _dialogueAnimation = null;
        }
    }

}