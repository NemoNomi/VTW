using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerStats", menuName = "Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Movement Settings")]
    public float speed = 8f;
    public float climbingSpeedUp = 4f;
    public float climbingSpeedDown = 2f;
    public float jumpingPower = 16f;
    public float airControl = 5f;

    [Header("Feature Toggles")]
    public bool canMove = true;
    public bool canJump = true;
    public bool canDoubleJump = true;
    public bool canClimb = true;

    [Header("Timing Settings")]
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.2f;
    public float climbBufferTime = 0.2f;
}