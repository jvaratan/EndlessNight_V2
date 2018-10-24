using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour {

    //private GameManager m_gameManager;

    private EnemyAnimation m_enemyAnim;

    [SerializeField] private float m_runSpeedMin, m_runSpeedMax;
    [SerializeField] private float m_accelelation;
    private float m_runSpeed;

    [SerializeField] private AudioClip m_hitSound;

    private void Awake()
    {
        //m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        m_enemyAnim = GetComponentInChildren<EnemyAnimation>();
    }

    void Start ()
    {
        m_enemyAnim.SetAnimation(EnemyAnimationStatus.Run);
	}
	
	void Update ()
    {
        CheckOffScreen();

        if (m_enemyAnim.Status == EnemyAnimationStatus.Run)
        {
            if (m_runSpeed < m_runSpeedMax)
            {
                m_runSpeed += m_accelelation;
                if (m_runSpeed > m_runSpeedMax)
                    m_runSpeed = m_runSpeedMax;
            }

            transform.position += new Vector3(m_runSpeed, 0, 0) * Time.deltaTime;
        }
        else
        {
            m_runSpeed = 0;
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Unit")
        {
            //m_gameManager.TimeManager.DoSlowmotion();
            m_enemyAnim.SetAnimation(EnemyAnimationStatus.Attack);

            StartCoroutine(HitUnit(other.GetComponent<Unit>()));
        }
    }

    IEnumerator HitUnit(Unit unit)
    {
        yield return new WaitForSeconds(0.4f);
        //m_gameManager.SoundManager.PlaySFXOneShot(m_hitSound);
        unit.UnitDie();

        // Stasrt record Gif
        yield return new WaitForSeconds(0.5f);
        //m_gameManager.Record.DoRecord();
    }

    // When boss is off screen show icon
    private void CheckOffScreen()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(this.transform.position);
        if ((0f <= viewPos.x && viewPos.x <= 1F) && (0f <= viewPos.y && viewPos.y <= 1F) && (Camera.main.transform.position.z < this.transform.position.z))
        {
            print("target is in the camera! ----- ");
            //m_gameManager.ShowBossIcon(false);
        }
        else if ((0f > viewPos.x))
        {
            print("target is on the left side!");
            //m_gameManager.ShowBossIcon(true);
        }
        else if ((viewPos.x > 1F))
        {
            print("target is on the right side!");
        }
        else
        {
            print("target is NOT in the camera!");
        }
    }
}
