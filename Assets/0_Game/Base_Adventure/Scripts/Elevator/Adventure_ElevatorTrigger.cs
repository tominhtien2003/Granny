using UnityEngine;

public class Adventure_ElevatorTrigger : MonoBehaviour
{
    Adventure_ElevatorController controller;

    public void Init(Adventure_ElevatorController c)
    {
        controller = c;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            controller.OnEnter(PlayerType.Player);
        else if (other.CompareTag("G3_Bot"))
            controller.OnEnter(PlayerType.Bot);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("G3_Bot"))
            controller.OnExit();
    }
}

public enum PlayerType
{
    Player,
    Bot
}
