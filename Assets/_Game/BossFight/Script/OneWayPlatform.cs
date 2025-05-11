using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    [Tooltip("Altura da toler�ncia para permitir atravessar a plataforma")]
    public float heightTolerance = 0.05f;

    public static bool IgnoreOneWayPlatformsThisFrame;

    private void LateUpdate()
    {
        // Resetamos no fim do frame
        IgnoreOneWayPlatformsThisFrame = false;
    }
}
