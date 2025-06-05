using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    [SerializeField] private GunData defaultGun;
    [SerializeField] private SpriteRenderer gunSpriteRenderer;
    [SerializeField] private Transform firePoint;

    private GunShooting gunShooting;
    private AudioSource audioSource;

    private GunRuntime currentGun;

    // Mantemos um dicion�rio para as inst�ncias �nicas por arma
    private Dictionary<GunData, GunRuntime> gunInstances = new Dictionary<GunData, GunRuntime>();

    public GunData smgData;
    public GunData shotgunData;

    public GunRuntime CurrentGun => currentGun;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;

        // Inicializar a arma padr�o no dicion�rio
        gunInstances[defaultGun] = new GunRuntime(defaultGun);
    }

    void Start()
    {
        gunShooting = GetComponent<GunShooting>();
        EquipGun(defaultGun);
    }

    public void EquipGun(GunData newGun)
    {
        // Se a arma j� existe, s� equipa
        if (gunInstances.TryGetValue(newGun, out var runtime))
        {
            EquipGun(runtime);
        }
        else
        {
            // Se for nova, cria inst�ncia, adiciona e equipa
            GunRuntime newRuntime = new GunRuntime(newGun);
            gunInstances[newGun] = newRuntime;
            EquipGun(newRuntime);
        }
    }

    // Recarrega a arma correspondente se j� existe
    public void CollectGun(GunData collectedGun)
    {
        if (gunInstances.TryGetValue(collectedGun, out var runtime))
        {
            // Recarrega apenas a arma do tipo correspondente
            runtime.currentAmmo = collectedGun.magazineSize;
            runtime.reserveAmmo = collectedGun.maxReserveAmmo;
        }
        else
        {
            // Cria nova inst�ncia com muni��o cheia
            GunRuntime newRuntime = new GunRuntime(collectedGun);
            gunInstances[collectedGun] = newRuntime;
        }

        // Equipa ao coletar
        EquipGun(collectedGun);
    }

    private void EquipGun(GunRuntime runtimeToEquip)
    {
        currentGun = runtimeToEquip;
        gunSpriteRenderer.sprite = currentGun.data.gunSprite;
        gunShooting.UpdateGun(currentGun);
    }

    public void EquipDefaultGun() => EquipGun(defaultGun);
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
