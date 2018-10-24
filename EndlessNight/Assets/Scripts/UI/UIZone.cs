using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIZone : MonoBehaviour {

    private RectTransform m_zoneRect;
    public Image ZoneImg { get { return m_zoneImg; } }
    private Image m_zoneImg;

    public float Range { get { return m_range; } set { m_range = value; } }
    private float m_range;
    [SerializeField][Range(0, 1)] private float m_rangeMin;
    private float m_rangeMax;

    public float StartAngle { get { return m_startAngle; } set { m_startAngle = value; } }
    private float m_startAngle;

    public float EndAngle { get { return m_endAngle; } set { m_endAngle = value; } }
    private float m_endAngle;

    private float m_startAngleOrigin;
    private float m_endAngleOrigin;

    [SerializeField][Range(0, 100)] private float m_reduceSizePercentage;

    private bool b_enableMoving;

    private void Awake()
    {
        m_zoneRect = GetComponent<RectTransform>();
        m_zoneImg = GetComponent<Image>();
    }

    void Start ()
    {
        if (m_zoneImg.fillAmount <= 0)
            Debug.LogError("FillAmount can't be 0.");
        if (m_rangeMin <= 0)
            Debug.LogError("ZoneRangeMin can't be 0.");

        UpdateZone();

        m_rangeMax = m_zoneImg.fillAmount;
        m_startAngleOrigin = m_startAngle;
        m_endAngleOrigin = m_endAngle;
        b_enableMoving = false;
    }
	
	void Update ()
    {

	}

    private void UpdateZone()
    {
        m_range = m_zoneImg.fillAmount * 360;
        m_startAngle = Mathf.Abs(m_zoneRect.localEulerAngles.z - 360);
        m_endAngle = Mathf.Abs(m_zoneRect.localEulerAngles.z - 360) + m_range;

        if (m_startAngle >= 360) m_startAngle -= 360;
        if (m_endAngle >= 360) m_endAngle -= 360;
    }

    public bool CheckHit(float barAngle)
    {
        bool isHit = false;

        // Check if the ball is on the hit area or not
        if (Mathf.Abs(m_endAngle - m_startAngle) >= (m_range - 0.1f) &&
            Mathf.Abs(m_endAngle - m_startAngle) <= (m_range + 0.1f))
        {
            if (barAngle >= m_startAngle && barAngle <= m_endAngle)
            {
                isHit = true;
            }
            else
            {
                isHit = false;
            }
        }
        else // When Begin and End Areas are between 0 angle, do this
        {
            if (barAngle >= m_startAngle || barAngle <= m_endAngle)
            {
                isHit = true;
            }
            else
            {
                isHit = false;
            }
        }

        return isHit;
    }

    public void GenerateNewZone()
    {
        m_zoneRect.localEulerAngles = new Vector3(0, 0, Random.Range(0f, 360f));
        UpdateZone();
    }
    public void GenerateNewZone(float angle)
    {
        m_zoneRect.localEulerAngles = new Vector3(0, 0, -angle);
        UpdateZone();
    }

    public void ReduceZoneSize()
    {
        if (m_zoneImg.fillAmount > m_rangeMin)
        {
            m_zoneImg.fillAmount -= ((m_rangeMin + m_rangeMax) * m_reduceSizePercentage) / 100;

            if(m_zoneImg.fillAmount < m_rangeMin)
                m_zoneImg.fillAmount = m_rangeMin;
        }
        else
        {
            m_zoneImg.fillAmount = m_rangeMin;
        }
    }

    public void ZoneMoving(float speed)
    {
        m_zoneRect.localEulerAngles += new Vector3(0, 0, -speed);
        UpdateZone();
    }

    public void ResetVariables()
    {
        m_startAngle = m_startAngleOrigin;
        m_endAngle = m_endAngleOrigin;

        m_zoneImg.fillAmount = m_rangeMax;

        GenerateNewZone();
    }
}
