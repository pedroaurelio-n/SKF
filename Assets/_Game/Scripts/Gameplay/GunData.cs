using UnityEngine;

[CreateAssetMenu(fileName = "NewGun", menuName = "Guns/New Gun")]
public class GunData : ScriptableObject
{
    public string weaponName;
    public Sprite gunSprite;
    public Bullet bulletPrefab;
    public float fireRate = 0.2f;
    public int ammo = 10;
    public int damage = 1;
}
