using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePlay : MonoBehaviour
{
    [SerializeField] private Button handButton;
    [SerializeField] private Button dropButton;
    [SerializeField] private TextMeshProUGUI txtDescription;
    [SerializeField] private TextMeshProUGUI txtRequiredItem;
    private Gr_IInteractable objInteractable;

    private void OnEnable()
    {
        Gr_EventManager.AddListener<ObjectInteractableChangedEvent>(UpdateUIObjectInteractableChanged);
        Gr_EventManager.AddListener<SelectedItemEvent>(UpdateUISelectedItem);
        Gr_EventManager.AddListener<FocusItemEvent>(UpdateUIWhenFocusItem);
        Gr_EventManager.AddListener<MissingRequiredItemEvent>(UpdateUIRequiredItem);
        handButton.onClick.AddListener(OnInteractHandButton);
        dropButton.onClick.AddListener(OnInteractDropButton);
    }
    private void OnDisable()
    {
        Gr_EventManager.RemoveListener<ObjectInteractableChangedEvent>(UpdateUIObjectInteractableChanged);
        Gr_EventManager.RemoveListener<SelectedItemEvent>(UpdateUISelectedItem);
        Gr_EventManager.RemoveListener<FocusItemEvent>(UpdateUIWhenFocusItem);
        Gr_EventManager.RemoveListener<MissingRequiredItemEvent>(UpdateUIRequiredItem);
        handButton.onClick.RemoveListener(OnInteractHandButton);
        dropButton.onClick.RemoveListener(OnInteractDropButton);
    }

    private void UpdateUIRequiredItem(MissingRequiredItemEvent e)
    {
        Tween.StopAll(txtRequiredItem);
        
        txtRequiredItem.gameObject.SetActive(true);
        txtRequiredItem.alpha = 1f;
        
        txtRequiredItem.text = "Need " + e.message;
        Tween.Alpha(txtRequiredItem, 1f, 0f, 1.5f).OnComplete(() =>
        {
            txtRequiredItem.gameObject.SetActive(false);
        });
    }
    private void UpdateUIWhenFocusItem(FocusItemEvent e)
    {
        txtDescription.text = e.txtDescriptiopn;
    }
    private void UpdateUISelectedItem(SelectedItemEvent e)
    {
        dropButton.gameObject.SetActive(e.obj != null);
    }
    private void UpdateUIObjectInteractableChanged(ObjectInteractableChangedEvent e)
    {
        txtDescription.gameObject.SetActive(e.obj != null);
        objInteractable = e.obj;
        handButton.gameObject.SetActive(objInteractable != null);
    }

    private void OnInteractDropButton()
    {
        Gr_PlayerHolder.Instance.Drop();
        dropButton.gameObject.SetActive(false);
    }
    private void OnInteractHandButton()
    {
        objInteractable?.Interact();
    }
}
