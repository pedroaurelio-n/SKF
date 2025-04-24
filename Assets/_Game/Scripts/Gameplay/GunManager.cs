using UnityEngine;

public class GunManager : MonoBehaviour
{
    [SerializeField] private GunData defaultGun;
    [SerializeField] private SpriteRenderer gunSpriteRenderer;
    [SerializeField] private Transform firePoint;

    private GunShooting gunShooting;
    private GunData currentGunInstance;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 2D sound
    }

    void Start()
    {
        gunShooting = GetComponent<GunShooting>();
        EquipGun(defaultGun);
    }

    public Transform GetFirePoint() => firePoint;

    public void EquipGun(GunData newGun)
    {
        currentGunInstance = Instantiate(newGun); // cria uma cópia independente
        gunSpriteRenderer.sprite = currentGunInstance.gunSprite;
        gunShooting.UpdateGun(currentGunInstance);
    }

    public void EquipDefaultGun()
    {
        EquipGun(defaultGun);
    }

    public void PlayFireSound()
    {
        if (currentGunInstance.fireSound != null)
        {
            audioSource.pitch = currentGunInstance.firePitch;
            audioSource.PlayOneShot(
                currentGunInstance.fireSound,
                currentGunInstance.fireVolume
            );
        }
    }
}
