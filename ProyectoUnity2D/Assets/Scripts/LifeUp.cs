using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUp : MonoBehaviour
{
    [SerializeField] private float cantidadCuracion = 5f; // cuánto cura cada corazón

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();

            if (player != null)
            {
                // Si la vida no está al máximo
                if (player.vida < 10f) 
                {
                    player.vida = Mathf.Min(player.vida + cantidadCuracion, 10f); // evita pasar del máximo
                }

                Destroy(gameObject); // Eliminar el corazón tras recogerlo
            }
        }
    }
}
