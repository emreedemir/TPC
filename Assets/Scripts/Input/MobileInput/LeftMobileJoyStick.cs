using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

namespace Tpc_CharacterInput
{

    [RequireComponent(typeof(UnityEngine.UI.AspectRatioFitter))]
    public class LeftMobileJoyStick : MobileJoyStick, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        public Action<float, float> OnLeftJoyStickValueChanged;


        public RectTransform Background;
        public RectTransform Knob;

        public float offset;

        Vector2 PointPosition;

        public void OnDrag(PointerEventData eventData)
        {

            PointPosition = new Vector2((eventData.position.x - Background.position.x) / ((Background.rect.size.x - Knob.rect.size.x) / 2),
                (eventData.position.y - Background.position.y) / ((Background.rect.size.y - Knob.rect.size.y) / 2));

            PointPosition = (PointPosition.magnitude > 1.0f) ? PointPosition.normalized : PointPosition;

            Knob.transform.position = new Vector2((PointPosition.x * ((Background.rect.size.x - Knob.rect.size.x) / 2) * offset) + Background.position.x,
                (PointPosition.y * ((Background.rect.size.y - Knob.rect.size.y) / 2) * offset) + Background.position.y);

        }

        public void OnEndDrag(PointerEventData eventData)
        {
            PointPosition = new Vector2(0f, 0f);
            Knob.transform.position = Background.position;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnEndDrag(eventData);
        }


        void Update()
        {
            OnLeftJoyStickValueChanged?.Invoke(PointPosition.x, PointPosition.y);
        }
    }
}

