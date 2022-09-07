using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{

    private enum AIState
    {
        Passive,
        Investigating,
        Hostile
    }
    [SerializeField]
    private AIState _AIState;
    private NavMeshAgent _navMeshAgent;
    public float radius = 20f;
    public float angle = 90f;
    public bool _isChasingPlayer = false;
    public GameObject _player;
    private bool _IAmWaiting;

    [Range(0, 500)] public float walkRadius;
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _player = GameObject.Find("Player");
    }

    void Update()
    {
        switch(_AIState)
        {
            case AIState.Passive:
                Wander();
                break;
            case AIState.Investigating:
                // Investigating
                break;
            case AIState.Hostile:
                ChasePlayer();
                break;
        }
    }

    private void Wander()
    {
        if (_navMeshAgent != null && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && _IAmWaiting == false)
        {
            _navMeshAgent.SetDestination(RandomNavMeshLocation());
            _IAmWaiting = true;
            StartCoroutine(RandomWaitTimer());

        }
    }
    IEnumerator RandomWaitTimer()
    {
        
        int wait_time = Random.Range(1, 4);
        yield return new WaitForSeconds(wait_time);
        print("I waited for " + wait_time + "sec");
        _IAmWaiting = false;
    }

    public Vector3 RandomNavMeshLocation()
    {
        Vector3 finalPosition = Vector3.zero;
        Vector3 randomPosition = Random.insideUnitSphere * walkRadius;
        randomPosition += transform.position;
        if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, walkRadius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
    private void ChasePlayer()
    {
        _isChasingPlayer = true;
        _navMeshAgent.destination = _player.transform.position;
    }

    private void Attack()
    {
        // Play animation
        // Add Hitbox
        // Deal Damage (Call Damage function on Player)
        // Cooldown
    }

}
