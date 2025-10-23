using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CinematicaImagen : MonoBehaviour
{
    public Image imagenCinematica;   // Asignar desde el Inspector
    public float duracionVisible = 2f; // Tiempo visible antes del fade
    public float duracionFade = 2f;    // Tiempo del desvanecimiento

    void Start()
    {
        StartCoroutine(MostrarCinematica());
    }

    IEnumerator MostrarCinematica()
    {
        // Asegura que empiece completamente visible
        Color color = imagenCinematica.color;
        color.a = 1f;
        imagenCinematica.color = color;

        // Espera mientras se muestra la imagen
        yield return new WaitForSeconds(duracionVisible);

        // Desvanece la imagen gradualmente
        float elapsed = 0f;
        while (elapsed < duracionFade)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duracionFade);
            color.a = alpha;
            imagenCinematica.color = color;
            yield return null;
        }

        // Asegura que quede completamente invisible al final
        color.a = 0f;
        imagenCinematica.color = color;

        imagenCinematica.gameObject.SetActive(false);

    }
}
