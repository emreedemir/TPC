using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tpc_CharacterInput
{
    public abstract class BaseCharacterInput : MonoBehaviour
    {
        public Action attackLButtonPressed { get; set; }

        public Action attackRButtonPressed { get; set; }

        public bool defenceButtonPressed { get; private set; }

        public float RightVerticalAxis { get; private set; }

        public float RightHorizontalAxis { get; private set; }

        public float LeftVerticalAxis { get; private set; }

        public float LeftHorizontalAxis { get; private set; }

        protected virtual void Start()
        {

        }

        protected void SetAttackLButtonState()
        {
            attackLButtonPressed?.Invoke();
        }

        protected void SetAttackRButtonState()
        {
            attackRButtonPressed?.Invoke();
        }

        protected void SetDefenceButtonState(bool state)
        {
            defenceButtonPressed = state;
        }

        protected void OnLeftAxisChanged(float verticalAxis, float horizontalAxis)
        {
            this.LeftVerticalAxis = verticalAxis;
            this.LeftHorizontalAxis = horizontalAxis;
        }

        protected void OnRightAxisChanged(float verticalAxis, float horizontalAxis)
        {
            this.RightVerticalAxis = verticalAxis;
            this.RightHorizontalAxis = horizontalAxis;

            Debug.Log("Vertical Right" + verticalAxis + " Horizontal" + horizontalAxis);
        }
    }
}
