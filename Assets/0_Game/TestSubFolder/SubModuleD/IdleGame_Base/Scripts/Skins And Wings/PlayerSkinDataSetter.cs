using UnityEngine;
using UnityEngine.Events;
public class PlayerSkinDataSetter : MonoBehaviour,ISkinControllerUser
{
    public SkinnedMeshRenderer m_playerMesh;
    public bool m_assignAtStart = true;
    public static PlayerSkinDataSetter Instance { get; private set; }

    public bool m_isStandingOnSkin = false;
    public ProgressBarData m_progressBar;
    public SkinController BloxSkinController { get; set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        m_progressBar = FindObjectOfType<ProgressBarData>(true);
       
    }
    private void Start()
    {
        if (BloxSkinController.m_skinData)
        {
            AssignPlayerModel(BloxSkinController.m_skinData.m_shopItemData[BloxSkinController.UserCurrentSkin]);
        }
    }
   
    public UnityAction m_onSkinAssigned;

   
    public virtual void AssignPlayerModel(SkinShopData shopData)
    {
        SkinAssignUtils.AssignModel(m_playerMesh, shopData);
        if (m_progressBar != null) m_progressBar.m_playerRaceImage.sprite = shopData.m_raceIcon;
        m_onSkinAssigned?.Invoke();
        if (PlayerWingDataSetter.Instance)
        {
            PlayerWingDataSetter.Instance.CheckActiveWing(!BloxSkinController.m_skinData.m_shopItemData[BloxSkinController.UserCurrentSkin].m_wingAvailable);
        }
        
    }
}
