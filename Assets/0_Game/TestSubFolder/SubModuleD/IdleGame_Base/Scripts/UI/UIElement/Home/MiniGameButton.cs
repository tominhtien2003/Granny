using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MiniGameButton : MonoBehaviour
{
    [Header("MINIGAME_BUTTON")]
    [SerializeField] private ButtonEffectLogic buttonMiniGame;
    [SerializeField] private GameObject mostPlayer, newGame, completed, comingSoon;
    [SerializeField] private Text gameNameText, countOnlineText;
    [SerializeField] private Image gameIconImage;

    private int gameID;
    private string sceneGameName;

    private bool isLocked;
    

    private void Awake()
    {
        buttonMiniGame.onClick.AddListener(PlayMiniGame);
    }

    public void Init(int id)
    {
        gameID = id;
        var data = DataController.Instance.m_minigames.miniGameList[id];
        gameNameText.text = data.name;
        
        gameIconImage.sprite = data.gameIcon;
        sceneGameName = data.sceneName;
        isLocked = data.isLocked;

        if (isLocked)
        {
            //Turn off Leftbar
            mostPlayer.transform.parent.parent.gameObject.SetActive(false);
            comingSoon.SetActive(true);
            
            return;
        }
        
        mostPlayer.SetActive(gameID < 3);
        if(gameID is 3 or 4) newGame.SetActive(true);
        
        RefreshPlayerOnline();
    }

    public void RefreshPlayerOnline()
    {
        var onlineCount = Random.Range(gameID < 5 ? 3000 : 500, gameID < 5 ? 5000 : 2500);
        int hour = DateTime.Now.Hour;
        if ((hour is > 10 and < 13) || (hour is > 20 and < 22))
        {
            onlineCount *= 2;
        }
        countOnlineText.text = onlineCount.ToString();
    }

    private void PlayMiniGame()
    {
        if (isLocked)
        {
            HomeUIController.Instance.SendNotify("Coming soon!");
            return;
        }
        
        HomeUIController.Instance.CallLoadingPlayGame(gameID,() =>
        {
            HomeUIController.Instance.uiHome.Hide();
            GameManager.Instance.PlayMiniGame(sceneGameName, gameID);
        }, false);
    }

}
