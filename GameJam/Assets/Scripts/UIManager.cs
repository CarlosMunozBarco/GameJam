using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TMP_Text coins;
    public TMP_Text lifes;
    public TMP_Text time;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateCoins(int coinCount)
    {
        coins.text = "Coins: " + coinCount.ToString("D2");
    }

    public void UpdateLifes(float current, float max)
    {
        lifes.text = $"Lifes: {current}/{max}";
    }

    public void UpdateTime(float timeCount)
    {
        time.text = timeCount.ToString("F0");
    }

}
