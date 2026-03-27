using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gr_PlayerIneractWithDoor : MonoBehaviour
{
    public static Gr_PlayerIneractWithDoor Instance;

    [Header("Settings")]
    [SerializeField] private Transform viewTransform; 
    [SerializeField] private float viewAngleThreshold = 60f; 
    [SerializeField] private float checkInterval = 0.1f; 

    public List<Gr_Door> doorList = new List<Gr_Door>();
    private Gr_Door currentActiveDoor;
    private Coroutine checkRoutine;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (viewTransform == null) viewTransform = transform;
    }

    public void AddDoor(Gr_Door door)
    {
        if (!doorList.Contains(door))
        {
            doorList.Add(door);
            if (checkRoutine == null) checkRoutine = StartCoroutine(CheckViewRoutine());
        }
    }

    public void RemoveDoor(Gr_Door door)
    {
        doorList.Remove(door);
        if (currentActiveDoor == door)
        {
            //currentActiveDoor.HideBtn();
            currentActiveDoor = null;
        }

        if (doorList.Count == 0 && checkRoutine != null)
        {
            ClearCurrentActive();
            StopCoroutine(checkRoutine);
            checkRoutine = null;
        }
    }

    private IEnumerator CheckViewRoutine()
    {
        var wait = new WaitForSeconds(checkInterval);
        while (true)
        {
            UpdateActiveDoorByView();
            yield return wait;
        }
    }

    private void UpdateActiveDoorByView()
    {
        if (doorList.Count == 0)
        {
            ClearCurrentActive();
            return;
        }

        Gr_Door bestDoor = null;
        float maxDot = -1f;

        float dotThreshold = Mathf.Cos(viewAngleThreshold * 0.5f * Mathf.Deg2Rad);
        foreach (Gr_Door door in doorList)
        {
            Vector3 dirToDoor = (door.transform.position - viewTransform.position).normalized;


            float dot = Vector3.Dot(viewTransform.forward, dirToDoor);
            if (dot > dotThreshold) 
            {
                if (dot > maxDot)
                {
                    maxDot = dot;
                    bestDoor = door;
                }
            }
        }

        if (currentActiveDoor != bestDoor)
        {
            if (currentActiveDoor != null)
            {
                //currentActiveDoor.HideBtn();
            }
            currentActiveDoor = bestDoor;
            //if (currentActiveDoor != null) currentActiveDoor.DisplayBtn();
        }
    }
    private void ClearCurrentActive()
    {
        if (currentActiveDoor != null)
        {
            //currentActiveDoor.HideBtn();
            currentActiveDoor = null;
        }
    }
#if UNITY_EDITOR
    //private void OnDrawGizmos()
    //{
    //    if (viewTransform == null) return;

    //    // 1. Vẽ hướng nhìn thẳng của Player (Màu xanh dương)
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawRay(viewTransform.position, viewTransform.forward * 2f);

    //    // 2. Vẽ "Hình nón" góc nhìn (Màu vàng)
    //    Gizmos.color = Color.yellow;
    //    float halfAngle = viewAngleThreshold * 0.5f;

    //    // Tính toán 2 đường biên của góc nhìn
    //    Vector3 leftRayDirection = Quaternion.Euler(0, -halfAngle, 0) * viewTransform.forward;
    //    Vector3 rightRayDirection = Quaternion.Euler(0, halfAngle, 0) * viewTransform.forward;

    //    Gizmos.DrawRay(viewTransform.position, leftRayDirection * 3f);
    //    Gizmos.DrawRay(viewTransform.position, rightRayDirection * 3f);

    //    // 3. Vẽ đường nối tới các cửa trong danh sách
    //    foreach (var door in doorList)
    //    {
    //        if (door == null) continue;

    //        if (door == currentActiveDoor)
    //        {
    //            // Cửa đang được chọn (Màu xanh lá)
    //            Gizmos.color = Color.black;
    //            Gizmos.DrawLine(viewTransform.position, door.transform.position);
    //            Gizmos.DrawWireSphere(door.transform.position, 0.5f);
    //        }
    //        else
    //        {
    //            // Cửa nằm trong vùng Trigger nhưng không được chọn (Màu đỏ)
    //            Gizmos.color = Color.red;
    //            Gizmos.DrawLine(viewTransform.position, door.transform.position);
    //        }
    //    }
    //}
#endif
}