using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingPetMultiplierInjector : MonoBehaviour
{
    public PunchWallScript m_punchWall;

    public void Awake()
    {
        FindObjectOfType<InventoryListDisplayPopup>(true).m_petMultiplySetter = m_punchWall;
        Destroy(this);
    }

}
