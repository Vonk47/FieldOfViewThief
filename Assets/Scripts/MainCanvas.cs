using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Mechanics.Controllers
{
    public class MainCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject _endGameRoot;
        [SerializeField] private GameObject _nextLevelRoot;



        public void TriggerEndGame()
        {
            _endGameRoot.SetActive(true);
        }

        public void TriggerNextLevel()
        {
            _nextLevelRoot.SetActive(true);
        }
    }
}
