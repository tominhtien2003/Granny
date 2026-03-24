using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressSetter : MonoBehaviour
{
    //private Parkour_ProgressBar m_bar;
    private IOnPlayerReachedCheckpoint m_reachedCheckpoint;
    private void Start()
    {
        //m_bar = UIController.GetUI<Parkour_ProgressBar>();
        m_reachedCheckpoint = GetComponent<IOnPlayerReachedCheckpoint>();
        m_reachedCheckpoint.OnPlayerCheckpointSet += UpdateProgressBar;
    }

    private void UpdateProgressBar()
    {

        //m_bar.SetProgress(m_reachedCheckpoint.GetCurrentCheckpoint() + 1, m_reachedCheckpoint.GetMaxCheckpoint());
    }




}
