using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float maxHealth = 100f;
    public float currentHealth = 100f;
}
