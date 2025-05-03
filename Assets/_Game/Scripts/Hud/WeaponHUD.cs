using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class WeaponHUD : MonoBehaviour
{
    [Header("UI da Flor de Armas")]
    [SerializeField] private Image primaryIcon;
    [SerializeField] private Image secondaryLeft;
    [SerializeField] private Image secondaryRight;
    [SerializeField] private TMP_Text ammoText;

    [Header("Referência ao GunManager")]
    [SerializeField] private GunManager gunManager;

    private List<GunData> ownedGuns = new List<GunData>();
    private int currentIndex = 0;

    private readonly Vector3 bigScale = Vector3.one;
    private readonly Vector3 smallScale = Vector3.one;

    IEnumerator Start()
    {
        yield return new WaitUntil(() => gunManager.CurrentGun != null); // espera a arma ser equipada
        AddGunToHUD(gunManager.CurrentGun.data);
    }

    void Update()
    {
        var asset = gunManager.CurrentGun?.data;
        if (asset != null && !ownedGuns.Contains(asset))
        {
            AddGunToHUD(asset);
        }

        if (ownedGuns.Count > 1)
        {
            if (Input.GetKeyDown(KeyCode.Q)) SwitchWeapon(-1);
            if (Input.GetKeyDown(KeyCode.E)) SwitchWeapon(1);
        }

        UpdateHUD();
    }

    public void ForceUpdate()
    {
        UpdateHUD();
    }


    private void AddGunToHUD(GunData newGun)
    {
        ownedGuns.Add(newGun);
        currentIndex = ownedGuns.IndexOf(newGun);

        if (newGun == gunManager.smgData)
            secondaryLeft.gameObject.SetActive(true);
        else if (newGun == gunManager.shotgunData)
            secondaryRight.gameObject.SetActive(true);
    }

    private void SwitchWeapon(int dir)
    {
        int attempts = ownedGuns.Count;
        do
        {
            currentIndex = (currentIndex + dir + ownedGuns.Count) % ownedGuns.Count;
            attempts--;
        }
        while (ownedGuns[currentIndex] == null && attempts > 0);

        var nextGun = ownedGuns[currentIndex];
        if (nextGun != null)
            gunManager.EquipGun(nextGun);
    }

    private void UpdateHUD()
    {
        if (ownedGuns.Count == 0) return;

        GunData currentGun = ownedGuns[currentIndex];
        GunData leftGun = ownedGuns.Count > 1 ? ownedGuns[(currentIndex - 1 + ownedGuns.Count) % ownedGuns.Count] : null;
        GunData rightGun = ownedGuns.Count > 2 ? ownedGuns[(currentIndex + 1) % ownedGuns.Count] : null;

        primaryIcon.sprite = currentGun?.hudSprite;
        secondaryLeft.sprite = leftGun?.hudSprite;
        secondaryRight.sprite = rightGun?.hudSprite;

        primaryIcon.transform.localScale = bigScale;
        secondaryLeft.transform.localScale = currentGun == gunManager.smgData ? bigScale : smallScale;
        secondaryRight.transform.localScale = currentGun == gunManager.shotgunData ? bigScale : smallScale;

        if (gunManager.CurrentGun != null)
        {
            int ammo = gunManager.CurrentGun.ammo;
            ammoText.text = ammo > 0 ? ammo.ToString() : "∞";
        }
        else
        {
            ammoText.text = "-";
        }
    }
}