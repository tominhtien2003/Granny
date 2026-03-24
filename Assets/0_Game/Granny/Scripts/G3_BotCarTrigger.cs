using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G3_BotCarTrigger : MonoBehaviour
{
    public bool IsOnCar;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            IsOnCar = false;
        }
    }
}
