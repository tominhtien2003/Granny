using System.Collections;
using UnityEngine;

public class G3_Manager : Adventure_GameManager
{
    public G3_BotFollowFloor bot;
    public float introDuration = 2f;
    [SerializeField] private G3_Door[] doorOpenStart;
    [SerializeField] private bool gameStarted = false;
    [SerializeField] private bool isOnCar;
    [SerializeField] private bool isInCombat;
    public bool IsOnCar
    {
        get {  return isOnCar; }
        set { isOnCar = value; }
    }
    public bool IsInCombat
    {
        get {  return isInCombat; }
        set { isInCombat = value; }
    }
    public static new G3_Manager Instance { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    protected override void Start()
    {
        base.Start();
        cpManager.OnPlayerRespawnEvent += ActiveDeep;
        G3_UIManager.Instance.noti1.gameObject.SetActive(true);
    }
    protected override void OnPlayerRespawn()
    {
        if (Adventure_CameraSystem.Instance != null)
        {
            if (IsInCombat)
            {
                Adventure_CameraSystem.Instance.ResetCamera(true);
            }
        }
        if (IsInCombat)
        {
            gunObject.gameObject.SetActive(true);
            G3_UIManager.Instance.SetUICombat(true);
        }
    }
    protected override void OnPlayerDeath()
    {
        base.OnPlayerDeath();
        if (IsInCombat)
        {
            gunObject.gameObject.SetActive(false);
            G3_UIManager.Instance.SetUICombat(false);
        }
        if (isOnCar)
        {
            carExplosion.Play();
        }
    }

    [Header("CakeSteal")]
    [SerializeField] G3_Cake cake;
    public void TriggerStart()
    {
        if (gameStarted) return;
        gameStarted = true;

        StartCoroutine(GameIntro());
    }
    IEnumerator GameIntro()
    {
        yield return new WaitForSeconds(3f);
        bot.gameObject.SetActive(true);
        bot.EnableChase(false);
        G3_UIManager.Instance.uiDeep.SetActive(false);
        G3_UIManager.Instance.SetUIWarning(true);
        foreach (var door in doorOpenStart)
        {
            door.OpenDoor();
        }

        cake.DisableCake();
        yield return new WaitForSeconds(.5f);
        Adventure_CameraSystem.Instance.ZoomToBot(false);
        bot.animator.Play("Angry");
        G3_AudioManager.Instance.PlayGrannyScreamSound();
        G3_UIManager.Instance.noti1.gameObject.SetActive(false);
        yield return new WaitForSeconds(introDuration);
        G3_UIManager.Instance.uiDeep.SetActive(true);
        cpManager.TeleportPlayerToCheckpoint(1);
        G3_UIManager.Instance.noti2.gameObject.SetActive(true);
        Adventure_CameraSystem.Instance.ResetCamera(false);
        bot.EnableChase(true);
    }

    [Header("LastCombat")]
    public G3_GunController gunObject;
    [SerializeField] GameObject wallCombat, lastTruss;
    [SerializeField] int botAmountTarget, botDestroyed;
    public void EnableGunMode()
    {
        Adventure_CameraSystem.Instance.EnableGunCam();
        gunObject.gameObject.SetActive(true);
        gunObject.EnableGun();
        G3_BotSpawner.Instance.StartSpawning();
        G3_UIManager.Instance.SetUICombat(true);
        IsInCombat = true;
        wallCombat.SetActive(true);
    }
    public void UpdateBotDestroyed()
    {
        botDestroyed++;
        G3_UIManager.Instance.UpdateBotRate((float)botDestroyed/botAmountTarget);
        if(botDestroyed == botAmountTarget)
        {
            Adventure_CameraSystem.Instance.ResetCamera(false);
            G3_BotSpawner.Instance.StopSpawning();
            G3_UIManager.Instance.SetUICombat(false);
            gunObject.gameObject.SetActive(false);
            wallCombat.SetActive(false);
            lastTruss.SetActive(true);
            IsInCombat = false;
            ParticleManager.Instance.PlayConfetti();
            AudioManager.Instance.PlayCongrats();
        }
    }

    void ActiveDeep()
    {
        G3_UIManager.Instance.uiDeep.SetActive(true);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        cpManager.OnPlayerRespawnEvent -= ActiveDeep;
    }
}
