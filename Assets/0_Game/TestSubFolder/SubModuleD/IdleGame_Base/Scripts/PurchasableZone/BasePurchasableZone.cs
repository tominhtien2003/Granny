using KinematicCharacterController;
using LitMotion;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePurchasableZone : MonoBehaviour
{
    /*[SerializeField] private SpriteRenderer missionFill;
    private Material m_mat;
    private CompositeMotionHandle m_tween = new();
    private static readonly int Arc1 = Shader.PropertyToID("_Arc1");
    public float m_duration = 1f;*/

    public bool m_instant = true;

    public List<KinematicCharacterMotor> m_insideMotors = new List<KinematicCharacterMotor>();
    protected virtual void Awake()
    {
        //m_mat = missionFill.material;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!other.TryGetComponent<KinematicCharacterMotor>(out var motor)) return;
        if (!m_insideMotors.Contains(motor))
        {
            m_insideMotors.Add(motor);
            UnlockDone();
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!other.TryGetComponent<KinematicCharacterMotor>(out var motor)) return;
        if (m_insideMotors.Contains(motor))
        {
            m_insideMotors.Remove(motor);
            ProcessExitAction(motor);
        }
    }
    protected virtual void ProcessEnterAction(KinematicCharacterMotor other)
    {
        UnlockDone();
    }

    protected virtual void ProcessExitAction(KinematicCharacterMotor other)
    {
        
    }

    protected virtual void UnlockDone()
    {

    }
}
