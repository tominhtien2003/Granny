using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PrimeTween;

public class Adventure_UIManager : MonoBehaviour
{
    [Header("ButtonEffectLogic")]
    [SerializeField] protected ButtonEffectLogic rewardButtonSkip;
    [SerializeField] protected ButtonEffectLogic rewardButtonSummonJetpack;
    [SerializeField] protected ButtonEffectLogic buttonGoHome;
    [SerializeField] protected ButtonEffectLogic btnJetpackUp;
    [SerializeField] protected ButtonEffectLogic btnJetpackDown;

    [Header("Jetpack")]
    private bool m_jetpackAvailable = true;
    public Image m_jetpackFill;
    public Image m_jetpackAdIcon;
    public float m_jetpackDuration = 75f;
    public Jetpack jetpack;
    public MeshRenderer m_jetpackModel;

    [Header("Cheat")]
    public InputField inputField;
    public ButtonEffectLogic applyBtn;

    [Header("UIMenu")]
    public UIMenu UIMenu;
    
    public GameObject uiDeep;
    public GameObject btnJump;
    [SerializeField] protected Animator uiWarningAnimator;
    protected virtual void Awake()
    {
        //if (Utils_UIController.on_off_reward_button)
        //{
        rewardButtonSkip.onClick.AddListener(() => Adventure_GameManager.Instance.cpManager.Reward_TeleportPlayerToNextCheckpoint());
        rewardButtonSummonJetpack.onClick.AddListener(SummonJetpack);
        //}
        //else
        //{
        //    rewardButtonSkip.gameObject.SetActive(false);
        //    rewardButtonSummonJetpack.gameObject.SetActive(false);
        //}
        buttonGoHome.onClick.AddListener(BackHome);
        btnJetpackUp.onClick.AddListener(() => jetpack.OnJetpackUpButtonPressed());
        btnJetpackDown.onClick.AddListener(() => jetpack.OnJetpackDownButtonPressed());
        if (applyBtn != null)
        {
            applyBtn.onClick.AddListener(() => { TeleportFromInput(); });
        }
        UIMenu.Init();

    }
    public void TeleportFromInput()
    {
        if (int.TryParse(inputField.text, out int index))
        {
            int realIndex = index - 1;
            Adventure_GameManager.Instance.TeleportCheat(realIndex);
        }
    }
    public void BackHome()
    {
        HomeUIController.Instance.CallLoadingPlayGame(-1, () =>
        {
            GameManager.Instance.BackToMenu();
            HomeUIController.Instance.BackHome();
        }, true, false);
    }
    public void SummonJetpack()
    {
        if (!m_jetpackAvailable) return;

#if ADS_AVAILABLE
        AdsAdapter.Instance.ShowRewardedVideo(Mediation_Manager.GameID(), () =>
        {
#if FIREBASE_AVAILABLE
            TrackingEvent.LogFirebase(AdsAdapter.@where.rw_jetpack.ToString(), new Parameter[]
            {
                new Parameter(Consts.GameID, Mediation_Manager.GameLevelString())
            });
#endif
            RewardJetPack();
        }, Consts.NotiAdsFail, AdsAdapter.@where.rw_jetpack);
#elif NO_ADS
        RewardJetPack();
#else
            Consts.NotiNoAds();
#endif
    }

    private void RewardJetPack()
    {
        SetJetpackState(true);
        m_jetpackFill.fillAmount = 1f;
        Tween.Custom(
                target: this,
                startValue: 1f,
                endValue: 0f,
                duration: m_jetpackDuration,
                onValueChange: (t, v) =>
                {
                    t.m_jetpackFill.fillAmount = v;
                }
            )
            .OnComplete(target: this, t =>
            {
                t.SetJetpackState(false);
            });
    }

    private void SetJetpackState(bool state)
    {
        btnJetpackUp.gameObject.SetActive(state);
        btnJetpackDown.gameObject.SetActive(state);
        btnJump.SetActive(!state);

        m_jetpackAvailable = !state;
        rewardButtonSummonJetpack.interactable = !state;
        m_jetpackAdIcon.enabled = !state;
        m_jetpackModel.enabled = state;

        jetpack.canJetpack = state;

        if (state)
        {
            EnterJetpackState();
        }
        else
        {
            ExitJetpackState();
        }
    }

    private void EnterJetpackState()
    {
        Adventure_GameManager.Instance.playerBloxBrain.EnterJetpack();
    }

    private void ExitJetpackState()
    {
        Adventure_GameManager.Instance.playerBloxBrain.ExitJetpack();
    }

    public void SetUIWarning(bool isInWarning)
    {
        uiWarningAnimator.Play(isInWarning ? "Warning" : "Idle");
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
