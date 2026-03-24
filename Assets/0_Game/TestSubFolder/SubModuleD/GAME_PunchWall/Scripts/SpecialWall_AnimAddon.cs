using LitMotion.Animation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialWall_AnimAddon : MonoBehaviour
{
    public LitMotionAnimation m_anim;
    private void Awake()
    {
        GetComponent<WallStats>().OnHit += PlayAnim;
    }

    void PlayAnim(int i)
    {
        m_anim.Restart();
    }
}
