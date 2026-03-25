using PrimeTween;
using UnityEngine;

public class Gr_Door : Adventure_InteractObj
{
    [SerializeField] private Vector3 direct;

    private Collider colDoor;
    
    private const string actionOpen = "Open";
    private const string actionClose = "Close";

    private Vector3 start, end;

    private bool isOpened = false;
    private bool isMoving = false;
    protected override void OnTriggerEnter_Custom(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInside = true;
            DisplayBtn();
        }
    }
    protected override void OnTriggerExit_Custom(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInside = false;
            HideBtn();
        }
    }
    protected override void Awake()
    {
        base.Awake();
        colDoor = GetComponent<Collider>();
    }
    protected override void Start()
    {
        base.Start();
        start = transform.localEulerAngles;
        end = start + direct * 90;
    }
    public void DisplayBtn()
    {
        ToggleOutline(true);
        IndicatorController.Instance.DisplayClickBtn(indicatorTransform, isOpened ? actionClose : actionOpen, G4InteractAction, false);
    }
    public void HideBtn()
    {
        ToggleOutline(false);
        IndicatorController.Instance.HideClickBtn(true);
    }
    private void OpenDoor()
    {
        Tween.LocalEulerAngles(transform, start, end, .5f, Ease.Linear).OnComplete(() =>
        {
            isMoving = false;
            colDoor.enabled = true;
            if (_playerInside) DisplayBtn();
        });
        isOpened = true;
    }
    private void CloseDoor()
    {
        Tween.LocalEulerAngles(transform, end, start, .5f, Ease.Linear).OnComplete(() =>
        {
            isMoving = false;
            colDoor.enabled = true;
            if (_playerInside) DisplayBtn();
        });
        isOpened = false;
    }
    public override void Interact()
    {
        if (!_playerInside || isMoving) return;
        ToggleOutline(false);
        DisplayBtn();

        isMoving = true;
        colDoor.enabled = false;
        if (isOpened)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }
}
