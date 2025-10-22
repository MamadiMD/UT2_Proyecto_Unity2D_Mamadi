using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    private Rigidbody2D playerRb;
    private Vector2 moveImput;
    private Animator playerAnimator;

    private Vector2 lastMove;

    private float fueraRebote = 4f;
    private bool recibeDanio;

    private bool atacando;
    public float attackDuration = 0.4f;

    private SworHitbox hitboxUp;
    private SworHitbox hitboxDown;
    private SworHitbox hitboxLeft;
    private SworHitbox hitboxRight;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();

        hitboxUp = hitboxUp.GetComponent<SworHitbox>();
        hitboxDown = hitboxDown.GetComponent<SworHitbox>();
        hitboxLeft = hitboxLeft.GetComponent<SworHitbox>();
        hitboxRight = hitboxRight.GetComponent<SworHitbox>();

        DesactivarHitboxes();
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
            recibeDanio = true;
            Vector2 rebote = ((Vector2)transform.position - direccion).normalized;

            playerAnimator.SetFloat("LastMoveHorizontal", rebote.x);
            playerAnimator.SetFloat("LastMoveVertical", rebote.y);
            playerAnimator.SetBool("RecibeDanio", true);

            playerRb.AddForce(rebote * fueraRebote, ForceMode2D.Impulse);
        }

    }

    private IEnumerator AtacandoCoroutine()
    {
        atacando = true;
        ActivarHitbox();
        yield return new WaitForSeconds(attackDuration);
        DesactivarHitboxes();
        NoAtacando();
    }

    private void ActivarHitbox()
    {
        DesactivarHitboxes();

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
}
