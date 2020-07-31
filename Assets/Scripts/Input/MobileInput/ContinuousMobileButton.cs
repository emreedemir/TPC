using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

namespace Tpc_CharacterInput
{
    public class ContinuousMobileButton : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
    {
        public Action<bool> OnButtonPressed;

        public void OnPointerDown(PointerEventData eventData)
        {
            GetComponent<Image>().color = Color.red;
            OnButtonPressed?.Invoke(true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            GetComponent<Image>().color = Color.green;

            OnButtonPressed?.Invoke(false);
        }
    }
}
