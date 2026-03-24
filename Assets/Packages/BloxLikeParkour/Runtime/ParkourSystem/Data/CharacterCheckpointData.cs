using System.Collections.Generic;
using UnityEngine;
public class CharacterCheckpointData : MonoBehaviour
{
    public int CurrentCheckpointId;
    public DeathToggler m_deathToggler;
    public List<ICheckpointAction> m_checkpointActions = new();
    public bool isTeleportToPlayerFar = true;
}
