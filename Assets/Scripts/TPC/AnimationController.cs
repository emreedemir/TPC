using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPC_CharacterController
{
    public static class AnimatorParameters
    {
        public static int idleParam = Animator.StringToHash("idle");


    }

    public class AnimationController : MonoBehaviour
    {
        public Animator animator;

        public Transform targetHead;

        public string idleParameter = "idle";

        public void TransitToWeaponTypeAnimation(WeaponType weaponType)
        {
            if (weaponType == WeaponType.UnArmed)
            {
                animator.SetBool("unArmed", true);
            }
            else if (weaponType == WeaponType.TwoHandAxe)
            {

            }
            else if (weaponType == WeaponType.TwoHandSword)
            {

            }
            else
            {
                Debug.LogWarning("Weapon type non-defined");
                return;
            }
        }

        public void Idle(bool state) { animator.SetBool("idle", state); }

        public void AttackL(bool state) { animator.SetBool("attackL", state); }

        public void AttackR(bool state) { animator.SetBool("attackR", state); }

        public void Move(bool state) { animator.SetBool("walk", state); }

        public void Defence(bool state) { animator.SetBool("defence", state); }

        public void Dead(bool state) { animator.SetBool("dead", state); }

        public void RollTrigger() { animator.SetTrigger("rollTrigger"); }

        public void LeftAxis(float vertical, float horizontal)
        {
            animator.SetFloat("vertical", vertical);

            animator.SetFloat("horizontal", horizontal);
        }

        public void SetHit(bool state) { animator.SetBool("hit", state); }

        public void SetHitValue(int hitValue)
        {
            animator.SetInteger("hitValue", hitValue);
        }

        public void TurnAxis(float horizontal, float vertical)
        {
            Vector2 rig = new Vector2(horizontal, vertical);

            if (Math.Abs(horizontal) > (1.6f * Math.Abs(vertical)))
            {
                animator.SetFloat("turn", rig.magnitude);

                animator.SetFloat("verticalR", -vertical);

                animator.SetFloat("horizontalR", -horizontal);
            }
            else
            {
                animator.SetFloat("turn", 0);

                animator.SetFloat("verticalR", -vertical);

                animator.SetFloat("horizontalR", -horizontal);
            }
        }
        public void TurnTrigger()
        {
            animator.SetTrigger("turnTrigger");
        }

        public void SetNextAttackL()
        {
            int value = animator.GetInteger("attackLCount");

            Debug.Log("AttackLCount " + value);

            if (value < 3)
            {
                animator.SetInteger("attackLCount", value + 1);
            }
        }

        public void SetNextAttackR()
        {
            int value = animator.GetInteger("attackRCount");

            if (value < 3)
            {
                animator.SetInteger("attackRCount", value + 1);
            }
        }

        public void ResetAttackLCount()
        {
            animator.SetInteger("attackLCount", 0);
        }

        public void ResetAttackRCount()
        {
            animator.SetInteger("attackRCount", 0);
        }
    }
}

