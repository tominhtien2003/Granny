using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventure_ElevatorDoor : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float doorAnimTime = 1f;

    public IEnumerator Open()
    {
        animator.Play("Open");
        yield return new WaitForSeconds(doorAnimTime);
    }

    public IEnumerator Close()
    {
        yield return new WaitForSeconds(doorAnimTime);
        animator.Play("Close");
        yield return new WaitForSeconds(doorAnimTime);
    }
}
