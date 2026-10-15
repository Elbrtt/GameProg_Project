using UnityEngine;

public class MovemenComponent : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Movement Stats")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float rotateSpeed = 8f;

    [Header("Collission Config")]
    [SerializeField] private float avoidanceAngleStep = 15f;
    [SerializeField] private float avoidanceMaxAngle = 90f;

    private CapsuleCollider selfCollider;

    private void Awake()
    {
        selfCollider = GetComponent<CapsuleCollider>();
    }

    private void GetCapsulePoints(out Vector3 p1, out Vector3 p2, out float capsuleRadius)
    {
        float colliderHeight = selfCollider.height;
        float colliderRadius = selfCollider.radius;

        Vector3 center = transform.position + selfCollider.center;

        float halfHeight = Mathf.Max(colliderHeight / 2f - colliderRadius, 0f);

        p1 = center + Vector3.up * halfHeight;
        p2 = center - Vector3.up * halfHeight;

        capsuleRadius = colliderRadius;
    }

    private bool isPathBlocked(Vector3 dir, float moveDistance)
    {
        GetCapsulePoints(out Vector3 p1, out Vector3 p2, out float r);
        return Physics.CapsuleCast(p1, p2, r, dir, moveDistance, obstacleLayer);
    }
    
    private Vector3 ResolveMoveDir(Vector3 moveDir, float moveDistance)
    {
        if (moveDir == Vector3.zero) return Vector3.zero;

        if (!isPathBlocked(moveDir, moveDistance)) return moveDir;

        for(float angle = avoidanceAngleStep; angle <= avoidanceMaxAngle; angle += avoidanceAngleStep)
        {
            Vector3 rightDir = Quaternion.Euler(0, angle, 0) * moveDir;

            if (!isPathBlocked(rightDir, moveDistance)) return rightDir;

            Vector3 leftDir = Quaternion.Euler(0, -angle, 0) * moveDir;

            if (!isPathBlocked(leftDir, moveDistance)) return leftDir;
        }

        return Vector3.zero;
    }

    public void Move(Vector3 rawDir, float moveDistance)
    {
        Vector3 resolvedDir = ResolveMoveDir(rawDir, moveDistance);

        Vector3 nextPos = transform.position + resolvedDir * moveDistance;
        nextPos = SnapToGroundY(nextPos);

        transform.position = nextPos;
    }
    
    public Vector3 SnapToGroundY(Vector3 position)
    {
        Vector3 rayOriginGround = position + Vector3.up * selfCollider.height;
        float rayDistance = 2f + selfCollider.height;

        if(Physics.Raycast(rayOriginGround,Vector3.down,out RaycastHit hit, rayDistance, groundLayer))
        {
            position.y = hit.point.y;
        }

        return position;
    }

    public float GetSpeed() => speed;
}
