using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
public class PlatformerController2D : MonoBehaviour {

    public LayerMask CollisonMask;

    const float m_skinWidth = .015f;
    public int HorizontalRaycount = 4;
    public int VerticalRaycount = 4;

    private float m_horizontalRaySpacing;
    private float m_verticalRaySpacing;

    private float m_maxClimbAngle = 80;
    private float m_maxDescendAngle = 75;

    private BoxCollider2D m_collider;
    private RaycastOrigins m_raycastOrigins;

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

        if(velocity.y < 0)
        {
            DescendSlope(ref velocity);
        }
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

        for (int i = 0; i < HorizontalRaycount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? m_raycastOrigins.bottomLeft : m_raycastOrigins.bottomRight; // To see which moving direction and set the raycast
            rayOrigin += Vector2.up * (m_horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, CollisonMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit) // If raycast hit something
            {
                // Climb slope
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (i == 0 && slopeAngle <= m_maxClimbAngle)
                {
                    float distanceToSlopeStart = 0;
                    if(slopeAngle != m_collisions.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - m_skinWidth;
                        velocity.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlopeStart * directionX;
                }

                if (!m_collisions.climbingSlope || slopeAngle > m_maxClimbAngle)
                {
                    velocity.x = (hit.distance - m_skinWidth) * directionX;
                    rayLength = hit.distance;

                    if(m_collisions.climbingSlope)
                    {
                        velocity.y = Mathf.Tan(m_collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }

                    m_collisions.left = directionX == -1;
                    m_collisions.right = directionX == 1;
                }
            }
        }
    }

    private void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y); // Down = -1, Up = 1
        float rayLength = Mathf.Abs(velocity.y) + m_skinWidth;

        for (int i = 0; i < VerticalRaycount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? m_raycastOrigins.bottomLeft : m_raycastOrigins.topLeft; // To see which moving direction and set the raycast
            rayOrigin += Vector2.right * (m_verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, CollisonMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit) // If raycast hit something
            {
                velocity.y = (hit.distance - m_skinWidth) * directionY;
                rayLength = hit.distance;

                if(m_collisions.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(m_collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                m_collisions.below = directionY == -1;
                m_collisions.above = directionY == 1;
            }
        }

        if(m_collisions.climbingSlope)
        {
            float directionX = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x) + m_skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? m_raycastOrigins.bottomLeft : m_raycastOrigins.bottomRight) + Vector2.up * velocity.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, CollisonMask);

            if(hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if(slopeAngle != m_collisions.slopeAngle)
                {
                    velocity.x = (hit.distance - m_skinWidth) * directionX;
                    m_collisions.slopeAngle = slopeAngle;
                }
            }
        }
    }

    private void ClimbSlope(ref Vector3 velocity, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        // Can jump on slope
        if (velocity.y <= climbVelocityY)
        {
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            m_collisions.below = true;
            m_collisions.climbingSlope = true;
            m_collisions.slopeAngle = slopeAngle;
        }
    }

    private void DescendSlope(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        Vector2 rayOrigin = (directionX == 1) ? m_raycastOrigins.bottomRight : m_raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, CollisonMask);

        if(hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle != 0 && slopeAngle <= m_maxDescendAngle)
            {
                if (Mathf.Sign(hit.normal.x) == directionX)
                {
                    if(hit.distance - m_skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendVelocityY;

                        m_collisions.slopeAngle = slopeAngle;
                        m_collisions.descendingSlope = true;
                        m_collisions.below = true;
                    }
                }
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
        HorizontalRaycount = Mathf.Clamp(HorizontalRaycount, 2, int.MaxValue);
        VerticalRaycount = Mathf.Clamp(VerticalRaycount, 2, int.MaxValue);

        m_horizontalRaySpacing = bounds.size.y / (HorizontalRaycount - 1);
        m_verticalRaySpacing = bounds.size.x / (VerticalRaycount - 1);
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

        public bool climbingSlope;
        public bool descendingSlope;
        public float slopeAngle, slopeAngleOld;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }
}
