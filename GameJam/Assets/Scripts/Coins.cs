using UnityEngine;

public class Coins : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        // si es el jugador el que entra en nuestro trigger...
        if (collision.gameObject.CompareTag("Player"))
        {
            // TO_DO que sume los puntos en el ui del player
            // destruimos la fruta
            Destroy(gameObject);
        }
    }
}
