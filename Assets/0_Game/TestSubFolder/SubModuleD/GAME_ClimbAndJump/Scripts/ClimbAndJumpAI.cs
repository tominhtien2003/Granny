using System.Collections;
using UnityEngine;

public class ClimbAndJumpAI: SimpleAIScript
{
    public bool m_isWandering = false;

    public BoxCollider m_trussCollider;
    private Coroutine m_wanderCoroutine = null;
    
  
    public override void InitBot()
    {
        m_anim.SetBool("Grounded", true);
        m_anim.SetBool("IsClimbing", false);
        m_anim.SetFloat("VerticalSpeed", 0);
        if (m_wanderCoroutine != null)
        {
            StopCoroutine(m_wanderCoroutine);
            m_wanderCoroutine = null;
        }
        m_wanderCoroutine = StartCoroutine(Wander());
        m_selfCollider = GetComponent<Collider>();
        m_selfCollider.enabled = true;

    }
    public float m_turnSpeed;
    public void Update()
    {
        float dt = Time.deltaTime;
        m_pos += m_currentVelocity * dt;
        if (m_pos.y > 2170f) m_pos.y = 2170f;
        if (m_rotDir != Vector3.zero) transform.forward = Vector3.Slerp(transform.forward, m_rotDir, dt * m_turnSpeed);

        if (m_isFalling)
        {
            m_pos += Vector3.down * m_currentFallSpeed * dt;

            if (m_currentFallSpeed < 10) m_currentFallSpeed += m_gravity * dt;
            else m_currentFallSpeed += 50 * dt;

            if (m_pos.y < .1f)
            {
                m_pos.y = 0;
                m_isFalling = false;

            }
        }
        transform.position = m_pos;
    }

   
    private void OnTriggerEnter(Collider other)
    {
        if (m_wanderCoroutine != null)
        {
            StopCoroutine(m_wanderCoroutine);
            m_wanderCoroutine = null;
            m_isWandering = false;
        }
        StartCoroutine(ClimbUp());
    }

    private IEnumerator Wander()
    {
        m_isWandering = true;
        m_anim.SetBool("Grounded", true);
        m_aiPets.SetRendererStates(false);
        while (true)
        {
            //Random Move
            int iteration = Random.Range(1, 5);
            for (int i = 0; i < iteration; i++)
            {
                Vector3 des = m_wanderZone.transform.position + Random.insideUnitSphere * m_wanderZone.Radius;
                Vector3 dir = des - m_pos;
                dir.y = 0;
                SetMoveDir(dir, m_moveSpeed);
                m_rotDir = dir;
                yield return new WaitForSeconds(Random.Range(.2f, 3f));

                SetMoveDir(Vector3.zero, 0);
                yield return new WaitForSeconds(Random.Range(.5f, 2f));
            }
        }
    }
    private bool m_heightReached = false;
    private IEnumerator ClimbUp()
    {
        m_selfCollider.enabled = false;
        Vector3 closestTrussPoint = m_trussCollider.ClosestPoint(m_pos);
        Vector3 diffs = closestTrussPoint - m_pos;
        diffs.y = 0;


        m_pos = closestTrussPoint - diffs.normalized * 0.5f;
        transform.position = m_pos;

        m_rotDir = diffs.normalized;


        //Climb
        m_anim.SetBool("Grounded", false);
        m_aiPets.SetRendererStates(true);
        m_anim.SetBool("IsClimbing", true);
        m_anim.SetFloat("VerticalSpeed", m_moveSpeed);
        SetMoveDir(Vector3.up, m_climbSpeed);
        m_heightReached = false;
        float maxHeight = Mathf.Max(m_moveSpeed * Random.Range(2, 10), 2170f);
        while (m_pos.y < maxHeight)
        {
            yield return null;
        }

        //Fall

        Teleport(m_pos - m_rotDir);

        SetMoveDir(Vector3.zero, 0);
        m_anim.SetBool("IsClimbing", false);
        m_anim.SetBool("IsFallingFast", true);
        m_currentFallSpeed = 0;
        m_isFalling = true;

        while (m_isFalling) yield return null;
        m_anim.SetBool("Grounded", true);
        m_aiPets.SetRendererStates(false);
        m_anim.SetBool("IsFallingFast", false);
        yield return Yielder.Get(Random.Range(1, 2));

        m_selfCollider.enabled = true;
        if (m_wanderCoroutine != null)
        {
            StopCoroutine(m_wanderCoroutine);
            m_wanderCoroutine = null;
        }
        m_wanderCoroutine = StartCoroutine(Wander());

    }
    public override void ResetBot()
    {
        StopAllCoroutines();
        InitBot();
        SetMoveDir(Vector3.zero, 0);

    }
}
