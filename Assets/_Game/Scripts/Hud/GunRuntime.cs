public class GunRuntime
{
    public GunData data;

    public int currentAmmo;
    public int reserveAmmo;

    public GunRuntime(GunData data)
    {
        this.data = data;
        this.currentAmmo = data.magazineSize;
        this.reserveAmmo = data.maxReserveAmmo;
    }
}
