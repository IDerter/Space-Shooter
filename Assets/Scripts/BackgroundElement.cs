using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class BackgroundElement : MonoBehaviour
    {
        [Range(0.0f, 4.0f)]
        [SerializeField] private float _parallaxPower;

        [SerializeField] private float _textureScale;

        private Material _quadMaterial;
        private Vector2 _initialOffset;

        private void Start()
        {
            _quadMaterial = GetComponent<MeshRenderer>().material;
            _initialOffset = UnityEngine.Random.insideUnitCircle;

            _quadMaterial.mainTextureScale = Vector2.one * _textureScale;
        }

        private void Update()
        {
            Vector2 offset = _initialOffset;

            offset.x += transform.position.x / transform.localScale.x / _parallaxPower;
            offset.y += transform.position.y / transform.localScale.y / _parallaxPower;

            _quadMaterial.mainTextureOffset = offset;
        }
    }
}

