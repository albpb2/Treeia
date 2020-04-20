using System.Collections;
using Pathfinding;
using UnityEngine;

namespace Enemies
{
    public class PatrolEnemyWithGun : MonoBehaviour
    {
        [SerializeField]
        private Transform[] _routePoints;
        [SerializeField] 
        private Transform _routeCenter;
        [SerializeField] 
        private float _maxPlayerDistanceToShoot;
        [SerializeField] 
        private float _maxPlayerDistanceToIgnore;
        [SerializeField] 
        private float _maxPlayerDistanceToIgnoreInAnyAngle;
        [SerializeField] 
        private Color _shotColor;
        [SerializeField] 
        private GameObject _gunTriangle;

        private AIPath _aiPath;
        private AIDestinationSetter _aiDestinationSetter;
        private Seeker _seeker;
        private Transform _currentTarget;
        private Player _player;
        private Enemy _enemy;
        private PatrolEnemyWithGunStates _state;
        private int _currentTargetIndex = -1;
        private Vector3 _previousPosition;
        private float _nextShotTime;

        private void Awake()
        {
            _aiDestinationSetter = GetComponent<AIDestinationSetter>();
            _aiPath = GetComponent<AIPath>();
            _seeker = GetComponent<Seeker>();
            _enemy = GetComponent<Enemy>();
            _player = GameObject.FindWithTag(Tags.Player).GetComponent<Player>();
        }

        private void Start()
        {
            if (_routePoints == null || _routePoints.Length == 0)
            {
                Debug.LogError("Must specify route points");
                return;
            }

            SelectNextRoutePoint();
        }

        private void Update()
        {
            switch (_state)
            {
                case PatrolEnemyWithGunStates.Patrolling:
                    Patrol();
                    _previousPosition = transform.position;
                    break;
                case PatrolEnemyWithGunStates.Shooting:
                    Shoot();
                    break;
                default:
                    break;
            }
        }

        private void Patrol()
        {
            if (IsPlayerClose())
            {
                var direction = (transform.position - _previousPosition).normalized;
                var vectorToPlayer = _player.transform.position - _previousPosition;
                var angleToPlayer = Vector3.Angle(direction, vectorToPlayer);
                Debug.Log($"Angle to player = {angleToPlayer}");

                if (angleToPlayer < 60 || IsPlayerTooClose())
                {
                    _state = PatrolEnemyWithGunStates.Shooting;
                    _aiDestinationSetter.enabled = false;
                    _aiPath.enabled = false;
                    _seeker.enabled = false;
                    _aiDestinationSetter.target = null;
                }
            }
        }

        private void Shoot()
        {
            if (IsPlayerTooFarToShoot())
            {
                _state = PatrolEnemyWithGunStates.Patrolling;
                _aiDestinationSetter.enabled = true;
                _aiPath.enabled = true;
                _seeker.enabled = true;
                SetAiDestination(_currentTarget);
                return;
            }
            
            if (Time.time < _nextShotTime)
            {
                return;
            }
            
            const float failAxisProbability = 0.5f;
            const float errorMargin = 1f;
            var failXRandom = Random.value;
            var failYRandom = Random.value;

            Vector2 shotTarget = _player.transform.position;
            if (failXRandom < failAxisProbability)
            {
                var xError = Random.Range(-errorMargin, errorMargin);
                shotTarget.x = shotTarget.x + xError;
                Debug.Log($"Failed x by {xError}");
            }
            if (failYRandom < failAxisProbability)
            {
                var yError = Random.Range(-errorMargin, errorMargin);
                shotTarget.y = shotTarget.y + yError;
                Debug.Log($"y x by {yError}");
            }

            var shot = SharedGunShotPool.Instance.GetNextShot();
            shot.SetActive(true);
            shot.GetComponent<SpriteRenderer>().color = _shotColor;
            Vector2 position = transform.position;
            Shooting.Shoot(position, (shotTarget - position), shot, _enemy.HitPoints);
            SharedGunShotPool.Instance.ReturnShot(shot);

            _gunTriangle.SetActive(true);
            Debug.Log($"Angle before = {transform.rotation.eulerAngles}");
            transform.right = _player.transform.position - transform.position;
            Debug.Log($"Angle after = {transform.rotation.eulerAngles}");
            StartCoroutine(DisableGunTriangle());
            
            const float minSecondsBetweenShots = .1f;
            const float maxSecondsBetweenShots = 1f;
            _nextShotTime = Time.time + Random.Range(minSecondsBetweenShots, maxSecondsBetweenShots);
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
        
        private bool IsPlayerTooClose()
        {
            var isPlayerClose = (_player.transform.position - transform.position).sqrMagnitude < _maxPlayerDistanceToIgnoreInAnyAngle;
            if (isPlayerClose)
            {
                Debug.Log("Player is too close");
            }

            return isPlayerClose;
        }

        private bool IsPlayerTooFarToShoot()
        {
            var isPlayerTooFarToFollow = (_player.transform.position - transform.position).sqrMagnitude > _maxPlayerDistanceToShoot;
            if (isPlayerTooFarToFollow)
            {
                Debug.Log("Player is too far to shoot");
            }

            return isPlayerTooFarToFollow;
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
            if (_state == PatrolEnemyWithGunStates.Patrolling)
            {
                SelectNextRoutePoint();
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

        private IEnumerator DisableGunTriangle()
        {
            yield return new WaitForSeconds(0.1f);
            {
                _gunTriangle.SetActive(false);
            }
        }
    }
}