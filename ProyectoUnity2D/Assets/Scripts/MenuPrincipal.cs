using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioSource musicaFondo; 

    private bool musicaPausada = false;

    private bool isPaused = false;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Busca los AudioSource en los objetos correspondientes
        GameObject camara = GameObject.Find("Main Camera");
        if (camara != null)
            musicaFondo = camara.GetComponent<AudioSource>();

    }

    public void StartGame()
    {
        StartCoroutine(StartGameWithDelay());
    }

    IEnumerator StartGameWithDelay()
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit....");
        StartCoroutine(QuitWithDelay());
    }

    IEnumerator QuitWithDelay()
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        Application.Quit();
    }

    public void pauseGame()
    {
        audioSource.Play();
        if (isPaused)
        {
            isPaused = false;
            Time.timeScale = 1f; // Reanuda el juego
            return;
        }
        isPaused = true;
        Time.timeScale = 0f; // Pausa el juego
    }

    //Método Pausar/Reanudar la música de fondo 
    public void ToggleMusicaFondo()
    {
        audioSource.Play();
        if (musicaFondo == null) return;

        if (musicaPausada)
        {
            musicaFondo.UnPause();
            musicaPausada = false;
        }
        else
        {
            musicaFondo.Pause();
            musicaPausada = true;
        }
    }

    public void HacerSonido()
    {
        audioSource.Play();
    }

    
}
