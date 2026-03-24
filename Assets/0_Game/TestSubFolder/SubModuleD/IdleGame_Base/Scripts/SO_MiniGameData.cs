using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "MiniGame_Data", menuName = "Data/MiniGame_Data")]
public class SO_MiniGameData : ScriptableObject
{
	[Serializable]
	public class MiniGameData
	{
		public string name;
		public int id;
		public string sceneName;
		public string gameLogName;
		public Sprite gameIcon;
		public int maxCheckPoint;
		public bool isLocked;
		public bool useAALoader;
		public bool adsBreak;
		public bool adsAFK;
		
		#if UNITY_EDITOR

		public MiniGameData(string name,string sceneName, Sprite gameIcon, int maxCheckPoint)
		{
			this.name = name;
			this.sceneName = sceneName;
			this.gameIcon = gameIcon;
			this.maxCheckPoint = maxCheckPoint;
		}
		
		#endif
	}

	public List<MiniGameData> miniGameList = new ();
	
	#if UNITY_EDITOR

	[Header("DEV_INIT")]
	[SerializeField] private List<SceneAsset> sceneAssets = new ();
	[SerializeField] private List<Sprite> gameIcon = new ();
	[SerializeField] private List<int> maxCheckPoint = new ();

	public void DEV_INIT_DATA()
	{
		miniGameList = new List<MiniGameData>(sceneAssets.Count);

		for (int i = 0; i < sceneAssets.Count; i++)
		{
			var gameName =  System.Text.RegularExpressions.Regex.Replace(sceneAssets[i].name, @"^G\d+_", "").Replace("_", " ").ToUpper();
			var data = new MiniGameData(gameName,sceneAssets[i].name, gameIcon[i], maxCheckPoint[i]);
			miniGameList.Add(data);
		}
		
		this.SO_SetDirty();
	}

	public void DEV_RESET_ID()
	{
		for (int i = 0; i < miniGameList.Count; i++)
		{
			miniGameList[i].id = i;
		}
		
		this.SO_SetDirty();
	}
	
    #endif
}

#if UNITY_EDITOR

#endif
