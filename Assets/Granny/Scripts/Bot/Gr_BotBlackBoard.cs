using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gr_BotBlackBoard
{
    public Transform PlayerTransform;
    public bool CanSeePlayer;

    public bool CanAttack;
    
    public bool HasHeardSound;
    public Vector3 HeardPosition;
    
    
    public Waypoint CurrentWaypoint;

    public void ClearSoundData()
    {
        HasHeardSound = false;
        HeardPosition = Vector3.zero;
    }
}
