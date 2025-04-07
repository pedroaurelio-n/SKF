using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class GunShooting : MonoBehaviour
{
    [SerializeField] private Transform firePoint; // Ponto de origem do tiro
    [SerializeField] private Bullet shotPrefab; // Prefab do tiro
    [SerializeField] private float timeShot = 0.2f; // Tempo entre disparos

    private bool podeAtirar = true;
    private Transform player; // Refer�ncia ao jogador

    void Start()
    {
        // Obt�m a refer�ncia do jogador (supondo que o script esteja na arma)
        player = transform.root; // Pegando o objeto raiz (personagem)
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && podeAtirar) // Bot�o esquerdo do mouse
        {
            StartCoroutine(Atirar());
        }
    }

    private IEnumerator Atirar()
    {
        podeAtirar = false;

        // Instancia o tiro na posi��o e rota��o do firePoint
        Bullet bullet = Instantiate(shotPrefab, firePoint.position, Quaternion.identity, null);
        bullet.Setup(firePoint.right);

        yield return new WaitForSeconds(timeShot);

        podeAtirar = true;
    }
}
