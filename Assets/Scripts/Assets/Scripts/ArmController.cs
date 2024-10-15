using UnityEngine;

public class ArmController : LimbController
{
    void FixedUpdate()
    {
        Vector2 shoulderPosition = upperArm.position;
        Vector2 toTarget = targetPosition - shoulderPosition;
        
        // 限制手臂长度
        if (toTarget.magnitude > totalArmLength)
        {
            toTarget = toTarget.normalized * totalArmLength;
        }

        // 计算手肘位置（简化版，可能需要更复杂的IK算法来获得更真实的效果）
        Vector2 elbowPosition = shoulderPosition + toTarget * 0.5f;

        // 为上臂和下臂分别计算力
        Vector2 upperArmForce = (elbowPosition - (Vector2)upperArm.position) * armStrength;
        Vector2 bottomArmForce = (targetPosition - (Vector2)bottomArm.position) * armStrength;

        // 应用力
        upperArm.AddForce(upperArmForce);
        bottomArm.AddForce(bottomArmForce);

        // 对身体施加较小的力以创造拖拽效果
        // body.AddForce(toTarget.normalized * armStrength * bodyForceMultiplier);

        // 为手臂添加一些阻尼，防止过度震荡
        upperArm.velocity *= 0.9f;
        bottomArm.velocity *= 0.9f;
    }


}
