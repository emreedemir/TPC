using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TPC_CharacterController
{
    public abstract class AgentState
    {
        public enum Stage
        {
            Enter, Update, Exit
        }

        protected Stage stage;

        protected AgentState nextAgentState;

        protected NavMeshAgent navMeshagent;

        protected Transform agentTransform;

        protected AnimationController animationController;

        protected AgentPlayer agentPlayer;

        public AgentState(Transform playerTransform, NavMeshAgent agent, AnimationController animationController)
        {
            agentPlayer = playerTransform.GetComponent<AgentPlayer>();

            this.agentTransform = playerTransform;

            this.navMeshagent = agent;

            this.animationController = animationController;

        }

        public virtual void Enter() { stage = Stage.Update; }

        public virtual void Update() { stage = Stage.Update; }

        public virtual void Exit() { stage = Stage.Exit; }

        public AgentState Process()
        {
            if (stage == Stage.Enter)
            {
                Enter();
            }
            else if (stage == Stage.Update)
            {
                Update();
            }
            else
            {
                stage = Stage.Enter;
                return nextAgentState;
            }

            return this;
        }
    }

    public class IdleAgent : AgentState
    {

        public IdleAgent(Transform playerTransform, NavMeshAgent agent, AnimationController animationController) : base(playerTransform, agent, animationController)
        {
        }

        public override void Enter()
        {
            animationController.Idle(true);
            base.Enter();
        }

        public override void Update()
        {
            if (base.agentPlayer.IsTarget())
            {
                if (base.agentPlayer.AttackMode())
                {
                    if (base.agentPlayer.IsTargetFarAway())
                    {
                        //KOŞ YANINA

                        ResetTurnAxis();

                        nextAgentState = new RunAgent(base.agentTransform, base.navMeshagent, base.animationController);

                        Exit();
                    }
                    else if (base.agentPlayer.IsTargetNear())
                    {
                        if (base.agentPlayer.IsLookAtTarget())
                        {
                            ResetTurnAxis();

                            if (base.agentPlayer.ShouldDefence())
                            {
                                nextAgentState = new DefenceAgent(base.agentTransform, base.navMeshagent, base.animationController);

                                Exit();
                            }
                            else if (base.agentPlayer.ShouldAttack())
                            {
                                nextAgentState = new AgentAttackL(base.agentTransform, base.navMeshagent, base.animationController);

                                Exit();
                            }
                        }
                        else
                        {
                            float angle = base.agentPlayer.GetAngleBetweenFromAgentToTarget();

                            if (angle > 0)
                            {
                                animationController.TurnAxis(-angle * Time.deltaTime, 0);
                            }
                            else
                            {
                                animationController.TurnAxis(angle * Time.deltaTime, 0);
                            }
                        }
                    }
                    else if (base.agentPlayer.IsTargetTooNear())
                    {

                        if (!base.agentPlayer.IsLookAtTarget())
                        {

                            float angle = base.agentPlayer.GetAngleBetweenFromAgentToTarget();

                            if (angle > 0)
                            {
                                animationController.TurnAxis(-angle * Time.deltaTime, 0);
                            }
                            else
                            {
                                animationController.TurnAxis(angle * Time.deltaTime, 0);
                            }

                        }
                        else if (base.agentPlayer.ShouldAttack())
                        {
                            ResetTurnAxis();
                            nextAgentState = new AgentAttackL(base.agentTransform, base.navMeshagent, base.animationController);

                            Exit();
                        }
                        else if (base.agentPlayer.ShouldDefence())
                        {
                            ResetTurnAxis();
                            nextAgentState = new DefenceAgent(base.agentTransform, base.navMeshagent, base.animationController);

                            Exit();
                        }
                    }
                }
            }
            /*
            if (PlayerFarAway())
            {
                nextAgentState = new RunAgent(base.agentTransform, base.navMeshagent, base.animationController);
                Exit();
            }
            else if (PlayerInAttackableDistance())
            {
                if (base.agentPlayer.IsLookAtTarget())
                {
                    if (base.agentPlayer.ShouldAttack())
                    {
                        nextAgentState = GetRandomAttack();

                        Exit();
                    }
                    else if (base.agentPlayer.ShouldDefence())
                    {
                        nextAgentState = new DefenceAgent(base.agentTransform, base.navMeshagent, base.animationController);

                        Exit();
                    }
                }
                else
                {
                    Vector2 lookAtDirectionAxis = base.agentPlayer.GetLookAtDirection();

                    animationController.TurnAxis(lookAtDirectionAxis.x, lookAtDirectionAxis.y);
                }
            }
        }
        else
        {
            Debug.Log("Target yok");
        }
        */



        }

        public override void Exit()
        {
            animationController.Idle(false);
            base.stage = Stage.Exit;
            base.Exit();
        }

        public AgentState GetRandomAttack()
        {
            return new AgentAttackR(base.agentTransform, base.navMeshagent, base.animationController);
        }

        private void ResetTurnAxis()
        {
            base.animationController.TurnAxis(0, 0);
        }
    }

    public class RunAgent : AgentState
    {
        public RunAgent(Transform playerTransform, NavMeshAgent agent, AnimationController animationController) : base(playerTransform, agent, animationController)
        {
        }

        public override void Enter()
        {
            animationController.Move(true);

            base.Enter();
        }

        public override void Update()
        {
            if (base.agentPlayer.IsTarget())
            {
                if (base.agentPlayer.AttackMode())
                {
                    if (base.agentPlayer.IsTargetNear())
                    {
                        nextAgentState = new IdleAgent(base.agentTransform, base.navMeshagent, base.animationController);

                        Exit();
                    }
                    else
                    {
                        animationController.LeftAxis(0, 1);

                        if (!base.agentPlayer.IsLookAtTarget())
                        {

                            if (base.agentPlayer.IsTargetClockwise())
                            {

                                base.agentTransform.Rotate(0, 5, 0);
                            }
                            else
                            {
                                base.agentTransform.Rotate(0, -5, 0);
                            }
                        }
                    }
                }
            }
        }

        public override void Exit()
        {
            animationController.Move(false);
            base.Exit();
        }
    }

    public class DefenceAgent : AgentState
    {

        float rollingTime = 0;

        bool rolled = false;

        public DefenceAgent(Transform playerTransform, NavMeshAgent agent, AnimationController animationController) : base(playerTransform, agent, animationController)
        {

        }

        public override void Enter()
        {
            animationController.Defence(true);

            base.Enter();
        }

        public override void Update()
        {
            if (base.agentPlayer.IsTarget())
            {
                if (base.agentPlayer.IsTargetFarAway())
                {

                    //KOŞ YANINA
                }
                else if (base.agentPlayer.IsTargetNear())
                {
                    //Eger  düşman saldırıyorsa
                    if (base.agentPlayer.ShouldDefence())
                    {
                        nextAgentState = new DefenceAgent(base.agentTransform, base.navMeshagent, base.animationController);

                    }
                }
                else if (base.agentPlayer.IsTargetTooNear())
                {
                    //
                    //
                }
                if (!base.agentPlayer.ShouldDefence())
                {
                    nextAgentState = new IdleAgent(base.agentTransform, base.navMeshagent, base.animationController);

                    Exit();
                }
                else if (!InSecure())
                {
                    /*
                    if (base.agentPlayer.ShouldEscapeAttack())
                    {
                        Vector2 directionToEscape = base.agentPlayer.GetBestEscapeDirection();

                        directionToEscape = directionToEscape.normalized;

                        animationController.LeftAxis(directionToEscape.x, directionToEscape.y);

                    }
                    else if (!base.PlayerFarAway())
                    {
                        //nextAgentState = new RunAgent();

                    }
                    */
                    if (rolled == true)
                    {
                        rollingTime += Time.deltaTime;
                    }
                    if (rollingTime > 2 || rolled == false)
                    {
                        if (base.agentPlayer.GetDistanceFromAgentToTarget() < 2)
                        {
                            animationController.RollTrigger();

                            //Get BEst escape point from player
                            animationController.LeftAxis(0, -1);

                            rollingTime = 0;
                            rolled = true;
                        }
                    }
                }
            }
        }

        public override void Exit()
        {
            animationController.Defence(false);

            base.Exit();
        }

        public bool InSecure()
        {
            return base.agentPlayer.IsLookAtTarget() || !base.agentPlayer.ShouldDefence();
        }
    }

    public class AgentAttackR : AgentState
    {
        public AgentAttackR(Transform playerTransform, NavMeshAgent agent, AnimationController animationController) : base(playerTransform, agent, animationController)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
        }
    }

    public class AgentAttackL : AgentState
    {
        public AgentAttackL(Transform playerTransform, NavMeshAgent agent, AnimationController animationController) : base(playerTransform, agent, animationController)
        {
        }

        public override void Enter()
        {
            animationController.AttackL(true);
            base.Enter();
        }

        public override void Update()
        {

            Debug.Log("Şuan saldıryor");

            if (!SightOfAttack())
            {
                nextAgentState = new IdleAgent(base.agentTransform, base.navMeshagent, base.animationController);

                Exit();
            }
        }


        public override void Exit()
        {
            base.Exit();
        }



        public bool SightOfAttack()
        {
            return base.agentPlayer.inAttackSight;
        }
    }
}


