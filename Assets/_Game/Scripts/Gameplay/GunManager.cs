using UnityEngine;

public class GunManager : MonoBehaviour
{
    public SpriteRenderer gunSpriteRenderer;
    public Transform firePoint;
    public GunData defaultGun;

    private GunData currentGun;
    private GunShooting gunShooting;

    private void Start()
    {
        gunShooting = GetComponent<GunShooting>();
        EquipGun(defaultGun);
    }

    public void EquipGun(GunData newGun)
    {
        currentGun = newGun;
        gunSpriteRenderer.sprite = newGun.gunSprite;
        gunShooting.UpdateGun(newGun);
    }

    public void UseAmmo()
    {
        if (currentGun == defaultGun) return;

        currentGun.ammo--;
        if (currentGun.ammo <= 0)
        {
            EquipGun(defaultGun);
        }
    }
}
