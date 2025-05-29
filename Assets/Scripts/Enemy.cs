using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private Transform[] waypoints;
    private int currentIndex;
    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //agent.SetDestination(waypoint.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance < 0.5)
        {
            agent.SetDestination(GetNextWayPoints());
        }
    }

    private Vector3 GetNextWayPoints()
    {
        if (currentIndex >= waypoints.Length)
        {
            return transform.position;
        } 
        Vector3 nextDestination = waypoints[currentIndex].position;

        currentIndex++;
        return nextDestination;
    }
}
