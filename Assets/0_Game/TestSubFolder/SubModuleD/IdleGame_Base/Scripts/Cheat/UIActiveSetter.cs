
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIActiveSetter : MonoBehaviour//, IPointerDownHandler
{
#if CHEAT
    private int clicks = 0;
    private float m_lastClickTime;
    public List<GameObject> canvasGroupObject;
    private List<CanvasGroup> m_uis = new List<CanvasGroup>();

    private bool isHide;
    
    /*public void OnPointerDown(PointerEventData data)
    {
        float curr = Time.time;
        if (clicks == 1 && curr - m_lastClickTime < .5f)
        {
            foreach (CanvasGroup canv in m_uis) canv.alpha =1 - canv.alpha;
            clicks = 0;
            return;
        }

        m_lastClickTime = curr;
        clicks = 1;
    }*/
    
    public void ToggleUI()
    {
        if (m_uis.Count == 0)
        {
            foreach (var ca in canvasGroupObject)
            {
                if(ca == null) continue;
                var canvasG =  ca.AddComponent<CanvasGroup>();
                m_uis.Add(canvasG);
                canvasG.ignoreParentGroups = true;
            }
        }
        
        
        isHide = !isHide;
        var alpha = isHide ? 1 / 255f : 1;
        foreach (CanvasGroup canv in m_uis)
        {
            canv.alpha = alpha;
        }
    }
    
#endif
}
