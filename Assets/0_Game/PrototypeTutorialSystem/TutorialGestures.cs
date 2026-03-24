using UnityEngine;

public class TutorialGestures: MonoBehaviour
{
    public GameObject m_tapGesture;
    public GameObject m_pointGesture;
    public static TutorialGestures Instance;
    public GuideLine m_line;

    private Transform m_ogPointParent;
    private void Awake()
    {
        Instance = this;
        m_ogPointParent = m_pointGesture.transform.parent;
    }
    public void SetLine(Transform a, Transform b)
    {
        if (!m_line.gameObject.activeSelf) m_line.gameObject.SetActive(true);
        m_line.SetDes(a, b, true);
    }
    public void DisableLine()
    {
        m_line.gameObject.SetActive(false);
    }
    public void SetTapAtParented(Transform t)
    {
        m_pointGesture.transform.SetParent(t);
        m_pointGesture.transform.localPosition = Vector3.zero;
        m_pointGesture.transform.localScale = Vector3.one;
        m_pointGesture.SetActive(true);
    }

    public void DisableTapAtParented()
    {
        m_pointGesture.transform.SetParent(m_ogPointParent);
        m_pointGesture.transform.localScale = Vector3.one * 2.5f;
        m_pointGesture.SetActive(false);
    }
    public void SetTapAt(Transform t, bool inverted = false)
    {
        m_pointGesture.transform.position = t.position;
        if (inverted != (m_pointGesture.transform.localScale.x < 0))
        {
            Vector3 p = m_pointGesture.transform.localScale;
            p.x *= -1;
            m_pointGesture.transform.localScale = p;
        }
        m_pointGesture.SetActive(true);
    }
    public void SetTapAt(Vector3 pos)
    {
        m_pointGesture.transform.position = pos;
        m_pointGesture.SetActive(true);
    }
    public void DisableTapAt()
    {
        m_pointGesture.SetActive(false);
    }
    public void SetTapGesture(bool state)
    {
        if (m_tapGesture.activeSelf != state) m_tapGesture.SetActive(state);
    }
    public void ResetAll()
    {
        m_pointGesture.SetActive(false);
        DisableLine();
    }

}