using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

// Enum to define different enemy types
public enum EnemyType
{
    Basic,
    Fast,
    None
}

public class Enemy : MonoBehaviour, IDamagable
{
    private EnemyPortal myPortal;
    public int healthPoints = 4;
    [SerializeField] private EnemyType enemyType;
    [FormerlySerializedAs("myNewWaypoints")] 
    [Header("Movement")] 
    [SerializeField] private List<Transform> myWaypoints;

    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private Transform centerPoint;
    private NavMeshAgent agent;
    private int nextWayPointIndex;
    private int currentWayPointIndex;
    private float totalDistance;

    private void Awake()
    {
        // Initialize NavMeshAgent and configure settings
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.avoidancePriority = (int)(agent.speed * 10);
    }

    // Sets up enemy with waypoints and portal reference
    public void SetupEnemy(List<Waypoint> newWaypoints, EnemyPortal myNewPortal)
    {
        myWaypoints = new List<Transform>();
        foreach (var point in newWaypoints)
        {
            myWaypoints.Add(point.transform);
        }
        CollectTotalDistance();
        myPortal = myNewPortal;
    }

    private void Update()
    {
        // Rotate to face the current steering target
        FaceTarget(agent.steeringTarget);
        // Check if the agent should move to the next waypoint
        if (ShouldChangeWaypoint())
        {
            agent.SetDestination(GetNextWayPoints());
        }
    }

    // Determines if the agent should switch to the next waypoint
    private bool ShouldChangeWaypoint()
    {
        if (nextWayPointIndex >= myWaypoints.Count) return false;

        if (agent.remainingDistance < 0.5) return true;

        Vector3 currentWaypoint = myWaypoints[currentWayPointIndex].position;
        Vector3 nextWaypoint = myWaypoints[nextWayPointIndex].position;

        float distanceToNextWayPoint = Vector3.Distance(transform.position, nextWaypoint);
        float distanceBetweenPoints = Vector3.Distance(currentWaypoint, nextWaypoint);

        return distanceBetweenPoints > distanceToNextWayPoint;
    }

    // Applies damage to the enemy
    public void TakeDamage(int damage)
    {
        healthPoints -= damage;
        if (healthPoints <= 0) Die();
    }

    // Calculates distance to the finish line
    public float DistanceToFinishLine()
    {
        return totalDistance + agent.remainingDistance;
    }

    // Computes total distance between waypoints
    private void CollectTotalDistance()
    {
        for (var i = 0; i < myWaypoints.Count - 1; i++)
        {
            var distance = Vector3.Distance(myWaypoints[i].position, myWaypoints[i + 1].position);
            totalDistance += distance;
        }
    }

    // Rotates enemy to face the target direction
    private void FaceTarget(Vector3 newTarget)
    {
        var direction = newTarget - transform.position;
        direction.y = 0;
        var targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    // Retrieves the next waypoint position
    private Vector3 GetNextWayPoints()
    {
        if (nextWayPointIndex >= myWaypoints.Count)
        {
            return transform.position;
        }

        var nextDestination = myWaypoints[nextWayPointIndex].position;
        if (nextWayPointIndex > 0)
        {
            var distance = Vector3.Distance(myWaypoints[nextWayPointIndex].position, myWaypoints[nextWayPointIndex - 1].position);
            totalDistance -= distance;
        }

        nextWayPointIndex++;
        currentWayPointIndex = nextWayPointIndex - 1;
        return nextDestination;
    }

    // Returns the center point position
    public Vector3 CentrePoint() => centerPoint.position;

    // Returns the enemy type
    public EnemyType GetEnemyType() => enemyType;

    // Handles enemy death
    private void Die()
    {
        myPortal.RemoveActiveEnemy(gameObject);
        Destroy(gameObject);
    }
}