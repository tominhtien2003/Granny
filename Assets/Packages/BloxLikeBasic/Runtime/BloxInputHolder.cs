using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloxInputHolder 
{
    public Vector3 moveInputVector;
    public Vector3 ExternalPlanarDirection;
    public Vector3 ExternalUpDirection = Vector3.up;
    public Vector3 ExternalForwardDirection = Vector3.forward;

    public bool JumpDown;
    public bool CrouchDown;
    public bool CrouchUp;
    public bool Jetpack;
}
