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
    [SerializeField] private Vector3 playerOffset = new Vector3(0, 1.5f, 0);

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
        
        Vector3 targetPosition = playerTransform.position + playerOffset;
        
        float distance = Vector3.Distance(eyeTransform.position, targetPosition);
        //if (distance <= attackDistance)
        //{
            //Debug.DrawLine(eyeTransform.position, targetPosition, Color.red);
        //}
        if (distance <= viewDistance)
        {
            //Debug.Log("Oke");
            //Debug.DrawLine(eyeTransform.position, targetPosition, Color.red);
            Vector3 directionToPlayer = (targetPosition - eyeTransform.position).normalized;
            
            float dotProduct = Vector3.Dot(eyeTransform.forward, directionToPlayer);

            if (dotProduct >= viewThreshold)
            {
                //Debug.Log("Oke");
                //Debug.DrawLine(eyeTransform.position, playerTransform.position + playerOffset, Color.red);
                if (!Physics.Raycast(eyeTransform.position, directionToPlayer, out var hit, distance,obstacleMask, QueryTriggerInteraction.Ignore))
                {
                    //Debug.Log("Oke");
                    //Debug.DrawLine(eyeTransform.position, playerTransform.position, Color.red);
                    isSeeing = true;
                }
                else
                {
                    //Debug.Log(hit.collider.gameObject.name);
                }
            }
        }
        
        bot.BlackBoard.CanSeePlayer = isSeeing;

        bot.BlackBoard.CanAttack = isSeeing && (distance <= attackDistance);
        
        if (isSeeing)
        {
            bot.BlackBoard.HasLastKnownPlayerPosition = true;
            
            bot.BlackBoard.LastKnownPlayerPosition = playerTransform.position; 
        }
        //Debug.Log(bot.BlackBoard.CanAttack + " " + isSeeing);
        //Debug.Log(isSeeing);
    }
}
