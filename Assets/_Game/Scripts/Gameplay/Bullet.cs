using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    private int damage;
    private Vector2 direction;

    public void Setup(Vector2 dir, int dmg = 1)
    {
        direction = dir.normalized;
        damage = dmg;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Tenta pegar o BossHealth (ou outro script de vida)
        if (other.TryGetComponent<BossHealth>(out BossHealth boss))
        {
            boss.TakeDamage(damage);
        }

        // Você pode adicionar aqui verificação para outros inimigos, se quiser
        // if (other.TryGetComponent<EnemyHealth>(out var enemy)) enemy.TakeDamage(damage);

        Destroy(gameObject);
    }
}
