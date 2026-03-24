using UnityEngine;

public abstract class BloxBrain : MonoBehaviour
{
    protected BloxInputHolder m_inputHolder;
    public BloxInputHolder InputHolder => m_inputHolder;
    public Transform m_externalRotationReference;
    protected bool m_useExternalReference = false;
    protected virtual void Awake()
    {
       
        m_inputHolder = new BloxInputHolder();
        if (m_externalRotationReference != null) m_useExternalReference = true;
        else m_inputHolder.ExternalPlanarDirection = Vector3.forward;
    }
    public virtual void OnGetInput() {
        if (m_useExternalReference)
        {
            m_inputHolder.ExternalPlanarDirection = Vector3.ProjectOnPlane(m_externalRotationReference.forward, Vector3.up);
            m_inputHolder.ExternalUpDirection = m_externalRotationReference.up;
            m_inputHolder.ExternalForwardDirection = m_externalRotationReference.forward;
        }
    }
}
