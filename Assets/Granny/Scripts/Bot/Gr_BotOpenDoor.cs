using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gr_BotOpenDoor : MonoBehaviour
{
    private Gr_Door door;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            //Debug.Log("Oke");
            door = other.GetComponentInParent<Gr_Door>();
            if (door != null)
            {
                door.OpenObj();
            }
        }
    }
}
