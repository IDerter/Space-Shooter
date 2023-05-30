using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SpaceShooter
{
    public class PointerClickHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private bool _hold;
        public bool IsHold => _hold;
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            _hold = true;
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            _hold = false;
        }
    }
}

