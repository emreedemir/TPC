using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tpc_CharacterInput;
using Gladiator_ThirdPersonCamera;

namespace TPC_CharacterController
{
    public class TpcCharacterController : MonoBehaviour
    {
        public WeaponType weaponType;

        private AnimationController animationContoller;

        public State state { get; private set; }

        public BaseCharacterInput baseCharacterInput;

        public ThirdPersonCamera thirdPersonCamera;

        Queue<Command> commands = new Queue<Command>();

        public int attackValue = 0;

        public Vector3 focusPoint;


        private void Start()
        {
            animationContoller = GetComponent<AnimationController>();

            animationContoller.animator = GetComponent<Animator>();

            animationContoller.TransitToWeaponTypeAnimation(weaponType);

            state = new Idle(animationContoller, this.transform, baseCharacterInput);
        }

        private void AddAttackLCommand()
        {
            commands.Enqueue(Command.AttackL);
        }

        private void AddAttackRCommand()
        {
            commands.Enqueue(Command.AttackR);
        }


        private void Update()
        {
            if (state == null)
            {
                Debug.Log("State is null");
            }
            else
            {
                state = state.Process();
            }
        }

        public void HandleCommands()
        {
            Command command = commands.Dequeue();

            if (command == Command.AttackL)
            {
                animationContoller.SetNextAttackL();
            }
            else if (command == Command.AttackR)
            {
                animationContoller.SetNextAttackR();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.name == "enemy")
            {
                Debug.Log("Enemy Attack");

                if (state.currentStateName != StateName.Defence)
                {
                    attackValue++;
                }
            }
        }
    }

    public enum WeaponType
    {
        None,
        UnArmed,
        TwoHandAxe,
        TwoHandSword,
    }
}
