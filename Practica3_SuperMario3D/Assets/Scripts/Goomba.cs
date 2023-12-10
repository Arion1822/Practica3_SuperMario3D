using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Goomba : MonoBehaviour
{
    public float patrolSpeed = 1.5f;
    public float alertSpeed = 2.5f;
    public float patrolTime = 3f;
    public float alertRadius = 3f;
    public float timeToLoseAlert = 5f;

    private Transform player;
    private NavMeshAgent navMeshAgent;
    private Vector3 patrolStartPosition;
    private float timer;
    private Animator animator;


    private enum GoombaState
    {
        Patrol,
        Alert
    }

    private GoombaState currentState = GoombaState.Patrol;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        patrolStartPosition = transform.position;
        timer = patrolTime;
        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        switch (currentState)
        {
            case GoombaState.Patrol:
                Patrol();
                break;

            case GoombaState.Alert:
                Alert();
                break;
        }
        animator.SetBool("alert", currentState == GoombaState.Alert);

    }

    private void Patrol()
    {
        // Patrol between the start position and a random point within the patrol radius
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            navMeshAgent.speed = patrolSpeed;
            timer = patrolTime;
            Vector3 randomPatrolPoint = patrolStartPosition + Random.insideUnitSphere * alertRadius;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomPatrolPoint, out hit, alertRadius, NavMesh.AllAreas);
            navMeshAgent.SetDestination(hit.position);
        }

        // Check if the player is within the alert radius
        if (Vector3.Distance(transform.position, player.position) < alertRadius)
        {
            currentState = GoombaState.Alert;
        }
    }

    private void Alert()
    {
        // Move towards the player
        navMeshAgent.speed = alertSpeed;
        navMeshAgent.SetDestination(player.position);

        // Check if the player is still within the alert radius
        if (Vector3.Distance(transform.position, player.position) > alertRadius)
        {
            currentState = GoombaState.Patrol;
            timer = patrolTime;
        }

        // Check if the player is touched, and remove life
        /*if (Vector3.Distance(transform.position, player.position) < 1.5f)
        {
            GameManager.Instance.RemoveLife(1);
        }*/

        // Check if the alert time has elapsed
        timeToLoseAlert -= Time.deltaTime;
        if (timeToLoseAlert <= 0f)
        {
            currentState = GoombaState.Patrol;
            timer = patrolTime;
            timeToLoseAlert = 5f;
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            GameManager.Instance.RemoveLife(1);
            //Destroy(gameObject);
        }
    }
}
