#if UNITY_EDITOR
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeleportDebug : MonoBehaviour
{
    public Button m_buttonPrefab;
    public Transform m_spawnParent;

    public void Start()
    {
        GameObject[] g = GameObject.FindGameObjectsWithTag("DebugTransform");

        foreach (var v in g)
        {
            var b = Instantiate(m_buttonPrefab, m_spawnParent);
            b.GetComponentInChildren<TMP_Text>().SetText(v.name);
            Transform t = v.transform;
            //b.onClick.AddListener(() => BloxLikeUtility.Instance.TeleportPlayerTo(t));
        }
        m_buttonPrefab.gameObject.SetActive(false);
    }
}
#endif