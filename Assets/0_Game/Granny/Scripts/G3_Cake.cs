using UnityEngine;

public class G3_Cake : Adventure_InteractObj
{
    [SerializeField] Animator cakeAnimator, playerAnimator;
    [SerializeField] GameObject cake, atticKeyArrow;
    [SerializeField] Transform hand;
    [SerializeField] ParticleSystem particle;
    public override void Interact()
    {
        _triggerBox.gameObject.SetActive(false);
        ToggleOutline(false);
        CakeFall();
        G3_Manager.Instance.TriggerStart();
        IndicatorController.Instance.HideClickBtn(true);
    }
    public void CakeFall()
    {
        cakeAnimator.Play("Fall");
    }
    public void EatCake()
    {
        G3_AudioManager.Instance.PlayEatSound();
        particle.Play();
        cake.transform.SetParent(hand);
        cake.transform.localPosition = Vector3.zero;
        playerAnimator.Play("Eat");
    }
    public void DisableCake()
    {
        cake.SetActive(false);
        G3_AudioManager.Instance.StopEatSound();
        particle.gameObject.SetActive(false);
        atticKeyArrow.gameObject.SetActive(true);
    }
}
