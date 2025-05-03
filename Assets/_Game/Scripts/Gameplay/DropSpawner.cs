using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropItem
{
    public GameObject prefab;
    [Range(0, 100)] public float dropChance; // Chance em %
}

public class DropSpawner : MonoBehaviour
{
    [Header("Itens que podem ser dropados")]
    [SerializeField] private List<DropItem> possibleDrops = new List<DropItem>();

    [Header("Altura do drop para não colidir com o chão")]
    [SerializeField] private float dropOffsetY = 0.5f;

    public void TrySpawnDrop()
    {
        float roll = Random.Range(0f, 100f);
        float accumulatedChance = 0f;

        foreach (DropItem item in possibleDrops)
        {
            accumulatedChance += item.dropChance;
            if (roll <= accumulatedChance)
            {
                Vector3 dropPosition = transform.position + Vector3.up * dropOffsetY;
                Instantiate(item.prefab, dropPosition, Quaternion.identity);
                return; // Dropou um item, então para
            }
        }
    }
}
