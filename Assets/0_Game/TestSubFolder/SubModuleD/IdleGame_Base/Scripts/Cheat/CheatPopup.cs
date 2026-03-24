using UnityEngine;

public class CheatPopup : MonoBehaviour
{
#if CHEAT
	public static CheatPopup Instance;

	public ButtonEffectLogic buttonUI;
	public ButtonEffectLogic buttonUITop;
	public ButtonEffectLogic buttonGameplay;
	public ButtonEffectLogic buttonUIButton;

	public CurrencyCheat cup;
	public CurrencyCheat coin;
	public CurrencyCheat gem;
	
	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		HomeUIController.Instance.settingPopup.gameObject.SetActive(false);
	}
#endif	
}


