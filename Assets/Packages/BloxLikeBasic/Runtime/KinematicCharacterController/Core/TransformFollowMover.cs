using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;

public class TransformFollowMover : MonoBehaviour, IMoverController
{
    public Transform tr;
    void Awake()
    {
        GetComponentInParent<PhysicsMover>().MoverController = this;
    }
    public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
    {
        // Rigidbody position/rotation changes are always applied at the end of the frame,
        // so here instead of simply setting the goalPosition to the followed rigidbody position,
        // we also anticipate the extra movement from its velocity. Same goes for rotation

        goalPosition = tr.position;
        goalRotation = tr.rotation;
    }
}