using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// ������� ����� ���� ������������� �������� �� �����
    /// </summary>
    public abstract class Entity : MonoBehaviour
    {
        /// <summary>
        /// �������� ������� ��� ������������
        /// </summary>
        [SerializeField] private string _nickName;
        public string NickName => _nickName;
    }
}

