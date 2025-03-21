using System.Collections;
using UnityEngine;

public class GunShooting : MonoBehaviour
{
    [SerializeField] private Transform firePoint; // Ponto de origem do tiro
    [SerializeField] private GameObject shotPrefab; // Prefab do tiro
    [SerializeField] private float timeShot = 0.2f; // Tempo entre disparos

    private bool podeAtirar = true;
    private Transform player; // Referência ao jogador

    void Start()
    {
        // Obtém a referência do jogador (supondo que o script esteja na arma)
        player = transform.root; // Pegando o objeto raiz (personagem)
    }

    void Update()
    {
        // Ajusta a rotação do firePoint para sempre apontar para frente
        firePoint.right = player.localScale.x > 0 ? Vector2.right : Vector2.left;

        if (Input.GetMouseButton(0) && podeAtirar) // Botão esquerdo do mouse
        {
            StartCoroutine(Atirar());
        }
    }

    private IEnumerator Atirar()
    {
        podeAtirar = false;

        // Instancia o tiro na posição e rotação do firePoint
        Instantiate(shotPrefab, firePoint.position, firePoint.rotation);

        yield return new WaitForSeconds(timeShot);

        podeAtirar = true;
    }
}
