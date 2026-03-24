using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventure_FlagCheckpoint : MonoBehaviour
{
    bool isCheckpointReached = false;
    [SerializeField] List<MeshRenderer> flagRenderer;
    [SerializeField] Material greenMaterial;
    [SerializeField] GameObject particle;
    private void OnTriggerEnter(Collider other)
    {
        if (isCheckpointReached) return;
        if (other.CompareTag("Player"))
        {
            SetReached();
        }
    }
    public void SetReached()
    {
        isCheckpointReached = true;
        foreach (MeshRenderer renderer in flagRenderer)
        {
            renderer.material = greenMaterial;
        }
        if (particle != null)
        {
            particle.gameObject.SetActive(true);
        }
    }
}
