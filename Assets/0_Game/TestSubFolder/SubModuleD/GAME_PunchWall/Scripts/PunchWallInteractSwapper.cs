using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchWallInteractSwapper : MonoBehaviour
{
    public GameObject m_punchButton;
    public GameObject m_trainPanel;
    public GameObject m_trainButton;
    public GameObject m_trainLeaveButton;

    public void SetTrainState(bool isTraining)
    {
        m_punchButton.SetActive(!isTraining);
        m_trainPanel.SetActive(isTraining);
        m_trainButton.SetActive(isTraining);
        m_trainLeaveButton.SetActive(isTraining);
    }
}
