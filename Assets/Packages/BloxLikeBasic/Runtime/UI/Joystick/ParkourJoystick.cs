using UnityEngine.EventSystems;
namespace BloxLikeBasic
{
    public class ParkourJoystick : Joystick
    {
        protected override void Start()
        {
            base.Start();
            background.gameObject.SetActive(false);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {

            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            base.OnPointerDown(eventData); 
            background.gameObject.SetActive(true);

        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);background.gameObject.SetActive(false);

        }
    }
}