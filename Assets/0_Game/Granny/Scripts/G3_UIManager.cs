using System.Collections;
using System.Collections.Generic;
//using Firebase.Analytics;
using UnityEngine;
using UnityEngine.UI;
using PrimeTween;
public class G3_UIManager : Adventure_UIManager
{
    public static G3_UIManager Instance;
    [SerializeField] ButtonEffectLogic btnStopCar, btnShoot;
    public GameObject uiCombat;
    public G3_UiNotify noti1, noti2;
    [SerializeField] private Image combatBotRateImage;

    public CalculatorPopup calculatorPopup;
    protected override void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        btnStopCar.onClick.AddListener(() =>
        {
            G3_Manager.Instance.IsOnCar = false;
            btnStopCar.gameObject.SetActive(false);
        });
        btnShoot.onClick.AddListener(() => G3_Manager.Instance.gunObject.TryShoot());
        base.Awake();
    }
    
    public void SetOnCar()
    {
        btnStopCar.gameObject.SetActive(true);
    }
    public void SetUICombat(bool isInCombat)
    {
        uiCombat.gameObject.SetActive(isInCombat);
        btnShoot.gameObject.SetActive(isInCombat);
    }
    public void UpdateBotRate(float rate)
    {
        combatBotRateImage.fillAmount = rate;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    void OnDisable()
    {
        Tween.StopAll(this);
    }

}
