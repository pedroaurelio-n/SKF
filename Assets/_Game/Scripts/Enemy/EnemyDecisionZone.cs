using UnityEngine;

public class EnemyDecisionZone : MonoBehaviour
{
    [field: SerializeField] public bool Jump { get; private set; }
    [field: SerializeField] public bool FromRight { get; private set; }
}