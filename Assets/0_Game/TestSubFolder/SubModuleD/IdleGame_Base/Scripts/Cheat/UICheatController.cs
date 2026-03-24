using UnityEngine;

public class UICheatController : MonoBehaviour
{
#if CHEAT
    public static UICheatController Instance;

    public UIActiveSetter ui;
    public UIActiveSetter buttons;
    public UIActiveSetter top;
    public UIActiveSetter gameplay;

    public HeightMoneyController coin;
    public MoneyController gem;
    public MoneyController cup;

    public Transform cupTransform;
	
    private void Awake()
    {
        Instance = this;
		
        if (CheatPopup.Instance == null)
        {
            return;
        }
        if(buttons) CheatPopup.Instance.buttonUIButton.onClick.AddListener(() =>
        {
            buttons.ToggleUI();
        });
        if(top) CheatPopup.Instance.buttonUITop.onClick.AddListener(() =>
        {
            top.ToggleUI();
        });
        if(ui) CheatPopup.Instance.buttonUI.onClick.AddListener(() =>
        {
            ui.ToggleUI();
        });
		
        if(gameplay) CheatPopup.Instance.buttonGameplay.onClick.AddListener(() =>
        {
            gameplay.ToggleUI();
        });

        if (cup == null && cupTransform != null)
        {
            cup = cupTransform.GetComponents<MoneyController>()[1];
        }
		
        if(cup) CheatPopup.Instance.cup.SetController(cup);
        if(coin) CheatPopup.Instance.coin.SetController(coin);
        if(gem) CheatPopup.Instance.gem.SetController(gem);
    }

    private void OnDisable()
    {
        if (CheatPopup.Instance == null)
        {
            return;
        }
        if(CheatPopup.Instance.buttonGameplay) CheatPopup.Instance.buttonGameplay.onClick.RemoveAllListeners();
        if(CheatPopup.Instance.buttonUITop) CheatPopup.Instance.buttonUITop.onClick.RemoveAllListeners();
        if(CheatPopup.Instance.buttonUI) CheatPopup.Instance.buttonUI.onClick.RemoveAllListeners();
        if(CheatPopup.Instance.buttonUIButton) CheatPopup.Instance.buttonUIButton.onClick.RemoveAllListeners();
    }
#endif
}

