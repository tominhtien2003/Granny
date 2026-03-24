using System;
using System.Collections.Generic;
using UnityEngine;
public class UIHome : BasePanel
{
	[Header("UI_HOME")]
	[SerializeField] private ButtonEffectLogic buttonSetting;
	
	[Header("MINIGAME_BUTTON")]
	[SerializeField] private MiniGameButton miniGameButton;
	[SerializeField] private Transform miniGameParent;
	
	private List<MiniGameButton>  miniGameButtons = new ();

	private void Awake()
	{
		buttonSetting.onClick.AddListener(() =>
		{
			HomeUIController.Instance.OpenSetting();
		});
	}

	private void Start()
	{
		foreach (var miniGameData in DataController.Instance.m_minigames.miniGameList)
		{
			var miniGame = Instantiate(miniGameButton, miniGameParent);
			miniGame.Init(miniGameData.id);
			miniGameButtons.Add(miniGame);
		}
		// for (int i = 0; i < DataController.Instance.m_minigames.miniGameList.Count; i++)
		// {
		// 	var miniGame = Instantiate(miniGameButton, miniGameParent);
		// 	miniGame.Init(i);
		// 	miniGameButtons.Add(miniGame);
		// }

		
		AudioManager.Instance.ChangeBackgroundMusic(0);
	}
}
