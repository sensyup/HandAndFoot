using UnityEngine;

public class LegController : LimbController
{
    public enum ControlType
    {
        Keyboard,
        Mouse
    }

    public ControlType controlType = ControlType.Keyboard;
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public float keyboardMoveSpeed = 5f;
    public float legForce = 100f;
    public float frictionCoefficient = 0.5f;

    protected override void Start()
    {
        base.Start();
        targetPosition = bottomArm.position;
    }

    protected override void Update()
    {
        base.Update();
        if (controlType == ControlType.Keyboard)
        {
            UpdateKeyboardControl();
        }
        else
        {
            UpdateMouseControl();
        }
    }

    protected void FixedUpdate()
    {
        MoveLeg();
        ApplyFriction();
    }

    private void UpdateKeyboardControl()
    {
        Vector2 input = Vector2.zero;
        if (Input.GetKey(upKey)) input.y += 1;
        if (Input.GetKey(downKey)) input.y -= 1;
        if (Input.GetKey(leftKey)) input.x -= 1;
        if (Input.GetKey(rightKey)) input.x += 1;

        if (input != Vector2.zero)
        {
            input.Normalize();
            targetPosition += input * keyboardMoveSpeed * Time.deltaTime;
        }
    }

    private void UpdateMouseControl()
    {
        targetPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    protected Vector3 GetTargetPosition()
    {
        return targetPosition;
    }

    private void MoveLeg()
    {
        Vector2 toTarget = targetPosition - (Vector2)bottomArm.position;
        bottomArm.AddForce(toTarget * legForce);

        // 上部腿的运动
        Vector2 kneePosition = (upperArm.position + bottomArm.position) / 2;
        Vector2 upperLegForce = (kneePosition - (Vector2)upperArm.position) * legForce;
        upperArm.AddForce(upperLegForce);

        // 限制腿的总长度
        Vector2 legVector = bottomArm.position - upperArm.position;
        if (legVector.magnitude > totalArmLength)
        {
            bottomArm.position = upperArm.position + legVector.normalized * totalArmLength;
        }
    }

    private void ApplyFriction()
    {
        if (IsGrounded())
        {
            Vector2 frictionForce = -body.velocity * frictionCoefficient;
            body.AddForce(frictionForce);
        }
    }

    private bool IsGrounded()
    {
        float rayLength = 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(bottomArm.position, Vector2.down, rayLength);
        return hit.collider != null;
    }

    // protected override void OnDrawGizmos()
    // {
    //     base.OnDrawGizmos();
    //     if (Application.isPlaying)
    //     {
    //         Gizmos.color = Color.blue;
    //         Gizmos.DrawLine(upperArm.position, bottomArm.position);
    //         Gizmos.DrawWireSphere(targetPosition, 0.1f);
    //     }
    // }
}
