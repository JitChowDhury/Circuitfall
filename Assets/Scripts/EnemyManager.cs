using System;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WaveDetails
{
    public int basicEnemy;
    public int fastEnemy;
}
public class EnemyManager : MonoBehaviour
{
    public List<EnemyPortal> enemyPortals;
    [SerializeField] private WaveDetails currentWave;
     
    [Header("Enemy Prefabs")] 
    [SerializeField] private GameObject basicEnemy;
    [SerializeField] private GameObject fastEnemy;


    private void Awake()
    {
        enemyPortals = new List<EnemyPortal>(FindObjectsByType<EnemyPortal>(FindObjectsSortMode.InstanceID));
        //gets all the enemy portals in scene
    }

    [ContextMenu(("Setup Next Wave"))]
    private void SetupNextWave()//equally distribute enemy
    {
        List<GameObject> newEnemies = NewEnemyWave();
        int portalIndex = 0;
        for (int i = 0; i < newEnemies.Count; i++)
        {
            GameObject enemyToAdd = newEnemies[i];
            EnemyPortal portalToReceiveEnemy = enemyPortals[portalIndex];
            portalToReceiveEnemy.GetEnemyList().Add(enemyToAdd);//add to createenemylist in enemyportal
            portalIndex++;
            if (portalIndex >= enemyPortals.Count)
            {
                portalIndex = 0;
            }
        }
    }
    private List<GameObject> NewEnemyWave()
    {
        List<GameObject> newEnemyList = new List<GameObject>();
        for (int i = 0; i < currentWave.basicEnemy; i++)
        {
            newEnemyList.Add(basicEnemy);
        }
        
        for (int i = 0; i < currentWave.fastEnemy; i++)
        {
            newEnemyList.Add(fastEnemy);
        }

        return newEnemyList;
    }
}
