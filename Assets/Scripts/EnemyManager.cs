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

    [SerializeField] private WaveDetails[] levelWaves;
    private int waveIndex;
     
    [Header("Enemy Prefabs")] 
    [SerializeField] private GameObject basicEnemy;
    [SerializeField] private GameObject fastEnemy;
    
    private List<EnemyPortal> enemyPortals;



    private void Awake()
    {
        enemyPortals = new List<EnemyPortal>(FindObjectsByType<EnemyPortal>(FindObjectsSortMode.InstanceID));
        //gets all the enemy portals in scene
    }

    private void Start()
    {
        SetupNextWave();
    }

    [ContextMenu(("Setup Next Wave"))]
    private void SetupNextWave()//equally distribute enemy
    {
        List<GameObject> newEnemies = NewEnemyWave();
        int portalIndex = 0;

        if (newEnemies == null)
        {
            Debug.LogWarning("I had no wave to setup");
            return;
        }
        for (int i = 0; i < newEnemies.Count; i++)
        {
            GameObject enemyToAdd = newEnemies[i];
            EnemyPortal portalToReceiveEnemy = enemyPortals[portalIndex];
            portalToReceiveEnemy.AddEnemy(enemyToAdd);//add to createenemylist in enemyportal
            portalIndex++;
            if (portalIndex >= enemyPortals.Count)
            {
                portalIndex = 0;
            }
        }
    }
    private List<GameObject> NewEnemyWave()
    {
        if (waveIndex >= levelWaves.Length)
        {
            Debug.LogWarning("You Have no more wave left");
            return null;
        }
        List<GameObject> newEnemyList = new List<GameObject>();
        for (int i = 0; i < levelWaves[waveIndex].basicEnemy; i++)
        {
            newEnemyList.Add(basicEnemy);
        }
        
        for (int i = 0; i < levelWaves[waveIndex].fastEnemy; i++)
        {
            newEnemyList.Add(fastEnemy);
        }

        waveIndex++;
        return newEnemyList;
    }
}
