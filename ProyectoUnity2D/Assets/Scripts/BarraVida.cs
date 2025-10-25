using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image rellenoVida;
    private PlayerMovement playerMovement;
    private float vidaMaxima;

    private float tiempoOculto = 2f;

    // Start is called before the first frame update
    void Start()
    {
        rellenoVida = GameObject.Find("Fill").GetComponent<Image>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        vidaMaxima = playerMovement.vida;
        StartCoroutine(MostrarCanvasDespuesDeTiempo());
    }

    // Update is called once per frame
    void Update()
    {
        rellenoVida.fillAmount = playerMovement.vida / vidaMaxima;
    }

    private IEnumerator MostrarCanvasDespuesDeTiempo()
    {
        // 👇 Asegúrate de que el canvas está asignado en el Inspector
        if (canvas != null)
        {
            canvas.enabled = false; // Oculta el canvas
            yield return new WaitForSeconds(tiempoOculto);
            canvas.enabled = true;  // Muestra el canvas después del tiempo
        }
    }
}
