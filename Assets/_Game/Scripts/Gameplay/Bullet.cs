using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    private int damage;
    private Vector2 direction;

    public void Setup(Vector2 dir, int dmg = 1)
    {
        Debug.Log(dir);
        direction = dir.normalized;
        damage = dmg;
        
        Destroy(gameObject, 12f);
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
            Destroy(gameObject);
        }
    }

    internal void SetDirection(Vector3 right)
    {
        throw new NotImplementedException();
    }
}
