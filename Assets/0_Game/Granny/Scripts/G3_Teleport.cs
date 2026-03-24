using BloxLikeBasic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G3_Teleport : MonoBehaviour
{
    [Header("Teleport Settings")]
    [SerializeField] int checkpointIndex;
    [SerializeField] float teleportDelay = 3f;

    [Header("References")]
    [SerializeField] ParticleSystem teleportParticle;

    bool isTeleporting;

    private void OnTriggerEnter(Collider other)
    {
        if (isTeleporting) return;
        if (other.CompareTag("Player"))
        {
            StartCoroutine(IETeleport());
        }
    }

    IEnumerator IETeleport()
    {
        isTeleporting = true;
        G3_UIManager.Instance.uiDeep.SetActive(false);
        if (teleportParticle)
            teleportParticle.Play();
        if (G3_Manager.Instance.joystick != null)
        {
            G3_Manager.Instance.joystick.ResetJoystick();
        }
        yield return new WaitForSeconds(teleportDelay);

        TeleportPlayerToCheckpoint(checkpointIndex);
        G3_UIManager.Instance.uiDeep.SetActive(true);
        isTeleporting = false;
    }

    public void TeleportPlayerToCheckpoint(int cp)
    {
        G3_Manager.Instance.cpManager.TeleportPlayerToCheckpoint(cp);
    }
}
