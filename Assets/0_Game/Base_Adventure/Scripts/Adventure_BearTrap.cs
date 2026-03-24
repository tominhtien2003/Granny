using UnityEngine;

public class Adventure_BearTrap : MonoBehaviour
{
    [SerializeField] Collider coll;
    [SerializeField] Collider killCollider;
    [SerializeField] Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        coll = GetComponent<Collider>();
    }
    public void TrapActive()
    {
        animator.Play("TrapActive", 0, 0f);
    }

    public void DisableCollider()
    {
        coll.enabled = false;
        coll.isTrigger = false;
        killCollider.enabled = true;
    }
    public void EnableCollider()
    {
        killCollider.enabled = false;
        coll.enabled = true;
        coll.isTrigger = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && other.CompareTag("Player"))
        {
            if (ShieldComponent.Instance != null && ShieldComponent.isShield) return;
            TrapActive();
        }
    }
}
