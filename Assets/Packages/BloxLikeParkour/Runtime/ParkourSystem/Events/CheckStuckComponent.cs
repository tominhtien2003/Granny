using KinematicCharacterController;
using UnityEngine;

[System.Serializable]
public class CheckStuckComponent
{
    [SerializeField] float checkInterval = 1f;
    [SerializeField] float minDistancePerSecond = 1f;
    [SerializeField] float stuckTimeToDie = 3f;

    Vector3 _lastCheckPos;
    float _checkTimer;
    float _stuckTimer;
    public bool IgnoreY = true;
    public Transform transform;
    public KinematicCharacterMotor Motor;
    public void Reset()
    {
        _checkTimer = 0;
        _stuckTimer = 0;
        _lastCheckPos = transform.position;
    }
    public CheckStuckComponent(Transform tr)
    {
        transform = tr;
       
    }
    public CheckStuckComponent(Transform tr, float checkInterval, float minDis, float stuckTime)
    {
        transform = tr;
        _lastCheckPos = transform.position;
        this.checkInterval = checkInterval;
        this.minDistancePerSecond = minDis;
        stuckTimeToDie = stuckTime;
    }
    public bool CheckStuck(float deltaTime)
    {
        _checkTimer += deltaTime;
        if (_checkTimer < checkInterval)
            return false;

        _checkTimer = 0f;

        Vector3 currentPos = transform.position;

        /*float flatDistance = Vector2.Distance(
            new Vector2(_lastCheckPos.x, _lastCheckPos.z),
            new Vector2(currentPos.x, currentPos.z)
        );*/

        Vector3 dis = _lastCheckPos - currentPos;
        if (IgnoreY) dis.y = 0;
        float flatDistance = dis.magnitude;
        bool isTryingToMove = Motor.Velocity.sqrMagnitude > 0.04f;
        bool isStuck = flatDistance < minDistancePerSecond;

        if (isTryingToMove && isStuck)
        {
            _stuckTimer += checkInterval;

            if (_stuckTimer >= stuckTimeToDie)
            {
                _lastCheckPos = currentPos;
                _stuckTimer = 0f;
                return true;
            }
        }
        else
        {
            _stuckTimer = 0f;
        }

        _lastCheckPos = currentPos;
        return false;
    }
}
