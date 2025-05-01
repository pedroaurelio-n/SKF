public class GunRuntime
{
    public GunData data;
    public int ammo;

    public GunRuntime(GunData data)
    {
        this.data = data;
        this.ammo = data.ammo;
    }
}
