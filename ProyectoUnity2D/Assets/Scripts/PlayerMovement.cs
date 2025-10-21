using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private float speed = 3f;
    private Rigidbody2D playerRb;
    private Vector2 moveImput;
    private Animator playerAnimator;

    private Vector2 lastMove;

    private float fueraRebote = 4f;
    private bool recibeDanio;

    private bool atacando;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        // Obtener la entrada del jugador
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveImput = new Vector2(moveX, moveY).normalized;

        // Actualizar la última dirección de movimiento si hay entrada
        if (moveImput != Vector2.zero)
        {
            lastMove = moveImput;
        }

        if (Input.GetKeyDown(KeyCode.E) && !atacando)
        {
            Atacando();
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
        if (recibeDanio) return;
        playerRb.MovePosition(playerRb.position + moveImput * speed * Time.fixedDeltaTime);
    }

    public void RecibeDanio(Vector2 direccion, int cantidadDanio)
    {

        if (!recibeDanio)
        {
            recibeDanio = true;
            // Aplicar una fuerza de rebote en la dirección opuesta al daño recibido
            Vector2 rebote = ((Vector2)transform.position - direccion).normalized;

            playerAnimator.SetFloat("LastMoveHorizontal", rebote.x);
            playerAnimator.SetFloat("LastMoveVertical", rebote.y);
            playerAnimator.SetBool("RecibeDanio", true);
            

            playerRb.AddForce(rebote * fueraRebote, ForceMode2D.Impulse);
        }

        // Aquí puedes agregar lógica para reducir la salud del jugador
    }

    public void Atacando()
    {
        atacando = true;
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
