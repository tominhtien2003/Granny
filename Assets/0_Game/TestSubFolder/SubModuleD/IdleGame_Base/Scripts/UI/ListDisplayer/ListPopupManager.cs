using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IInitable
{
    public void Init();
}
public class ListPopupManager : MonoBehaviour
{
    public IInitable[] m_childInitables;

    private void Awake()
    {
        m_childInitables = GetComponentsInChildren<IInitable>(true);
       
        
    }
    private void Start()
    {
        for (int i = 0; i < m_childInitables.Length; i++)
        {
            m_childInitables[i].Init();
        }
        
    }
}
