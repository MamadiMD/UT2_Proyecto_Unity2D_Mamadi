using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatallaBoss : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject pared;
    private AudioSource musicaFondo;

    private AudioSource musicaBoss;

    void Start()
    {
        pared.SetActive(false);
        musicaBoss = GetComponent<AudioSource>();
        GameObject camara = GameObject.Find("Main Camera");
        if (camara != null)
            musicaFondo = camara.GetComponent<AudioSource>();

        
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            pared.SetActive(true);
            musicaFondo.volume = 0;
            musicaBoss.Play();
        }
    }
}
