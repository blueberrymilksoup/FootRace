using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform ball; // select the ball in inspector as well
    public Transform goal; // in the inspector select the enemy's goal
    private NavMeshAgent navMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ball != null) // makes the enemy chase for the ball
        {
            navMeshAgent.SetDestination(ball.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            navMeshAgent.SetDestination(goal.position); // when the enemy gets the ball it will go to the goal
        }
    }
}
