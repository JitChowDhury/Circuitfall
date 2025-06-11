using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPortal : MonoBehaviour
{
    [SerializeField] private List<Waypoint> waypointsList;
    [SerializeField] float spawnCooldown;
    
    private float spawnTimer;
    [Space]
    
    private List<GameObject> enemiesToCreate = new List<GameObject>();//when you make it public it is initialized by unity and when its private u have tio initialize it
    private List<GameObject> activeEnemies = new List<GameObject>();
    private void Awake()
    {
        CollectWaypoints();
    }


    private void Update()
    {
    if(CanMakeNewEnemy())
        CreateEnemy();
       
    }

    private bool CanMakeNewEnemy()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0 && enemiesToCreate.Count>0)
         {
             spawnTimer = spawnCooldown;
             return true;
         }

        return false;

    }

    private void CreateEnemy()
    {
        GameObject randomEnemy = GetRandomEnemy();
        GameObject newEnemy=Instantiate(randomEnemy, transform.position, Quaternion.identity);

        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        enemyScript.SetupEnemy(waypointsList,this);

        activeEnemies.Add(newEnemy);
    }

    private GameObject GetRandomEnemy()
    {
        int randomIndex = UnityEngine.Random.Range(0,enemiesToCreate.Count);
        GameObject chooseEnemy = enemiesToCreate[randomIndex];
        enemiesToCreate.Remove(chooseEnemy);
        return chooseEnemy;
    }

    public void AddEnemy(GameObject enemyToAdd) => enemiesToCreate.Add(enemyToAdd);
    public List<GameObject> GetActiveEnemies() => activeEnemies;

    public void RemoveActiveEnemy(GameObject enemyToRemove)
    {
        if (activeEnemies.Contains(enemyToRemove))
            activeEnemies.Remove(enemyToRemove);
    }

    [ContextMenu("Collect Waypoints")]
    private void CollectWaypoints()//collects all the child waypoints
    {
        waypointsList = new List<Waypoint>();
        foreach (Transform child in transform)
        {
            
            Waypoint waypoint = child.GetComponent<Waypoint>();
            if(waypoint!=null)
                waypointsList.Add(waypoint);//add in waypoint list
            
        }
    }
}
