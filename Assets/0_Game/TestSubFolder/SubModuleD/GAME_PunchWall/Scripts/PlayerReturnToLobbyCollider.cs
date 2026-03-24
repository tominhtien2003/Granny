using UnityEngine;

public class PlayerReturnToLobbyCollider : MonoBehaviour
{
    public PunchWallScript m_punchWallScript;
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        m_punchWallScript.SetReturnState(!(other.transform.position.x > transform.position.x));

    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        m_punchWallScript.SetReturnState(true);

    }
}
