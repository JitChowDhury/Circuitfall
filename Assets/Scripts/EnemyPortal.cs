using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPortal : MonoBehaviour
{
    [SerializeField] private List<Waypoint> waypointsList;
    [SerializeField] private float spawnCooldown;
    private float spawnTimer;

    private List<GameObject> enemiesToCreate = new List<GameObject>();
    private List<GameObject> activeEnemies = new List<GameObject>();

    private void Awake()
    {
        // Initialize waypoints
        CollectWaypoints();
    }

    private void Update()
    {
        // Spawn new enemy if conditions are met
        if (CanMakeNewEnemy())
        {
            CreateEnemy();
        }
    }

    // Checks if a new enemy can be spawned
    private bool CanMakeNewEnemy()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0 && enemiesToCreate.Count > 0)
        {
            spawnTimer = spawnCooldown;
            return true;
        }
        return false;
    }

    // Instantiates a new enemy
    private void CreateEnemy()
    {
        GameObject randomEnemy = GetRandomEnemy();
        GameObject newEnemy = Instantiate(randomEnemy, transform.position, Quaternion.identity);

        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        enemyScript.SetupEnemy(waypointsList, this);

        activeEnemies.Add(newEnemy);
    }

    // Selects a random enemy from the queue
    private GameObject GetRandomEnemy()
    {
        int randomIndex = UnityEngine.Random.Range(0, enemiesToCreate.Count);
        GameObject chosenEnemy = enemiesToCreate[randomIndex];
        enemiesToCreate.Remove(chosenEnemy);
        return chosenEnemy;
    }

    // Adds an enemy to the spawn queue
    public void AddEnemy(GameObject enemyToAdd) => enemiesToCreate.Add(enemyToAdd);

    // Returns list of active enemies
    public List<GameObject> GetActiveEnemies() => activeEnemies;

    // Removes an enemy from active list
    public void RemoveActiveEnemy(GameObject enemyToRemove)
    {
        if (activeEnemies.Contains(enemyToRemove))
        {
            activeEnemies.Remove(enemyToRemove);
        }
    }

    // Collects waypoints from child objects
    [ContextMenu("Collect Waypoints")]
    private void CollectWaypoints()
    {
        waypointsList = new List<Waypoint>();
        foreach (Transform child in transform)
        {
            Waypoint waypoint = child.GetComponent<Waypoint>();
            if (waypoint != null)
            {
                waypointsList.Add(waypoint);
            }
        }
    }
}