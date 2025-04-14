using UnityEngine;

public class GunManager : MonoBehaviour
{
    [SerializeField] private GunData defaultGun;
    [SerializeField] private SpriteRenderer gunSpriteRenderer;
    [SerializeField] private Transform firePoint;

    private GunShooting gunShooting;
    private GunData currentGunOriginal; // Referência ao ScriptableObject original
    private GunData currentGunInstance; // Cópia que usamos no jogo (runtime)

    void Start()
    {
        gunShooting = GetComponent<GunShooting>();
        EquipGun(defaultGun);
    }

    public Transform GetFirePoint()
    {
        return firePoint;
    }


    public void EquipGun(GunData newGun)
    {
        currentGunOriginal = newGun;

        // Cria uma cópia para runtime
        currentGunInstance = Instantiate(newGun);

        gunSpriteRenderer.sprite = currentGunInstance.gunSprite;
        gunShooting.UpdateGun(currentGunInstance);
    }
    public void UseAmmo()
    {
        if (currentGunInstance != defaultGun)
        {
            currentGunInstance.ammo--;

            if (currentGunInstance.ammo <= 0)
            {
                RevertToDefaultGun();
            }
        }
    }


    public void RevertToDefaultGun()
    {
        EquipGun(defaultGun);
    }
}
