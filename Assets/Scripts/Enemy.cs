using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour,IDamagable
{
    private NavMeshAgent agent;

    public int healthPoints = 4;
    [Header("Movement")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float turnSpeed = 10f;
    private int currentIndex;

    private float totalDistance;

    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.avoidancePriority = (int)(agent.speed * 10);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waypoints = FindFirstObjectByType<WayPointManager>().GetWayPoints();
        CollectTotalDistance();
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

    public float DistanceToFinishLine() => totalDistance + agent.remainingDistance;
    private void CollectTotalDistance()
    {
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            float distance = Vector3.Distance(waypoints[i].position, waypoints[i + 1].position);
            totalDistance += distance;
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
        //check if the waypoint index is beyonf the last waypoint
        if (currentIndex >= waypoints.Length)
        {
        //if true . return the agent's current position,effectively stopping it
            return transform.position;
        } 
        //get the current target point from the waypoints array
        Vector3 nextDestination = waypoints[currentIndex].position;
//if this is not the first waypoint , calculate the distance from the previous waypoint
        if (currentIndex > 0)
        {
            float distance = Vector3.Distance(waypoints[currentIndex].position, waypoints[currentIndex - 1].position);
            //substract this distance from the total distance
            totalDistance = totalDistance - distance;
        }
//increment the waypoint index to move to the next waypoint on the next call
        currentIndex++;
        //return the current target point;
        return nextDestination;
    }

    public void TakeDamage(int damage)
    {
        healthPoints -= damage;
        if(healthPoints<=0) Destroy(gameObject);
    }
}
