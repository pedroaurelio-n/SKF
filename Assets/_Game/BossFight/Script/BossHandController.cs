using System.Collections;
using UnityEngine;

public class BossHandController : MonoBehaviour
{

    [Header("Laser")]
    [SerializeField] private GameObject laserObject;
    [SerializeField] private float laserDuration = 1.5f;

    [Header("Referências das mãos")]
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;

    [Header("Pontos de movimento")]
    [SerializeField] private Transform[] handPositions; // 5 posições

    [Header("Configuração")]
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private float delayBetweenMoves = 0.3f;
    [SerializeField] private int movementsPerCycle = 5;

    private int currentMoveIndex = 0;

    void Start()
    {
        StartCoroutine(RepeatHandCycle());
    }

    IEnumerator RepeatHandCycle()
    {
        while (true)
        {
            for (int i = 0; i < movementsPerCycle; i++)
            {
                Vector3 targetPos = handPositions[currentMoveIndex].position;

                StartCoroutine(MoveHandToPosition(leftHand, targetPos));
                StartCoroutine(MoveHandToPosition(rightHand, targetPos));

                currentMoveIndex = (currentMoveIndex + 1) % handPositions.Length;

                yield return new WaitForSeconds(moveDuration + delayBetweenMoves);
            }

            yield return StartCoroutine(LaserAttack());
        }
    }

    IEnumerator LaserAttack()
    {
        // Ativa o laser visualmente
        laserObject.SetActive(true);

        // Você pode colocar aqui uma animação de carga ou som se quiser
        yield return new WaitForSeconds(laserDuration);

        // Desativa o laser
        laserObject.SetActive(false);
    }


    IEnumerator MoveHandToPosition(Transform hand, Vector3 target)
    {
        Vector3 start = hand.position;
        float elapsed = 0;

        while (elapsed < moveDuration)
        {
            hand.position = Vector3.Lerp(start, target, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        hand.position = target;
    }
}
