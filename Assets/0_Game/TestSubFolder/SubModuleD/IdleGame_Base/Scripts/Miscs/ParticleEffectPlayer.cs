using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectPlayer : MonoBehaviour
{
    public ParticleSystem m_particle;
    public bool PlayEffect = false;
    public void PlayParticle()
    {
        if (!PlayEffect || m_particle == null) return;
        m_particle.transform.position = transform.position;
        m_particle.Play();
    }

    public static void PlayParticleAt(Vector3 position, ParticleSystem particle)
    {
        particle.transform.position = position;
        particle.Play();
    }
}
