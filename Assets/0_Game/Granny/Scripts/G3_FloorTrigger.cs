using UnityEngine;

public class G3_FloorTrigger : MonoBehaviour
{
    public FloorLevel zoneFloor;
    [SerializeField] G3_BotFollowFloor bot;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<G3_BotFollowFloor>() != null)
        {
            bot.SetBotFloor(zoneFloor);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && other.CompareTag("Player"))
        {
            bot.SetPlayerFloor(zoneFloor);
        }
    }
}