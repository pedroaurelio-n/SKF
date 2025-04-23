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
        // cria um AudioSource automáticamente
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
        // instancia uma cópia em runtime
        currentGunInstance = Instantiate(newGun);
        gunSpriteRenderer.sprite = currentGunInstance.gunSprite;

        // avisa o GunShooting da nova arma
        gunShooting.UpdateGun(currentGunInstance);
    }

    // este método será chamado no disparo
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
