using UnityEngine;

public class G3_Bullet : MonoBehaviour
{
    private float speed;
    private System.Action<RaycastHit> hitCallback;

    public void Init(float bulletSpeed)
    {
        speed = bulletSpeed;
    }

    void Update()
    {
        float distance = speed * Time.deltaTime;
        transform.position += transform.forward * distance;
    }

    private bool _despawned = false;

    void OnEnable()
    {
        _despawned = false;
        CancelInvoke(nameof(Despawn));
        Invoke(nameof(Despawn), 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_despawned) return;
        var bot = other.GetComponent<G3_BotHealth>();
        if (bot != null)
        {
            bot.DieInstant();
        }
        Despawn();
    }

    void Despawn()
    {
        if (_despawned) return;
        _despawned = true;
        G3_Manager.Instance.gunObject.PlayParticle(transform);
        G3_PoolManager.Instance.ReturnBullet(this);
    }
}
