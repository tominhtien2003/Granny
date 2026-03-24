using Cysharp.Text;
using LitMotion;
using LitMotion.Animation;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrainingTap : MonoBehaviour, IInitable
{
	public float m_currentMultiplier;
	public float m_falloffDuration;
	private float m_currentDur;

	public float m_baseMaxCooldown;
	private float m_currentMaxCooldown;
	private float m_currentCooldown;

	private bool m_canTrain = true;
	private bool m_isClicking = false;
	private bool m_clickLock = false;

	public RectTransform m_trainBarImage;
	public RectTransform m_effectImage;
	public Image m_superFillImage;
	public TMP_Text m_multiplierText;

	public float m_defaultWidth;
	public TrainingState m_trainingState;
	public LitMotionAnimation m_effectAnim;
	public LitMotionAnimation m_hintAnim;
	public PunchWallScript m_trainingScript;
	public float m_defaultClickSpeed = 1f;

	public ButtonEffectLogic m_auto;
	public ButtonEffectLogic m_auto3x;
	public ButtonGraphicToggler m_autoGraphic;

	private float m_autoClickSpeed = 1f;
	private bool m_tapped = false;
	private bool m_autoAdded = false;
	public Image m_autoAdImage;

	public Button m_camButton;
	void OnEnable()
	{

		m_tapped = false;
		m_hintAnim.Restart();
		m_clickLock = false;
		SetAutoClickState(false);
		m_camButton.enabled = true;
	}
	void ToggleAutoClickState()
	{
		if (!m_autoAdded)
		{

			/*TrackingEvent.LogFirebase($"rw_auto_train", new Parameter[]
			{
				new Parameter(Consts.bodyPartID, PunchWall_GlobalStatusHolder.CurrentBodypartId)
			});

			AdsAdapter.Instance.ShowRewardedVideo(GameManager.MiniGameIndex, () =>
			{*/
                SetAutoAdState(true);
				m_handle1.TryCancel();
				m_handle1 = LMotion.Create(1f, 0f, 180f).WithOnComplete(() => SetAutoAdState(false)).RunWithoutBinding().AddTo(gameObject);


           /* },
			Consts.NotiAdsFail, AdsAdapter.@where.rw_punch_wall_auto_training);*/
		}
		else
		{
			SetAutoClickState(!AutoClicking);
		}

	}
	private MotionHandle m_handle1, m_handle2;
	void SetAutoAdState(bool state)
	{
        m_autoAdded = state;
        m_autoAdImage.enabled = !state;
		SetAutoClickState(state);
    }
	void SetAutoClickState(bool state)
	{
		if (AutoClicking == state) return;
		AutoClicking = state;
		m_autoGraphic.Toggle(state);
		m_isClicking = !state;
		m_defaultClickSpeed = state ? m_autoClickSpeed : 1f;
		m_auto3x.gameObject.SetActive(!m_auto3xPressed & state);

        if (!m_clickLock) SetMultiplier(m_defaultClickSpeed);
	}
	private bool m_auto3xPressed = false;
	public void Init()
	{
		m_auto.onClick.AddListener(ToggleAutoClickState);
		m_auto3x.onClick.AddListener(() =>
		{
			/*if (!m_auto3xPressed)
			{
                TrackingEvent.LogFirebase($"rw_auto_train_3x", new Parameter[]
            {
                new Parameter(Consts.bodyPartID, PunchWall_GlobalStatusHolder.CurrentBodypartId)
            });

                AdsAdapter.Instance.ShowRewardedVideo(GameManager.MiniGameIndex, Reward_Auto3x,
                    Consts.NotiAdsFail, AdsAdapter.@where.rw_punch_wall_auto_training_3x);
            }*/
			Reward_Auto3x();


        });
	}

	private void Reward_Auto3x()
	{
		SetAuto3xState(true);
		m_handle2.TryCancel();
        m_handle2 = LMotion.Create(1f, 0f, 180f).WithOnComplete(() => SetAuto3xState(false)).RunWithoutBinding().AddTo(gameObject);

    }
	public void SetAuto3xState(bool state)
	{
		m_auto3xPressed = state;
		m_auto3x.gameObject.SetActive(!m_auto3xPressed & AutoClicking);
		m_autoClickSpeed = state ? 3f : 1f;
		if (!m_clickLock && AutoClicking) SetMultiplier(m_autoClickSpeed);
		
    }
    public void OnTap()
	{

		if (AutoClicking) return;
		if (m_currentMultiplier < 1.1f)
		{
			m_currentDur = 0;
			m_falloffDuration = .3f;
		}

		else m_falloffDuration = .1f;
		//if (m_effectAnim.IsPlaying) m_circleAnim.Restart();
		if (!m_tapped)
		{
			m_tapped = true;
			m_hintAnim.Stop();
		}
		m_effectAnim.Restart();
		if (!m_clickLock) SetMultiplier(m_currentMultiplier + .2f);
		m_isClicking = true;
		PerformClick();
	}
	void SetMultiplier(float newMult, bool clamped = true)
	{
		if (clamped) m_currentMultiplier = Mathf.Clamp(newMult, 1f, 3f);
		else m_currentMultiplier = newMult;
		m_trainBarImage.sizeDelta = new Vector2(m_defaultWidth * m_currentMultiplier, m_trainBarImage.rect.height);
		m_effectImage.sizeDelta = new Vector2(m_defaultWidth * m_currentMultiplier, m_effectImage.rect.height);
		m_multiplierText.SetTextFormat("x{0:F1}", m_currentMultiplier);
		m_trainingState.SetAnimSpeed(m_currentMultiplier);
		m_currentMaxCooldown = m_baseMaxCooldown / m_currentMultiplier;
	}
	public bool AutoClicking = false;
	private MotionHandle m_handle;
	public void SetSuper()
	{
		m_clickLock = true;
		m_handle.TryCancel();
		SetMultiplier(6f, false);
		m_handle = LMotion.Create(1f, 0f, 5f).WithOnComplete(() =>
		{
			m_clickLock = false;
			SetMultiplier(m_defaultClickSpeed);
		}).BindToFillAmount(m_superFillImage);
	}
	private void OnDisable()
	{
		m_camButton.enabled = false;
        m_handle.TryCancel();
	}
	void PerformClick()
	{
		if (!m_canTrain) return;
		if (!m_clickLock)
		{
			m_superFillImage.fillAmount += .05f;
			if (m_superFillImage.fillAmount >= 1f) SetSuper();
		}
		m_trainingState.SetLiftAnim();
		m_trainingScript.UpdateTrain(PunchWall_GlobalStatusHolder.CurrentBodypartId);
		m_canTrain = false;
		m_currentCooldown = 0;
	}
	void Update()
	{
		float dt = Time.deltaTime;
		if (AutoClicking) PerformClick();
		else if (m_isClicking && !m_clickLock)
		{
			m_currentDur += dt;
			if (m_currentDur > m_falloffDuration)
			{
				SetMultiplier(m_currentMultiplier - .3f);
				m_currentDur -= m_falloffDuration;
				if (m_currentMultiplier < 1.1f)
				{
					m_currentDur = 0;
					m_isClicking = false;
				}

			}
		}
		if (m_canTrain) return;
		m_currentCooldown += dt;
		if (m_currentCooldown > m_currentMaxCooldown)
		{
			m_canTrain = true;
			m_trainingScript.AnimateMuscle();
            m_currentCooldown -= m_currentMaxCooldown;
		}


	}


}
