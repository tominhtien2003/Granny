using LitMotion.Animation;
using UnityEngine;

public class PlayerGroundCrackSetter : MonoBehaviour
{
    public LitMotionAnimation m_crackAnim;
    public ParticleSystem m_particle;
    public void EnableCrack()
    {
        Vibration.Vibrate(50);
        Vector3 ground = transform.position + Vector3.up * 0.1f;
        m_crackAnim.transform.position = ground;
        m_particle.transform.position = ground;

        m_particle.Play();
        m_crackAnim.Restart();
    }
}
