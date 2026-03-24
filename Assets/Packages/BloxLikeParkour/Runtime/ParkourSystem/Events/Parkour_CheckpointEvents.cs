using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Parkour_CheckpointEvents : MonoBehaviour
{
    public UnityAction<CharacterCheckpointData, GameObject> OnCheckpointReached;
    public int CurrentCheckpointId;
    public CharacterCheckpointData m_data;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Checkpoint")) OnCheckpointReached?.Invoke(m_data, other.gameObject);
    }
}
