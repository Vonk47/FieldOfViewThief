using Game.Core;
using Game.Mechanics.Levels;
using Game.Mechanics.Movement;
using Game.Mechanics.Visuals;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Mechanics.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] PlayerController _player;
        [SerializeField] GameDirector _director;
        [SerializeField] PickObjectsRaycaster _raycaster;
        [SerializeField] TMP_Text _moneyCounter;
        [SerializeField] Level _currentLevel;
        [SerializeField] MainCanvas _canvas;

        private int _money = 0;
        private static bool _gameOver;

        private void Start()
        {
            MoveController moveController = new MoveController();
            _player.Setup(moveController);
            _raycaster.Setup(moveController, _player);
            _currentLevel.PickableRegistered += OnPickableRegistered;
            _currentLevel.UserStateChanged += ChangePlayerState;
            _moneyCounter.text = _money.ToString();
        }

        private void OnPickableRegistered(PickableObject pickable)
        {
            if (pickable is PickableObjectValuable valuable)
                _money += valuable.Value;
            _moneyCounter.text = _money.ToString();
            if (_currentLevel.EverythingPicked)
                Trigger_Win();

        }

        private void OnDestroy()
        {
            _currentLevel.PickableRegistered -= OnPickableRegistered;
            _currentLevel.UserStateChanged -= ChangePlayerState;
        }

        private void ChangePlayerState()
        {
            _player.ChangeState();
        }

        public static void Trigger_Wasted()
        {
            _gameOver = true;
        }

        private void Trigger_Win()
        {
            _canvas.TriggerNextLevel();
            Time.timeScale = 0;
        }

        private void Update()  // could been better solution, but this a fast one
        {
            if (_gameOver)
            {
                _director.ChangeVignette();
                _canvas.TriggerEndGame();
                _gameOver = false;
                Time.timeScale = 0;
            }
        }

    }
}
