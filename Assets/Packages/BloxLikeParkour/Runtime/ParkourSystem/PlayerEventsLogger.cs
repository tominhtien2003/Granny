using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventsLogger : MonoBehaviour
{
    private void Awake()
    {
        var v = GetComponent<CheckpointManager>();
        GetComponent<IOnPlayerDeath>().OnPlayerDeathEvent += LogDeath;
        GetComponent<IOnPlayerReachedCheckpoint>().OnPlayerReachedNewCheckpointEvent += LogCheckpoint;
        GetComponent<IOnPlayerRespawn>().OnPlayerRespawnEvent += LogRespawn;
    }
    void LogDeath()
    {

    }
    void LogCheckpoint()
    {

    }
    void LogRespawn()
    {

    }
}
