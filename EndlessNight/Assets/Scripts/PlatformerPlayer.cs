using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (PlatformerController2D))]
public class PlatformerPlayer : MonoBehaviour {

    private float m_moveSpeed = 6;
    private float m_gravity;
    private float m_jumpVelocity;

    private float m_accelerationTimeAirbone = .2f;
    private float m_accelerationTimeGrounded = .1f;

    [SerializeField] private float m_jumpHeight = 4;
    [SerializeField] private float m_timeToJumpApex = .4f; // How long to make player to reach the high point

    private Vector3 m_velocity;

    private float m_velocityXSmoothing;

    private PlatformerController2D m_controller;

    private void Awake()
    {
        m_controller = GetComponent<PlatformerController2D>();
    }

    void Start ()
    {
        m_gravity = -(2 * m_jumpHeight) / Mathf.Pow(m_timeToJumpApex, 2);
        m_jumpVelocity = Mathf.Abs(m_gravity * m_timeToJumpApex);

	}
	
	void Update ()
    {
        if (m_controller.Collisions.above || m_controller.Collisions.below)
        {
            m_velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space) && m_controller.Collisions.below)
        {
            m_velocity.y = m_jumpVelocity;
        }

        float targetVelocityX = input.x * m_moveSpeed;
        m_velocity.x = Mathf.SmoothDamp(m_velocity.x, targetVelocityX, ref m_velocityXSmoothing, (m_controller.Collisions.below) ? m_accelerationTimeGrounded : m_accelerationTimeAirbone);
        m_velocity.y += m_gravity * Time.deltaTime;
        m_controller.Move(m_velocity * Time.deltaTime);
	}
}
