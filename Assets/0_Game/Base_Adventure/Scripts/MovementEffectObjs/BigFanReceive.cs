using BloxLikeBasic;
using KinematicCharacterController;
using UnityEngine;

public class BigFanReceive : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] KinematicCharacterMotor motor;
    [SerializeField] NormalMovement movement;

    private float baseGravity;

    private int fanTriggerCount = 0;

    private Adventure_BigFanZone currentZone;

    private float targetY;

    private void Awake()
    {
        baseGravity = movement.Gravity.y;
    }
    private void Start()
    {
        if(Adventure_GameManager.Instance != null)
        {
            Adventure_GameManager.Instance.cpManager.OnPlayerDeathEvent += ResetFanState;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Adventure_BigFanZone zone = other.GetComponent<Adventure_BigFanZone>();

        if (zone != null)
        {
            fanTriggerCount++;
            currentZone = zone;

            if (fanTriggerCount == 1)
            {
                targetY = motor.TransientPosition.y + zone.hoverHeight;

                var g = movement.Gravity;
                g.y = 0f;
                movement.Gravity = g;

                motor.ForceUnground();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Adventure_BigFanZone zone = other.GetComponent<Adventure_BigFanZone>();

        if (zone != null)
        {
            fanTriggerCount = Mathf.Max(0, fanTriggerCount - 1);

            if (fanTriggerCount == 0)
            {
                currentZone = null;

                var g = movement.Gravity;
                g.y = baseGravity;
                movement.Gravity = g;
            }
        }
    }
    public void ResetFanState()
    {
        fanTriggerCount = 0;
        currentZone = null;

        var g = movement.Gravity;
        g.y = baseGravity;
        movement.Gravity = g;
    }

    private void Update()
    {
        if (fanTriggerCount <= 0 || currentZone == null)
            return;

        float currentY = motor.TransientPosition.y;
        float delta = targetY - currentY;

        Vector3 vel = motor.BaseVelocity;

        float liftSpeed = currentZone.liftSpeed;
        float damping = currentZone.hoverDamping;
        float oscillation = currentZone.oscillation;
        float oscSpeed = currentZone.oscillationSpeed;

        if (delta > 0.25f)
        {
            vel.y = liftSpeed;
        }
        else
        {
            float oscillate = Mathf.Sin(Time.time * oscSpeed) * oscillation;

            vel.y = Mathf.Lerp(vel.y, oscillate, damping * Time.deltaTime);
        }

        motor.BaseVelocity = vel;

        motor.ForceUnground();
    }
    private void OnDestroy()
    {
        if (Adventure_GameManager.Instance != null)
        {
            Adventure_GameManager.Instance.cpManager.OnPlayerDeathEvent -= ResetFanState;
        }
    }
}
