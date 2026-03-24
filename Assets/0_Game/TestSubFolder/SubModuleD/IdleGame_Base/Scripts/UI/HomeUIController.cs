using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class HomeUIController : MonoBehaviour
{
	public static HomeUIController Instance;

	[Header("UIPANEL")]
	public UIHome uiHome;
	[SerializeField] private UILoading uiLoading;
	[SerializeField] private UINotify uiNotify;

	[Header("UIPOPUP")]
	public SettingPopup settingPopup;
    public SkinShopListDisplayPopup skinPopup;
    public WingShopListDisplayPopup wingPopup;
    

    private void Awake()
	{
		Instance = this;

	}

    private void Start()
    {
	    skinPopup.InitialiseUI();
    }
    
	public void OpenSetting(bool isInGame = false)
	{
		//settingPopup.Show(isInGame);
		settingPopup.Show();
	}


	public void SendNotify(string text)
	{
		uiNotify.Notify(text);
	}

	public void BackHome()
	{
		uiHome.Show();
		AudioManager.Instance.ChangeBackgroundMusic(0);
	}

	public void CallLoadingPlayGame(int indexG, Action action, bool autoHide = true, bool inGamePlay = true)
	{
		action += () =>
		{
			settingPopup.ToggleHomeButton(inGamePlay);
		};
		uiLoading.CallLoadingPlayGame(indexG, action, autoHide);
	}
	
	public void CallLoadingFadePanel(Action action, bool autoHide = true, bool inGamePlay = true)
	{
		action += () =>
		{
			settingPopup.ToggleHomeButton(inGamePlay);
		};
		uiLoading.CallLoadingShowFade(action, autoHide);
	}

	public void CallHidingPlayGPanel()
	{
		uiLoading.CallLoadingPlayGHide();
	}

	public void CallHidingFadePanel()
	{
		uiLoading.CallLoadingFadeHide();
	}
}
