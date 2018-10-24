using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    //private GameManager m_gameManager;
    private PlayerController m_playerController;

    public UnitAnimation UnitAnim { get { return m_unitAnim; } }
    private UnitAnimation m_unitAnim;

    public UnitDummy m_unitDummy;

    public int StandingNum { get { return m_standingNum; } }
    private int m_standingNum;

    [SerializeField][Range(3,12)] private int m_timesToStanding;

    private UISkillCheckSystem m_uiSkillCheckSystem;

    [SerializeField] private float m_runSpeedMin, m_runSpeedMax;
    public float RunSpeed { get { return m_runSpeed; } }
    private float m_runSpeed;
    private float m_runSpeedAfterFell;

    [SerializeField] private float m_accelelationMin, m_accelelationMax, m_accelelationFall;
    public float Accelelation { get { return m_accelelation; } }
    private float m_accelelation;

    [SerializeField] private float m_perfectSpeed, m_perfectSpeedTime;

    [SerializeField] private float m_animMovingBFSpeed;
    private bool b_enableAnimBFMoving;

    [SerializeField] private Transform m_startPoint, m_endPoint;

    private Coroutine m_coroutine;

    [SerializeField] private AudioClip m_dieSound;

    private void Awake()
    {
       //m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        m_playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
        m_uiSkillCheckSystem = GameObject.Find("UISkillCheck").GetComponent<UISkillCheckSystem>();
        m_unitDummy = GetComponentInParent<UnitDummy>();
        m_unitAnim = GetComponentInChildren<UnitAnimation>();
    }

    void Start ()
    {
        m_accelelation = m_accelelationMin;
        ResetVariables(UnitAnimStatus.Fall);
        m_uiSkillCheckSystem.SetActiveUIZones(false, true);
    }
	
	void Update ()
    {
        //if(m_gameManager.GameStatus != GameStatus.End)
         //   UnitRunning();
	}

    public void UnitRunning()
    {
        switch (m_unitAnim.UnitAnimStat)
        {
            case UnitAnimStatus.Run:
                if (m_runSpeed < m_runSpeedMax)
                {
                    m_runSpeed = Mathf.Clamp(m_runSpeed + m_accelelation, m_runSpeedMin, m_runSpeedMax);
                }
                break;
            case UnitAnimStatus.Fall:
            case UnitAnimStatus.Standing1:
            case UnitAnimStatus.Standing2:
            case UnitAnimStatus.Standing3:
                if (m_runSpeed > m_runSpeedMin)
                {
                    m_runSpeed = Mathf.Clamp(m_runSpeed - m_accelelationFall, m_runSpeedMin, m_runSpeedMax);
                }
                break;

        }

        if (m_unitAnim.UnitAnimStat != UnitAnimStatus.Idle)
        {
            if(m_unitAnim.UnitAnimStat == UnitAnimStatus.Run)
                AnimationMovingBackAndForth();

            m_unitDummy.Moving(m_runSpeed + m_perfectSpeed);
        }
    }

    // Move unit forward when hit PERFECT
    public void AnimationMovingBackAndForth()
    {
        if (b_enableAnimBFMoving == true)
        {
            transform.position = Vector3.Lerp(transform.position, m_endPoint.position, m_animMovingBFSpeed * Time.deltaTime);
        }
        else if(b_enableAnimBFMoving == false && transform.position != m_startPoint.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_startPoint.position, m_animMovingBFSpeed * Time.deltaTime);
        }
    }

    public void UnitFall()
    {
        m_playerController.EnableCheckHit = false;
        m_unitAnim.SetAnimation(UnitAnimStatus.Fall);
        m_unitAnim.SetOriginalSpeed();
        m_uiSkillCheckSystem.SetActiveUIZones(false, true);
        SetRunSpeedAfterFell();
    }

    public void UnitStanding()
    {
        ++m_standingNum;

        if (m_standingNum < m_timesToStanding)
        {
            m_unitAnim.SetAnimation("Standing" + Mathf.Ceil(m_standingNum / (m_timesToStanding / 3f)));
        }
        else
        {
            ResetVariables(UnitAnimStatus.Run);
            m_runSpeed = 1.5f;
            m_playerController.DelayCheckHit();
        }

    }

    public void UnitDie()
    {
        //m_gameManager.SoundManager.PlaySFXOneShot(m_dieSound);
        m_unitAnim.SetAnimation(UnitAnimStatus.Die);
        m_unitAnim.SetOriginalSpeed();
        m_runSpeed = 0;
        m_perfectSpeed = 0;
        m_animMovingBFSpeed = 0;
        m_uiSkillCheckSystem.EnableMovingZone(false);
        m_uiSkillCheckSystem.SetActiveUIZones(false, false);
       //m_uiSkillCheckSystem.GameManager.GameEnd(false);
    }

    public void ResetVariables(UnitAnimStatus animStat)
    {
        m_unitAnim.SetAnimation(animStat);
        m_unitAnim.SetOriginalSpeed();
        m_standingNum = 0;
        m_perfectSpeed = 0;
        m_uiSkillCheckSystem.SetActiveUIZones(true, false);
    }

    public void EnablePerfectSpeed()
    {
        b_enableAnimBFMoving = true;

        m_perfectSpeed = m_runSpeed * 0.2f;
        m_unitAnim.SetPerfectSpeed();

        if(m_coroutine != null)
            StopCoroutine(m_coroutine);
        m_coroutine = StartCoroutine(PerfectSpeedTurnOn());
    }

    //When hit perfect, unit move faster
    IEnumerator PerfectSpeedTurnOn()
    {
        yield return new WaitForSeconds(m_perfectSpeedTime);
        b_enableAnimBFMoving = false;
        m_perfectSpeed = 0;
        m_unitAnim.ResetPerfectSpeed();
    }

    public void SetRunSpeedAfterFell()
    {
        m_accelelation = Mathf.Clamp(m_runSpeed / 300f, m_accelelationMin, m_accelelationMax);
    }

    public float GetTotalRunSpeed()
    {
        return m_runSpeed + m_perfectSpeed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "FinishPoint")
        {
            print("Finish!!");
            //GameManager.GamePause();
            m_uiSkillCheckSystem.SetActiveUIZones(false, false);
            //m_gameManager.GameEnd(true);
        }
    }
}
