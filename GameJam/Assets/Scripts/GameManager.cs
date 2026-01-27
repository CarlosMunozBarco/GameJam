using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int coins = 0;
    public float time = 0;


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

    private void Start()
    {
        UIManager.Instance.UpdateCoins((int)coins);
        UIManager.Instance.UpdateTime(time);
    }

    private void Update()
    {
        time += Time.deltaTime;
        UIManager.Instance.UpdateTime(time);
    }
    public void AddCoins(int value)
    {
        coins += value;
        UIManager.Instance.UpdateCoins(coins);
    }
}
