using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float turnSpeed = 10f;
    private int currentIndex;
    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.avoidancePriority = (int)(agent.speed * 10);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //agent.SetDestination(waypoint.position);
    }

    // Update is called once per frame
    void Update()
    {
        FaceTarget(agent.steeringTarget);
            //checks if the agent is close to the current target point
        if (agent.remainingDistance < 0.5)
        {
            //set the destination to the next waypoint
            agent.SetDestination(GetNextWayPoints());
        }
    }

    private void FaceTarget(Vector3 newTarget)
    {
        //calculate the distance from current position to the new target
        Vector3 direction = newTarget - transform.position;
        direction.y = 0;//removes vertical component
        //create a rotation that points the forward vector up the calculated direction
        Quaternion targetRotation=Quaternion.LookRotation(direction);
        //smoothly rotate from the current rotation to the target rotation at the defined speed
        transform.rotation=Quaternion.Lerp(transform.rotation,targetRotation,turnSpeed*Time.deltaTime);
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
