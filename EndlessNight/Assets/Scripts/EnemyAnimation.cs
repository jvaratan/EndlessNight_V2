using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyAnimationStatus
{
    Idle,
    Run,
    Attack,
}

public class EnemyAnimation : MonoBehaviour {

    private Animator m_anim;

    public EnemyAnimationStatus Status { get { return m_status; } }
    private EnemyAnimationStatus m_status;

    [SerializeField] private float m_animStartSpeed, m_animSpeedLimit;

    void Awake()
    {
        m_anim = GetComponent<Animator>();
    }

    void Start ()
    {
        if(m_animSpeedLimit <= 0)
            Debug.LogError("AnimSpeedLimit can't be 0");
        if (m_animStartSpeed <= 0)
            Debug.LogError("AnimStartSpeed can't be 0");

        m_anim.speed = m_animStartSpeed;
    }
	
	void Update ()
    {
		
	}

    public void SetAnimation(string animName)
    {
        m_status = (EnemyAnimationStatus)System.Enum.Parse(typeof(UnitAnimStatus), animName);
        m_anim.Play("Giant" + animName);
    }
    public void SetAnimation(EnemyAnimationStatus animName)
    {
        m_status = animName;
        m_anim.Play("Giant" + animName.ToString());
    }

    public void SetSpeed(float animSpeed)
    {
        if (m_anim.speed < m_animSpeedLimit)
        {
            m_anim.speed = animSpeed;

        }
        else
        {
            m_anim.speed = m_animSpeedLimit;
        }
    }
    public void SetOriginalSpeed()
    {
        m_anim.speed = m_animStartSpeed;
    }
}
