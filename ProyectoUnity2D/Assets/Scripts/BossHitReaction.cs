using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossHitReaction : MonoBehaviour
{
    private Animator animator;
    public float vida = 4f;

    public Image cinematicaFinal; 
    public float duracionVisible = 4f; // Tiempo visible antes del fade
    public float duracionFade = 4f;
    void Awake()
    {
        animator = GetComponent<Animator>();
        if (cinematicaFinal != null)
            cinematicaFinal.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Si choca con algo con tag "Espada"
        if (other.CompareTag("Espada"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            vida -= 1f;
            if (vida <= 0f)
            {
                animator.SetBool("isDie", true);
                if (rb != null)
                {
                    rb.bodyType = RigidbodyType2D.Kinematic;
                    StartCoroutine(MostrarCinematica()); 
                }
                Destroy(gameObject, 0.5f); 

            }
            Vector2 knockDir = (transform.position - other.transform.position).normalized;

            // Activar animación de daño
            animator.SetBool("isHit", true);

            // Si tiene Rigidbody2D, aplica un pequeño rebote
            
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


    public IEnumerator MostrarCinematica()
    {
        
        cinematicaFinal.gameObject.SetActive(true);

        // Asegura que empiece completamente visible
        Color color = cinematicaFinal.color;
        color.a = 1f;
        cinematicaFinal.color = color;

        // Espera mientras se muestra la imagen
        yield return new WaitForSeconds(duracionVisible);

        // Desvanece la imagen gradualmente
        float elapsed = 0f;
        while (elapsed < duracionFade)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duracionFade);
            color.a = alpha;
            cinematicaFinal.color = color;
            yield return null;
        }

        

        // Asegura que quede completamente invisible al final
        color.a = 0f;
        cinematicaFinal.color = color;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

        cinematicaFinal.gameObject.SetActive(false);

        
    }
}
