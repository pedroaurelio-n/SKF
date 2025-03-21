using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSystem : MonoBehaviour
{
    [SerializeField] private Transform player; // Referência ao jogador
    [SerializeField] private float distanceMin = 0.3f; // Distância mínima da arma ao jogador
    [SerializeField] private float distanceMax = 1f; // Distância máxima da arma ao jogador
    [SerializeField] private float smoothMove = 10f; // Controla a suavidade da transição
    [SerializeField] private SpriteRenderer srGun; // Sprite da arma
    [SerializeField] private Transform firePoint; // Ponto de disparo

    private Vector2 mousePosi;
    private Vector2 dirArma;
    private float angle;

    [SerializeField] private float limiteInferiorFisico = -0.2f; // Posição mínima da arma relativa ao player

    void Update()
    {
        mousePosi = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        // Calcula direção da arma em relação ao player
        dirArma = (mousePosi - (Vector2)player.position).normalized;

        // Calcula a posição alvo da arma respeitando os limites de distância
        float distanciaAtual = Mathf.Clamp(Vector2.Distance(player.position, mousePosi), distanceMin, distanceMax);
        Vector2 targetPosition = (Vector2)player.position + dirArma * distanciaAtual;

        // Impede que a arma ultrapasse fisicamente a parte inferior do jogador
        if (targetPosition.y < player.position.y + limiteInferiorFisico)
        {
            targetPosition.y = player.position.y + limiteInferiorFisico;
        }

        // Move a arma suavemente para a posição desejada
        transform.position = Vector2.Lerp(transform.position, targetPosition, Time.deltaTime * smoothMove);

        // Calcula o ângulo da arma
        angle = Mathf.Atan2(dirArma.y, dirArma.x) * Mathf.Rad2Deg;

        // Rotaciona a arma de forma mais precisa e suave
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * smoothMove * 100f);

        // Ajusta espelhamento corretamente
        bool mirandoParaEsquerda = mousePosi.x < player.position.x;
        srGun.flipY = mirandoParaEsquerda;

        // Ajusta a rotação do firePoint para que fique sempre apontando para frente
        firePoint.right = mirandoParaEsquerda ? Vector2.left : Vector2.right;
    }
}
