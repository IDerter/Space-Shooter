using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class MainMenuController : SingletonBase<MainMenuController>
    {
        [SerializeField] private SpaceShip _defaultSpaceShip;

        [SerializeField] private GameObject _episodeSelection;
        [SerializeField] private GameObject _shipSelection;
        [SerializeField] private GameObject _statisticPanel;

        private void Start()
        {
            LevelSequenceController.PlayerShip = _defaultSpaceShip;
        }

        public void OnButtonStartNew()
        {
            _episodeSelection.gameObject.SetActive(true);
            
            gameObject.SetActive(false);
        }

        public void OnSelectShip()
        {
            _shipSelection.SetActive(true);
            gameObject.SetActive(false);
        }

        public void OnButtonExit()
        {
            Application.Quit();
        }
    }

}
