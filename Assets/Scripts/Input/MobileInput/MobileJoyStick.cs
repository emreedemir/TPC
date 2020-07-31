using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tpc_CharacterInput
{
    public abstract class MobileJoyStick : MonoBehaviour
    {
        public float Vertical { get; private set; }
        public float Horizontal { get; private set; }

    }
}
