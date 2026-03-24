using UnityEngine;

public class G3_GunController : MonoBehaviour
{
    [Header("Gun Settings")]
    public Transform bulletPos;
    public float fireRate = 0.15f;
    public float bulletSpeed = 50f;

    [Header("Recoil")]
    public Transform gunModel;
    public float recoilBack = 0.1f;
    public float recoilReturnSpeed = 8f;
    private Vector3 gunOriginalLocalPos;

    [Header("VFX")]
    public ParticleSystem impactVFX;

    private bool enabledGun = false;
    private float lastShoot = 0f;

    void Start()
    {
        gunOriginalLocalPos = gunModel.localPosition;
    }

    public void EnableGun() => enabledGun = true;

    void Update()
    {
        if (!enabledGun) return;
        //if (Input.GetKeyDown(KeyCode.F)) TryShoot();

        gunModel.localPosition = Vector3.Lerp(
            gunModel.localPosition,
            gunOriginalLocalPos,
            Time.deltaTime * recoilReturnSpeed
        );
    }

    public void TryShoot()
    {
        if (Time.time - lastShoot < fireRate) return;
        lastShoot = Time.time;

        G3_Bullet bullet = G3_PoolManager.Instance.GetBullet();
        bullet.transform.position = bulletPos.position;
        bullet.transform.rotation = bulletPos.rotation;
        bullet.Init(bulletSpeed);

        gunModel.localPosition -= new Vector3(0, 0, recoilBack);
    }
    public void PlayParticle(Transform hitPoint)
    {
         impactVFX.transform.position = hitPoint.position;
         impactVFX.Play();
    }
}
