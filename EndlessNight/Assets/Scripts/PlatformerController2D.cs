using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
public class PlatformerController2D : MonoBehaviour {

    public LayerMask m_collisonMask;

    const float m_skinWidth = .015f;
    [SerializeField] private int m_horizontalRaycount = 4;
    [SerializeField] private int m_verticalRaycount = 4;

    private float m_horizontalRaySpacing;
    private float m_verticalRaySpacing;

    private BoxCollider2D m_collider;
    RaycastOrigins m_raycastOrigins;

    public CollisionInfo Collisions { get { return m_collisions; } }
    private CollisionInfo m_collisions;

    private void Awake()
    {
        m_collider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        CalculateRaySpacing();
    }

    void Update()
    {

    }

    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();
        m_collisions.Reset();

        if (velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }

        transform.Translate(velocity);
    }

    // "REF" = Any change inside a method will actually change the variable
    private void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x); // Left = -1, Right = 1
        float rayLength = Mathf.Abs(velocity.x) + m_skinWidth;

        for (int i = 0; i < m_horizontalRaycount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? m_raycastOrigins.bottomLeft : m_raycastOrigins.bottomRight; // To see which moving direction and set the raycast
            rayOrigin += Vector2.up * (m_horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, m_collisonMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit) // If raycast hit something
            {
                velocity.x = (hit.distance - m_skinWidth) * directionX;
                rayLength = hit.distance;

                m_collisions.left = directionX == -1;
                m_collisions.right = directionX == 1;
            }
        }
    }

    private void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y); // Down = -1, Up = 1
        float rayLength = Mathf.Abs(velocity.y) + m_skinWidth;

        for (int i = 0; i < m_verticalRaycount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? m_raycastOrigins.bottomLeft : m_raycastOrigins.topLeft; // To see which moving direction and set the raycast
            rayOrigin += Vector2.right * (m_verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, m_collisonMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit) // If raycast hit something
            {
                velocity.y = (hit.distance - m_skinWidth) * directionY;
                rayLength = hit.distance;

                m_collisions.below = directionY == -1;
                m_collisions.above = directionY == 1;
            }
        }
    }

    public void UpdateRaycastOrigins()
    {
        Bounds bounds = m_collider.bounds;
        bounds.Expand(m_skinWidth * -2);

        m_raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        m_raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        m_raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        m_raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    // Calculate spacing between each raycast
    void CalculateRaySpacing()
    {
        Bounds bounds = m_collider.bounds;
        bounds.Expand(m_skinWidth * -2);

        // Raycast minimum is 2
        m_horizontalRaycount = Mathf.Clamp(m_horizontalRaycount, 2, int.MaxValue);
        m_verticalRaycount = Mathf.Clamp(m_verticalRaycount, 2, int.MaxValue);

        m_horizontalRaySpacing = bounds.size.y / (m_horizontalRaycount - 1);
        m_verticalRaySpacing = bounds.size.x / (m_verticalRaycount - 1);
    }

    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }
}
