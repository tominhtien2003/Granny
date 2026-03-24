using LitMotion;
using LitMotion.Extensions;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PunchWall_ValueReceiveAnimator : MonoBehaviour
{
    public TMP_Text m_valueTxt;
    public Transform m_targetMove;
    public List<Transform> m_transformsList;
    private List<TMP_Text> m_valueList;
    private int m_index = 0;
    public List<MotionHandle> m_handles;
    public Image m_fillImage;

    public int m_extraGraphics = 0;
    private void Awake()
    {
        m_handles = new List<MotionHandle>();
        m_valueList = new List<TMP_Text>();
        for (int i = 0; i < m_transformsList.Count; i++)
        {
            m_handles.Add(new MotionHandle());
            m_valueList.Add(m_transformsList[i].GetComponentInChildren<TMP_Text>());
        }
    }
    
    public void SetValue(double val, double nextStage)
    {
        MoneyUIView.UpdateMoney(val, m_valueTxt);
        m_fillImage.fillAmount = (float) (val / nextStage);
    }
    
    public void ReceiveValue(double afterValue, double receiveValue = -1, double nextStage = -1)
    {
        double aft = afterValue;
       
        //Debug.Log("jiwajdfp " + m_index);
        m_index = (m_index + 1) % m_transformsList.Count;
        
        m_transformsList[m_index].gameObject.SetActive(true);
        if (receiveValue > 0)
        {
            m_valueList[m_index].enabled = true;
            MoneyUIView.UpdateMoney(receiveValue, m_valueList[m_index]);
        }
        int tindx = m_index;
        m_handles[tindx].TryComplete();
        m_handles[tindx] = LMotion.Create((Vector3)Random.insideUnitCircle * 200f + m_transformsList[tindx].parent.position, m_targetMove.position, .3f)
                                        .WithDelay(.5f)
                                        .WithOnComplete(() =>
                                        {
                                            MoneyUIView.UpdateMoney(aft, m_valueTxt);
                                            m_transformsList[tindx].gameObject.SetActive(false);
                                            if (nextStage >= 0 && m_fillImage != null) m_fillImage.fillAmount = (float)(afterValue / nextStage);
                                            //Debug.LogError("Complete");
                                        })
                                        .BindToPosition(m_transformsList[tindx]);
        for (int j = 1; j <= m_extraGraphics; j++)
        {
            int indx = (m_index + j) % m_transformsList.Count;
            m_handles[indx].TryComplete();
            m_valueList[indx].enabled = false;
            m_transformsList[indx].gameObject.SetActive(true);
            m_handles[indx] = LMotion.Create((Vector3)Random.insideUnitCircle * 200f + m_transformsList[indx].parent.position, m_targetMove.position, .3f)
                                        .WithDelay(.5f + .05f * j)
                                        .WithOnComplete(() =>m_transformsList[indx].gameObject.SetActive(false))
                                        .BindToPosition(m_transformsList[indx]);
        }
    }
    private void OnDisable()
    {
        foreach (MotionHandle handl in m_handles) handl.TryCancel();
    }
}
