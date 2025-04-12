using System.Collections;
using UnityEngine;

public class GunShooting : MonoBehaviour
{
    private GunData currentGun;
    private GunManager gunManager;
    private bool podeAtirar = true;

    private void Start()
    {
        gunManager = GetComponent<GunManager>();
    }

    public void UpdateGun(GunData gunData)
    {
        currentGun = gunData;
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
        podeAtirar = false;

        // Instancia o projétil
        Bullet bullet = Instantiate(currentGun.bulletPrefab, gunManager.firePoint.position, Quaternion.identity);
        bullet.Setup(gunManager.firePoint.right, currentGun.damage);

        // Consome munição se não for a arma padrão
        gunManager.UseAmmo();

        yield return new WaitForSeconds(currentGun.fireRate);
        podeAtirar = true;
    }
}
