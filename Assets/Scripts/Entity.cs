using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Базовый класс всех интерактивных объектов на сцене
    /// </summary>
    public abstract class Entity : MonoBehaviour
    {
        /// <summary>
        /// Название объекта для пользователя
        /// </summary>
        [SerializeField] private string _nickName;
        public string NickName => _nickName;
    }
}

