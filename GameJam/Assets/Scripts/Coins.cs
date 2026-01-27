using UnityEngine;

public class Coins : MonoBehaviour
{
    // valor monedas
    [SerializeField] private int value;
    void OnTriggerEnter2D(Collider2D collision)
    {
        // si es el jugador el que entra en nuestro trigger...
        if (collision.gameObject.CompareTag("Player"))
        {
            // suma los puntos en el ui del player
            GameManager.Instance.AddCoins(value);
            // destruimos la fruta
            Destroy(gameObject);
        }
    }
}
