using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

public class NoInternetPopup : BasePopup
{
    [SerializeField] private ButtonEffectLogic btnWifi;
    
    protected override void Awake()
    {
        base.Awake();
        btnWifi.onClick.AddListener(OpenWifi);
    }
    private void OpenWifi()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
	        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
	        AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", "android.settings.WIFI_SETTINGS");
	        currentActivity.Call("startActivity", intent);
#endif
    }

    private void Update()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            Hide();
        }
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

    public override void Hide()
    {
        isShow = true; //fake popup show
        LMotion.Create(1f, 0f, .5f).WithScheduler(MotionScheduler.UpdateIgnoreTimeScale).WithOnComplete(() =>
        {
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }).BindToAlpha(main);
        //AdsAdapter.Instance.HideMrec();
        /*main.DOFade(0f, .5f).SetUpdate(true).OnComplete(() =>
        {
            gameObject.SetActive(false);
            Time.timeScale = 1;
        });*/
    }
}
