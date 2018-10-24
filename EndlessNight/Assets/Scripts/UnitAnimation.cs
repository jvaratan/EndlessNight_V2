using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitAnimStatus
{
    Idle,
    Run,
    Fall,
    Standing1,
    Standing2,
    Standing3,
    Die,
}

public class UnitAnimation : MonoBehaviour {

    private Unit m_unit;

    private Animator m_anim;

    public UnitAnimStatus UnitAnimStat { get { return m_unitAnimStat; } }
    private UnitAnimStatus m_unitAnimStat;

    [SerializeField] private float m_animSpeedMin, m_animSpeedMax;
    private float m_animSpeed;

    [SerializeField] private float m_animAcelelationMin, m_animAcelelationMax;
    private float m_animAcelelation;

    private float m_animPerfectSpeed;

    void Awake()
    {
        m_unit = GetComponentInParent<Unit>();
        m_anim = GetComponent<Animator>();
    }

    void Start ()
    {
        if(m_animSpeedMax <= 0)
            Debug.LogError("AnimSpeedLimit can't be 0");
        if (m_animSpeedMin <= 0)
            Debug.LogError("AnimStartSpeed can't be 0");

        m_anim.speed = m_animSpeedMin;
    }
	
	void Update ()
    {

	}

    public void SetAnimation(string animName)
    {
        m_unitAnimStat = (UnitAnimStatus)System.Enum.Parse(typeof(UnitAnimStatus), animName);
        m_anim.Play("Stickman" + animName);
    }
    public void SetAnimation(UnitAnimStatus animName)
    {
        m_unitAnimStat = animName;
        m_anim.Play("Stickman" + animName.ToString());
    }

    public void SetSpeed()
    {
        if (m_anim.speed < m_animSpeedMax)
        {
            SetAccelelation();
            m_animSpeed = Mathf.Clamp(m_animSpeed + m_animAcelelation, m_animSpeedMin, m_animSpeedMax);
        }

        m_anim.speed = m_animSpeed + m_animPerfectSpeed;
    }
    public void SetOriginalSpeed()
    {
        m_anim.speed = m_animSpeed = m_animSpeedMin;
        m_animPerfectSpeed = 0;
    }

    public void SetAccelelation()
    {
        m_animAcelelation = Mathf.Clamp((m_unit.RunSpeed / 200f), m_animAcelelationMin, m_animAcelelationMax);
    }

    public void SetPerfectSpeed()
    {
        m_animPerfectSpeed = m_animSpeed * 0.8f;
        SetSpeed();
    }
    public void ResetPerfectSpeed()
    {
        m_animPerfectSpeed = 0;
        SetSpeed();
    }
}
