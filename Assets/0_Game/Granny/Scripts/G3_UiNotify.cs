using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G3_UiNotify : MonoBehaviour
{
    public GameObject successMark;

    public void Suscess()
    {
        successMark.SetActive(true);
    }
}
