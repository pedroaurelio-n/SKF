using System;
using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Enemy enemyPrefab;
    [SerializeField] float resetTimer;

    Enemy _enemy;
    bool _hasSpawned;
    WaitForSeconds _waitForReset;

    void Awake ()
    {
        _enemy = Instantiate(enemyPrefab, transform);
        _waitForReset = new WaitForSeconds(resetTimer);
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine ()
    {
        while (true)
        {
            if (_enemy.gameObject.activeInHierarchy)
                yield return _waitForReset;
            
            _enemy.gameObject.SetActive(true);
            _enemy.Reset();
            yield return _waitForReset;
        }
    }
}