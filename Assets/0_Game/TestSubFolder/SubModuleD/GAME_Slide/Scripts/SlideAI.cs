using PrimeTween;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
[Serializable]
public class SimpleAIMovementComponent
{
    public Animator m_anim;
    public Transform m_skin;
    public Transform m_parentTransform;
    public float m_moveSpeed = 5f;
    private Action StopMove;
    public SimpleAIMovementComponent()
    {
        StopMove = ResetSpeed;
    }
    public Sequence MoveChain(List<Vector3> points, float speed)
    {
        //Debug.LogError("Sequence created");
        m_anim.SetBool("Grounded", true);
        m_anim.SetFloat("PlanarSpeed", speed);
        Sequence t = Sequence.Create();
        Vector3 p = m_parentTransform.position;
        p.y = points[0].y;
        m_parentTransform.position = p;

        for (int i = 0; i < points.Count; i++)
        {
            t.Chain(Tween.PositionAtSpeed(m_parentTransform, p, points[i], speed, ease: Ease.Linear));
            Vector3 diff = points[i] - m_parentTransform.position;
            diff.y = 0;
            p = points[i];
            if (diff.sqrMagnitude < .01f) continue;
            Quaternion q = Quaternion.LookRotation(diff, Vector3.up);
            t.Group(Tween.Rotation(m_parentTransform, q, .5f));

        }
        t.ChainCallback(target: this, target => target.StopMove(), false);
        return t;
    }
    public void PerformJump()
    {
        m_jumpHandle.Stop();
        m_anim.SetBool("Grounded", false);
        m_jumpHandle = Sequence.Create()
            .Chain(Tween.LocalPositionY(m_skin, 0f, 1.8f, .4f, Ease.OutQuad))
            .Chain(Tween.LocalPositionY(m_skin, 1.8f, 0f, .4f, Ease.InQuad))
            .ChainCallback(target: m_anim , target => target.SetBool("Grounded", true), false);
    }
    public Tween m_motionHandle;
    public Tween m_rotateHandle;
    public Sequence m_jumpHandle;
    public void MoveTo(Vector3 des, float speed = -1)
    {
        if (speed < 0) speed = m_moveSpeed;
        des.y = m_parentTransform.position.y;
        float diff = Vector3.Distance(m_parentTransform.position, des);
        m_anim.SetFloat("PlanarSpeed", m_moveSpeed);
        RotateTo(des);
        if (diff <= .1f) StopMove();
        else m_motionHandle = Tween.PositionAtSpeed(m_parentTransform, des, speed, ease: Ease.Linear).OnComplete(target:this, target => target.StopMove(), false);
    }
    public void RotateTo(Vector3 des, bool lockY = true)
    {
        Vector3 diff = des - m_parentTransform.position;
        if (lockY) diff.y = 0;
        if (diff.sqrMagnitude < .01f) return;
        Quaternion q = Quaternion.LookRotation(diff, Vector3.up);
        m_rotateHandle.Stop();
        m_rotateHandle = Tween.Rotation(m_parentTransform, q, .5f);
    }
    public void ResetSpeed()
    {
        m_anim.SetFloat("PlanarSpeed", 0);
    }

    public void DisableAll()
    {
        m_motionHandle.Stop();
        m_rotateHandle.Stop();
        m_jumpHandle.Stop();
    }
}
public class SlideAI : SimpleAIScript
{
    public SlideData m_slideData;
    private List<Transform> m_waypoints;
    private List<Transform> m_shopWaypoints;

    public float m_turnSpeed;

    Vector3 m_slideDir;
    private bool m_isSliding;
    public FloatieSwapper m_swapper;
    public bool m_rotateY = false;

    public SimpleAIMovementComponent m_movementComponent;
    protected override void Awake()
    {
        m_waypoints = m_slideData.m_waypoints;
        m_shopWaypoints = m_slideData.m_shopWaypoints;
        m_slideDir = m_slideData.GetSlideDirection();
    }
    public override void InitBot()
    {
        StopAllCoroutines();
        StartCoroutine(SlideCoroutine());
        m_anim.SetBool("Grounded", true);
        m_anim.SetBool("IsSliding", false);
        m_anim.SetFloat("VerticalSpeed", 0);
        m_isSliding = false;
        m_movementComponent.DisableAll();
    }

    public override void ResetBot()
    {
        InitBot();
    }
    private void Update()
    {
        /*float dt = Time.deltaTime;
        m_pos += m_currentVelocity * dt;
        if (m_isSliding)
        {
            m_currentVelocity += m_currentVelocity.normalized * m_gravity * dt;
            if (m_pos.y < m_currentDes.y + .1f)
            {
                Vector3 dir = m_currentVelocity.normalized;
                float diff = Mathf.Abs(m_pos.y - m_currentDes.y - .1f);
                m_pos.x -= diff * dir.x / dir.y;
                m_pos.z -= diff * dir.z / dir.y;
                m_pos.y = 0;
                m_isMovingTo = false;
                m_isSliding = false;
                return;
            }
        }
        else if (m_isMovingTo)
        {
            Debug.DrawLine(m_pos + Vector3.up, m_currentDes, Color.red);


            Vector3 diff = m_pos - m_currentDes;
            diff.y = 0;
            if (diff.magnitude < .1f)
            {
                m_isMovingTo = false;
                m_isSliding = false;
            }
        }
        if (m_rotDir != Vector3.zero) transform.forward = Vector3.Slerp(transform.forward, m_rotDir, dt * m_turnSpeed);
        transform.position = m_pos;*/
    }
    private float m_height;
    private IEnumerator SlideCoroutine()
    {
        m_aiPets.SetRendererStates(false);
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        while (true)
        {

            int repeat = Random.Range(3, 12);
            int cur = -1, cur2 = -1;
            for (int i = 0; i < repeat; i++)
            {
                bool goToShop = (Random.Range(0, 4) == 0);
                int rand = 0;
                if (!goToShop)
                {
                    rand = Random.Range(0, m_waypoints.Count - 1);

                    if (rand >= cur) rand++;
                    cur = rand;
                }
                else
                {
                    rand = Random.Range(0, m_shopWaypoints.Count - 1);

                    if (rand >= cur2) rand++;
                    cur2 = rand;
                }

                Vector3 randRad = Random.insideUnitSphere * 3f;
                randRad.y = 0;
                m_movementComponent.MoveTo(m_waypoints[rand].position + randRad);
                yield return m_movementComponent.m_motionHandle.ToYieldInstruction();


               
                if (!goToShop) yield return new WaitForSeconds(Random.Range(.3f, .5f));
                else yield return Yielder.Get(Random.Range(5, 15));
                int rotRepeat = 0;
                bool canRot = Random.Range(0, 3) == 0;
                if (canRot) rotRepeat = Random.Range(1, 4);
                for (int j = 0; j < rotRepeat; j++)
                {
                    m_movementComponent.RotateTo(Random.insideUnitCircle.normalized * transform.position);
                    yield return Yielder.Get(.1f);
                }
            }

            m_movementComponent.MoveTo(m_slideData.m_climbStart.position);
            yield return m_movementComponent.m_motionHandle.ToYieldInstruction();

            //Move to start of slide climb
            Teleport(m_slideData.m_climbStart.position + Vector3.up * .2f);

            //Change pet position
            m_aiPets.SetRendererStates(true);

            //Move to a certain height
            m_height = -m_slideDir.y * m_climbSpeed * Random.Range(5, 10);
            if (m_height >= 400) m_height = 400;
            Vector3 slideDestination = m_slideData.m_climbStart.position - m_slideDir * m_height / Mathf.Abs(m_slideDir.y) + Vector3.up * .2f;
            m_anim.SetFloat("VerticalSpeed", m_climbSpeed);
            m_movementComponent.m_motionHandle = Tween.PositionAtSpeed(transform, slideDestination, m_climbSpeed, Ease.Linear);
            m_movementComponent.RotateTo(slideDestination, false);
            yield return m_movementComponent.m_motionHandle.ToYieldInstruction();

            //Swap floatie if it exists
            if (m_swapper != null) m_swapper.SetFloatiePos(true);

            //Start sliding
            m_anim.SetBool("IsSliding", true);
            Vector3 startSlidingPos = m_slideData.GetClosestPointOnSlide(transform.position);
            Teleport(startSlidingPos + Vector3.up * .2f);
            m_isSliding = true;
            float timeTaken = Mathf.Sqrt(m_height / Mathf.Abs(m_slideDir.y) * 2 / m_gravity);
            m_movementComponent.m_motionHandle = Tween.Position(transform, m_slideData.m_slideEnd.position + Vector3.up * .2f, timeTaken, Ease.InQuad);
            m_movementComponent.RotateTo(m_slideData.m_slideEnd.position + Vector3.up * .2f, false);
            yield return m_movementComponent.m_motionHandle.ToYieldInstruction();

            if (m_swapper != null) m_swapper.SetFloatiePos(false);
            m_anim.SetBool("IsSliding", false);
            m_aiPets.SetRendererStates(false);
        }
    }
}