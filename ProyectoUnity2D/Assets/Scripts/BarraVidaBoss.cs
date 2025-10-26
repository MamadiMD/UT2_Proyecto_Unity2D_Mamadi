using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVidaBoss: MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image rellenoVida;
    [SerializeField] private EnemyHitReaction enemigo; // Referencia al script de vida del enemigo
    private float vidaMaxima;

    private float tiempoOculto = 2f;

    void Start()
    {
        // Si no está asignado en el inspector, lo busca automáticamente en el mismo objeto
        if (enemigo == null)
            enemigo = GetComponentInParent<EnemyHitReaction>();

        if (enemigo != null)
            vidaMaxima = enemigo.vida;

        rellenoVida = GameObject.Find("Fill").GetComponent<Image>();

        if (canvas != null)
            canvas.enabled = false; // Oculta el canvas al inicio
    }

    void Update()
    {
        if (enemigo == null) return;

        // Actualiza el relleno de la barra
        rellenoVida.fillAmount = enemigo.vida / vidaMaxima;

        // Mostrar la barra si recibe daño
        if (!canvas.enabled && enemigo.vida < vidaMaxima)
            StartCoroutine(MostrarCanvasTemporal());
    }

    private IEnumerator MostrarCanvasTemporal()
    {
        if (canvas != null)
        {
            canvas.enabled = true;
            yield return new WaitForSeconds(tiempoOculto);
            canvas.enabled = false;
        }
    }
}
