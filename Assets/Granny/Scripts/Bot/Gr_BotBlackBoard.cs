using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gr_BotBlackBoard
{
    public Transform PlayerTransform;
    public bool CanSeePlayer;

    public bool CanAttack;

    public bool HasLastKnownPlayerPosition;  
    public Vector3 LastKnownPlayerPosition; 
    
    public bool HasHeardSound;
    public Vector3 HeardPosition;
    
    
    public Waypoint CurrentWaypoint;

    public void ClearSoundData()
    {
        HasHeardSound = false;
        HeardPosition = Vector3.zero;
    }
    public void ClearLastKnownPosition()
    {
        HasLastKnownPlayerPosition = false;
        LastKnownPlayerPosition = Vector3.zero;
    }
}
