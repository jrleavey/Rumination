using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    public float radius = 20f;
    public float angle = 90f;
    public bool _isChasingPlayer = false;
    public GameObject _player;
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
        // Play animation
        // Add Hitbox
        // Deal Damage (Call Damage function on Player)
        // Cooldown
    }

}
