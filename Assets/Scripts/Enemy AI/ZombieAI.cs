using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;

    [SerializeField]
    private GameObject _player;
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _player = GameObject.Find("Player");
    }

    void Update()
    {
        Movement();
    }
    private void Movement()
    {
        _navMeshAgent.destination = _player.transform.position;
    }

    private void Attack()
    {

    }

}
