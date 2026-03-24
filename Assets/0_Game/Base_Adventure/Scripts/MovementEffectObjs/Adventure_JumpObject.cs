using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventure_JumpObject : MonoBehaviour
{
    [SerializeField] float multipleAirSpeed = 1.75f;
    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Adventure_JumpForceReceive>() != null)
        {
            other.GetComponent<Adventure_JumpForceReceive>().JumpForce(multipleAirSpeed);
        }
    }
}
