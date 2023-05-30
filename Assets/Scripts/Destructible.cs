using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceShooter
{
    /// <summary>
    /// ������������ ������ �� �����. �� ��� ����� ����� ��� ������
    /// </summary>
    public class Destructible : Entity
    {
        #region Properties
        /// <summary>
        /// ������ ���������� �����������
        /// </summary>
        [SerializeField] private bool _InDestructible;
        public bool IsInDestructible
        {
            get { return _InDestructible; }
            set { _InDestructible = value; }
        }

        /// <summary>
        /// ��������� ���������� ����������
        /// </summary>
        [SerializeField] private int _hitPoints;
        public int StartHitPoints => _hitPoints;

        /// <summary>
        /// ������� ���������
        /// </summary>
        private int _currentHitPoints;
        public int HitPoints => _currentHitPoints;

        private bool IsDie = false;
        #endregion

        #region Unity Events
        protected virtual void Start()
        {
            _currentHitPoints = _hitPoints;
        }
        #endregion

        #region Public API
        /// <summary>
        /// ��������� ����� �������
        /// </summary>
        /// <param name="damage">���� ��������� �������</param>
        public void ApplyDamage(int damage)
        {
            if (_InDestructible) return;

            _currentHitPoints -= damage;
            Debug.Log(gameObject.name);

            if (_currentHitPoints <= 0)
                OnDeath();
        }

        #endregion
        /// <summary>
        /// ���������������� ������� ����������� �������
        /// </summary>
        protected virtual void OnDeath()
        {
            if (IsDie) return;
            IsDie = true;
            
            _eventOnDeath?.Invoke();
        }

        [SerializeField] private UnityEvent _eventOnDeath;
        public UnityEvent EventOnDeath => _eventOnDeath;

        private static HashSet<Destructible> _allDestructibles;
        public static IReadOnlyCollection<Destructible> AllDestructibles => _allDestructibles;

        protected virtual void OnEnable()
        {
            if (_allDestructibles == null)
            {
                _allDestructibles = new HashSet<Destructible>();
            }

            _allDestructibles.Add(this);
        }

        protected virtual void OnDestroy()
        {
            _allDestructibles.Remove(this);
        }

        public const int TeamIdNeutral = 0;

        [SerializeField] private int _teamId;
        public int TeamId => _teamId;

        #region Score
        [SerializeField] private int _scoreValue;
        public int ScoreValue => _scoreValue;
        #endregion
    }
}

