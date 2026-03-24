using UnityEngine;

public class RateManager : MonoBehaviour
{
    public static RateManager Instance;
    
    public static bool PopupShowing = false;
    
    [SerializeField] GameObject ratePrefab;
    [SerializeField] Transform canvasTrans;

    private bool _rated;
    
    private void Awake()
    {
        Instance = this;
        _rated = PrefData.User_RateShowed;
        if (_rated)
        {
            Destroy(canvasTrans.gameObject);
        }
    }
    
    [Button]
    //public bool ShowRate()
    //{
    //    if (_rated || !InitGame.on_off_rate) return false;
    //    DataController.UserRateShowed = true;
    //    _rated = true;
    //    CallRatePopup();
    //    return true;
    //}
    
    public void CallRatePopup()
    {
        var temp = Instantiate(ratePrefab, canvasTrans);
        temp.GetComponent<G_RatePopup>().ShowPopup();
    }

}
