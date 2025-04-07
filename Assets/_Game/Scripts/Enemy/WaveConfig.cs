using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveConfig", menuName = "WaveConfig")]
public class WaveConfig : ScriptableObject
{
    [System.Serializable]
    public class EnemyGroup
    {
        public Enemy EnemyPrefab;
        public int TotalCount;
    }

    public List<EnemyGroup> EnemyGroups;
    public Vector2Int TotalSpawnRange;
}
