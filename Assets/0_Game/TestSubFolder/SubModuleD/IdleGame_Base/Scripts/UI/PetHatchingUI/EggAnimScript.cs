using Coffee.UIExtensions;
using UnityEngine;
using UnityEngine.Events;

public class EggAnimScript : MonoBehaviour
{
    public UnityAction OnEggBreakAnimFinish;
    public ParticleSystem m_particle;
    public void SendFinishSignal()
    {
        OnEggBreakAnimFinish?.Invoke();
    }
    public void PlayEffect()
    {
        m_particle.Emit(15);
    }
    public void PlayEggCrackEffect()
    {
        Vibration.Vibrate(50);
        AudioManager.Instance.PlayEggCrackSound();
    }
}
