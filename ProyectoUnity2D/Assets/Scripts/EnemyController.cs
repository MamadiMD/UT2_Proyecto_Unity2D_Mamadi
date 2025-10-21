using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 5.0f;
    public float moveSpeed = 2.0f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool enMovimiento;
    private Animator animator;
    private bool mirandoDerecha = true;

    private bool puedeHacerDanio = true;
    public float tiempoEntreGolpes = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;

            if (direction.x < 0 && mirandoDerecha)
            {
                Flip();
            }
            if (direction.x > 0 && !mirandoDerecha)
            {
                Flip();

            }

            movement = new Vector2(direction.x, direction.y);

            enMovimiento = true;
        }
        else
        {
            movement = Vector2.zero;
            enMovimiento = false;
        }
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        animator.SetBool("enMovimiento", enMovimiento);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (puedeHacerDanio && collision.gameObject.CompareTag("Player"))
        {
            puedeHacerDanio = false;
            Vector2 direccionDanio = (collision.transform.position - transform.position).normalized;

            collision.gameObject.GetComponent<PlayerMovement>().RecibeDanio(direccionDanio, 1);
            StartCoroutine(ReiniciarGolpe());
        }
    }

    void Flip()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

    }
    
    private IEnumerator ReiniciarGolpe()
    {
        yield return new WaitForSeconds(tiempoEntreGolpes);
        puedeHacerDanio = true;
    }
}
