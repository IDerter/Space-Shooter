using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    [RequireComponent(typeof(SpaceShip))]
    public class AIController : MonoBehaviour
    {
        public enum AIBehaviour
        {
            Null,
            Patrol,
            RoutePatrol
        }

        [SerializeField] private AIBehaviour _aIBehaviour;
        [SerializeField] private AIPointPatrol _aIPointPatrol;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float _navigationLinear;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float _navigationAngular;

        [SerializeField] private float _randomSelectMovePointTime;

        [SerializeField] private float _randomSelectEvadeObstaclesTime;

        [SerializeField] private float _findNewTargetTime;

        [SerializeField] private float _shootDelay;

        [SerializeField] private float _evadeRayLength;

        [SerializeField] private GameObject[] _masPatrolRoutes;

        private SpaceShip _spaceShip;

        private Vector3 _movePosition;

        private Destructible _selectedTarget;

        private Timer _randomizeDirectionTimer;
        private Timer _randomizeEvadeTimer;
        private Timer _fireTimer;
        private Timer _findNewTargetTimer;
        private int count = 0;
        private float minMagnitude = 1f;

        private void Start()
        {
            _spaceShip = GetComponent<SpaceShip>();
            InitTimers();
        }

        private void Update()
        {
            UpdateTimers();

            UpdateAI();

            
        }

        private void UpdateAI()
        {
            if (_aIBehaviour == AIBehaviour.Patrol || _aIBehaviour == AIBehaviour.RoutePatrol)
            {
                UpdateBehaviourPatrol();
            }
        }

        private void UpdateBehaviourPatrol()
        {
            ActionFindNewMovePosition();
            ActionControlShip();
            ActionFindNewAttackTarget();
            ActionFire();
            ActionEvadeCollision();
        }

        private void ActionFindNewMovePosition()
        {
            if (_aIBehaviour == AIBehaviour.RoutePatrol)
            {
                if (_masPatrolRoutes != null && count < _masPatrolRoutes.Length)
                {
                    if ((transform.position - _masPatrolRoutes[count].transform.position).magnitude > minMagnitude)
                    {
                        _movePosition = _masPatrolRoutes[count].transform.position;
                    }
                    else
                    {
                        count++;
                        if(count == _masPatrolRoutes.Length)
                        {
                            count = 0;
                        }
                    }
                }
            }


            if (_aIBehaviour == AIBehaviour.Patrol)
            {
                if (_selectedTarget != null && _findNewTargetTimer.IsFinished)
                {
                    if (_selectedTarget.GetComponent<SpaceShip>() != null)
                    {
                        Vector2 pos = CalculateLeadTarget(_selectedTarget.transform.position, _selectedTarget.transform.up * _selectedTarget.GetComponent<SpaceShip>().ThrustControl, 15f, transform.position);
                        _movePosition = new Vector3(pos.x, pos.y, _selectedTarget.transform.position.z);
                    }
                    
                    _findNewTargetTimer.Start(_findNewTargetTime);
                }
                else
                {
                    if (_aIPointPatrol != null)
                    {
                        bool isInsidePatrolZone = (_aIPointPatrol.transform.position - transform.position).sqrMagnitude < _aIPointPatrol.Radius * _aIPointPatrol.Radius;

                        if (isInsidePatrolZone)
                        {
                            if (_randomizeDirectionTimer.IsFinished)
                            {
                                Vector2 newPoint = UnityEngine.Random.onUnitSphere * _aIPointPatrol.Radius + _aIPointPatrol.transform.position;

                                _movePosition = newPoint;

                                _randomizeDirectionTimer.Start(_randomSelectMovePointTime);
                            }

                        }

                        else
                        {
                            _movePosition = _aIPointPatrol.transform.position;
                        }
                    }
                }
            }
        }

        private Vector2 CalculateLeadTarget(Vector2 targetPosition, Vector2 targetVelocity, float bulletSpeed, Vector2 shooterPosition)
        {
            Vector2 relativePosition = targetPosition - shooterPosition;
            float distance = relativePosition.magnitude;
            float timeToIntercept = distance / bulletSpeed;

            Vector2 leadTarget = targetPosition + (targetVelocity * timeToIntercept);
            return leadTarget;
        }

        

        private void ActionEvadeCollision()
        {
            
            var allRaycast = Physics2D.RaycastAll(transform.position, (transform.up + new Vector3(Random.Range(0, 3), Random.Range(-3, 3), 0)) * _evadeRayLength / 2);
           // Debug.DrawRay(transform.position, (transform.up + new Vector3(Random.Range(0, 3), Random.Range(-3, 3), 0)) * _evadeRayLength / 2);

            RaycastHit2D ray = allRaycast[0];

            float maxLen = _evadeRayLength / 3;
            bool flag = false;

            foreach (var i in allRaycast)
            {
                if (i.transform.gameObject != null)
                {
                    if (i.transform.GetComponent<SpaceShip>() != _spaceShip)
                    {
                        if ((i.transform.position - transform.position).magnitude < maxLen)
                        {
                            maxLen = (i.transform.position - transform.position).magnitude;
                            ray = i;
                            flag = true;
                           // Debug.Log(ray.transform.name);
                        }
                    }
                }
                if (_randomizeEvadeTimer.IsFinished)
                {
                    if (ray.transform.gameObject != null && flag)
                    {
                        _movePosition = transform.position + transform.right * 100f;
                        _randomizeEvadeTimer.Start(_randomSelectEvadeObstaclesTime);
                    }
                }
                
            }
           

        }

        private void ActionControlShip()
        {
            _spaceShip.ThrustControl = _navigationLinear;

            _spaceShip.TorqueControl = ComputeAliginTorqueNormalized(_movePosition, _spaceShip.transform) * _navigationAngular;
        }

        private const float MAX_ANGLE = 45.0f;

        private static float ComputeAliginTorqueNormalized(Vector3 targetPosition, Transform ship)
        {
            Vector2 localTargetPosition = ship.InverseTransformPoint(targetPosition); // делаем target дочерним к ship

            float angle = Vector3.SignedAngle(localTargetPosition, Vector3.up, Vector3.forward);

            angle = Mathf.Clamp(angle, -MAX_ANGLE, MAX_ANGLE) / MAX_ANGLE;

            //Debug.Log(angle);

            return -angle;

        }

        private void ActionFindNewAttackTarget()
        {
            if (_findNewTargetTimer.IsFinished)
            {
                _selectedTarget = FindNearestDestructibleTarget();

                _findNewTargetTimer.Start(_findNewTargetTime);
            }
        }

        private void ActionFire()
        {
            if(_selectedTarget != null)
            {
                if (_fireTimer.IsFinished)
                {
                    _spaceShip.Fire(TurretMode.Primary);
                    _fireTimer.Start(_shootDelay);
                }
            }
        }

        private Destructible FindNearestDestructibleTarget()
        {
            float maxDist = float.MaxValue;

            Destructible potentialTarget = null;

            foreach(var i in Destructible.AllDestructibles)
            {
                if (i != null)
                {
                    if (i.GetComponent<SpaceShip>() == _spaceShip) continue;

                    if (i.TeamId == Destructible.TeamIdNeutral) continue;

                    if (i.TeamId == _spaceShip.TeamId) continue;

                    float dist = Vector2.Distance(_spaceShip.transform.position, i.transform.position);
                    if (maxDist > dist)
                    {
                        maxDist = dist;
                        potentialTarget = i;
                    }
                }
            }

            return potentialTarget;
        }


        #region Timers
        private void InitTimers()
        {
            _randomizeDirectionTimer = new Timer(_randomSelectMovePointTime);
            _randomizeEvadeTimer = new Timer(_randomSelectEvadeObstaclesTime);
            _fireTimer = new Timer(_shootDelay);
            _findNewTargetTimer = new Timer(_findNewTargetTime);
        }

        private void UpdateTimers()
        {
            _randomizeDirectionTimer.RemoveTime(Time.deltaTime);
            _randomizeEvadeTimer.RemoveTime(Time.deltaTime);
            _fireTimer.RemoveTime(Time.deltaTime);
            _findNewTargetTimer.RemoveTime(Time.deltaTime);
        }

        private void SetPatrolBehaviour(AIPointPatrol point)
        {
            _aIBehaviour = AIBehaviour.Patrol;
            _aIPointPatrol = point;
        }

        #endregion
    }
}

