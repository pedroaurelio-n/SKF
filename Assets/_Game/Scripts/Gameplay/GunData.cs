using UnityEngine;

[CreateAssetMenu(fileName = "NewGun", menuName = "Guns/New Gun")]
public class GunData : ScriptableObject
{
    [Header("Identificação")]
    public string weaponName;
    public Sprite gunSprite;     // Sprite da arma na mão do player
    public Sprite hudSprite;     // Sprite personalizada para a HUD

    [Header("Projétil")]
    public Bullet bulletPrefab;

    [Header("Estatísticas de Tiro")]
    [Tooltip("Intervalo entre tiros, em segundos")]
    public float fireRate = 0.2f;
    public int damage = 1;

    [Header("Munição")]
    public int magazineSize = 10;        // Pente atual
    public int maxReserveAmmo = 30;      // Máximo da reserva

    [Header("Áudio")]
    [Tooltip("Som a tocar a cada disparo")]
    public AudioClip fireSound;
    [Range(0f, 1f)]
    public float fireVolume = 1f;
    [Tooltip("Variação de pitch (1 = normal)")]
    public float firePitch = 1f;
}
