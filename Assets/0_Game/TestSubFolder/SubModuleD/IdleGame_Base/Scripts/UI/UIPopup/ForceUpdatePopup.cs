using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

public class ForceUpdatePopup : BasePopup
{
    [SerializeField] private ButtonEffectLogic buttonForce;

    protected override void Awake()
    {
        base.Awake();
        buttonForce.onClick.AddListener(OpenStore);
    }

    public void OpenStore()
    {
#if UNITY_ANDROID
        Application.OpenURL($"https://play.google.com/store/apps/details?id={Application.identifier}");
#elif UNITY_IOS
           
#endif
    }

    public override void Show()
    {

        isShow = true;
        gameObject.SetActive(true);

        LMotion.Create(0, 1f, .5f).WithScheduler(MotionScheduler.UpdateIgnoreTimeScale).BindToAlpha(main);
        //main.DOFade(1f, .5f).From(0).SetUpdate(true);
        //AdsAdapter.Instance.ShowMrec();
        Time.timeScale = 0;
    }
}
