using BloxLikeBasic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBloxBrain : BloxBrain
{
    private const string HorizontalInput = "Horizontal";
    private const string VerticalInput = "Vertical";

    public ParkourJoystick m_joystick;
    public override void OnGetInput()
    {
        base.OnGetInput();
        m_inputHolder.JumpDown |= Input.GetKeyDown(KeyCode.Space);
        m_inputHolder.moveInputVector = Vector3.ClampMagnitude(
            new Vector3(Input.GetAxisRaw(HorizontalInput) + m_joystick.Horizontal, 0f, 
            Input.GetAxisRaw(VerticalInput) + m_joystick.Vertical), 1f);
    }

    public virtual void GetInputMobile()
    {
        m_inputHolder.JumpDown = true;
    }
    public void EnterJetpack()
    {
        m_inputHolder.Jetpack = true;
    }
    public void ExitJetpack()
    {
        m_inputHolder.Jetpack = false;
    }

}
