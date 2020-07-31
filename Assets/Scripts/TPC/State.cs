using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tpc_CharacterInput;

namespace TPC_CharacterController
{
    public enum StateName
    {
        None,Idle, Run, AttackR, AttackL, Dead, Roll, Defence, Turn
    }

    public abstract class State
    {     
        public enum Stage { Enter, Update, Exit }

        public StateName currentStateName;

        protected State nextState;

        protected Stage currentStage;

        protected AnimationController animationController;

        protected Transform player;

        protected BaseCharacterInput baseCharacterInput;

        protected Queue<Command> commands = new Queue<Command>();

        public State(AnimationController animator, Transform player, BaseCharacterInput baseCharacterInput)
        {
            this.animationController = animator;

            this.player = player;

            this.baseCharacterInput = baseCharacterInput;

            baseCharacterInput.attackLButtonPressed += AttackLInput;

            baseCharacterInput.attackRButtonPressed += AttackRInput;
        }

        protected virtual void Enter() { currentStage = Stage.Update; }

        protected virtual void Update() { currentStage = Stage.Update; }

        protected virtual void Exit() { currentStage = Stage.Exit; }

        public State Process()
        {
            if (currentStage == Stage.Enter)
            {
                Enter();
            }
            else if (currentStage == Stage.Update)
            {
                Update();
            }
            else
            {
                currentStage = Stage.Enter;
                return nextState;
            }

            return this;
        }

        public void AttackLInput()
        {
            commands.Enqueue(Command.AttackL);
        }

        public void AttackRInput()
        {
            commands.Enqueue(Command.AttackR);
        }

        public bool DefenceInput() { return baseCharacterInput.defenceButtonPressed; }
    }

    public class Idle : State
    {

        Quaternion headQueternion = new Quaternion();

        public Idle(AnimationController animationController, Transform player, BaseCharacterInput baseCharacterInput) : base(animationController, player, baseCharacterInput)
        {
           
        }

        protected override void Enter()
        {
            currentStateName = StateName.Idle;

            animationController.Idle(true);

            Debug.Log("Enter İdle");

            base.Enter();

        }

        protected override void Update()
        {
            Vector2 mag = new Vector2(baseCharacterInput.LeftVerticalAxis, baseCharacterInput.LeftHorizontalAxis);

            Vector2 magRight = new Vector2(baseCharacterInput.RightHorizontalAxis, baseCharacterInput.RightVerticalAxis);

            animationController.TurnAxis(baseCharacterInput.RightHorizontalAxis, baseCharacterInput.RightVerticalAxis);

            if (base.player.GetComponent<TpcCharacterController>().attackValue > 0)
            {
                nextState = new Hit(base.animationController, base.player, base.baseCharacterInput);

                Exit();
            }
            else if (commands.Count > 0)
            {
                Command command = commands.Dequeue();

                if (command == Command.AttackL)
                {
                    nextState = new AttackL(base.animationController, base.player, base.baseCharacterInput);
                    Exit();
                }
                else if (command == Command.AttackR)
                {
                    nextState = new AttackR(base.animationController, base.player, base.baseCharacterInput);
                    Exit();
                }
            }
            else if (DefenceInput())
            {
                nextState = new Defence(base.animationController, base.player, base.baseCharacterInput);

                Exit();
            }
            else if (mag.magnitude != 0)
            {
                nextState = new Run(base.animationController, base.player, base.baseCharacterInput);

                Exit();
            }
        }

        protected override void Exit()
        {
            Debug.Log("Exit İdle");

            animationController.Idle(false);

            base.Exit();
        }

    }

    public class Run : State
    {
        public Run(AnimationController animationController, Transform player, BaseCharacterInput baseCharacterInput) : base(animationController, player, baseCharacterInput)
        { }

        protected override void Enter()
        {
            animationController.Move(true);

            Debug.Log("Enter Run");

            base.Enter();
        }

        protected override void Update()
        {

            animationController.LeftAxis(baseCharacterInput.LeftVerticalAxis, baseCharacterInput.LeftHorizontalAxis);

            Vector2 mag = new Vector2(baseCharacterInput.LeftVerticalAxis, baseCharacterInput.LeftHorizontalAxis);

            float rightVertical = baseCharacterInput.RightVerticalAxis;

            float rightHorizontal = baseCharacterInput.RightHorizontalAxis;


            if (mag.magnitude == 0)
            {
                nextState = new Idle(base.animationController, base.player, base.baseCharacterInput);

                Exit();
            }

            else if (mag.magnitude != 0 && rightHorizontal != 0)
            {
                base.player.transform.Rotate(0, rightHorizontal / 1.7f, 0, Space.World);

                Debug.Log("Dönüyor");
            }
        }

        protected override void Exit()
        {
            animationController.Move(false);
            Debug.Log("Exit Move");
            base.Exit();
        }
    }

    public class AttackL : State
    {
        /// <summary>
        /// deltaTime is animation clip lenght for it is 0.5f
        /// </summary>
        public float deltaTime = 0;

        public AttackL(AnimationController animator, Transform player, BaseCharacterInput baseCharacterInput) : base(animator, player, baseCharacterInput)
        {
            base.currentStateName = StateName.AttackL;
        }

        protected override void Enter()
        {
            animationController.AttackL(true);

            commands.Enqueue(Command.AttackL);

            Debug.Log("Enter AttackL");

            base.Enter();
        }

        protected override void Update()
        {
            deltaTime += Time.deltaTime;

            if (commands.Count > 0)
            {
                Command command = commands.Dequeue();

                if (command == Command.AttackR)
                {
                    nextState = new AttackR(base.animationController, base.player, base.baseCharacterInput);
                    Exit();
                }
                else if (command == Command.AttackL)
                {
                    deltaTime = 0;
                }
            }
            else if (deltaTime > 0.5f)
            {
                nextState = new Idle(base.animationController, base.player, base.baseCharacterInput);

                Exit();
            }
        }

        protected override void Exit()
        {
            animationController.AttackL(false);

            Debug.Log("Exit AttackL");

            base.Exit();
        }
    }

    public class AttackR : State
    {
        private float deltaTime = 0;

        public AttackR(AnimationController animator, Transform player, BaseCharacterInput baseCharacterInput) : base(animator, player, baseCharacterInput)
        {
            base.currentStateName = StateName.AttackR;

        }

        protected override void Enter()
        {
            deltaTime = 0;

            animationController.AttackR(true);

            Debug.Log("Enter Attack R");

            base.Enter();
        }

        protected override void Update()
        {
            //Fix this with animation time
            deltaTime += Time.deltaTime;

            if (commands.Count > 0)
            {
                Command command = commands.Dequeue();

                if (command == Command.AttackL)
                {
                    nextState = new AttackL(base.animationController, base.player, base.baseCharacterInput);

                    Exit();
                }
                else if (command == Command.AttackR)
                {
                    deltaTime = 0;
                }
            }
            else if (deltaTime > 0.5f)
            {
                nextState = new AttackL(base.animationController, base.player, base.baseCharacterInput);

                Exit();
            }
        }

        protected override void Exit()
        {

            animationController.AttackR(false);

            Debug.Log("Exit AttackR");
            base.Exit();
        }

    }

    public class Defence : State
    {
        public Defence(AnimationController animator, Transform player, BaseCharacterInput baseCharacterInput) : base(animator, player, baseCharacterInput)
        {

        }

        protected override void Enter()
        {
            animationController.Defence(true);

            Debug.Log("Enter Defence");

            base.Enter();
        }

        protected override void Update()
        {
            animationController.LeftAxis(baseCharacterInput.LeftVerticalAxis, baseCharacterInput.LeftHorizontalAxis);

            Vector2 mag = new Vector2(baseCharacterInput.LeftVerticalAxis, baseCharacterInput.LeftHorizontalAxis);

            if (mag.magnitude != 0)
            {
                animationController.RollTrigger();
            }

            if (!DefenceInput())
            {
                nextState = new Idle(base.animationController, base.player, base.baseCharacterInput);

                Exit();
            }
        }

        protected override void Exit()
        {
            animationController.Defence(false);
            Debug.Log("Exit Defence");
            base.Exit();
        }
    }

    public class Hit : State
    {
        public Hit(AnimationController animator, Transform player, BaseCharacterInput baseCharacterInput) : base(animator, player, baseCharacterInput)
        {
        }

        protected override void Enter()
        {
            int hitValue = base.player.GetComponent<TpcCharacterController>().attackValue;

            animationController.SetHit(true);
            animationController.SetHitValue(hitValue);

            Debug.Log("Enter Hit");

            base.Enter();
        }

        protected override void Update()
        {
            int attackValue = base.player.GetComponent<TpcCharacterController>().attackValue;

            nextState = new Idle(base.animationController, base.player, base.baseCharacterInput);

            Exit();
        }

        protected override void Exit()
        {
            player.GetComponent<TpcCharacterController>().attackValue = 0;
            animationController.SetHit(false);
            animationController.SetHitValue(0);
            base.Exit();
        }

    }

    public class Dead : State
    {
        public Dead(AnimationController animator, Transform player, BaseCharacterInput baseCharacterInput) : base(animator, player, baseCharacterInput)
        {
        }

        protected override void Enter()
        {
            animationController.Dead(true);
            base.Enter();
        }


        protected override void Update()
        {
            base.Update();
        }

        protected override void Exit()
        {
            base.Exit();
        }
    }
}

public enum Command
{
    AttackL,
    AttackR
}


