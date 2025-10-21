using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ataque : MonoBehaviour
{

    Vector2 ataqueIzq;
    Collider2D colliderEspada;


    void Start()
    {
        colliderEspada = GetComponent<Collider2D>();
        ataqueIzq = transform.position;
    }

    public void ataqueIzquierda()
    {
        colliderEspada.enabled = true;
        transform.position = ataqueIzq;
    }

    public void ataqueDerecha()
    {
        colliderEspada.enabled = true;
        transform.position = new Vector2(1.45f,0.20f);
    }



    public void detenerAtaque()
    {
        colliderEspada.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
