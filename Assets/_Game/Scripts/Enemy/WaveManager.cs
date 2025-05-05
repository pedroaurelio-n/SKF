using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] float delayBetweenWaves;

    List<Enemy> _activeEnemies = new();
    int _currentWaveIndex;
    bool _waveInProgress;

    void Start()
    {
        StartCoroutine(RunWaves());
    }

    IEnumerator RunWaves()
    {
        yield return new WaitForSeconds(delayBetweenWaves);

        while (_currentWaveIndex < waveConfigs.Count)
        {
            _waveInProgress = true;
            WaveConfig wave = waveConfigs[_currentWaveIndex];

            EventManager.TriggerWaveStarted(_currentWaveIndex + 1, waveConfigs.Count);

            yield return StartCoroutine(HandleWave(wave));

            EventManager.TriggerWaveDefeated(_currentWaveIndex + 1);
            _currentWaveIndex++;

            if (_currentWaveIndex < waveConfigs.Count)
                yield return new WaitForSeconds(delayBetweenWaves);
        }

        EventManager.TriggerAllWavesDefeated();
    }

    IEnumerator HandleWave(WaveConfig wave)
    {
        // Build full enemy pool
        List<Enemy> enemyPool = new();
        foreach (WaveConfig.EnemyGroup group in wave.EnemyGroups)
        {
            for (int i = 0; i < group.TotalCount; i++)
                enemyPool.Add(group.EnemyPrefab);
        }

        System.Random rng = new(); // C# System RNG for better randomness than UnityEngine.Random
        while (enemyPool.Count > 0)
        {
            int spawnCount = rng.Next(wave.TotalSpawnRange.x, wave.TotalSpawnRange.y + 1);
            spawnCount = Mathf.Min(spawnCount, enemyPool.Count); // Clamp to pool size

            for (int i = 0; i < spawnCount; i++)
            {
                int index = rng.Next(0, enemyPool.Count);
                Enemy prefab = enemyPool[index];
                enemyPool.RemoveAt(index);

                SpawnEnemy(prefab);
                yield return new WaitForSeconds(0.2f); // Optional: staggered spawns
            }

            // Wait until all spawned enemies are dead
            yield return new WaitUntil(() => _activeEnemies.Count == 0);
            _waveInProgress = false;

            yield return new WaitForSeconds(delayBetweenWaves * 0.5f);
        }
    }

    void SpawnEnemy(Enemy prefab)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Enemy enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        _activeEnemies.Add(enemy);

        if (enemy != null)
            enemy.OnDeath += () => OnEnemyDeath(enemy);
    }

    void OnEnemyDeath(Enemy enemy)
    {
        _activeEnemies.Remove(enemy);
    }
}