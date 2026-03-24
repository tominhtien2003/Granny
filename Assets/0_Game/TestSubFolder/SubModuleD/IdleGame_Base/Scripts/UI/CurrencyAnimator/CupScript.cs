using UnityEngine;

public class CupScript : MonoBehaviour
{
    public MoneyReceiveAnimator m_cupAnimator;

    private Collider m_selfCollider;
    private MeshRenderer m_renderer;

    private bool m_currentState = true;
    public int m_cupMultiplier = 1;

    private void Awake()
    {
        m_selfCollider = GetComponent<Collider>();
        m_renderer = GetComponent<MeshRenderer>();

    }

    public void EnableCollider(bool state)
    {
        if (m_currentState == state) return;
        m_currentState = state;
        m_selfCollider.enabled = state;
        m_renderer.enabled = state;
        transform.GetChild(0).gameObject.SetActive(state);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!m_selfCollider.enabled) return;
        if (other.CompareTag("Player"))
        {
#if FIREBASE_AVAILABLE
            TrackingEvent.LogFirebase(Consts.take_cup, new Parameter[]
            {
                new Parameter(Consts.MapID, ClimbAndJump_DataController.CurrentMap.ToString())
            });
#endif   

            //QuestManager.Instance.AddProgress(QuestType.Win);
            //QuestManager.Instance.AddProgress(QuestType.Type1);
            ParticleManager.Instance.PlayConfetti();
            m_cupAnimator.StartAnimate(m_cupMultiplier);
            EnableCollider(false);
        }

    }
    public void TakeCup()
    {
        if (!m_selfCollider.enabled) return;
        //TrackingEvent.LogFirebase(Consts.take_cup, new Parameter[]
        //{
        //    new Parameter(Consts.MapID, ClimbAndJump_DataController.CurrentMap.ToString())
        //});
        ParticleManager.Instance.PlayConfetti();
        m_cupAnimator.StartAnimate(m_cupMultiplier);
        EnableCollider(false);
    }
    public void SetCupMultiplier(int multiplier)
    {
        m_cupMultiplier = multiplier;
    }
}
