using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSetter : MonoBehaviour
{
    public Transform Arrow;
    private IOnPlayerReachedCheckpoint m_cpEvent;
    private void Awake()
    {
        m_cpEvent = GetComponent<IOnPlayerReachedCheckpoint>();
       m_cpEvent.OnPlayerCheckpointSet += SetArrowCP;
    }

    void SetArrowCP()
    {
        var v =  m_cpEvent.GetPlayerNextCheckpoint();
        if (v == null) Arrow.gameObject.SetActive(false);
        else
        {
            if (!Arrow.gameObject.activeSelf) Arrow.gameObject.SetActive(true);
            Arrow.position = v.m_transform.position + Vector3.up * 4f;
        }
    }
}
