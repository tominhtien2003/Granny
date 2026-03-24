using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBloxState
{
    public void OnStateEnter();
    public void OnStateUpdate(float dt);
    public void OnStateExit();
}
