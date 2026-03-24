using Cysharp.Text;
using Febucci.UI;
using Febucci.UI.Core;
using LitMotion;
using LitMotion.Animation;
using LitMotion.Extensions;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HatchEggPopup : BasePopup
{
    public List<PetChanceItem> m_petChanceItem;
    public int m_currentIndex = -1;
    public SO_Rarity m_rarityData;

    public TMP_Text m_rarityText;
    public TMP_Text m_nameText;

    public ButtonEffectLogic m_hatchButton;
    public ButtonEffectLogic m_autoHatchButton;
    public ButtonEffectLogic m_quickHatchButton;
    public ButtonEffectLogic m_stopAutoHatch;
    public Button m_doneHatch;

    public MeshRenderer m_petMeshRenderer;
    public MeshFilter m_petMesh;

    public InventoryListDisplayPopup m_inventory;

    public GameObject m_hatchPopupContainer;
    public GameObject m_petAnimContainer;

   
    //public LitMotionAnimation m_spinAnim;
    public LitMotionAnimation m_scaleAnim;

    public RectTransform m_petPanel;
    public Transform m_petAnim;

    public Animator m_eggAnim;
    private PetItem m_hatched;

    public bool m_quickHatch = false;
    public bool m_autoHatch = false;
    public GameObject m_eggPanel;

    public EggsData m_currentData;
    public int m_currentEggStart;
    public long m_price;

    public MoneyController m_moneyController;
    public MeshRenderer m_eggMeshRenderer;
    public MeshFilter m_eggMeshFilter;

    public Image m_petHatchBackground;
    public LitMotionAnimation m_bgAnim;

    public Image m_quickHatchPressed;
    public Image m_quickHatchUnPressed;
    public TMP_Text m_priceText;
    public GameObject m_petPreviewZone;
    public GameObject m_petObject;
    public TAnimCore m_textAnimatorName;
    public TAnimCore m_textAnimatorRarity;
    protected override void Awake()
    {
        base.Awake();
        m_reHatch = Hatch;
        m_hatchButton.onClick.AddListener(()=>Hatch());
        m_doneHatch.onClick.AddListener(DoneHatch);
        m_quickHatchButton.onClick.AddListener(ToggleQuickHatch);
        m_stopAutoHatch.onClick.AddListener(StopAutoHatch);
        m_eggAnim.GetComponent<EggAnimScript>().OnEggBreakAnimFinish += () =>
        {
            m_eggAnim.gameObject.SetActive(false);
            m_petObject.SetActive(true);
            PetRevealAnim();
        };
        
        if (Utils_UIController.on_off_reward_button)
        {
            m_autoHatchButton.onClick.AddListener(AutoHatch);
        }
        else
        {
            m_autoHatchButton.gameObject.SetActive(false);
        }
    }
    protected void StopAutoHatch()
    {
        m_autoHatch = false;
        m_rehatchHandle.TryCancel();
        m_stopAutoHatch.gameObject.SetActive(false);
        m_doneHatch.gameObject.SetActive(true);
    }
    protected virtual void AutoHatch()
    {
        if (m_inventory.IsFull()) return;
        if (m_moneyController.m_totalMoney < m_price)
        {
            UINotify.Instance.NotifyMoney();
            return;
        }
#if ADS_AVAILABLE
        AdsAdapter.Instance.ShowRewardedVideo(Mediation_Manager.GameID(), () =>
        {
#if FIREBASE_AVAILABLE
            TrackingEvent.LogFirebase($"rw_auto_hatch_eggs", new Parameter[]
            {
                new Parameter(Consts.GameID, Mediation_Manager.GameLevelString())
            });
#endif
            m_autoHatch = true;
            m_stopAutoHatch.gameObject.SetActive(true);
        
            Hatch();
        }, Consts.NotiAdsFail, AdsAdapter.@where.rw_auto_hatch_egg);
#elif NO_ADS
        m_autoHatch = true;
        m_stopAutoHatch.gameObject.SetActive(true);
    
        Hatch();
#else
    Consts.NotiNoAds();
#endif

        
    }
    protected void ToggleQuickHatch()
    {
        m_quickHatch = !m_quickHatch;
        m_quickHatchPressed.enabled = m_quickHatch;
        m_quickHatchUnPressed.enabled = !m_quickHatch;
    }
    public void SetCurrentPetPool(int data, MeshInfo eggGraphic)
    {
        if (data == m_currentIndex) return;
        m_eggMeshRenderer.sharedMaterial = eggGraphic.m_mat;
        m_eggMeshFilter.sharedMesh = eggGraphic.m_mesh;


        m_price = m_currentData.m_eggs[data].m_price;
        MoneyUIView.UpdateMoney(m_price, m_priceText);
        for (int i = 0; i < m_petChanceItem.Count; i++)
        {
            PetItem pet = ClimbAndJump_DataController.Instance.m_petData.m_petItems[data * 5 + i + m_currentEggStart];
            m_petChanceItem[i].SetPetUnlocked(ClimbAndJump_DataController.Instance.IsPetUnlock(data * 5 + i));
            m_petChanceItem[i].SetPetChanceItem(pet.m_displayIcon, Utils.RarityChances[i]); 
        }
        m_currentIndex = data;
    }
    protected void SetContainerState(bool hatching)
    {
        m_hatchPopupContainer.SetActive(!hatching);
        m_petAnimContainer.SetActive(hatching);
        m_doneHatch.gameObject.SetActive(false);
        m_petPanel.anchoredPosition = new Vector2(0, 0);
        if (m_petPreviewZone.activeSelf != hatching) m_petPreviewZone.SetActive(hatching);
        if (!hatching) m_scaleAnim.Pause();
    }
    private BasePopup m_calledFrom;
    public void ReceivePet(int id, BasePopup calledFrom = null)
    {
        ParticleManager.Instance.PlayConfetti();
        m_hatched = ClimbAndJump_DataController.Instance.m_petData.m_petItems[id];
        Show();
        ClimbAndJump_DataController.Instance.SetPetUnlock(id, true);
        //m_calledFrom = calledFrom;
        //if (m_calledFrom != null) m_calledFrom.gameObject.SetActive(false );
        SetContainerState(true);
        PetRevealAnim(false);
    }
    protected void Hatch()
    {
        if (m_inventory.IsFull() || !m_moneyController.AttemptPurchase(m_price))
        {
            if (m_autoHatch)
            {
                StopAutoHatch();
                
            }
            return;
        }
        if (!m_autoHatch)
        {
#if FIREBASE_AVAILABLE
            TrackingEvent.LogFirebase("hatch_pet", new Parameter[]
            {
                new Parameter(Consts.GameID, Mediation_Manager.GameLevelString())
            });
#endif
        }
        SetContainerState(true);
        m_hatched = null;
        var items = ClimbAndJump_DataController.Instance.m_petData.m_petItems;
        int rand = Random.Range(0, 100);
        //Debug.LogError("Pre rand " + rand);
        for (int i = 0; i < 5; i++)
        {
            int id = i + m_currentIndex * 5 + m_currentEggStart;
            int r = Utils.RarityChances[i];
            if (rand > r) rand -= r;
            else
            {
                m_hatched = items[id];

                if (!ClimbAndJump_DataController.Instance.IsPetUnlock(id))
                {
                    ClimbAndJump_DataController.Instance.SetPetUnlock(id, true);
                    m_petChanceItem[i].SetPetUnlocked(true);
                }
                break;
            }
        }
        if (m_hatched == null)
        {
            //Debug.LogError("null pet " + rand);
            m_doneHatch.gameObject.SetActive(true);
            return;
        }
        if (m_quickHatch || m_autoHatch) PetRevealAnim();
        else PlayEggAnim();

    }
    private bool m_fromHatch = true;
    protected void PetRevealAnim(bool fromHatch = true)
    {
        if (m_hatched.m_rarityIndex < 4) AudioManager.Instance.PlayPetUnlocked();
        else AudioManager.Instance.PlayDivine();
        m_eggPanel.SetActive(false);
        m_petPanel.gameObject.SetActive(true);
        

        m_fromHatch = fromHatch;
        m_petHatchBackground.gameObject.SetActive(true);
        m_bgAnim.Play();
        Color c = m_rarityData.m_rarityColors[m_hatched.m_rarityIndex].m_color;
        c.a = .8f;
        m_petHatchBackground.color = c;

        m_petMesh.sharedMesh = m_hatched.m_mesh;
        m_petMeshRenderer.sharedMaterials = m_hatched.m_mats;
        m_inventory.AddPetItem(new PetItemStruct(m_hatched, 0));
        if (m_inventory.m_currentEmptySlot != -1) m_inventory.AutoSortAndEquipPet();

        if (m_hatched.m_rarityIndex < 4)
        {
            m_nameText.SetText(m_hatched.m_itemName);
            m_rarityText.SetText(m_rarityData.m_rarityColors[m_hatched.m_rarityIndex].m_rarity);
            m_rarityText.color = m_rarityData.m_rarityColors[m_hatched.m_rarityIndex].m_color;
        }
        else
        {
            m_textAnimatorRarity.SetText(ZString.Format("<rainb>{0}</rainb>", m_rarityData.m_rarityColors[m_hatched.m_rarityIndex].m_rarity));
            m_textAnimatorName.SetText(ZString.Format("<rainb>{0}</rainb>", m_hatched.m_itemName));
            /*m_rarityText.SetTextFormat("<rainb>{0}</rainb>", m_rarityData.m_rarityColors[m_hatched.m_rarityIndex].m_rarity);
            m_nameText.SetTextFormat("<rainb>{0}</rainb>",m_hatched.m_itemName);*/
        }


        Vector3 newRot = new Vector3(21.17f, 180, 0);
        LMotion.Create(Vector3.zero, newRot, .2f).WithEase(Ease.InSine).WithOnComplete(() =>
        {
            LMotion.Create(newRot, Vector3.up * 360, .2f).WithOnComplete(DoneHatchAction).BindToLocalEulerAngles(m_petAnim.transform);
        }).BindToLocalEulerAngles(m_petAnim.transform).AddTo(m_petAnim);
        m_scaleAnim.Restart();
    }
    protected Action m_reHatch;
    private MotionHandle m_rehatchHandle;
    void DoneHatchAction()
    {
        if (m_autoHatch) m_rehatchHandle = LMotion.Create(0, 1, .5f).WithOnComplete(m_reHatch).RunWithoutBinding().AddTo(gameObject);
        else m_doneHatch.gameObject.SetActive(true);
    }
    private MotionHandle m_handle;
    private bool m_playingAnim = false;
    void PlayEggAnim()
    {
        m_petObject.SetActive(false);
        m_eggPanel.SetActive(true);
        m_eggAnim.gameObject.SetActive(true);
        m_petPanel.gameObject.SetActive(false);
        m_eggAnim.Play("Egg");

    }
    protected void DoneHatch()
    {
        if (m_playingAnim)return;
        m_playingAnim = true;
        m_doneHatch.gameObject.SetActive(false);
       
        m_handle = LMotion.Create(0f, 50f, .2f).WithOnComplete(()=>
        LMotion.Create(50f, -1000f, .3f).WithOnComplete(()=>
        {
            SetContainerState(false);
            m_playingAnim = false;
            m_petHatchBackground.gameObject.SetActive(false);
            m_bgAnim.Pause();
            if (!m_fromHatch)
            {
                gameObject.SetActive(false);
                //if (m_calledFrom != null) m_calledFrom.Show();
            }
        }).BindToAnchoredPositionY(m_petPanel)).BindToAnchoredPositionY(m_petPanel);

    }
    protected override void OnDisable()
    {
        base.OnDisable();
        m_handle.TryCancel();
    }

    public override void Hide()
    {
        base.Hide();
#if ADS_AVAILABLE
        AdsAdapter.Instance.ShowInterstitial(Mediation_Manager.GameID(), AdsAdapter.@where.inter_close_popup);
#endif
    }
}
