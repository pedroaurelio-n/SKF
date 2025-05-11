using UnityEngine;

public class OneWayPlatform3D : MonoBehaviour
{
    internal float heightTolerance;

    // private Collider platformCollider;
    //
    // void Awake()
    // {
    //     platformCollider = transform.parent.GetComponent<Collider>();
    // }
    //
    // void OnTriggerStay(Collider other)
    // {
    //     if (other.TryGetComponent<Player>(out Player player))
    //     {
    //         if (!player.CharacterController.Motor.GroundingStatus.IsStableOnGround)
    //         {
    //             Physics.IgnoreCollision(other, platformCollider, true);
    //         }
    //         else
    //         {
    //             Physics.IgnoreCollision(other, platformCollider, false);
    //         }
    //     }
    // }
    //
    // void OnTriggerExit(Collider other)
    // {
    //     if (other.TryGetComponent<Player>(out Player player))
    //     {
    //         Physics.IgnoreCollision(other, platformCollider, false);
    //     }
    // }
    public static bool IgnoreOneWayPlatformsThisFrame { get; internal set; }
}
