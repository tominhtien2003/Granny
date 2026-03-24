using TMPro;
using UnityEngine;
#if PRIMETWEEN
using PrimeTween;
#elif LIT_MOTION
using LitMotion;
using LitMotion.Extensions;
#elif  DOTWEEN
using DG.Tweening;
#endif

public class UINotify : MonoBehaviour
{
    public static string NOT_ENOUGH_MONEY = "Not enough money!";
    public static string NOT_ENOUGH_GEM = "Not enough gem!";
    public static string NOT_ENOUGH_CUP = "Not enough cup! Climb to the top of the tower to receive cup!";
    public static string BAG_FULL = "Bag is full! Remove or craft pets to free up slots";
    [SerializeField] private GameObject[] notifies;
    [SerializeField] private TMP_Text[] txtNotifies;
    private int _index = 0;
    public static UINotify Instance;
    
    private bool[] notiStatus;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        notiStatus = new bool[notifies.Length];
        for (int i = 0; i < notiStatus.Length; i++)
        {
            notiStatus[i] = false;
        }
        
    }

    public void  Notify(string s)
    {
        if (notiStatus[_index])
        {
            return;
        }

        notiStatus[_index] = true;
        
        AudioManager.Instance.PlayNotifySound();
        txtNotifies[_index].SetText(s);
        var noti = notifies[_index];
        noti.SetActive(true);
        var temp = _index;
        _index = (_index + 1) % notifies.Length;
        
#if PRIMETWEEN
        Tween.Delay(1.5f, () =>
#elif LIT_MOTION
		Utils.LitVirtual(1.5f, () =>
#elif  DOTWEEN
		DOVirtual.DelayedCall(1.5f, () =>
#endif
        {
            notiStatus[temp] = false;
            noti.SetActive(false);
        });
    }
    public void NotifyMoney()
    {
        Notify(NOT_ENOUGH_MONEY);
    }
    public void NotifyCup()
    {
        Notify(NOT_ENOUGH_CUP);
    }
    public void NotifyGem()
    {
        Notify(NOT_ENOUGH_GEM);
    }
    public void NotifyBag()
    {
        Notify(BAG_FULL);
    }

}
