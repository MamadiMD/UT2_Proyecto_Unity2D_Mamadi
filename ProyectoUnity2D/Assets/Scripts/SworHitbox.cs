using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SworHitbox : MonoBehaviour
{
    private BoxCollider2D hitboxCollider;

    private void Awake()
    {
        hitboxCollider = GetComponent<BoxCollider2D>();
        hitboxCollider.enabled = false; // Desactivada al inicio
    }

    public void ActivarHitbox()
    {
        hitboxCollider.enabled = true;
    }

    public void DesactivarHitbox()
    {
        hitboxCollider.enabled = false;
    }
}
