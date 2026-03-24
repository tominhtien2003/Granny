using UnityEngine;

public class Tutorial_DataController: MonoBehaviour
{
    public static Tutorial_DataController Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        m_currentTutorialStep = TutorialPrefData.GetTutorialState();
    }
    private static int m_currentTutorialStep;
    public static int CurrentTutorialStep
    {
        get => m_currentTutorialStep;
        set
        {
            m_currentTutorialStep = value;
            TutorialPrefData.SetTutorialState(m_currentTutorialStep);
        }
    }

    
}