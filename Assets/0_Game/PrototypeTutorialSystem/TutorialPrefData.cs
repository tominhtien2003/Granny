using Cysharp.Text;
using UnityEngine;

public static class TutorialPrefData 
{
    public static void SetTutorialState(int tutorialId)
    {
        PlayerPrefs.SetInt(ZString.Format("tutorial_{0}", PrefData.CurrentMinigame), tutorialId);
    }

    public static int GetTutorialState()
    {
        return PlayerPrefs.GetInt(ZString.Format("tutorial_{0}", PrefData.CurrentMinigame), 0);
    }

}
