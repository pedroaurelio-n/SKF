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
        if (Input.GetMouseButton(0) && podeAtirar && currentGun != null)
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
        Bullet bullet = Instantiate(currentGun.bulletPrefab, gunManager.GetFirePoint().position, Quaternion.identity);
        bullet.Setup(gunManager.GetFirePoint().right, currentGun.damage);

        Debug.Log("Direção da bala: " + gunManager.GetFirePoint().right);

        // Toca o som do disparo
        gunManager.PlayFireSound();

        yield return new WaitForSeconds(currentGun.fireRate);
        podeAtirar = true;
    }

    public void UpdateGun(GunData gunData)
    {
        currentGun = gunData;
    }
}
