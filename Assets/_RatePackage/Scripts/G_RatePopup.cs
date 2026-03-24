#if GOOGLE_REVIEW
using Google.Play.Review;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if PRIME_TWEEN
using PrimeTween;
#elif DOTWEEN
using DG.Tweening;
#endif
public class G_RatePopup : MonoBehaviour
{
	#region SERIALIZEFIELD

	[SerializeField] List<GameObject> starsOn;
	[SerializeField] RectTransform starRow;
	[SerializeField] GameObject feedbackObj, titleObject;
	[SerializeField] RectTransform container;
	[SerializeField] Transform topPoint;
	[SerializeField] Canvas canvas;
	[SerializeField] CanvasGroup canvasGroup;
	[SerializeField] Button btnBackdrop;
	[SerializeField] GameObject objSubmitGray, objSubmitGreen;

	#endregion

	#region VARIABLES

	float effectTime = 0.2f;

	#if PRIME_TWEEN || DOTWEEN
	Sequence sequence1, sequence2;
	#elif LITMOTION
		MotionHandle m_showHandle1, m_hideHandle1, m_showHandle2, m_hideHandle2;
	#endif


	bool showFB = false;
	float panelHeight = 852;

	#endregion

	#region FUNTITONS

	public void ShowPopup()
	{
		RateManager.PopupShowing = true;
		objSubmitGray.SetActive(true);
		objSubmitGreen.SetActive(false);
		for (int i = 0; i < starsOn.Count; i++) starsOn[i].SetActive(false);
		showFB = false;

		canvas.enabled = true;
		container.anchoredPosition = new Vector2(0, -panelHeight);

	#if PRIME_TWEEN
		sequence1.Stop();
		sequence1 = Sequence.Create().Group(Tween.Alpha(canvasGroup, 1, 1))
			.Chain(Tween.UIAnchoredPositionY(container, 0, effectTime, ease: Ease.Linear))
			.OnComplete(() =>
			{
				btnBackdrop.onClick.AddListener(HidePopup);
			});
	#elif LITMOTION
	        m_showHandle1.TryCancel();
	        m_showHandle2.TryCancel();
	        m_hideHandle1.TryCancel();
	        m_hideHandle2.TryCancel();

	        m_showHandle1 = LMotion.Create(0, 1, effectTime).Bind(x =>
	        {
	            canvasGroup.alpha = x;
	        });
	        m_showHandle2 = LMotion.Create(container.anchoredPosition.y, 0, effectTime).
	                    WithOnComplete(() => btnBackdrop.onClick.AddListener(HidePopup))
	                    .BindToAnchoredPositionY(container);
	#elif DOTWEEN
			sequence1?.Kill();
			sequence1 = DOTween.Sequence();
			sequence1.Append(DOVirtual.Float(0, 1, effectTime, (val) => { canvasGroup.alpha = val; }));
			sequence1.Join(container.DOAnchorPosY(0, effectTime).SetEase(Ease.Linear));
			sequence1.OnComplete(() => { btnBackdrop.onClick.AddListener(HidePopup); });
	#endif

	}
	public void HidePopup()
	{
		btnBackdrop.onClick.RemoveAllListeners();
	#if PRIME_TWEEN
		sequence1.Stop();
		sequence1 = Sequence.Create().Group(Tween.UIAnchoredPositionY(container, -panelHeight, effectTime, Ease.Linear))
			.Chain(Tween.Alpha(canvasGroup, 1, 0, effectTime)).OnComplete(() =>
			{
				RateManager.PopupShowing = false;
				canvas.enabled = false;
			});
	#elif LITMOTION
			m_showHandle1.TryCancel();
	        m_showHandle2.TryCancel();
	        m_hideHandle1.TryCancel();
	        m_hideHandle2.TryCancel();

	        m_hideHandle1 = LMotion.Create(1, 0, effectTime).Bind(x =>
	        {
	            canvasGroup.alpha = x;
	        });
	        m_hideHandle2 = LMotion.Create(container.anchoredPosition.y, -panelHeight, effectTime).
	        WithOnComplete(() =>
	        {
	            HGRate.PopupShowing = false;
	            canvas.enabled = false;
	        })
	        .BindToAnchoredPositionY(container);
	#elif DOTWEEN
			sequence1?.Kill();
			sequence1 = DOTween.Sequence();
			sequence1.Append(container.DOAnchorPosY(-panelHeight, effectTime).SetEase(Ease.Linear));
			sequence1.Join(DOVirtual.Float(1, 0, effectTime, (val) => { canvasGroup.alpha = val; }));
			sequence1.OnComplete(() =>
			{
				HGRate.PopupShowing = false;
				canvas.enabled = false;
				//AdsManager.ShowInter()
			});
	#endif
	}

	#endregion

	#region OTHERS

	public void OnStarClick(int value)
	{
		objSubmitGray.SetActive(false);
		objSubmitGreen.SetActive(true);
		for (int i = 0; i < starsOn.Count; i++)
		{
			starsOn[i].SetActive(value >= i);
		}
		if (value < 4)
		{
			ShowFeedback();
			return;
		}
		RateAndReview();

	}


	void ShowFeedback()
	{
		if (showFB) return;
		showFB = true;
		feedbackObj.transform.localScale = Vector3.zero;
		titleObject.SetActive(false);
#if PRIME_TWEEN
		sequence2.Stop();
		sequence2 = Sequence.Create()
			.Group(Tween.Position(starRow, topPoint.position, effectTime, Ease.Linear))
			.Chain(Tween.Scale(feedbackObj.transform, 1, effectTime));
#elif LITMOTION
			m_fbHandle1.TryCancel();
			m_fbHandle2.TryCancel();
			m_fbHandle1 = LMotion.Create(starRow.position, topPoint.position, effectTime).BindToPosition(starRow);
			m_fbHandle2 = LMotion.Create(feedbackObj.transform.localScale, Vector3.one, effectTime).BindToLocalScale(feedbackObj.transform);
#elif DOTWEEN
			sequence2?.Kill();
			sequence2 = DOTween.Sequence();
			sequence2.Append(starRow.DOMove(topPoint.position, effectTime).SetEase(Ease.Linear));
			sequence2.Join(feedbackObj.transform.DOScale(1, effectTime).SetEase(Ease.Linear));
#endif
	}



	private void OnDestroy()
	{
#if PRIME_TWEEN
		sequence1.Stop();
		sequence2.Stop();
#elif LITMOTION
			m_seq1.Cancel();
			m_seq2.Cancel();
#elif DOTWEEN
			sequence1?.Kill();
			sequence2?.Kill();
#endif
	}

#if UNITY_ANDROID && GOOGLE_REVIEW

	private ReviewManager _reviewManager;
	private PlayReviewInfo _playReviewInfo;
#endif
	private Coroutine _coroutine;

	protected void Start()
	{
#if UNITY_ANDROID
		_coroutine = StartCoroutine(InitReview());
#endif
	}
	private IEnumerator InitReview(bool force = false)
	{
#if GOOGLE_REVIEW
		if (_reviewManager == null) _reviewManager = new ReviewManager();

		var requestFlowOperation = _reviewManager.RequestReviewFlow();
		yield return requestFlowOperation;
		if (requestFlowOperation.Error != ReviewErrorCode.NoError)
		{
			if (force) DirectlyOpen();
			yield break;
		}


		_playReviewInfo = requestFlowOperation.GetResult();
#else
		yield break;
#endif
	}
	public void RateAndReview()
	{

#if UNITY_IOS
	            Device.RequestStoreReview();
#elif UNITY_ANDROID
		StartCoroutine(LaunchReview());
#endif
		HidePopup();
	}
	public IEnumerator LaunchReview()
	{
#if GOOGLE_REVIEW
		if (_playReviewInfo == null)
		{
			if (_coroutine != null) StopCoroutine(_coroutine);
			yield return StartCoroutine(InitReview(true));
		}

		var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
		yield return launchFlowOperation;
		_playReviewInfo = null;

		if (launchFlowOperation.Error != ReviewErrorCode.NoError)
		{
			DirectlyOpen();
			yield break;
		}
#else
		yield break;
#endif
	}
	private void DirectlyOpen() { Application.OpenURL($"https://play.google.com/store/apps/details?id={Application.identifier}"); }

	#endregion
}
