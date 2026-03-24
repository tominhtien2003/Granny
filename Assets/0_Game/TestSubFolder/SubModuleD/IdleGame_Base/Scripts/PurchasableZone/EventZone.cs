using KinematicCharacterController;
using UnityEngine;
using UnityEngine.Events;

public class EventZone : MonoBehaviour
{
    public UnityEvent m_event;
    public UnityEvent m_exitEvent;
    void OnTriggerEnter(Collider other)
    {
        if (!IsPlayer(other)) return;
        m_event?.Invoke();
    }
    void OnTriggerExit(Collider other)
    {
        if (!IsPlayer(other)) return;
        m_exitEvent?.Invoke();
    }

    bool IsPlayer(Collider other)
    {
        return other.gameObject.CompareTag("Player") && other.TryGetComponent(out KinematicCharacterMotor controller);
    }
}
