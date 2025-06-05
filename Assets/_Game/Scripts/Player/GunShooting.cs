using System.Collections;
using UnityEngine;

public class GunShooting : MonoBehaviour
{
    [SerializeField] Player player;
    private GunRuntime currentGun;
    private bool podeAtirar = true;
    private GunManager gunManager;
    public Animator anim; // Animator to control animations

    void Start()
    {
        gunManager = GetComponent<GunManager>();
    }

    void Update()
    {
        if (player.InputHandler.IsShooting && podeAtirar && currentGun != null && currentGun.currentAmmo > 0)
        {
            anim.SetBool("IsShooting", true); // Activate shooting animation
            StartCoroutine(Atirar());
        }
        else
        {
            anim.SetBool("IsShooting", false); // Deactivate shooting animation
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
        Bullet bullet = Instantiate(
            currentGun.data.bulletPrefab,
            gunManager.GetFirePoint().position,
            Quaternion.Euler(0, 0, gunManager.GetFirePoint().eulerAngles.z)
        );
        bullet.Setup(gunManager.GetFirePoint().right, currentGun.data.damage);

        gunManager.PlayFireSound();

        currentGun.currentAmmo--;

        if (currentGun.currentAmmo <= 0)
        {
            Debug.Log("Munição esgotada. Trocando para arma padrão.");
            gunManager.EquipDefaultGun();
        }

        yield return new WaitForSeconds(currentGun.data.fireRate);
        podeAtirar = true;

        FindObjectOfType<WeaponHUD>()?.SendMessage("UpdateHUD");
    }

    public void UpdateGun(GunRuntime gun)
    {
        currentGun = gun;
    }
}
