using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToLobbyButton : MonoBehaviour
{
    public ButtonEffectLogic m_returnButton;
    public PunchWallScript m_punchWallScript;
    private void Awake()
    {
        m_returnButton.onClick.AddListener(m_punchWallScript.ReturnToLobby);
    }
}
