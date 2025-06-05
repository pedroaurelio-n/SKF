using UnityEngine;

public class GunManager : MonoBehaviour
{
    [SerializeField] private GunData defaultGun;
    [SerializeField] private SpriteRenderer gunSpriteRenderer;
    [SerializeField] private Transform firePoint;

    private GunShooting gunShooting;
    private AudioSource audioSource;

    private GunRuntime currentGun;

    // Agora armazenamos as instâncias runtime
    private GunRuntime defaultRuntime;
    private GunRuntime smgRuntime;
    private GunRuntime shotgunRuntime;

    public GunData smgData;
    public GunData shotgunData;

    public GunRuntime CurrentGun => currentGun;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;

        // Instanciar todas as armas com base nos GunData
        defaultRuntime = new GunRuntime(defaultGun);
        smgRuntime = new GunRuntime(smgData);
        shotgunRuntime = new GunRuntime(shotgunData);
    }

    void Start()
    {
        gunShooting = GetComponent<GunShooting>();
        EquipGun(defaultRuntime);
    }

    public void EquipGun(GunRuntime newGun)
    {
        currentGun = newGun;
        gunSpriteRenderer.sprite = newGun.data.gunSprite;
        gunShooting.UpdateGun(currentGun);
    }

    public void EquipGun(GunData newGun)
    {
        if (newGun == defaultGun) EquipGun(defaultRuntime);
        else if (newGun == smgData) EquipGun(smgRuntime);
        else if (newGun == shotgunData) EquipGun(shotgunRuntime);
        else EquipGun(new GunRuntime(newGun)); // Fallback para armas novas
    }

    public void EquipDefaultGun() => EquipGun(defaultRuntime);

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
