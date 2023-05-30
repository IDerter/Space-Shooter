using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class Asteroid : Destructible
    {
        public enum Size
        {
            Small,
            Normal,
            Big,
            Huge
        }

        [SerializeField] private float _randomSpeed;
        [SerializeField] private Size _size;
        [SerializeField] private Asteroid _asteroidObj;
        [SerializeField] private bool IsSpawn = false;

        private void Awake()
        {
            SetSize(_size);
            EventOnDeath.AddListener(OnAsteroidDestroyed);
        }

        protected override void Start()
        {
            base.Start();
            Rigidbody2D rb = transform.GetComponent<Rigidbody2D>();

            if (rb != null && _randomSpeed > 0)
            {
                rb.velocity = UnityEngine.Random.insideUnitCircle * _randomSpeed;
            }
        }

        protected override void OnDestroy()
        {
            EventOnDeath.RemoveListener(OnAsteroidDestroyed);
        }

        private void OnAsteroidDestroyed()
        {
            if (_size != Size.Small)
            {
                Debug.Log("«¿ÿ≈À ¬ ¿—“≈–Œ»ƒ!!");
                if (IsSpawn) return;

                SpawnStones();
                IsSpawn = true;
            }
            Debug.Log("DESTROYED!!!!");
            Destroy(gameObject);
        }

        private void SpawnStones()
        {
            for (int i = 0; i < 2; i++)
            {
                Asteroid asteroid = Instantiate(this, transform.position, Quaternion.identity);
                
                asteroid.SetSize(_size - 1);
            }
        }

        private void SetSize(Size size)
        {
            if (size < 0) return;
            transform.localScale = GetVectorFromSize(size);
            _size = size;
        }

        private Vector3 GetVectorFromSize(Size size)
        {
            if (size == Size.Small) return new Vector3(0.4f, 0.4f, 0.4f);
            if (size == Size.Normal) return new Vector3(0.6f, 0.6f, 0.6f);
            if (size == Size.Big) return new Vector3(0.75f, 0.75f, 0.75f);
            if (size == Size.Huge) return new Vector3(1f, 1f, 1f);


            return Vector3.one;
        }
    }
}

