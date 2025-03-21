using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float timeLife = 2f;

    void Start()
    {
        Destroy(gameObject, timeLife); // Destroi a bala após um tempo
    }

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
}
