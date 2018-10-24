using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eProgressBarStatus
{
    WaitForZone,
    InZone,
    PassedZone,
}

public class UIProgressBar : MonoBehaviour {

    public float Speed { get { return m_speed; } }
    [SerializeField][Range(0, 1)] private float m_speed;
    [SerializeField][Range(0, 1)]
    private float m_speedLimit;
    private float m_speedOrigin;

    public float Angle { get { return m_angle; } }
    private float m_angle;

    public eProgressBarStatus Status { get { return m_status; } set { m_status = value; } }
    private eProgressBarStatus m_status;

    [SerializeField][Range(0, 100)] private float m_increaseSpeedPercentage;

    void Start ()
    {
        m_speedOrigin = m_speed;
        ResetVariables();

        if (m_speed > m_speedLimit)
            Debug.LogError("BarSpeed can't be higher than BarSpeedLimit");
    }
	
	void Update ()
    {
        m_angle += m_speed * 360 * Time.deltaTime;
        this.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, -m_angle);

        if (m_angle >= 360)
        {
            m_angle = 0;
        }
    }

    public void IncreaseBarSpeed()
    {
        m_status = eProgressBarStatus.WaitForZone;

        if (m_speed < m_speedLimit)
        {
            m_speed += ((m_speedOrigin + m_speedLimit) * m_increaseSpeedPercentage) / 100;
        }
        else
        {
            m_speed = m_speedLimit;
        }
    }

    public void ResetVariables()
    {
        m_angle = 0;
        m_speed = m_speedOrigin;
        m_status = eProgressBarStatus.WaitForZone;
        this.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 0);
    }
}
