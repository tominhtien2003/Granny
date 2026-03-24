using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class G3_BotSpawner : MonoBehaviour
{
    public static G3_BotSpawner Instance;

    public Transform[] spawnPoints;
    public Transform playerTransform;
    public float spawnRate = 1.5f;

    private bool spawning = false;
    public int maxBots = 5;
    private List<G3_BotHealth> activeBots = new List<G3_BotHealth>();
    void Awake() => Instance = this;

    private void Start()
    {
        G3_Manager.Instance.cpManager.OnPlayerRespawnEvent += DespawnAllBot;
        G3_Manager.Instance.cpManager.OnPlayerDeathEvent += StopAllBot;
    }
    public void StartSpawning()
    {
        InvokeRepeating(nameof(SpawnBot), 0, spawnRate);
    }
    public void StopSpawning()
    {
        DespawnAllBot();
        CancelInvoke(nameof(SpawnBot));
    }
    void SpawnBot()
    {
        if (activeBots.Count >= maxBots) return;
        G3_BotHealth bot = G3_PoolManager.Instance.GetBot();
        if (bot == null) return;
        activeBots.Add(bot);
        Transform p = spawnPoints[Random.Range(0, spawnPoints.Length)];
        G3_BotChase botChase = bot.GetComponent<G3_BotChase>();
        botChase.agent.enabled = false;
        botChase.player = playerTransform;
        botChase.canChase = true;
        bot.transform.position = p.position;
        botChase.camAttack.Priority = 0;
        botChase.SetState(BotState.Chase);
        bot.gameObject.SetActive(true);
        botChase.agent.enabled = true;
    }
    public void RemoveBot(G3_BotHealth bot)
    {
        activeBots.Remove(bot);
    }
    public void DespawnAllBot()
    {
        if (!G3_Manager.Instance.IsInCombat) return;

        for (int i = activeBots.Count - 1; i >= 0; i--)
        {
            G3_BotHealth bot = activeBots[i];
            G3_PoolManager.Instance.ReturnBot(bot);
            RemoveBot(bot);
        }
    }
    public void StopAllBot()
    {
        if (!G3_Manager.Instance.IsInCombat) return;
        foreach (var bot in activeBots)
        {
            bot.GetComponent<G3_BotChase>().Stop();
        }
    }
    private void OnDestroy()
    {
        G3_Manager.Instance.cpManager.OnPlayerRespawnEvent -= DespawnAllBot;
        G3_Manager.Instance.cpManager.OnPlayerDeathEvent -= StopAllBot;
    }
}
