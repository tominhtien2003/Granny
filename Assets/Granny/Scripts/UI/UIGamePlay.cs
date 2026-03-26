using UnityEngine;
using UnityEngine.UI;

public class UIGamePlay : MonoBehaviour
{
    [SerializeField] private Button handButton;

    private Gr_PlayerInteractor playerInteractor;
    private void Start()
    {
        handButton.onClick.AddListener(OnInteract);
        handButton.gameObject.SetActive(false);

        playerInteractor = TransformAnchorManager.Instance.GetTransformPlayer().GetComponentInChildren<Gr_PlayerInteractor>();
        playerInteractor.OnObjectInteractChanged += UpdateUIHandButton;
    }
    private void Update()
    {
    }
    private void UpdateUIHandButton(Gr_IInteractable obj)
    {
        handButton.gameObject.SetActive(obj != null);
    }
    public void OnInteract()
    {
        playerInteractor.Interact();
    }
    private void OnDisable()
    {
        playerInteractor.OnObjectInteractChanged -= UpdateUIHandButton;
    }
}
