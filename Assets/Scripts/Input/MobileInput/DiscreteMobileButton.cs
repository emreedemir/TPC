using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

namespace Tpc_CharacterInput
{
    public class DiscreteMobileButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
    {
        public Action OnButtonPressed;

        public void OnPointerDown(PointerEventData eventData)
        {
            GetComponent<Image>().color = Color.green;
            OnButtonPressed?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            GetComponent<Image>().color = Color.blue;
        }
    }
}
