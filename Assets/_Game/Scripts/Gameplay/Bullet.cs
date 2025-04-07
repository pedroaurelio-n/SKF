using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float timeLife = 2f;

    Vector3 _shootingDirection;

    void Start()
    {
        Destroy(gameObject, timeLife); // Destroi a bala ap�s um tempo
    }

    public void Setup (Vector3 direction)
    {
        _shootingDirection = direction * speed;
    }

    void Update()
    {
        transform.Translate(Time.deltaTime * _shootingDirection);
    }

    void OnTriggerEnter (Collider other)
    {
        Destroy(gameObject);
    }
}
