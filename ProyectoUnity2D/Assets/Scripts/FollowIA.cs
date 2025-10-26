using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;


public class EnemyFollow : MonoBehaviour
{
    public Transform player;
    public float chaseRange = 5f;
    public float stoppingDistance = 0.5f;
    public float speed = 3f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool puedeHacerDanio = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (player == null)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go) player = go.transform;
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 pos = rb.position;
        Vector2 target = player.position;
        float dist = Vector2.Distance(pos, target);
        Vector2 dir = Vector2.zero;

        // Siempre mirar hacia el jugador
        if (player.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);  // mira derecha
        else
            transform.localScale = new Vector3(-1, 1, 1); // mira izquierda

        // Solo moverse si el jugador está dentro del rango
        if (dist <= chaseRange && dist > stoppingDistance)
        {
            dir = (target - pos).normalized;
            Vector2 move = dir * speed * Time.fixedDeltaTime;
            rb.MovePosition(pos + move);

            // Activar animación de movimiento
            animator.SetBool("isMoving", true);
        }
        else
        {
            // No se mueve, desactivar animación
            animator.SetBool("isMoving", false);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (puedeHacerDanio && collision.CompareTag("Player"))
        {
            puedeHacerDanio = false;
            Vector2 direccionDanio = (collision.transform.position - transform.position).normalized;

            PlayerMovement playerScript = collision.GetComponent<PlayerMovement>();
            if (playerScript != null)
                playerScript.RecibeDanio(direccionDanio, 1);

            StartCoroutine(ReiniciarGolpe());
        }
    }

    private IEnumerator ReiniciarGolpe()
    {
        yield return new WaitForSeconds(3f);
        puedeHacerDanio = true;
    }
}
    
