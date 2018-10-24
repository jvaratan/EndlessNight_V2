using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public enum eHitStatus
{
    Great,
    Perfect,
    Miss,
}

public class UISkillCheckSystem : MonoBehaviour
{

    // Get all UI skill check script and object
    [Header("UI Object")]
    [SerializeField] private GameObject m_target;
    [SerializeField] private UIProgressBar m_bar;
    [SerializeField] private UIZone m_greatZone;
    [SerializeField] private UIZone m_perfectZone;
    [SerializeField] private Text HitText;
    [SerializeField] private GameObject m_tapIcon;

    private bool m_isMoving;
    private bool m_isHitAreaMoving;
    private bool m_isInHitZone;

    //public GameManager GameManager { get { return m_gameManager; } }
    //private GameManager m_gameManager;

    // For set rotate speed
    [Header("Speed and Rotate")]
    public float SpeedRotateMin;
    public float SpeedRotateMax;
    public float SpeedRotate;
    public float StartRotatingAt;

    private void Awake()
    {
        //m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //m_unit = GameObject.Find("Unit").GetComponent<Unit>();
    }

    void Start()
    {
        m_isMoving = true;
        m_isHitAreaMoving = false;
        SpawnAllNewZones();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //if (m_target != null)
                CheckHit();
            //else
               //Debug.LogError("Missing target");
        }
#endif
#if UNITY_IOS || UNITY_ANDROID

#endif
        /*if (m_unit.UnitAnim.UnitAnimStat == UnitAnimStatus.Idle ||
            m_unit.UnitAnim.UnitAnimStat == UnitAnimStatus.Run)
        {
            CheckBarMovingPassZones();
        }*/

        CheckBarMovingPassZones();

        // Hit area rotating
        if (m_isHitAreaMoving == true)
        {
            // Rotating great zone
            m_greatZone.ZoneMoving(SpeedRotate);
            // Update perfect zone position that involve with great zone rotating
            m_perfectZone.GenerateNewZone(m_greatZone.EndAngle - (m_greatZone.ZoneImg.fillAmount * 100));
        }
    }

    public void SetIsMoving(bool isMoving)
    {
        m_isMoving = isMoving;
    }

    // Check if the bar is in the hit zone
    // If player not tap until it out of zone
    // Make it MISS!
    public void CheckBarMovingPassZones()
    {
        // Bar move in the zone
        if (m_greatZone.CheckHit(m_bar.Angle) == true && m_bar.Status == eProgressBarStatus.WaitForZone)
        {
            m_bar.Status = eProgressBarStatus.InZone;
        }

        // Bar move passed the zone
        if (m_greatZone.CheckHit(m_bar.Angle) == false && m_bar.Status == eProgressBarStatus.InZone)
        {
            m_bar.Status = eProgressBarStatus.PassedZone;
            // TODO
            // Send Miss status to Target
        }
    }

    public void CheckHit()
    {
        eHitStatus status = CheckHitStatus();

        switch(status)
        {
            // TODO
            // Send status to Target

            case eHitStatus.Great:
                break;
            case eHitStatus.Perfect:
                break;
            case eHitStatus.Miss:
                break;
            default:
                break;
        }

        if(status == eHitStatus.Miss)
        {
            ResetVariables();
        }
        else
        {
            ReduceSizeAndIncreaseSpeed();
        }

        SetHitText(status.ToString());
        SpawnAllNewZones();

        /*if (isHit == true)
        {
            HitText.text = isPerfect == true ? "PERFECT!" : "GREAT!";

            if (isPerfect == true)
            {
                //m_unit.EnablePerfectSpeed();
            }

           //m_gameManager.UpdateSpeedText(m_unit);
            //m_gameManager.IncrementCombo();
            ReduceSizeAndIncreaseSpeed();

            //m_unit.UnitAnim.SetAnimation(UnitAnimStatus.Run);
            //m_unit.UnitAnim.SetSpeed();

            //if (m_gameManager.Combo > m_startRotatingAt)
            //{
            //    EnableMovingZone(true);
            //}

            //m_gameManager.SoundManager.PlaySFXOneShot(m_hitSound);
        }
        else
        {
            HitText.text = "MISS!";
            //m_gameManager.SoundManager.PlaySFXOneShot(m_missSound);
            //m_unit.UnitFall();
             
            ResetVariables();
        }

        SpawnAllNewZones();*/
    }

    // Check if the bar hit the hitting areas or not
    private eHitStatus CheckHitStatus()
    {
        eHitStatus status = eHitStatus.Miss;

        if (m_perfectZone.CheckHit(m_bar.Angle))
        {
            status = eHitStatus.Perfect;
        }
        else if (m_greatZone.CheckHit(m_bar.Angle))
        {
            status = eHitStatus.Great;
        }
        else
        {
            status = eHitStatus.Miss;
        }

        return status;
    }

    public void SetHitText(string text)
    {
        HitText.text = text;
        StopAllCoroutines();
        StartCoroutine(HitTextShowing());
    }

    public IEnumerator HitTextShowing()
    {
        HitText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        HitText.gameObject.SetActive(false);
    }

    public void SpawnAllNewZones()
    {
        m_greatZone.GenerateNewZone(m_bar.Angle + Random.Range(150f, 220f));
        m_perfectZone.GenerateNewZone(m_greatZone.EndAngle - (m_greatZone.ZoneImg.fillAmount * 100));
    }

    public void ReduceSizeAndIncreaseSpeed()
    {
        m_greatZone.ReduceZoneSize();
        m_perfectZone.ReduceZoneSize();

        m_bar.IncreaseBarSpeed();
    }

    public void EnableMovingZone(bool enable)
    {
        m_isHitAreaMoving = enable;

        if (enable == true)
            SpeedRotate = Random.Range(SpeedRotateMin, SpeedRotateMax);
    }

    public void SetActiveUIZones(bool setActive, bool showTapIcon)
    {
        m_greatZone.gameObject.SetActive(setActive);
        m_perfectZone.gameObject.SetActive(setActive);
        m_bar.gameObject.SetActive(setActive);

        m_tapIcon.SetActive(showTapIcon);
    }

    public void ResetVariables()
    {
        EnableMovingZone(false);

        m_bar.ResetVariables();
        m_greatZone.ResetVariables();
        m_perfectZone.ResetVariables();
        SpawnAllNewZones();
        //m_gameManager.ResetCombo();
        //m_gameManager.UpdateSpeedText(m_unit);
    }
}
