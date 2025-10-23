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

        // Solo moverse si el jugador está dentro del rango
        if (dist <= chaseRange && dist > stoppingDistance)
        {
            dir = (target - pos).normalized;
            Vector2 move = dir * speed * Time.fixedDeltaTime;
            rb.MovePosition(pos + move);
        }

        // Actualizar Animator
        bool moving = dir != Vector2.zero;
        animator.SetBool("isMoving", moving);
        animator.SetFloat("moveX", dir.x);
        animator.SetFloat("moveY", dir.y);

        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), transform.localScale.z);
    }

    void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseRange);

        }
}
    
