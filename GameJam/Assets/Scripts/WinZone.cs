using TMPro;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    public GameObject winPanel;
    public TMP_Text timeText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Time.timeScale = 0f; // Pause the game
            winPanel.SetActive(true);
            timeText.text = $"Time: {GameManager.Instance.time:F2}";
        }
    }
}
