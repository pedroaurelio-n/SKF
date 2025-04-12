using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public enum PlatformType { Automatic, OnPlayerTouch }
    public PlatformType platformType = PlatformType.Automatic;

    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Vector3 target;
    private int playersOnPlatform = 0;

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
        else if (platformType == PlatformType.OnPlayerTouch && playersOnPlatform > 0)
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
            if (!other.transform.IsChildOf(transform))
            {
                other.transform.SetParent(transform);
                playersOnPlatform++;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("OnTriggerExit: Player saiu.");
            if (other.transform.IsChildOf(transform))
            {
                other.transform.SetParent(null);
                playersOnPlatform = Mathf.Max(0, playersOnPlatform - 1);
            }
        }
    }
}
