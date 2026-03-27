using UnityEngine;
using UnityEngine.UI;

public class UIGamePlay : MonoBehaviour
{
    [SerializeField] private Button handButton;
    private Gr_IInteractable objInteractable;

    private void OnEnable()
    {
        Gr_EventManager.AddListener<ObjectInteractableChangedEvent>(UpdateUIHandButton);
        handButton.onClick.AddListener(OnInteract);
    }
    private void OnDisable()
    {
        Gr_EventManager.RemoveListener<ObjectInteractableChangedEvent>(UpdateUIHandButton);
        handButton.onClick.RemoveListener(OnInteract);
    }
    private void Start()
    {
        handButton.gameObject.SetActive(false);
    }
    private void Update()
    {
    }
    private void UpdateUIHandButton(ObjectInteractableChangedEvent e)
    {
        objInteractable = e.obj;
        handButton.gameObject.SetActive(objInteractable != null);
    }
    public void OnInteract()
    {
        objInteractable.Interact();
    }
}
