using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class CinemachineCameraTouchController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public float m_sensitivity = 1.0f;
    private Vector2 m_delta = Vector2.zero;
    private Vector2 m_trueDelta = Vector2.zero;
    private bool m_isDragging = false;
    
    public bool LockedY = false;
    
    void Awake()
    {
        CinemachineCore.GetInputAxis = GetAxisCustom;
    }
    

    void Update()
    {
        if (!m_isDragging) return;
        m_trueDelta = m_delta;
        m_delta = Vector2.zero;
    }
    public float GetAxisCustom(string axisName)
    {
        if (axisName == "Mouse X")
        {
            return m_trueDelta.x;
        }
        else if (axisName == "Mouse Y")
        {
            return m_trueDelta.y;
        }
        return Input.GetAxis(axisName);
    }
    public void OnDrag(PointerEventData eventData)
    {
        m_delta = eventData.delta * m_sensitivity;
        if(LockedY) m_delta.y = 0;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        m_trueDelta = Vector2.zero;
        m_isDragging = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_isDragging = true;
    }
}
