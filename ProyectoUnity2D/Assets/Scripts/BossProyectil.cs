using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectil : MonoBehaviour
{
    public int damage = 1;
    public float lifetime = 5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                Vector2 dir = (other.transform.position - transform.position).normalized;
                player.RecibeDanio(dir, damage);
            }

            Destroy(gameObject);
        }
        else if (!other.isTrigger)
        {
            // Si choca con un muro u otro objeto sólido
            Destroy(gameObject);
        }
    }
}
