using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private UISkillCheckSystem m_uiSkillCheck;
    private Unit m_unit;

    public bool EnableCheckHit { set { b_enableCheckHit = value; } }
    private bool b_enableCheckHit;

    [SerializeField] private float m_delayTime;

    private Coroutine m_coroutine;

    private void Awake()
    {
        m_unit = GameObject.Find("Unit").GetComponent<Unit>();
        m_uiSkillCheck = GameObject.Find("UISkillCheck").GetComponent<UISkillCheckSystem>();
    }

    void Start ()
    {
        b_enableCheckHit = true;
        m_coroutine = null;
	}
	
	void Update ()
    {
        /*if (m_uiSkillCheck.GameManager.GameStatus != GameStatus.End)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if ((m_unit.UnitAnim.UnitAnimStat == UnitAnimStatus.Idle ||
                    m_unit.UnitAnim.UnitAnimStat == UnitAnimStatus.Run) && b_enableCheckHit == true)
                {
                    m_uiSkillCheck.CheckHit();
                }
                else if (m_unit.UnitAnim.UnitAnimStat == UnitAnimStatus.Fall ||
                         m_unit.UnitAnim.UnitAnimStat == UnitAnimStatus.Standing1 ||
                         m_unit.UnitAnim.UnitAnimStat == UnitAnimStatus.Standing2 ||
                         m_unit.UnitAnim.UnitAnimStat == UnitAnimStatus.Standing3)
                {
                    m_unit.UnitStanding();
                }
            }
        }*/
	}

    public void DelayCheckHit()
    {
        if(m_coroutine != null)
            StopCoroutine(m_coroutine);
        m_coroutine = StartCoroutine(DelayCheckHitCoroutine());
    }

    // Delay checkhit for a short time after stood up for making it not too difficult
    IEnumerator DelayCheckHitCoroutine()
    {
        yield return new WaitForSeconds(m_delayTime);
        b_enableCheckHit = true;
    }
}
