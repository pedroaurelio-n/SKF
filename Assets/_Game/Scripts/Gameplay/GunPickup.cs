using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public GunData weaponData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GunManager manager = other.GetComponentInChildren<GunManager>();
            if (manager != null)
            {
                manager.CollectGun(weaponData); // <- Usa o método correto
                Destroy(gameObject);
            }
        }
    }
}
