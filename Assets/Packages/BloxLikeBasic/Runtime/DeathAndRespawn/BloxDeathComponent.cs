using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloxDeathComponent : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Kill"))
        {
            // Handle death logic here
            Debug.Log("Player has died!");
            // You can add more logic here, like respawning the player or playing a death animation.
        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Kill"))
        {
            // Handle death logic here
            Debug.Log("Player has died! trigger");
            // You can add more logic here, like respawning the player or playing a death animation.
        }
    }
}
