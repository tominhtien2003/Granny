using BloxLikeBasic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
#if FIREBASE_AVAILABLE
using Firebase.Analytics;
#endif


public interface IOnPlayerDeath
{
    public UnityAction OnPlayerDeathEvent { get; set; }
}
public interface IOnPlayerRespawn
{
    public UnityAction OnPlayerRespawnEvent { get; set; }
}
public interface IOnPlayerReachedCheckpoint
{
    public UnityAction OnPlayerReachedNewCheckpointEvent { get; set; }
    public UnityAction OnPlayerCheckpointSet { get; set; }
    public CheckpointData GetPlayerNextCheckpoint();
    public int GetCurrentCheckpoint();
    public int GetMaxCheckpoint();
}
public class CheckpointManager : MonoBehaviour, IOnPlayerDeath, IOnPlayerRespawn, IOnPlayerReachedCheckpoint
{
    public GameObject[] Checkpoints;
    public List<WaypointScript> Waypoints = new();
    Dictionary<GameObject, CheckpointData> m_checkpointDict;


    public CharacterCheckpointData m_player;
    public CharacterCheckpointData[] m_bots;

    [Header("Bot spawn range")]
    public int MaxBehind;
    public int MaxAhead;
    public int MaxAfterLastCheckpoint = 2;

    public UnityAction OnPlayerDeathEvent { get; set; }
    public UnityAction OnPlayerReachedNewCheckpointEvent { get; set; }
    public UnityAction OnPlayerCheckpointSet { get; set; }
    public UnityAction OnPlayerRespawnEvent { get; set; }

    private Coroutine m_respawnCoroutine = null;

    [Header("Save Checkpoint map Parkour")]
    public bool saveCheckpoint = false;
    public string mapName;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        foreach (var v in Waypoints) if (v != null) Gizmos.DrawRay(v.transform.position, Vector3.up * 100f);
    }
    private void Awake()
    {
        if (saveCheckpoint)
        {
            m_player.CurrentCheckpointId = PrefData.GetUser_CurrentCheckpointParkour(mapName);
        }
        m_checkpointDict = new();
        for (int i = 0; i < Checkpoints.Length; i++)
        {
            WaypointScript w = null;
            if (i < Waypoints.Count) w = Waypoints[i];
            m_checkpointDict.Add(Checkpoints[i], new CheckpointData(i, w, Checkpoints[i].transform));
        }
        m_player.GetComponentInParent<Parkour_CheckpointEvents>().OnCheckpointReached += OnCheckpointReached;
        m_player.GetComponentInParent<Parkour_DeathEvent>().OnDeath += OnDeath;
        foreach (var v in m_player.GetComponents<ICheckpointAction>()) m_player.m_checkpointActions.Add(v);



        foreach (var b in m_bots)
        {
            b.GetComponentInParent<Parkour_CheckpointEvents>().OnCheckpointReached += OnCheckpointReached;
            b.GetComponentInParent<Parkour_DeathEvent>().OnDeath += OnDeath;
            foreach (var v in b.GetComponents<ICheckpointAction>()) b.m_checkpointActions.Add(v);
        }
    }
    public void InitTeleport()
    {
        SetCheckpointActions(m_player);
        OnPlayerCheckpointSet?.Invoke();
        foreach (var b in m_bots) SetCheckpointActions(b);
    }
    private void Start()
    {
        SetCheckpointActions(m_player);
        OnPlayerCheckpointSet?.Invoke();

        if (saveCheckpoint)
        {
            for (int i = 0; i < m_player.CurrentCheckpointId; i++)
            {
                Checkpoints[i].GetComponent<Adventure_FlagCheckpoint>().SetReached();
            }
            if (m_player.CurrentCheckpointId > 0)
            {
                TeleportBot();
            }
        }
        else
        {
            foreach (var b in m_bots) SetCheckpointActions(b);
        }
    }
    public CheckpointData GetPlayerNextCheckpoint()
    {
        if (m_player.CurrentCheckpointId + 1 >= Checkpoints.Length) return null;
        return m_checkpointDict[Checkpoints[m_player.CurrentCheckpointId + 1]];
    }

    private void OnDeath(CharacterCheckpointData data)
    {
        if (data == m_player)
        {
            if (m_respawnCoroutine != null) StopCoroutine(m_respawnCoroutine);
            m_respawnCoroutine = StartCoroutine(CharacterRespawn(data));
            OnPlayerDeathEvent?.Invoke();

#if ADS_AVAILABLE
            AdsAdapter.Instance.ShowInterstitialAdsBreak(Mediation_Manager.GameID(), AdsAdapter.@where.inter_player_respawn);
#endif
#if FIREBASE_AVAILABLE
            TrackingEvent.LogFirebase(Consts.player_dead, new Parameter[]
            {
                new Parameter(Consts.CheckPointID, data.CurrentCheckpointId.ToString())
            });
#endif    
        }

        else StartCoroutine(CharacterRespawn(data));
    }
    IEnumerator CharacterRespawn(CharacterCheckpointData data)
    {
        yield return new WaitForSeconds(2f);
        if (data != m_player) SetAIPosition(data);
        else
        {
            OnPlayerRespawnEvent?.Invoke();
            m_respawnCoroutine = null;
        }
        SetCheckpointActions(data);
    }
    public void SetCheckpointActions(CharacterCheckpointData data)
    {
        foreach (var v in data.m_checkpointActions) v.CheckpointAction(m_checkpointDict[Checkpoints[data.CurrentCheckpointId]]);
        data.m_deathToggler.IsDead = false;
    }
    private void OnCheckpointReached(CharacterCheckpointData data, GameObject go)
    {
        if (!m_checkpointDict.TryGetValue(go, out var v))
        {
#if UNITY_EDITOR
            Debug.LogError("Checkpoint not found in dictionary", go);
#endif
            return;
        }
        if (data.CurrentCheckpointId >= m_checkpointDict[go].CheckpointId) return;

        data.CurrentCheckpointId = m_checkpointDict[go].CheckpointId;


        if (data == m_player)
        {
#if ADS_AVAILABLE
        AdsAdapter.Instance.ShowInterstitialAdsBreak(Mediation_Manager.GameID(), AdsAdapter.where.inter_checkpoint_reached);
#endif
#if FIREBASE_AVAILABLE
            TrackingEvent.LogFirebase(Consts.player_checkpoint_reached, new Parameter[]
            {
                new Parameter(Consts.CheckPointID, data.CurrentCheckpointId)
            });
#endif

            OnPlayerReachedNewCheckpointEvent?.Invoke();
            OnPlayerCheckpointSet?.Invoke();
            SaveCurrentCheckpoint(data.CurrentCheckpointId);
        }
    }
    public void TeleportBot()
    {
        foreach(CharacterCheckpointData bot in m_bots)
        {
            if (!bot.isTeleportToPlayerFar) return;
            if (Mathf.Abs(bot.CurrentCheckpointId - m_player.CurrentCheckpointId) > 2)
            {
                int rand = Random.Range(m_player.CurrentCheckpointId - 1, m_player.CurrentCheckpointId + 1);
                rand = Mathf.Clamp(rand, 0, Checkpoints.Length - MaxAfterLastCheckpoint);
                TeleportToCheckpoint(bot, rand);
            }
        }
    }
    public void SaveCurrentCheckpoint(int id)
    {
        if (saveCheckpoint)
        {
            PrefData.SetUser_CurrentCheckpointParkour(mapName, id);
        }
    }
    #region TELEPORT
    public void TeleportToCheckpoint(CharacterCheckpointData data, int cp)
    {
        if (cp >= Checkpoints.Length) return;
        data.CurrentCheckpointId = cp;
        if (data == m_player)
        {
            OnPlayerReachedNewCheckpointEvent?.Invoke();
            OnPlayerCheckpointSet?.Invoke();
            SaveCurrentCheckpoint(data.CurrentCheckpointId);
        }
        SetCheckpointActions(data);
    }
    public void TeleportPlayerToCheckpoint(int cp)
    {
        if (m_respawnCoroutine != null)
        {
            StopCoroutine(m_respawnCoroutine);
        }
        TeleportToCheckpoint(m_player, cp);
        //SaveCurrentCheckpoint(cp);
    }
    public void Reward_TeleportPlayerToNextCheckpoint()
    {
        if (m_player.CurrentCheckpointId >= Checkpoints.Length - 1) return;
        //Watch ad here, and if succeed:
#if ADS_AVAILABLE
        AdsAdapter.Instance.ShowRewardedVideo(Mediation_Manager.GameID(), () =>
        {
#if FIREBASE_AVAILABLE
            TrackingEvent.LogFirebase(Consts.rw_skip_checkpoint, new Parameter[]
            {
                new Parameter(Consts.CheckPointID, m_player.CurrentCheckpointId.ToString())
            });
#endif

            TeleportPlayerToCheckpoint(m_player.CurrentCheckpointId + 1);
        }, Consts.NotiAdsFail, AdsAdapter.@where.rw_skip_checkpoint);
#elif NO_ADS
        TeleportPlayerToCheckpoint(m_player.CurrentCheckpointId + 1);
#else
        Consts.NotiNoAds();
#endif


    }
    public void TeleportPlayerToNextCheckpoint()
    {
        if (m_player.CurrentCheckpointId >= Checkpoints.Length - 1) return;
        //Watch ad here, and if succeed:
#if ADS_AVAILABLE
        AdsAdapter.Instance.ShowRewardedVideo(Mediation_Manager.GameID(), () =>
        {
            Reward_NextCheckpoint();
        }, Consts.NotiAdsFail, AdsAdapter.@where.rw_skip_checkpoint);
#elif NO_ADS

#else
    Consts.NotiNoAds();
#endif
    }

    private void Reward_NextCheckpoint()
    {
#if FIREBASE_AVAILABLE
        TrackingEvent.LogFirebase(Consts.rw_skip_checkpoint, new Parameter[]
        {
            new Parameter(Consts.CheckPointID, (m_player.CurrentCheckpointId + 1).ToString())
        });
#endif

        TeleportPlayerToCheckpoint(m_player.CurrentCheckpointId + 1);
    }

    #endregion
    public int GetCheckpointId(GameObject checkpoint)
    {
        return m_checkpointDict[checkpoint].CheckpointId;
    }

    void SetAIPosition(CharacterCheckpointData data)
    {
        int diff = data.CurrentCheckpointId - m_player.CurrentCheckpointId;
        int rand = 0;
        if (diff < 0 || diff >= 3) rand = Random.Range(m_player.CurrentCheckpointId - MaxBehind, m_player.CurrentCheckpointId + MaxAhead);
        else rand = data.CurrentCheckpointId;

        rand = Mathf.Clamp(rand, 0, Checkpoints.Length - MaxAfterLastCheckpoint);

        data.CurrentCheckpointId = rand;
    }

    public int GetCurrentCheckpoint()
    {
        return m_player.CurrentCheckpointId;
    }

    public int GetMaxCheckpoint()
    {
        return Checkpoints.Length;
    }
}
