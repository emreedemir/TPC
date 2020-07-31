using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tpc_CharacterInput
{
    public class MobileInput : BaseCharacterInput
    {
        public DiscreteMobileButton attackLButton;

        public DiscreteMobileButton attackRButton;

        public ContinuousMobileButton defenseButton;

        public LeftMobileJoyStick leftMobileJoyStick;

        public RightMobileJoyStick rightMobileJoystick;

        protected override void Start()
        {
            base.Start();

            leftMobileJoyStick.OnLeftJoyStickValueChanged += base.OnLeftAxisChanged;

            rightMobileJoystick.OnRightJoyStickValueChanged += base.OnRightAxisChanged;

            attackLButton.OnButtonPressed += base.SetAttackLButtonState;

            attackRButton.OnButtonPressed += base.SetAttackRButtonState;

            defenseButton.OnButtonPressed += base.SetDefenceButtonState;

            rightMobileJoystick.OnRightJoyStickValueChanged += base.OnRightAxisChanged;
        }
    }
}
