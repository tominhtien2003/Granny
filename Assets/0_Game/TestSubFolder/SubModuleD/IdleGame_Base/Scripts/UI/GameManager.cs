using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if FIREBASE_AVAILABLE
using Firebase.Analytics;
#endif

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	public bool _startGame = true;

	public static int MiniGameIndex => miniGameIndex;
	
	private static int miniGameIndex = -1;
	public GameObject _home;
	
	public List<int> gamePlayed = new List<int>();
	

    private void Awake()
	{
		Instance = this;
		if (_home)
		{
			_home.SetActive(true);
		}
		else
		{
			Debug.LogError("home la cai nao ?");
		}
	}

	#region LOADSCENE LEVEL

	public void BackToMenu()
	{
		_startGame = true;
		LoadScene("MainMenu");
		if (miniGameIndex != -1)
		{
			//HomeUIController.Instance.uiHome.MiniGameRefreshProcess(miniGameIndex);
			PrefData.CurrentMinigame = -1;
            miniGameIndex = -1;
		}
#if GOOGLE_REVIEW
		if (!RateManager.Instance.ShowRate())
		{
#if ADS_AVAILABLE
			AdsAdapter.Instance.ShowInterstitial(MiniGameIndex, AdsAdapter.@where.inter_back_home);
#endif
		}
#else
#if ADS_AVAILABLE
			AdsAdapter.Instance.ShowInterstitial(MiniGameIndex, AdsAdapter.@where.inter_back_home);
#endif
#endif
#if ADS_AVAILABLE
		AdsAdapter.Instance.ShowBanner();
#endif
	}

	public void PlayMiniGame(string nameScene, int index)
	{
		miniGameIndex = index;
		if (index >= 0) PrefData.CurrentMinigame = index;
		LoadScene(nameScene);
		if (gamePlayed.Contains(miniGameIndex))
		{
#if FIREBASE_AVAILABLE
			TrackingEvent.LogFirebase(Consts.replay_game, new Parameter[]
			{
				new Parameter(Consts.GameID, Mediation_Manager.GameLevelString())
			});
#endif
		}
		else
		{
			gamePlayed.Add(miniGameIndex);
#if FIREBASE_AVAILABLE
			TrackingEvent.LogFirebase(Consts.start_game, new Parameter[]
			{
				new Parameter(Consts.GameID, Mediation_Manager.GameLevelString())
			});
#endif
		}
	}

	public void PlayMiniGame(int index)
	{
		miniGameIndex = index;
		if (index > 0) PrefData.CurrentMinigame = index;
		HomeUIController.Instance.settingPopup.ToggleHomeButton(true);
	}
	
	public void LoadScene(string sceneName)
	{
		StartCoroutine(LoadNameSceneAsync(sceneName, HomeUIController.Instance.CallHidingPlayGPanel));
	}

	IEnumerator LoadIndexSceneAsync(string sceneIndex, Action onCompleted)
	{
		AsyncOperation async = SceneManager.LoadSceneAsync(sceneIndex);
		async.allowSceneActivation = false;
		yield return Yielder.Get(1f);

		while (!async.isDone)
		{
			yield return null;
			if (async.progress >= 0.9f)
			{
				async.allowSceneActivation = true;
			}
		}
		onCompleted?.Invoke();
		async.allowSceneActivation = true;
	}

	IEnumerator LoadNameSceneAsync(string nameScene, Action onCompleted)
	{
		AsyncOperation async = SceneManager.LoadSceneAsync(nameScene);
		async.allowSceneActivation = false;
		yield return Yielder.Get(1f);

		while (!async.isDone)
		{
			yield return null;
			if (async.progress >= 0.9f)
			{
				async.allowSceneActivation = true;
			}
		}
		async.allowSceneActivation = true;
        //yield return Yielder.Get(1f);
        onCompleted?.Invoke();
	}
	
	

	#endregion
}
