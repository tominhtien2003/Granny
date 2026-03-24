using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CongratsSetter : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<IOnPlayerReachedCheckpoint>().OnPlayerReachedNewCheckpointEvent += PlayCongrats;
    }

    private void PlayCongrats()
    {
        ParticleManager.Instance.PlayConfetti();
        Vibration.Vibrate(50);
    }
}
