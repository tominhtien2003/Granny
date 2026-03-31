using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundData
{
    public AudioClip clip;
    [Range(0,1)]
    public float volume = 1;

    [Range(0.5f,2)]
    public float pitch = 1;

    public bool loop;
}
public class Gr_AudioManager : MonoBehaviour
{
    public static Gr_AudioManager Instance { get; private set; }
    
    [SerializeField] private List<SoundData> sounds;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
