using UnityEngine;

public class LimbController : MonoBehaviour
{
    public Rigidbody2D upperArm;
    public Rigidbody2D bottomArm;
    public Rigidbody2D body;
    public float armStrength = 100f;
    public float bodyDrag = 5f;
    public float bodyForceMultiplier = 0.01f;  // 大幅减小施加在身体上的力

    protected Camera mainCamera;
    protected Vector2 targetPosition;
    protected float totalArmLength;

    protected virtual void Start()
    {
        mainCamera = Camera.main;
        body.drag = bodyDrag;

        // 计算总臂长
        float upperArmLength = GetArmSegmentLength(upperArm);
        float bottomArmLength = GetArmSegmentLength(bottomArm);
        totalArmLength = upperArmLength + bottomArmLength;

        Debug.Log($"Total arm length: {totalArmLength}");
    }

    float GetArmSegmentLength(Rigidbody2D armSegment)
    {
        // 假设手臂段是一个矩形碰撞体
        BoxCollider2D collider = armSegment.GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            // 使用碰撞体的尺寸
            return collider.size.y * armSegment.transform.lossyScale.y;
        }
        else
        {
            Debug.LogWarning("Arm segment does not have a BoxCollider2D. Using default length of 1.");
            return 1f;
        }
    }

    protected virtual void Update()
    {
        targetPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

   

    // void OnDrawGizmos()
    // {
    //     if (!Application.isPlaying) return;

    //     Gizmos.color = Color.red;
    //     Gizmos.DrawLine(upperArm.position, targetPosition);
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(targetPosition, 0.1f);
    // }
}