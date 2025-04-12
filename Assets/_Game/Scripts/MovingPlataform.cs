using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public enum PlatformType { Automatic, OnPlayerTouch }
    public PlatformType platformType = PlatformType.Automatic;

    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Vector3 target;
    private bool isPlayerOnPlatform = false;

    void Start()
    {
        target = pointB.position;
    }

    void Update()
    {
        if (platformType == PlatformType.Automatic)
        {
            MovePlatform();
        }
        else if (platformType == PlatformType.OnPlayerTouch && isPlayerOnPlatform)
        {
            MovePlatform();
        }
    }

    void MovePlatform()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            target = (target == pointA.position) ? pointB.position : pointA.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("OnTriggerEnter: Player entrou.");
            isPlayerOnPlatform = true;
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isPlayerOnPlatform)
        {
            Debug.Log("OnTriggerStay: Player detectado.");
            isPlayerOnPlatform = true;
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("OnTriggerExit: Player saiu.");
            isPlayerOnPlatform = false;
            other.transform.SetParent(null);
        }
    }
}
