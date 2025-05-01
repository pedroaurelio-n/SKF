using UnityEngine;

public class GunManager : MonoBehaviour
{
    [SerializeField] private GunData defaultGun;
    [SerializeField] private SpriteRenderer gunSpriteRenderer;
    [SerializeField] private Transform firePoint;

    private GunShooting gunShooting;
    private AudioSource audioSource;

    private GunRuntime currentGun;
    public GunData smgData;
    public GunData shotgunData;

    public GunRuntime CurrentGun => currentGun;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    void Start()
    {
        gunShooting = GetComponent<GunShooting>();
        EquipGun(defaultGun);
    }

    public void EquipGun(GunData newGun)
    {
        currentGun = new GunRuntime(newGun);
        gunSpriteRenderer.sprite = newGun.gunSprite;
        gunShooting.UpdateGun(currentGun);
    }

    public void EquipDefaultGun()
    {
        EquipGun(defaultGun);
    }

    public Transform GetFirePoint() => firePoint;

    public void PlayFireSound()
    {
        if (currentGun?.data.fireSound != null)
        {
            audioSource.pitch = currentGun.data.firePitch;
            audioSource.PlayOneShot(currentGun.data.fireSound, currentGun.data.fireVolume);
        }
    }
}