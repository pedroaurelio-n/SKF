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
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Aqui você pode aplicar o dano se o alvo tiver um script de vida
        Destroy(gameObject);
    }
}
