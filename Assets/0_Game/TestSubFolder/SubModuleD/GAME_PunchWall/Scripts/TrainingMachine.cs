using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrainingMachine : MonoBehaviour
{
    public AnimationClip m_idle;
    public AnimationClip m_train;
    public Transform m_pos;
    public PunchWall_StateMachineInitializer m_punchWallInit;
    public Animator m_animator;
    public int m_bodypartId;
    public bool Occupied = false;
    public UnityAction<bool, TrainingMachine> OnOccupyChanged;
    private void OnTriggerEnter(Collider other)
    {
        if (Occupied || !other.CompareTag("Player")) return;
        PunchWall_GlobalStatusHolder.CurrentBodypartId = m_bodypartId;
        m_punchWallInit.SetTraining(m_pos, m_idle, m_train, m_animator, this);
    }
    public void ChangeOccupiedState(bool state)
    {
        if (Occupied == state) return;
        Occupied = state;
        OnOccupyChanged?.Invoke(state, this);
    }
}
