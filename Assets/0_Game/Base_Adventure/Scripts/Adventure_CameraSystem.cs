using UnityEngine;
using Cinemachine;

public class Adventure_CameraSystem : MonoBehaviour
{
    public static Adventure_CameraSystem Instance;

    public CinemachineFreeLook freeLook;
    public CinemachineVirtualCamera botCamAttack, botCamAngry, gunCam;
    [SerializeField] Transform playerTarget;
    void Awake()
    {
        Instance = this;
    }
    public void ZoomToBot(bool isAttack, CinemachineVirtualCamera cam = null)
    {
        if (isAttack)
        {
            botCamAttack = cam;
            botCamAttack.Priority = 10;
            botCamAngry.Priority = 0;
        }
        else
        {
            botCamAttack.Priority = 0;
            botCamAngry.Priority = 10;
        }
        freeLook.Priority = 0;
        playerTarget.parent.gameObject.SetActive(false);
    }
    public void EnableGunCam()
    {
        gunCam.Priority = 10;
        freeLook.Priority = 0;
    }
    public void ResetCamera(bool isInCombat)
    {
        botCamAngry.Priority = 0;
        botCamAttack.Priority = 0;
        if(isInCombat)
        {
            gunCam.Priority = 10;
            freeLook.Priority = 0;
        }
        else
        {
            gunCam.Priority = 0;
            freeLook.Priority = 10;
        }
        playerTarget.parent.gameObject.SetActive(true);
    }
}
