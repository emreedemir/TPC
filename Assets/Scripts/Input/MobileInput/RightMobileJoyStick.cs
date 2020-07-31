using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tpc_CharacterInput
{
    public class RightMobileJoyStick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Action<float, float> OnRightJoyStickValueChanged;

        private bool touchBegun;

        public float sensivity = 10;

        public void OnBeginDrag(PointerEventData eventData)
        {
            GetComponent<Image>().color = Color.red;
        }


        public void OnDrag(PointerEventData eventData)
        {
            OnRightJoyStickValueChanged?.Invoke(eventData.delta.y, eventData.delta.x);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ResetAxis();
        }

        private void ResetAxis()
        {
            OnRightJoyStickValueChanged?.Invoke(0, 0);
        }
    }
}
