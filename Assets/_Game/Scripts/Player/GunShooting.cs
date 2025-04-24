using System.Collections;
using UnityEngine;

public class GunShooting : MonoBehaviour
{
    private GunData currentGun;
    private bool podeAtirar = true;
    private GunManager gunManager;

    void Start()
    {
        gunManager = GetComponent<GunManager>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && podeAtirar && currentGun != null && currentGun.ammo > 0)
        {
            StartCoroutine(Atirar());
        }
    }

    private IEnumerator Atirar()
    {
        if (currentGun == null)
        {
            Debug.LogWarning("Arma não atribuída!");
            yield break;
        }

        podeAtirar = false;

        // Instancia o projétil
        Bullet bullet = Instantiate(currentGun.bulletPrefab, gunManager.GetFirePoint().position, Quaternion.Euler(0, 0, gunManager.GetFirePoint().eulerAngles.z));
        bullet.Setup(gunManager.GetFirePoint().right, currentGun.damage);

        // Som
        gunManager.PlayFireSound();

        // Consome munição
        currentGun.ammo--;

        // Verifica se acabou a munição
        if (currentGun.ammo <= 0)
        {
            Debug.Log("Munição esgotada. Trocando para arma padrão.");
            gunManager.EquipDefaultGun();
        }

        yield return new WaitForSeconds(currentGun.fireRate);
        podeAtirar = true;
    }

    public void UpdateGun(GunData gunData)
    {
        currentGun = gunData;
    }
}
