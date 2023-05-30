using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SpaceShooter
{
    public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Image _joyBack;
        [SerializeField] private Image _joyStick;

        public Vector3 Value { get; private set; }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 position = Vector2.zero;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_joyBack.rectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out position);

            position.x = (position.x / _joyBack.rectTransform.sizeDelta.x);
            position.y = (position.y / _joyBack.rectTransform.sizeDelta.y);

            position.x = position.x * 2 - 1;
            position.y = position.y * 2 - 1;

            Value = new Vector3(position.x, position.y, 0);
            if (Value.magnitude > 1) // magnitude is a length of vector
                Value = Value.normalized;

            float offsetX = _joyBack.rectTransform.sizeDelta.x / 2 - _joyStick.rectTransform.sizeDelta.x / 2;
            float offsetY = _joyBack.rectTransform.sizeDelta.y / 2 - _joyStick.rectTransform.sizeDelta.y / 2;

            _joyStick.rectTransform.anchoredPosition = new Vector2(Value.x * offsetX, Value.y * offsetY);
        }

        

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Value = Vector3.zero;
            _joyStick.rectTransform.anchoredPosition = Vector3.zero;
        }
    }

}
