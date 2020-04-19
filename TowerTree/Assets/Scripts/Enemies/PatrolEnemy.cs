using Pathfinding;
using UnityEngine;

namespace Enemies
{
    public class PatrolEnemy : MonoBehaviour
    {
        [SerializeField]
        private Transform[] _routePoints;
        [SerializeField] 
        private Transform _routeCenter;
        [SerializeField] 
        private float _maxPlayerDistanceToFollow;
        [SerializeField] 
        private float _maxPlayerDistanceToIgnore;
        [SerializeField] 
        private float _maxDistanceFromRouteCenter;

        private AIPath _aiPath;
        private AIDestinationSetter _aiDestinationSetter;
        private Transform _currentTarget;
        private Player _player;
        private PatrolEnemyStates _state;
        private int _currentTargetIndex = -1;
        private float _endReachedDistance;
        private float _pickNextWaypointDistance;
        private float _maxSpeed;

        private void Awake()
        {
            _aiDestinationSetter = GetComponent<AIDestinationSetter>();
            _aiPath = GetComponent<AIPath>();
            _player = GameObject.FindWithTag(Tags.Player).GetComponent<Player>();
        }

        private void Start()
        {
            if (_routePoints == null || _routePoints.Length == 0)
            {
                Debug.LogError("Must specify route points");
                return;
            }
            
            _endReachedDistance = _aiPath.endReachedDistance;
            _pickNextWaypointDistance = _aiPath.pickNextWaypointDist;
            _maxSpeed = _aiPath.maxSpeed;

            SelectNextRoutePoint();
        }

        private void Update()
        {
            switch (_state)
            {
                case PatrolEnemyStates.Patrolling:
                    Patrol();
                    break;
                case PatrolEnemyStates.FollowingPlayer:
                    FollowPlayer();
                    break;
                default:
                    break;
            }
        }

        private void Patrol()
        {
            if (IsPlayerClose())
            {
                SetAiDestination(_player.transform);
                _state = PatrolEnemyStates.FollowingPlayer;
                _aiPath.endReachedDistance = _endReachedDistance;
                _aiPath.pickNextWaypointDist = _pickNextWaypointDistance;
                _aiPath.maxSpeed = _maxSpeed;
            }
        }

        private void FollowPlayer()
        {
            if (IsPlayerTooFarToFollow() || IsRouteCenterTooFar())
            {
                SetAiDestination(_currentTarget);
                _state = PatrolEnemyStates.BackToRoute;
            }
        }
        
        private bool IsPlayerClose()
        {
            var isPlayerClose = (_player.transform.position - transform.position).sqrMagnitude < _maxPlayerDistanceToIgnore;
            if (isPlayerClose)
            {
                Debug.Log("Player is close");
            }

            return isPlayerClose;
        }

        private bool IsPlayerTooFarToFollow()
        {
            var isPlayerTooFarToFollow = (_player.transform.position - transform.position).sqrMagnitude > _maxPlayerDistanceToFollow;
            if (isPlayerTooFarToFollow)
            {
                Debug.Log("Player is too far to follow");
            }

            return isPlayerTooFarToFollow;
        }

        private bool IsRouteCenterTooFar()
        {
            var isRouteCenterTooFar = (_routeCenter.position - transform.position).sqrMagnitude > _maxDistanceFromRouteCenter;
            if (isRouteCenterTooFar)
            {
                Debug.Log("Route center is too far");
            }

            return isRouteCenterTooFar;
        }

        private void OnEnable()
        {
            _aiPath.CurrentTargetReached += TargetReached;
        }

        private void OnDisable()
        {
            _aiPath.CurrentTargetReached -= TargetReached;
        }

        private void TargetReached()
        {
            Debug.Log("Target reached");
            if (_state == PatrolEnemyStates.Patrolling)
            {
                SelectNextRoutePoint();
            }

            if (_state == PatrolEnemyStates.BackToRoute)
            {
                _aiPath.endReachedDistance = 0.1f;
                _aiPath.pickNextWaypointDist = 0.1f;
                _aiPath.maxSpeed = 0.5f;
                
                _state = PatrolEnemyStates.Patrolling;
            }
        }

        private void SelectNextRoutePoint()
        {
            _currentTargetIndex = (_currentTargetIndex + 1) % _routePoints.Length;
            _currentTarget = _routePoints[_currentTargetIndex];
            SetAiDestination(_currentTarget);
        }

        private void SetAiDestination(Transform target)
        {
            _aiDestinationSetter.target = target;
            Debug.Log($"Following {target.name}");
        }
    }
}