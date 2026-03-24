using UnityEngine;

public class SlideButton : MonoBehaviour
{
    public SlideClimb m_climbState;
    public SlideStateMachineInitializer m_slideInit;
    private void Awake()
    {
        m_climbState.OnStateChanged += SetButton;
        GetComponentInChildren<ButtonEffectLogic>().onClick.AddListener(SetSlide);
        gameObject.SetActive(false);
    }

    void SetButton(bool state)
    {
        gameObject.SetActive(state);

    }

    void SetSlide()
    {
        m_slideInit.StartSlide = true;
    }


}
