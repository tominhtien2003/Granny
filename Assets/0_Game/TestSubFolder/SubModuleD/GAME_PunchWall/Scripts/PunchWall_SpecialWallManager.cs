using Cinemachine;
using PrimeTween;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class PunchWall_SpecialWallManager : MonoBehaviour
{
    public List<SpecialWall> InactiveWalls;
    public List<SpecialWall> ActiveWalls;

    public List<Transform> m_spawnPositions;
    public List<int> m_currentSpawnPosIds;

    public TransformFollowMover m_droppingCollisionBox;
    public static PunchWall_SpecialWallManager Instance;
    public PunchWallScript m_punchWallScript;
    public ParticleSystem m_slamParticle;

    public CinemachineVirtualCamera m_cutsceneCam;
    public Vector3 m_offset;
    private void Awake()
    {
        Instance = this;
        m_droppingCollisionBox.gameObject.SetActive(false);
        foreach (var v in InactiveWalls) v.gameObject.SetActive(false);
        m_offset = m_cutsceneCam.transform.position - InactiveWalls[0].transform.position;
    }
    public SpecialWall GetWall()
    {
        if (InactiveWalls.Count == 0) return null;
        SpecialWall wall = InactiveWalls[InactiveWalls.Count - 1];
        InactiveWalls.RemoveAt(InactiveWalls.Count - 1);
        ActiveWalls.Add(wall);
        wall.gameObject.SetActive(true);
        wall.SpawnId = m_currentSpawnPosIds[Random.Range(0, m_currentSpawnPosIds.Count)];
        m_currentSpawnPosIds.Remove(wall.SpawnId);
        wall.m_collider.enabled = false;
       
        return wall;

    }

    public void ReturnWall(SpecialWall wall)
    {
        wall.gameObject.SetActive(false);
        ActiveWalls.Remove(wall);
        m_currentSpawnPosIds.Add(wall.SpawnId);
        InactiveWalls.Add(wall);
    }
    public Action<SpecialWall> OnWallSpawned;
    public void TrySpawningWall()
    {
        SpecialWall wall = GetWall();
        if (wall == null) return;

        
        wall.LandingY = m_spawnPositions[wall.SpawnId].position.y;
        wall.transform.rotation = m_spawnPositions[wall.SpawnId].rotation;
        wall.transform.position = m_spawnPositions[wall.SpawnId].position + Vector3.up * 7f;
        if (!PunchWall_DataController.SpecialWallDone)
        {
            m_cutsceneCam.enabled = true;
            m_cutsceneCam.LookAt = wall.transform;
            Vector3 pos = wall.transform.position + m_offset;
            pos.y = wall.LandingY + m_offset.y;
            m_cutsceneCam.transform.position = pos;
            PunchWall_DataController.SpecialWallDone = true;
        }
        m_droppingCollisionBox.tr = wall.transform;
        m_droppingCollisionBox.gameObject.SetActive(true);
        wall.m_maxHp = m_punchWallScript.m_currentDamage * Random.Range(20, 35);
        wall.m_hp = wall.m_maxHp;
        wall.m_stats.SetHp(wall.m_hp, wall.m_maxHp);

        wall.transform.localScale = Vector3.one * .1f;
        Sequence t = Sequence.Create()
            .Chain(Tween.Scale(wall.transform, 1f, .5f, Ease.OutElastic))
            .ChainDelay(2.2f)
            .ChainCallback(()=>OnWallSpawned?.Invoke(wall))
            .Chain(
            Tween.PositionY(wall.transform, wall.LandingY, .5f, Ease.OutElastic))
            .InsertCallback(2.8f,()=>
            {
                wall.m_collider.enabled = true;
                m_droppingCollisionBox.gameObject.SetActive(false);
                m_slamParticle.transform.position = wall.transform.position + Vector3.up * .1f;
                m_slamParticle.Play();
            });
        if (m_cutsceneCam.enabled) t.ChainDelay(3f).ChainCallback(() => m_cutsceneCam.enabled = false);
    }
}
