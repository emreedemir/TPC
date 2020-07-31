using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TPC_CharacterController
{
    public class AgentPlayer : MonoBehaviour
    {
        Rigidbody rigidbody;

        AnimationController animationController;

        NavMeshAgent agent;

        Transform agentTransform;

        AgentState agentState;

        //Send with attack Direction
        public Action AgentGetHit;

        public Animator animator;

        Transform target;

        private bool shouldDefence;

        private bool shouldAttack;

        private bool shouldEscapeAttack;

        private bool beFree;

        private bool isNearTarget;

        private bool isLookAtTarget;

        public bool inAttackSight;

        public float attackDistance = 5;

        public float attackArenaDistance = 7;

        public float attackAngle = 60;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();

            agent = GetComponent<NavMeshAgent>();

            agentTransform = GetComponent<Transform>();

            animationController = GetComponent<AnimationController>();

            animationController.animator = GetComponent<Animator>();

            animationController.TransitToWeaponTypeAnimation(WeaponType.UnArmed);

            animationController.Idle(true);

            StartCoroutine(TargetCoroutine());

            agent.stoppingDistance = 2f;

            agentState = new IdleAgent(this.transform, this.agent, this.animationController);
        }

        private void HandleAttackSight(bool sightState)
        {
            inAttackSight = sightState;
        }

        IEnumerator TargetCoroutine()
        {

            yield return new WaitForSeconds(5f);

            target = FindObjectOfType<TpcCharacterController>().transform;
        }

        void Update()
        {
            agentState = agentState.Process();

            if (target != null)
            {
                HandleLineOfSight();
            }
        }

        public void HandleLineOfSight()
        {
            float distanceToTarget = GetDistanceFromAgentToTarget();

            if (distanceToTarget <= attackDistance)
            {
                if (IsLookAtTarget())
                {
                    Debug.DrawLine(transform.position, target.position, Color.green);
                }
                else
                {
                    Debug.DrawLine(transform.position, target.position, Color.cyan);
                }
            }
            else if (distanceToTarget <= attackArenaDistance)
            {
                Debug.DrawLine(transform.position, target.position, Color.blue);
            }
            else
            {
                Debug.DrawLine(transform.position, target.position, Color.red);
            }
        }

        public bool AttackMode()
        {
            return true;
        }

        public bool IsTarget()
        {
            return target != null;
        }

        public bool IsAgentInAttackDistance()
        {
            return Vector3.Distance(this.transform.position, target.position) <= attackDistance;
        }

        public bool IsAgentInAttackRotation()
        {


            return IsLookAtTarget();
        }

        public bool IsTargetFarAway()
        {
            if (target != null)
                return Vector3.Distance(this.transform.position, target.transform.position) > attackArenaDistance;
            return false;

        }

        public bool IsTargetNear()
        {
            if (target != null)
            {
                float distanceToTarget = Vector3.Distance(this.transform.position, target.transform.position);
                return distanceToTarget > attackDistance && distanceToTarget <= attackArenaDistance;
            }
            return false;
        }

        public bool IsTargetTooNear()
        {
            if (target != null)
            {
                float distanceToTarget = Vector3.Distance(this.transform.position, target.transform.position);
                return distanceToTarget <= attackDistance;
            }
            return false;
        }


        public bool IsThereTarget()
        {
            return target != null;
        }

        public bool AttackablePosition()
        {
            return false;
        }

        public void LookAtTarget()
        {

        }

        public Transform GetTarget()
        {
            return FindObjectOfType<TpcCharacterController>().transform;
        }

        public int AttackRiskValue()
        {
            Transform target = FindObjectOfType<TpcCharacterController>().transform;

            return Convert.ToInt32(Vector3.Distance(this.transform.position, target.position));
        }

        public bool ShouldDefence()
        {
            return shouldDefence;
        }

        public bool IsLookAtTarget()
        {
            Vector3 direction = target.position - this.transform.position;

            Vector3 forward = transform.forward;

            float dotProduct = Vector3.Dot(direction, forward);

            float angle = Vector3.Angle(forward, direction);

            if (angle > 3 || angle < -3)
            {
                return false;
            }

            return true;
        }

        public Vector2 GetLineOfSightNagle() { return new Vector2(-5, 5); }

        public bool IsTargetClockwise()
        {

            Vector3 direction = target.transform.position - transform.position;

            Vector3 up = transform.up;

            float angleBetween = Vector3.Angle(direction, up);

            Vector3 cross = Vector3.Cross(up, direction);

            if (cross.z > 0)
            {
                return true;
            }
            return false;
        }

        public bool ShouldAttack()
        {
            return shouldAttack;
        }

        public bool ShouldEscapeAttack()
        {
            return shouldEscapeAttack;
        }

        public Vector2 GetBestEscapeDirection()
        {
            return new Vector2(0, 0);
        }

        public Vector2 GetLookAtDirection()
        {
            return new Vector2(0, 0);
        }

        public Vector2 GetMovementAxis()
        {
            Vector2 direction = target.position - transform.position;

            return direction.normalized;
        }

        /// <summary>
        /// Return Distance from agent To Target,if target null return infinity distance
        /// </summary>
        /// <returns>Distance</returns>
        public float GetDistanceFromAgentToTarget()
        {
            if (target != null)
            {
                return Vector3.Distance(this.transform.position, target.position);
            }
            return Mathf.Infinity;
        }

        /// <summary>
        /// Return Angle Between Agent and Target,if target is null, return infinity
        /// </summary>
        /// <returns>Angle</returns>
        public float GetAngleBetweenFromAgentToTarget()
        {
            if (target != null)
            {
                Vector3 direction = target.transform.position - transform.position;

                Vector3 up = transform.up;

                return Vector3.Angle(direction, up);
            }
            return Mathf.Infinity;
        }
    }
}

