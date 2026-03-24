using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G3_AudioManager : MonoBehaviour
{
    public static G3_AudioManager Instance;
    //[SerializeField] private AudioClip musicSound;
    [SerializeField] private AudioClip doorSound, grannyScreamSound, eatSound, escalatorSound, grannyCatchedSound,
                                        elevatorReachSound;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //AudioManager.Instance.SetMusic(musicSound);
    }

    public void PlayDoorSound()
    {
        AudioManager.Instance.PlayShot(doorSound);
    }
    public void PlayGrannyScreamSound()
    {
        AudioManager.Instance.PlayShot(grannyScreamSound);
    }
    public void PlayGrannyCatchSound()
    {
        AudioManager.Instance.PlayShot(grannyCatchedSound);
    }
    public void PlayEscalatorSound()
    {
        AudioManager.Instance.PlayShot(escalatorSound);
    }
    public void PlayElevatorReachSound()
    {
        AudioManager.Instance.PlayShot(elevatorReachSound);
    }
    public void PlayEatSound()
    {
        AudioManager.Instance.PlayLoop(eatSound);
    }
    public void StopEatSound()
    {
        AudioManager.Instance.StopLoop(eatSound);
    }
}
