using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    public float vida = 10f;
    private Rigidbody2D playerRb;
    private Vector2 moveImput;
    private Animator playerAnimator;

    private Vector2 lastMove;

    private float fueraRebote = 4f;
    private bool recibeDanio;

    private bool atacando;
    public float attackDuration = 0.1f;

    private AudioSource musicaFondo; 

    private AudioSource sonidoAtaque;

    [SerializeField] private SworHitbox hitboxUp;
    [SerializeField] private SworHitbox hitboxDown;
    [SerializeField] private SworHitbox hitboxLeft;
    [SerializeField] private SworHitbox hitboxRight;

    public Image imagenCinematica;   // Asignar desde el Inspector
    public float duracionVisible = 4f; // Tiempo visible antes del fade
    public float duracionFade = 4f;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();

        hitboxUp = hitboxUp.GetComponent<SworHitbox>();
        hitboxDown = hitboxDown.GetComponent<SworHitbox>();
        hitboxLeft = hitboxLeft.GetComponent<SworHitbox>();
        hitboxRight = hitboxRight.GetComponent<SworHitbox>();

        DesactivarHitboxes();

        if (imagenCinematica != null)
            imagenCinematica.gameObject.SetActive(false);
        
        GameObject camara = GameObject.Find("Main Camera");
        if (camara != null)
            musicaFondo = camara.GetComponent<AudioSource>();

        sonidoAtaque = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (atacando) return; // No moverse mientras ataca

        // Obtener la entrada del jugador
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveImput = new Vector2(moveX, moveY).normalized;

        // Actualizar la última dirección de movimiento si hay entrada
        if (moveImput != Vector2.zero)
        {
            lastMove = moveImput;
        }

        // Ataque
        if (Input.GetKeyDown(KeyCode.E) && !atacando)
        {
            StartCoroutine(AtacandoCoroutine());
        }

        // Actualizar las variables del Animator
        playerAnimator.SetFloat("Horizontal", moveX);
        playerAnimator.SetFloat("Vertical", moveY);
        playerAnimator.SetFloat("Speed", moveImput.sqrMagnitude);
        playerAnimator.SetFloat("LastMoveHorizontal", lastMove.x);
        playerAnimator.SetFloat("LastMoveVertical", lastMove.y);
        playerAnimator.SetBool("RecibeDanio", recibeDanio);
        playerAnimator.SetBool("Atacando", atacando);
    }

    private void FixedUpdate()
    {   
        // Movimiento del jugador
        if (recibeDanio || atacando) return;
        playerRb.MovePosition(playerRb.position + moveImput * speed * Time.fixedDeltaTime);
    }

    public void RecibeDanio(Vector2 direccion, int cantidadDanio)
    {
        if (!recibeDanio)
        {
            vida -= cantidadDanio;
            recibeDanio = true;
            Vector2 rebote = ((Vector2)transform.position - direccion).normalized;

            playerAnimator.SetFloat("LastMoveHorizontal", rebote.x);
            playerAnimator.SetFloat("LastMoveVertical", rebote.y);
            playerAnimator.SetBool("RecibeDanio", true);

            playerRb.AddForce(rebote * fueraRebote, ForceMode2D.Impulse);

            if (vida <= 0)
            {
                StartCoroutine(Morir());

            }
            
            StartCoroutine(ResetearDanio());
            
        }

    }

    private IEnumerator ResetearDanio()
    {
        yield return new WaitForSeconds(0.3f);
        playerRb.velocity = Vector2.zero;
        recibeDanio = false;
        playerAnimator.SetBool("RecibeDanio", false);
    }

    private IEnumerator AtacandoCoroutine()
    {
        atacando = true;
        ActivarHitbox();
        sonidoAtaque.Play();
        yield return new WaitForSeconds(attackDuration);
        DesactivarHitboxes();
        NoAtacando();
    }

    private void ActivarHitbox()
    {
        // DesactivarHitboxes();

        if (Mathf.Abs(lastMove.x) > Mathf.Abs(lastMove.y))
        {
            if (lastMove.x > 0)
                hitboxRight.ActivarHitbox();
            else
                hitboxLeft.ActivarHitbox();
        }
        else
        {
            if (lastMove.y > 0)
                hitboxUp.ActivarHitbox();
            else
                hitboxDown.ActivarHitbox();
        }
    }

    private void DesactivarHitboxes()
    {
        hitboxUp.DesactivarHitbox();
        hitboxDown.DesactivarHitbox();
        hitboxLeft.DesactivarHitbox();
        hitboxRight.DesactivarHitbox();
    }


    public void NoAtacando()
    {
        atacando = false;
    }

    public void desactivarDanio()
    {
        recibeDanio = false;
    }

    public IEnumerator MostrarCinematica()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        imagenCinematica.gameObject.SetActive(true);

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

    private IEnumerator Morir()
    {
        recibeDanio = true;
        atacando = false;
        playerRb.velocity = Vector2.zero;

        playerAnimator.SetBool("isDie", true);

        yield return new WaitForSeconds(0.4f);

        yield return StartCoroutine(MostrarCinematica());

        
    }
}
