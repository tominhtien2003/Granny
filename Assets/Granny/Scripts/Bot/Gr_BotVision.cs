using UnityEngine;

public class Gr_BotVision : MonoBehaviour
{
    [Header("Vision Settings")]
    [Range(0, 360)]
    [SerializeField] private float viewAngle;
    [SerializeField] private float viewDistance;
    
    [Header("References")]
    [SerializeField] Transform eyeTransform; 
    [SerializeField] LayerMask obstacleMask;

    [SerializeField] private float attackDistance = 2f;

    private Gr_BotController bot;
    private Transform playerTransform;
    private float viewThreshold;

    private void Awake()
    {
        bot = GetComponent<Gr_BotController>();
    }

    private void Start()
    {
        playerTransform = bot.BlackBoard.PlayerTransform;
        viewThreshold = Mathf.Cos(Mathf.Deg2Rad * viewAngle * .5f);
    }

    private void Update()
    {
        CheckVision();
    }

    private void CheckVision()
    {
        if (!playerTransform) return;

        bool isSeeing = false;
        
        float distance = Vector3.Distance(eyeTransform.position, playerTransform.position);
        if (distance <= viewDistance)
        {
            //Debug.DrawLine(eyeTransform.position, playerTransform.position, Color.red);
            Vector3 directionToPlayer = (playerTransform.position - eyeTransform.position).normalized;
            
            float dotProduct = Vector3.Dot(eyeTransform.forward, directionToPlayer);

            if (dotProduct >= viewThreshold)
            {
                //Debug.Log("Oke");
                if (!Physics.Raycast(eyeTransform.position, directionToPlayer, out RaycastHit hit, distance,obstacleMask))
                {
                    isSeeing = true;
                }
            }
        }
        
        bot.BlackBoard.CanSeePlayer = isSeeing;

        bot.BlackBoard.CanAttack = isSeeing && distance <= attackDistance;
        //Debug.Log(isSeeing);
    }
}
