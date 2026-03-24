using KinematicCharacterController;
using UnityEngine;

public enum FuelState { NONE, CHARGING, DRAINING }

public class JetpackFuel : MonoBehaviour
{
    [Header("Fuel Settings")]
    public float maxFuel = 8f;           
    public float currentFuel;
    public float consumeRate;        
    public float rechargeRate;    

    [HideInInspector] public FuelState fuelState = FuelState.NONE;
    public KinematicCharacterMotor motor;
    public Jetpack jetpackState;

    private void Start()
    {
        jetpackState = GetComponent<Jetpack>();
        if (jetpackState != null)
        {
            jetpackState.onJetpackStarted.AddListener(OnJetpackStarted);
            jetpackState.onJetpackEnded.AddListener(OnJetpackEnded);
        }
        FillFuel();
    }
    public void FillFuel()
    {
        currentFuel = maxFuel;
    }
    void OnJetpackStarted() { fuelState = FuelState.DRAINING; }
    void OnJetpackEnded() { fuelState = motor.GroundingStatus.IsStableOnGround ? FuelState.CHARGING : FuelState.NONE; }

    private void Update()
    {
        if (motor.GroundingStatus.IsStableOnGround && fuelState != FuelState.DRAINING)
            fuelState = FuelState.CHARGING;
        switch (fuelState)
        {
            case FuelState.DRAINING:
                currentFuel -= consumeRate * Time.deltaTime;
                break;
            case FuelState.CHARGING:
                currentFuel += rechargeRate * Time.deltaTime;
                break;
        }

        currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);
        if (jetpackState != null)
            jetpackState.canJetpack = currentFuel > 0.5f;
    }
}
