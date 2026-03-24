using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSoundPlayer : MonoBehaviour
{
    public void PlayAudio(AudioClip audio)
    {
        AudioManager.Instance.PlayShot(audio);
    }
}
