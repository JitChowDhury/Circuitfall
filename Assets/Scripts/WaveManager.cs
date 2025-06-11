using System;
using UnityEngine;
using System.Collections.Generic;

// Defines wave configuration
[System.Serializable]
public class WaveDetails
{
    public int basicEnemy;
    public int fastEnemy;
}

public class WaveManager : MonoBehaviour
{
    public bool waveCompleted;
    public float timeBetweenWaves = 10;
    public float waveTimer;
    [SerializeField] private WaveDetails[] levelWaves;
    private int waveIndex;//tracks current wave
    private float checkInterval = 0.5f;
    private float nextCheckTime;

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject basicEnemy;
    [SerializeField] private GameObject fastEnemy;

    private List<EnemyPortal> enemyPortals;

    private void Awake()
    {
        // Find all EnemyPortal objects in the scene
        enemyPortals = new List<EnemyPortal>(FindObjectsByType<EnemyPortal>(FindObjectsSortMode.InstanceID));
    }

    private void Start()
    {
        // Initialize the first wave
        SetupNextWave();
    }

    private void Update()
    {
        HandleWaveCompletion();
        HandleWaveTiming();
    }

    // Manages wave timing logic
    private void HandleWaveTiming()
    {
        if (waveCompleted)
        {
            waveTimer -= Time.deltaTime;
            if (waveTimer <= 0) SetupNextWave();
        }
    }

    // Checks if the current wave is complete
    private void HandleWaveCompletion()
    {
        if (!ReadyToCheck()) return;
        if (!waveCompleted && AllEnemiesDefeated())
        {
            waveCompleted = true;
            waveTimer = timeBetweenWaves;
        }
    }

    // Forces the next wave if all enemies are defeated
    public void ForceNextWave()
    {
        if (!AllEnemiesDefeated()) return;
        SetupNextWave();
    }

    // Sets up the next wave of enemies
    [ContextMenu("Setup Next Wave")]
    private void SetupNextWave()
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
            portalToReceiveEnemy.AddEnemy(enemyToAdd);
            portalIndex++;
            if (portalIndex >= enemyPortals.Count)
            {
                portalIndex = 0;
            }
        }

        waveCompleted = false;
    }

    // Creates a new wave of enemies
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

    // Checks if all enemies are defeated
    private bool AllEnemiesDefeated()
    {
        foreach (EnemyPortal portal in enemyPortals)
        {
            if (portal.GetActiveEnemies().Count > 0)
            {
                return false;
            }
        }
        return true;
    }

    // Determines if it's time to check wave status
    private bool ReadyToCheck()
    {
        if (Time.time >= nextCheckTime)
        {
            nextCheckTime = Time.time + checkInterval;
            return true;
        }
        return false;
    }
}