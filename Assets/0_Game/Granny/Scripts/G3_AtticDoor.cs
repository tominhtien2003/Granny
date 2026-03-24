using UnityEngine;

public class G3_AtticDoor : MonoBehaviour
{
    public Animator anim;

    [SerializeField] WaypointScript waypointWait;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Respawn"))
        {
            OpenDoor();
            other.GetComponent<Adventure_ItemKey>().Use();
        }
    }
    public void OpenDoor()
    {
        GetComponent<Collider>().enabled = false; 
        anim.Play("Open");
        waypointWait.CanMove = true;
    }
}
