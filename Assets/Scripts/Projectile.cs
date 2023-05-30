using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public enum FireMode
    {
        Default,
        Homing
    }
    
    [RequireComponent(typeof(AudioSource))]
    public class Projectile : Entity
    {
        [SerializeField] private float _velocity;
        [SerializeField] private float _lifeTime;
        [SerializeField] private int _damage;
        [SerializeField] private FireMode _fireMode;
        public Collider2D[] masNew;

        [SerializeField] private ImpactEffect _impactEffectPrefab;
        [SerializeField] private AudioSource _audioProjectile;

        [SerializeField] private float radiusScanning = 5f;
        [SerializeField] private float radiusDamageOfFire = 3f;
        [SerializeField] private GameObject _particleFire;

        private float _timer;
        private float _maxDistance = 10000f;

        private Destructible _parent;

        public void SetShooterParent(Destructible parent)
        {
            _parent = parent;
        }

        private void Update()
        {
            if (_fireMode == FireMode.Default)
                CalculateDistance();

            if (_fireMode == FireMode.Homing)
                FireRacket();
        }

        public void CalculateDistance()
        {
            float stepLength = _velocity * Time.deltaTime;
            Vector2 step = transform.up * stepLength;

            CheckHit();

            transform.position += new Vector3(step.x, step.y, transform.position.z);
        }

        public void FireRacket()
        {
            float stepLength = _velocity * 2 * Time.deltaTime;
            Vector2 step = transform.up * stepLength;
            var mas = Physics2D.OverlapCircleAll(transform.position, radiusScanning);
            
            masNew = mas;

            int minIndex = 0;
            float minDistance = _maxDistance;
            SearchMinDistance(ref mas, ref minIndex, ref minDistance, 1);

            CheckHit();

            if (minDistance != 10000f)
            {
                transform.up = mas[minIndex].transform.position - transform.position;
                transform.position = Vector3.Slerp(transform.position, mas[minIndex].transform.position, stepLength);
            }
            else
            {
                _fireMode = FireMode.Default;
                transform.position += new Vector3(step.x, step.y, transform.position.z);
            }
                
        }

        private void SearchMinDistance(ref Collider2D[] mas, ref int minIndex, ref float minDistance, int number)
        {
            for (int i = 0; i < mas.Length; i++)
            {
                if (mas[i] != null)
                {
                    if (mas[i].transform.parent != null && mas[i].transform.parent.GetComponent<Destructible>() != null)
                    {
                        if (mas[i].transform.parent.gameObject != _parent.gameObject)
                        {
                            if (number == 1)
                            {
                                var temp = (mas[i].transform.position - transform.position).magnitude;
                                if (minDistance > temp)
                                {
                                    minDistance = temp;
                                    minIndex = i;
                                }
                            }
                            
                            
                            if (number == 2)
                            {
                                mas[i].transform.parent.GetComponent<Destructible>().ApplyDamage(_damage);
                                var _obj = Instantiate(_particleFire, transform);
                                _obj.transform.parent = null;
                                Debug.Log("Instantiate");
                                var audioFire = Instantiate(_audioProjectile, transform);
                                audioFire.transform.parent = null;
                            }
                        }
                    }
                }
            }
            Debug.Log(mas[minIndex]);
        }

        private void TakeRocketDamage()
        {
            var mas = Physics2D.OverlapCircleAll(transform.position, radiusDamageOfFire);
            int minIndex = 0;
            float minDistance = _maxDistance;
            SearchMinDistance(ref mas, ref minIndex, ref minDistance, 2);

        }

        private void CheckHit()
        {
            float stepLength = _velocity * Time.deltaTime;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, stepLength);
            if (hit)
            {
                Destructible dest = hit.collider.transform.root.GetComponent<Destructible>();
                if (dest != null && dest != _parent)
                {
                    dest.ApplyDamage(_damage);

                    if (_parent == Player.Instance.ActiveShip)
                    {
                        Player.Instance.AddScore(dest.ScoreValue);
                        if(dest.HitPoints <= 0)
                        {
                            if(_fireMode == FireMode.Default)
                            {
                                var audioFire = Instantiate(_audioProjectile, transform);
                                audioFire.transform.parent = null;
                                var _objParticleFire = Instantiate(_particleFire, transform);
                                _objParticleFire.transform.parent = null;

                                if(dest.GetComponent<SpaceShip>() != null)
                                {
                                    Player.Instance.AddKill();
                                    Debug.Log("KILL!!");
                                }
                            }
                        }
                        OnProjectileLifeEnd(hit.point, hit.collider);
                    }

                   

                    if (_fireMode == FireMode.Homing)
                        TakeRocketDamage();
                }

            }

            _timer += Time.deltaTime;

            if (_timer >= _lifeTime)
            {
                Destroy(gameObject);
            }
        }

        private void OnProjectileLifeEnd(Vector2 pos, Collider2D col)
        {
            Destroy(gameObject);
        }
    }
}
