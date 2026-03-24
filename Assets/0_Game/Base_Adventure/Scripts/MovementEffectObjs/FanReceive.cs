using BloxLikeBasic;
using KinematicCharacterController;
using System.Collections.Generic;
using UnityEngine;

public class FanReceive : MonoBehaviour
{
    private KinematicCharacterMotor _motor;
    [SerializeField] private NormalMovement _movement;

    private readonly HashSet<FanPhysicMover> _activeFans = new();

    void Awake()
    {
        _motor = GetComponent<KinematicCharacterMotor>();
        _motor.GetComponent<Parkour_DeathEvent>().OnDeath += ClearAllActiveFans;
    }

    void FixedUpdate()
    {
        if (_activeFans.Count == 0)
            return;

        bool grounded = _motor.GroundingStatus.IsStableOnGround;

        foreach (var fan in _activeFans)
        {
            if (fan == null) continue;

            if (grounded && !fan.affectGround) continue;
            if (!grounded && !fan.affectAir) continue;

            _movement.AddVelocity(
                fan.GetVelocity() * Time.fixedDeltaTime
            );
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out FanPhysicMover fan))
        {
            _activeFans.Add(fan);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out FanPhysicMover fan))
        {
            _activeFans.Remove(fan);
        }
    }
    public void ClearAllActiveFans(CharacterCheckpointData data)
    {
        _activeFans.Clear();
    }
    private void OnDestroy()
    {
        _motor.GetComponent<Parkour_DeathEvent>().OnDeath -= ClearAllActiveFans;
    }
}
