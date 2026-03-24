#if ADS_AVAILABLE
using GoogleCMP;
#endif

using UnityEngine;
using UnityEngine.UI;
public class SettingPopup : BasePopup
{
	[Header("SETTING_POPUP")]
	[SerializeField] private Slider sensitivitySliderBar;
	[SerializeField] private ButtonEffectLogic btnMusic, btnSound, btnConsent, btnVibration;

	[SerializeField] private GameObject musicOn, musicOff, soundOn, soundOff, vibrationOn, vibrationOff;
	
	[SerializeField] private float minSpeed = 1f, maxSpeed = 10f;
	//private float speedDistance;
	public ButtonEffectLogic m_buttonHome;
	public GameObject m_buttonHomeContainer;
	
#if CHEAT
	public GameObject cheat;
#endif

	protected override void Awake()
	{
		base.Awake();
		//speedDistance = maxSpeed - minSpeed;

		btnSound.onClick.AddListener(ChangeSound);
		//btnVibration.onClick.AddListener(ChangeVibration);
		btnMusic.onClick.AddListener(ChangeMusic);
        btnVibration.onClick.AddListener(ChangeVibration);

        DisplaySound(DataController.Setting_SFX);
		DisplayVibration(DataController.Setting_Vibrate);
		DisplayMusic(DataController.Setting_Music);
        m_buttonHome.onClick.AddListener(BackHome);
        sensitivitySliderBar.value = (DataController.Setting_Sensitivity - minSpeed) / (maxSpeed - minSpeed);
        sensitivitySliderBar.onValueChanged.AddListener((value) =>
        {
            DataController.Setting_Sensitivity = Mathf.Lerp(minSpeed, maxSpeed, value);
        });
        
#if CHEAT
		cheat.SetActive(true);
#endif

#if ADS_AVAILABLE
		if (CMPManager.RequireCmp)
		{
			btnConsent.gameObject.SetActive(true);
			btnConsent.onClick.AddListener(UMP.ShowPrivacyOptionsForm);
		}
#endif

	}
    private void ChangeMusic()
	{
		var curMusic = DataController.Setting_Music;
		DataController.Setting_Music = !curMusic;
		DisplayMusic(!curMusic);
		AudioManager.Instance.UpdateMute();
	}
	
	
	private void ChangeSound()
	{
		var curSfx = DataController.Setting_SFX;
		DataController.Setting_SFX = !curSfx;
		DisplaySound(!curSfx);
		AudioManager.Instance.UpdateMute();
	}

	private void ChangeVibration()
	{
		var curVibrate = DataController.Setting_Vibrate;
		DataController.Setting_Vibrate = !curVibrate;
		DisplayVibration(!curVibrate);
	}

	private void DisplaySound(bool isOn)
	{
		soundOn.SetActive(isOn);
		soundOff.SetActive(!isOn);
	}	
	
	private void DisplayMusic(bool isOn)
	{
		musicOn.SetActive(isOn);
		musicOff.SetActive(!isOn);
	}

	private void DisplayVibration(bool isOn)
	{
		vibrationOn.SetActive(isOn);
		vibrationOff.SetActive(!isOn);
	}

	public override void Hide()
	{
		base.Hide();

#if ADS_AVAILABLE
		AdsAdapter.Instance.HideMrec();
		AdsAdapter.Instance.ShowInterstitial(Mediation_Manager.GameID(), AdsAdapter.@where.inter_close_popup);
#endif

	}
	
	public override void Show()
	{
		base.Show();
#if ADS_AVAILABLE
		AdsAdapter.Instance.ShowMrec();
#endif
	}
    public void ToggleHomeButton(bool isActive)
    {
        m_buttonHomeContainer.SetActive(isActive);
    }

    public void BackHome()
    {
        Hide();
        HomeUIController.Instance.CallLoadingPlayGame(-1, () =>
        {
            GameManager.Instance.BackToMenu();
			HomeUIController.Instance.BackHome();
        }, true, false);
    }
}
