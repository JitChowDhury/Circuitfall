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
    [SerializeField] private WaveDetails currentWave;
    [Space]
    [SerializeField] private Transform respawn;
    [SerializeField] float spawnCooldown;
    
    private List<GameObject> enemiesToCreate;
    private float spawnTimer;

    [Header("Enemy Prefabs")] 
    [SerializeField] private GameObject basicEnemy;
    [SerializeField] private GameObject fastEnemy;

    private void Start()
    {
        enemiesToCreate = NewEnemyWave();
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0 && enemiesToCreate.Count>0)
        {
            CreateEnemy();
            spawnTimer = spawnCooldown;
        }
       
    }

    private void CreateEnemy()
    {
        GameObject randomEnemy = GetRandomEnemy();
        GameObject newEnemy=Instantiate(randomEnemy, respawn.position, Quaternion.identity);
    }

    private GameObject GetRandomEnemy()
    {
        int randomIndex = UnityEngine.Random.Range(0,enemiesToCreate.Count);
        GameObject chooseEnemy = enemiesToCreate[randomIndex];
        enemiesToCreate.Remove(chooseEnemy);
        return chooseEnemy;
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
