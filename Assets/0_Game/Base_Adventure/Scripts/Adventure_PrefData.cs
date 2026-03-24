using UnityEngine;

public partial class PrefData
{
    public static int GetUser_CurrentCheckpointParkour(string mapName)
    {
        return PlayerPrefs.GetInt($"user_checkpoint_parkour_{mapName}", 0);
    }

    public static void SetUser_CurrentCheckpointParkour(string mapName, int value)
    {
        PlayerPrefs.SetInt($"user_checkpoint_parkour_{mapName}", value);
    }
}
