using Coffee.UIExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance;
    
    public UIParticle m_confetti;
    [SerializeField] private UIParticle eat;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void PlayParticle(UIParticle particle)
    {
        particle.Play();
    }

    public void PlayConfetti()
    {
        PlayParticle(m_confetti);
        AudioManager.Instance.PlayCongrats();
    }
    public void PlayEat()
    {
        PlayParticle(eat);
    }

}
