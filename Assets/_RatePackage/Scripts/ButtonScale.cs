using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;
#if PRIME_TWEEN
//using PrimeTween;
#elif DOTWEEN
using DG.Tweening;
#endif

public class ButtonScale : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler, IPointerExitHandler, IPointerClickHandler
{
	[Header("Reference")]
	[SerializeField] Transform _tfTarget;

	[Header("Config")]
	[SerializeField] Vector2 _scaleValue = new Vector2(1f, 1.1f);
	[Min(0.1f)]
	[SerializeField] float _scaleSpeed = 1f;
	[SerializeField] bool _playSound = true;

	bool _isDown = false;
	
#if PRIME_TWEEN || DOTWEEN
	Tween _tween;
#elif LITMOTION
	MotionHandle _tween;
#endif
	
    #region MONOBEHAVIOUR

	void Awake()
	{
		if (_tfTarget == null) _tfTarget = transform;
		_tfTarget.localScale = new Vector3(_scaleValue.x, _scaleValue.x, _scaleValue.x);
	}

	void OnDestroy()
	{
#if PRIME_TWEEN
		_tween.Stop();
#elif LITMOTION
		_tween.TryCancel();
#elif DOTWEEN
	    _tween?.Kill();
#endif
		
	}

    #endregion

    #region PLAY_TWEEN

	void ScaleUp()
	{
		float duration = Mathf.Abs((_scaleValue.y - _tfTarget.localScale.x) / _scaleSpeed);
#if PRIME_TWEEN
		_tween.Stop();
		_tween = Tween.Scale(_tfTarget, _scaleValue.y, duration, useUnscaledTime: true);
#elif LITMOTION
		_tween.TryCancel();
		_tween = LMotion.Create(_tfTarget.transform.localScale, _scaleValue.y * Vector3.one, duration).WithScheduler(MotionScheduler.UpdateIgnoreTimeScale)
                .BindToLocalScale(_tfTarget).AddTo(_tfTarget);

#elif DOTWEEN
		_tween?.Kill();
		_tween = _tfTarget.DOScale(_scaleValue.y, duration).SetUpdate(true);
#endif

	}

	void ScaleDown()
	{
		float duration = Mathf.Abs((_scaleValue.x - _tfTarget.localScale.x) / _scaleSpeed);
#if PRIME_TWEEN
		_tween.Stop();
		_tween = Tween.Scale(_tfTarget, _scaleValue.x, duration, useUnscaledTime: true);
#elif LITMOTION
		_tween.TryCancel();
		_tween = LMotion.Create(_tfTarget.transform.localScale, _scaleValue.x * Vector3.one, duration).WithScheduler(MotionScheduler.UpdateIgnoreTimeScale)
                .BindToLocalScale(_tfTarget).AddTo(_tfTarget);
#elif DOTWEEN
		_tween?.Kill();
		_tween = _tfTarget.DOScale(_scaleValue.x, duration).SetUpdate(true);
#endif

	}

	#endregion

	#region POINTER_EVENTS

	void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
	{
		_isDown = true;

		ScaleUp();

		//if (_playSound)
		//    AudioManager.Play(PConfig.SfxButtonClick);
	}

	void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
	{
		if (_isDown)
			ScaleDown();
	}

	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		if (_isDown)
			ScaleUp();
	}

	void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
	{
		_isDown = false;

		ScaleDown();
	}

	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
	}

    #endregion
}
