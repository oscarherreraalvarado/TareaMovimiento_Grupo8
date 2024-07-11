using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontalMove;  // Movimiento horizontal del jugador
    private float verticalMove;    // Movimiento vertical del jugador
    private Vector3 playerInput;   // Vector que almacena la combinación de movimientos horizontal y vertical

    private Vector3 movePlayer;    // Vector que almacena la dirección y magnitud del movimiento del jugador

    public CharacterController player;  // Componente CharacterController del jugador
    public float gravity = 9.8f;        // Fuerza de gravedad
    public float fallVelocity;          // Velocidad de caída
    public float jumpForce;             // Fuerza de salto

    public float playerSpeed;           // Velocidad del jugador

    // Variables para la cámara
    public Camera mainCamara;           // Cámara principal
    private Vector3 camForward;         // Dirección hacia adelante de la cámara
    private Vector3 camRight;           // Dirección hacia la derecha de la cámara
          //video 5
    public bool isOnSlope = false;       // Booleano para verificar si estamos en una pendiente
   
    private Vector3 hitNormal;          // Dirección del vector normal cuando colisionamos con algo
    public float slideVelocity;         // Velocidad de deslizamiento en pendientes
    public float slopeForceDown;        // Fuerza hacia abajo en una pendiente

    // Start se llama antes de la primera actualización del frame
    void Start()
    {
        player = GetComponent<CharacterController>();  // Inicializa el CharacterController del jugador
    }

    // Update se llama una vez por frame
    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal");  // Obtiene el movimiento horizontal del input
        verticalMove = Input.GetAxis("Vertical");      // Obtiene el movimiento vertical del input

        playerInput = new Vector3(horizontalMove, 0, verticalMove);  // Crea un vector con el input del jugador
        playerInput = Vector3.ClampMagnitude(playerInput, 1);  // Limita la magnitud del vector a 1

        CamDirection();  // Ajusta la dirección de la cámara

        movePlayer = playerInput.x * camRight + playerInput.z * camForward;  // Calcula el movimiento del jugador en relación a la cámara
        movePlayer = movePlayer * playerSpeed;  // Escala el movimiento según la velocidad del jugador

        player.transform.LookAt(player.transform.position + movePlayer);  // Hace que el jugador mire en la dirección del movimiento

        SetGravity();  // Ajusta la gravedad del jugador
        PlayersKills();  // Controla el salto del jugador y el deslizamiento en pendientes

        player.Move(movePlayer * Time.deltaTime);  // Mueve al jugador

        Debug.Log(player.velocity.magnitude);  // Imprime la magnitud de la velocidad del jugador
    }

    void CamDirection()
    {
        camForward = mainCamara.transform.forward;  // Obtiene la dirección hacia adelante de la cámara
        camRight = mainCamara.transform.right;  // Obtiene la dirección hacia la derecha de la cámara

        camForward.y = 0;  // Anula la componente y de la dirección hacia adelante
        camRight.y = 0;    // Anula la componente y de la dirección hacia la derecha

        camForward = camForward.normalized;  // Normaliza la dirección hacia adelante
        camRight = camRight.normalized;      // Normaliza la dirección hacia la derecha
    }

    void SetGravity()
    {
        if (player.isGrounded)
        {
            fallVelocity = -gravity * Time.deltaTime;  // Si el jugador está en el suelo, ajusta la velocidad de caída
            movePlayer.y = fallVelocity;  // Aplica la velocidad de caída al movimiento
        }
        else
        {
            fallVelocity -= gravity * Time.deltaTime;  // Si el jugador está en el aire, aumenta la velocidad de caída
            movePlayer.y = fallVelocity;  // Aplica la velocidad de caída al movimiento
        }

        slideDown();  // Controla el deslizamiento en pendientes
    }

    public void PlayersKills()
    {
        if (player.isGrounded && Input.GetButtonDown("Jump"))
        {
            fallVelocity = jumpForce;  // Si el jugador está en el suelo y presiona el botón de salto, aplica la fuerza de salto
            movePlayer.y = fallVelocity;  // Aplica la fuerza de salto al movimiento
        }
       
    }
    //video 5
    public void slideDown()
    {
        isOnSlope = Vector3.Angle(Vector3.up, hitNormal) >= player.slopeLimit;  // Verifica si el jugador está en una pendiente

        if (isOnSlope)
        {
            movePlayer.x += ((1f - hitNormal.y) * hitNormal.x) * slideVelocity;  // Ajusta el movimiento en x según el deslizamiento
            movePlayer.z += ((1f - hitNormal.y) * hitNormal.z) * slideVelocity;  // Ajusta el movimiento en z según el deslizamiento
            movePlayer.y += slopeForceDown;  // Añade una fuerza hacia abajo en la pendiente
        }
    }

  
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;  // Almacena la dirección normal del punto de colisión
    }
}