using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;


public class Adventure_EventCheckpointManager : MonoBehaviour
{
    [System.Serializable]
    public class CheckpointEvent
    {
        public int CheckpointId;
        public UnityEvent OnActivatedActions; 
    }
    public List<CheckpointEvent> CheckpointActions;
    public List<CheckpointEvent> CheckpointStartActions;
    [SerializeField] private Parkour_CheckpointEvents checkpointManager;

    void Start()
    {
        if (checkpointManager != null)
        {
            checkpointManager.OnCheckpointReached += HandleCheckpoint;
            ExecutePreviousCheckpointActions();
        }
    }

    private void OnDestroy()
    {
        if (checkpointManager != null)
        {
            checkpointManager.OnCheckpointReached -= HandleCheckpoint;
        }
    }
    private void ExecutePreviousCheckpointActions()
    {
        int currentCheckpointId = checkpointManager.m_data.CurrentCheckpointId;

        foreach (var eventAction in CheckpointStartActions)
        {
            if (eventAction.CheckpointId <= currentCheckpointId)
            {
                eventAction.OnActivatedActions.Invoke();
            }
        }
    }
    private void HandleCheckpoint(CharacterCheckpointData data, GameObject checkpointObject)
    {
        int id = checkpointManager.m_data.CurrentCheckpointId;
        foreach (var eventAction in CheckpointActions)
        {
            if (eventAction.CheckpointId == id)
            {
                //Debug.Log("CPEvent " + id);
                eventAction.OnActivatedActions.Invoke(); 
                return;
            }
        }
    }
}