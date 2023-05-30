using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class EpisodeSelectionController : MonoBehaviour
    {
        [SerializeField] private Episode _episode;
        [SerializeField] private Text _episodeNickname;
        [SerializeField] private Image _previewImage;

        private void Start()
        {
            if (_episodeNickname != null)
            {
                _episodeNickname.text = _episode.EpisodeName;
            }

            if (_previewImage != null)
            {
                _previewImage.sprite = _episode.PreviewImage;
            }
        }

        public void OnStartEpisodeButtonClicked()
        {
            LevelSequenceController.Instance.StartEpisode(_episode);
        }
    }

}
