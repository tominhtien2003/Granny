using LitMotion;
using LitMotion.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PunchWall_AI : SimpleAIScript
{
    public PunchWall_AIBlackboard m_blackboard;
    public RuntimeAnimatorController m_runtimeAnimator;
    public AnimatorOverrideController m_overrideAnim;

    private Action StopMove;
    public Transform m_skin;
    
    private int m_trainRatio;
    public MeshRenderer[] m_meshes;
    private Material[] m_mats;

#if UNITY_EDITOR
    [Button]
    public void GetMeshes()
    {
        m_meshes = m_skin.GetComponentsInChildren<MeshRenderer>();
    }
#endif
    public override void InitBot()
    {
        m_overrideAnim = new AnimatorOverrideController(m_runtimeAnimator);
        m_anim.runtimeAnimatorController = m_overrideAnim;
        m_runtimeAnimator = m_overrideAnim;

        m_overrideAnim["P11"] = m_blackboard.m_replacements[0];
        m_overrideAnim["P21"] = m_blackboard.m_replacements[1];
        m_overrideAnim["ThunderClap"] = m_blackboard.m_replacements[2];
        StopMove = ResetSpeed;
        m_anim.SetBool("Grounded", true);
        m_trainRatio = Random.Range(1, 5);
        StartCoroutine(PunchWallCoroutine());
        
    }
    public override void SetModel(SkinShopData shopData)
    {
        m_mats = shopData.m_materials.ToArray();
        foreach (var mesh in m_meshes)
        {
            mesh.sharedMaterials = m_mats;
        }
    }
    void ResetSpeed()
    {
        m_anim.SetFloat("PlanarSpeed", 0);
    }
    public override void ResetBot()
    {
        StopAllCoroutines();
        m_anim.CrossFadeInFixedTime("StableGrounded", .1f);
        InitBot();
    }
    void PerformJump()
    {
        m_jumpHandle.TryCancel();
        m_anim.SetBool("Grounded", false);
        m_jumpHandle = LMotion.Create(0f, 1.8f, .4f).WithEase(Ease.OutQuad).WithOnComplete(() =>
        {
            LMotion.Create(1.8f, 0f, .4f).WithEase(Ease.InQuad).WithOnComplete(() =>
            {
                m_anim.SetBool("Grounded", true);
            }).BindToLocalPositionY(m_skin).AddTo(gameObject);
        }).BindToLocalPositionY(m_skin).AddTo(gameObject);
    }
    private MotionHandle m_motionHandle;
    private MotionHandle m_rotateHandle;
    private MotionHandle m_jumpHandle;
    private List<List<TrainingMachine>> m_availableMachines = new List<List<TrainingMachine>>();
    void MoveTo(Vector3 des)
    {
        des.y = transform.position.y;
        float diff = Vector3.Distance(transform.position, des);
        m_anim.SetFloat("PlanarSpeed", m_moveSpeed);
        RotateTo(des);
        m_motionHandle = LMotion.Create(transform.position, des, diff / m_moveSpeed).WithOnComplete(StopMove).BindToPosition(transform);
    }
    void RotateTo(Vector3 des)
    {
        Vector3 diff = des - transform.position;
        diff.y = 0;
        if (diff.sqrMagnitude < .01f) return;
        Quaternion q = Quaternion.LookRotation(diff, Vector3.up);
        m_rotateHandle.TryCancel();
        m_rotateHandle = LMotion.Create(transform.rotation, q, .5f).BindToRotation(transform);
    }
    private IEnumerator PunchWallCoroutine()
    {
        while (true)
        {
            int times = m_trainRatio * Random.Range(2, 6);
            int cur = -1;
            for (int i = 0; i < times; i++)
            {
                int midMove = Random.Range(1, 4);
                for (int j = 0; j < midMove; j++)
                {
                    int rand = Random.Range(0, m_blackboard.m_midWaypoints.Count - 1);
                    if (rand >= cur) rand++;
                    Vector3 des = m_blackboard.m_midWaypoints[rand].position;
                    MoveTo(des);
                    int hit = Random.Range(1, 10);
                    if (hit >= 6) PerformJump();
                    yield return m_jumpHandle.ToYieldInstruction();
                    yield return m_motionHandle.ToYieldInstruction();
                    yield return Yielder.Get(Random.Range(1, 2));
                    
                    if (hit <= 4) m_anim.CrossFadeInFixedTime("Punch1", .1f);
                    if (hit <= 2)
                    {
                        for (int h= 0; h < 2; h++)
                        {
                            MoveTo(des + Random.insideUnitSphere);
                            yield return m_motionHandle.ToYieldInstruction();
                            yield return Yielder.Get(.5f);
                        }
                    }
                    
                }
                m_availableMachines.Clear();
                foreach (var v in m_blackboard.m_machinesManager)
                {
                    if (v.m_unOccupiedMachines.Count <= 1) continue;
                    m_availableMachines.Add(v.m_unOccupiedMachines);

                }
                if (m_availableMachines.Count == 0) continue;
                int t = Random.Range(0, m_availableMachines.Count);

                List<TrainingMachine> machines = m_availableMachines[t];
                TrainingMachine m = machines[Random.Range(0, machines.Count)];
                MoveTo(m.transform.position);
                yield return m_motionHandle.ToYieldInstruction();
                if (m.Occupied || machines.Count <= 1) continue;


                m.ChangeOccupiedState(true);
                transform.position = m.m_pos.position;
                transform.forward = m.m_pos.forward;
                m_overrideAnim["Squatting_Idle"] = m.m_idle;
                m_overrideAnim["Squatting"] = m.m_train;

                Animator machineAnim = m.m_animator;
                int reps = m_trainRatio * Random.Range(5, 22);
                m_anim.SetFloat("TrainingSpeed", 1f);
                machineAnim.SetFloat("TrainingSpeed", 1f);
                for (int r = 0; r < reps; r++)
                {
                    m_anim.CrossFadeInFixedTime("TrainMove", .1f);
                    machineAnim.CrossFadeInFixedTime("Move", .1f);
                    yield return Yielder.Get(1.5f);
                }
                m_anim.SetFloat("TrainingSpeed", 3f);
                machineAnim.SetFloat("TrainingSpeed", 3f);
                if (reps >= 10)
                {
                    reps = Random.Range(10, 15) * m_trainRatio;
                    for (int r = 0; r < reps; r++)
                    {
                        m_anim.CrossFadeInFixedTime("TrainMove", .1f);
                        machineAnim.CrossFadeInFixedTime("Move", .1f);
                        yield return Yielder.Get(.4f);
                    }
                }
                m.ChangeOccupiedState(false);
                m_anim.CrossFadeInFixedTime("StableGrounded", .1f);
            }

            MoveTo(m_blackboard.m_startPunchWaypoint.position + Random.insideUnitSphere * Random.Range(0f, 2f));
            yield return m_motionHandle.ToYieldInstruction();
           
            
            int punchReps = (6 - m_trainRatio) * Random.Range(1, 3);
            
            for (int i = 0; i < punchReps; i++)
            {
                int stayRef = Random.Range(3, 12);
                int st = 0;
                int stWall = 20;
                int stHit = 0, celSt = 1 ;
                for (int j = 0; j < stayRef; j++)
                {
                    if (st >= PunchWall_GlobalStatusHolder.Instance.WallHps.Count - 1) break;
                    Vector3 randOffset = Vector3.forward * Random.Range(-2f, 2f);
                    Vector3 wallPos = m_blackboard.m_wall.transform.parent.TransformPoint(new Vector3(PunchWall_GlobalStatusHolder.Instance.WallStartOffset + PunchWall_GlobalStatusHolder.Instance.WallDistance * st, 0, 0));
                    wallPos += randOffset;
                    Debug.DrawRay(wallPos, Vector3.up * 1000, Color.red, 10f);
                    wallPos += Vector3.right * 2f;
                    MoveTo(wallPos);
                    yield return m_motionHandle.ToYieldInstruction();
                    yield return Yielder.Get(.5f);
                    stWall = Random.Range(1, Mathf.FloorToInt(stWall * .75f));

                    stHit = Random.Range(stHit, celSt);
                    for (int t= 0; t < stHit; t++)
                    {
                        m_anim.CrossFadeInFixedTime(t % 2 == 0?"Punch1":"Punch2", .1f);
                        yield return Yielder.Get(2f);
                    }
                    celSt++;
                    m_anim.CrossFadeInFixedTime("Clap", .1f);
                    if (stWall >= 10) m_moveSpeed = 60f;
                    else m_moveSpeed = 6f;
                        yield return Yielder.Get(2f);
                    int hit = Random.Range(1, 7);
                    if (hit <= 2)
                    {
                        for (int h = 0; h < 2; h++)
                        {
                            MoveTo(wallPos + Random.insideUnitSphere);
                            yield return m_motionHandle.ToYieldInstruction();
                            yield return Yielder.Get(.5f);
                        }
                    }
                    st = Mathf.Min(st + stWall, PunchWall_GlobalStatusHolder.Instance.WallHps.Count - 1);
                }
                transform.position = m_blackboard.m_startPunchWaypoint.position;
                yield return Yielder.Get(5f);
                m_moveSpeed = 6f;
            }
        }
    }

    private void OnDisable()
    {
        m_motionHandle.TryCancel();
        m_rotateHandle.TryCancel();
        m_jumpHandle.TryCancel();
    }
    private void OnDestroy()
    {
        m_motionHandle.TryCancel();
        m_rotateHandle.TryCancel();
        m_jumpHandle.TryCancel();
    }
}
