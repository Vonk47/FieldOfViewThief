using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Game.Core.Animators;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.SceneManagement;

namespace Game.Intro
{
    public class GameIntroDirector : MonoBehaviour
    {
        private const string HideAnimation = "Hide";
        private const string ShowAnimation = "Show";

        [SerializeField] CanvasGroup _firstCharacter;
        [SerializeField] CanvasGroup _secondCharacter;
        [SerializeField] TextAnimator _textAnimator;
        [SerializeField] Animator _backgroundAnimation;
        [SerializeField] Animator[] _charactersAnimator;
        [SerializeField] Sprite[] _backgrounds;
        [SerializeField] Image _background;
        IntroAction[] _actions;
        private int _index = 1;
        private int _backgroundCurrent = 0;
        private int _charachterCurrent = 0;

        private void Start()
        {
            if (SceneManager.GetSceneByBuildIndex(1) != null && SceneManager.GetSceneByBuildIndex(1).isLoaded)
                SceneManager.UnloadSceneAsync(1);

            _actions = new IntroAction[8]
            {
              new  IntroAction(1,0,0,0,"Just Another Day At The Office"),
              new  IntroAction(2,0,1,1,"Hudson, you're fired"),
              new  IntroAction(3,0,0,3,"But why???"),
              new  IntroAction(4,0,1,4,"Capitalism"),
              new  IntroAction(5,1,0,2,"Eh... i Guess i have nothing left"),
              new  IntroAction(6,1,0,3,"But what if..."),
              new  IntroAction(7,1,0,4,"I steal printer at my previous job to become rich again???"),
              new  IntroAction(8,1,0,5,"Lets get it")
            };
            RunAction(_actions[0]);
        }


        public void Trigger_Next()
        {
            if (_index == _actions.Length)
            {
                SceneManager.LoadScene(1);
                return;
            }

            RunAction(_actions[_index]);
            _index++;
        }


        private void RunAction(IntroAction action)
        {
            ChangeBackground(action.BackgroundId);
            ChangeCharacter(action.CharacterId);
            ChangeText(action.TextLine);
            ChangeMainAnimation(action.MainAnimationId, action.CharacterId);
        }

        private void ChangeBackground(int backgroundId)
        {
            if (_backgroundCurrent == backgroundId)
                return;
            StartCoroutine(ChangeBackgroundAnimation(backgroundId));

        }

        private void ChangeCharacter(int characterId)
        {
            if (_charachterCurrent == characterId)
                return;

            StartCoroutine(ChangeCharacterCoroutine(characterId));

        }

        private IEnumerator ChangeCharacterCoroutine(int characterId)
        {
            var currentCharacter = _charachterCurrent == 0 ? _firstCharacter : _secondCharacter;
            float timeRequired = 0.3f;
            float timeCurrent = 0f;
            while (timeCurrent < timeRequired)
            {
                currentCharacter.alpha = Mathf.Lerp(1f, 0f, timeCurrent / timeRequired);
                timeCurrent += Time.unscaledDeltaTime;
                yield return null;
            }
            timeCurrent = 0f;
            _charachterCurrent = characterId;
            currentCharacter = _charachterCurrent == 0 ? _firstCharacter : _secondCharacter;
            while (timeCurrent < timeRequired)
            {
                currentCharacter.alpha = Mathf.Lerp(0f, 1f, timeCurrent / timeRequired);
                timeCurrent += Time.unscaledDeltaTime;
                yield return null;
            }
        }

        private IEnumerator ChangeBackgroundAnimation(int backgroundId)
        {
            _backgroundAnimation.SetTrigger(HideAnimation);
            yield return new WaitForSecondsRealtime(1f); //somehow animation finished tracking just doesn't work, so i leave it as is
            _background.sprite = _backgrounds[backgroundId];
            _backgroundAnimation.SetTrigger(ShowAnimation);
            _backgroundCurrent = backgroundId;
        }

        private void ChangeMainAnimation(int animationId, int charId)
        {
            var currentAnimator = _charactersAnimator[charId];
            currentAnimator.SetTrigger(animationId.ToString());
        }

        private void ChangeText(string text)
        {
            _textAnimator.RunDialogAnimation(text);
        }

    }
}
