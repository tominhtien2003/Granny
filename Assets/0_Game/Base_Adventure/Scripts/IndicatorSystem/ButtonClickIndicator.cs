using TMPro;
using UnityEngine.EventSystems;

public class ButtonClickIndicator : ButtonIndicator, IPointerClickHandler
{
    public TMP_Text[] m_subNames;
    public void OnPointerClick(PointerEventData eventData)
    {
        Completed();
    }
}
