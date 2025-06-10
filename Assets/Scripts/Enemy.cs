using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public enum EnemyType
{
    Basic,
    Fast,
    None
};
public class Enemy : MonoBehaviour, IDamagable
{
    public int healthPoints = 4;
    [SerializeField] private EnemyType enemyType;
    [FormerlySerializedAs("myNewWaypoints")] [Header("Movement")] [SerializeField] 
    private List<Transform> myWaypoints;

    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private Transform centerPoint;
    private NavMeshAgent agent;
    private int nextWayPointIndex;
    private int currentWayPointIndex;

    private float totalDistance;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.avoidancePriority = (int)(agent.speed * 10);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetupEnemy(List<Waypoint> newWaypoints)
    {
        myWaypoints = new List<Transform>();
        foreach (var point in newWaypoints)
        {
            myWaypoints.Add(point.transform);
        }
        CollectTotalDistance();
    }


    // Update is called once per frame
    private void Update()
    {
        FaceTarget(agent.steeringTarget);
        //checks if the agent is close to the current target point
        if (ShouldChangeWaypoint())
            //set the destination to the next waypoint
            agent.SetDestination(GetNextWayPoints());
    }

    private bool ShouldChangeWaypoint()
    {
        if (nextWayPointIndex >= myWaypoints.Count) return false;

        if (agent.remainingDistance < 0.5) return true;
        
        Vector3 currentWaypoint = myWaypoints[currentWayPointIndex].position;
        Vector3 nextWaypoint = myWaypoints[nextWayPointIndex].position;

        float distanceToNextWayPoint = Vector3.Distance(transform.position, nextWaypoint);
        float distanceBetweenPoints = Vector3.Distance(currentWaypoint, nextWaypoint);

        return (distanceBetweenPoints > distanceToNextWayPoint);


    }

    public void TakeDamage(int damage)
    {
        healthPoints -= damage;
        if (healthPoints <= 0) Destroy(gameObject);
    }

    public float DistanceToFinishLine()
    {
        return totalDistance + agent.remainingDistance;
    }

    private void CollectTotalDistance()
    {
        for (var i = 0; i < myWaypoints.Count - 1; i++)
        {
            var distance = Vector3.Distance(myWaypoints[i].position, myWaypoints[i + 1].position);//loops through and calculates the distance between each waypoint and add it
            totalDistance += distance;
        }
    }

    private void FaceTarget(Vector3 newTarget)
    {
        //calculate the distance from current position to the new target
        var direction = newTarget - transform.position;
        direction.y = 0; //removes vertical component
        //create a rotation that points the forward vector up the calculated direction
        var targetRotation = Quaternion.LookRotation(direction);
        //smoothly rotate from the current rotation to the target rotation at the defined speed
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    private Vector3 GetNextWayPoints()
    {
        //check if the waypoint index is beyond the last waypoint
        if (nextWayPointIndex >= myWaypoints.Count)
            //if true . return the agent's current position,effectively stopping it
            return transform.position;
        //get the current target point from the waypoints array
        var nextDestination = myWaypoints[nextWayPointIndex].position;
        //if this is not the first waypoint , calculate the distance from the previous waypoint
        if (nextWayPointIndex > 0)
        {
            var distance = Vector3.Distance(myWaypoints[nextWayPointIndex].position, myWaypoints[nextWayPointIndex - 1].position);
            //subtract this distance from the total distance
            totalDistance = totalDistance - distance;
        }

        //increment the waypoint index to move to the next waypoint on the next call
        nextWayPointIndex++;
        currentWayPointIndex=nextWayPointIndex-1;
        //return the current target point;
        return nextDestination;
    }

    public Vector3 CentrePoint()=>centerPoint.position;
    public EnemyType GetEnemyType() => enemyType;

}