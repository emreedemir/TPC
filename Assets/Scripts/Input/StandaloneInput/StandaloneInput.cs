using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Tpc_CharacterInput
{
    public class StandaloneInput : BaseCharacterInput
    {
        public DiscreateStandaloneInput attackLButton;

        public DiscreateStandaloneInput attackRButton;

        public LeftStickStandalone leftStickStandalone;

        public RightStickStandalone rightStickStandalone;

        protected override void Start()
        {
            base.Start();
        }
    }
}

public class LeftStickStandalone : MonoBehaviour
{

    public StickStandaloneArrow up;
    public StickStandaloneArrow left;
    public StickStandaloneArrow right;
    public StickStandaloneArrow down;

}

public class RightStickStandalone : MonoBehaviour
{

}

public class StickStandaloneArrow : MonoBehaviour
{
    public Action<float> OnStickPressed;

    public KeyCode keyCode;

    float deltaPress = 0;

    private void Update()
    {
        if (Input.GetKey(keyCode))
        {
            deltaPress += Time.deltaTime / 60;

            OnStickPressed?.Invoke(deltaPress);
        }
        else
        {
            deltaPress = 0;
            OnStickPressed?.Invoke(deltaPress);
        }
    }
}


