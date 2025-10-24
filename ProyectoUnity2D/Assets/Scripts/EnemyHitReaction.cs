using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitReaction : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Si choca con algo con tag "Espada"
        if (other.CompareTag("Espada"))
        {
            Vector2 knockDir = (transform.position - other.transform.position).normalized;

            // Activar animación de daño
            animator.SetBool("isHit", true);

            // Si tiene Rigidbody2D, aplica un pequeño rebote
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(knockDir * 2f, ForceMode2D.Impulse); // Ajusta fuerza
            }

            // Desactivar animación después de 0.4 s
            StartCoroutine(ResetHit());
        }
    }

    private IEnumerator ResetHit()
    {
        yield return new WaitForSeconds(0.1f); // Duración de la animación
        animator.SetBool("isHit", false);
    }
}
