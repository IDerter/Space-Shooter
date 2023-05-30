using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class PlayerShipSelectionController : MonoBehaviour
    {
        [SerializeField] private SpaceShip _prefabSpaceShip;

        [SerializeField] private Text _shipName;
        [SerializeField] private Text _hitPoints;
        [SerializeField] private Text _speed;
        [SerializeField] private Text _agility;

        [SerializeField] private Image _imagePreview;

        private void Start()
        {
            if (_prefabSpaceShip != null)
            {
                _shipName.text = _prefabSpaceShip.NickName;
                _hitPoints.text = "HP: " + _prefabSpaceShip.StartHitPoints.ToString();
                _speed.text = "Speed: " + _prefabSpaceShip.CurrentVelocity.ToString();
                _agility.text = "Agility: " + _prefabSpaceShip.MaxAngularVelocity.ToString();
                _imagePreview.sprite = _prefabSpaceShip.PreviewImage;
            }
        }

        public void OnSelectShip()
        {
            LevelSequenceController.PlayerShip = _prefabSpaceShip;

            MainMenuController.Instance.gameObject.SetActive(true);

            transform.parent.gameObject.SetActive(false);
        }
    }
}

