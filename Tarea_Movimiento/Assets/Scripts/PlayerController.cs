using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontalMove;  // Movimiento horizontal del jugador
    private float verticalMove;    // Movimiento vertical del jugador
    private Vector3 playerInput;   // Vector que almacena la combinaci�n de movimientos horizontal y vertical

    private Vector3 movePlayer;    // Vector que almacena la direcci�n y magnitud del movimiento del jugador

    public CharacterController player;  // Componente CharacterController del jugador
    public float gravity = 9.8f;        // Fuerza de gravedad
    public float fallVelocity;          // Velocidad de ca�da
    public float jumpForce;             // Fuerza de salto

    public float playerSpeed;           // Velocidad del jugador

    // Variables para la c�mara
    public Camera mainCamara;           // C�mara principal
    private Vector3 camForward;         // Direcci�n hacia adelante de la c�mara
    private Vector3 camRight;           // Direcci�n hacia la derecha de la c�mara
          //video 5
    public bool isOnSlope = false;       // Booleano para verificar si estamos en una pendiente
   
    private Vector3 hitNormal;          // Direcci�n del vector normal cuando colisionamos con algo
    public float slideVelocity;         // Velocidad de deslizamiento en pendientes
    public float slopeForceDown;        // Fuerza hacia abajo en una pendiente

    // Start se llama antes de la primera actualizaci�n del frame
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

        CamDirection();  // Ajusta la direcci�n de la c�mara

        movePlayer = playerInput.x * camRight + playerInput.z * camForward;  // Calcula el movimiento del jugador en relaci�n a la c�mara
        movePlayer = movePlayer * playerSpeed;  // Escala el movimiento seg�n la velocidad del jugador

        player.transform.LookAt(player.transform.position + movePlayer);  // Hace que el jugador mire en la direcci�n del movimiento

        SetGravity();  // Ajusta la gravedad del jugador
        PlayersKills();  // Controla el salto del jugador y el deslizamiento en pendientes

        player.Move(movePlayer * Time.deltaTime);  // Mueve al jugador

        Debug.Log(player.velocity.magnitude);  // Imprime la magnitud de la velocidad del jugador
    }

    void CamDirection()
    {
        camForward = mainCamara.transform.forward;  // Obtiene la direcci�n hacia adelante de la c�mara
        camRight = mainCamara.transform.right;  // Obtiene la direcci�n hacia la derecha de la c�mara

        camForward.y = 0;  // Anula la componente y de la direcci�n hacia adelante
        camRight.y = 0;    // Anula la componente y de la direcci�n hacia la derecha

        camForward = camForward.normalized;  // Normaliza la direcci�n hacia adelante
        camRight = camRight.normalized;      // Normaliza la direcci�n hacia la derecha
    }

    void SetGravity()
    {
        if (player.isGrounded)
        {
            fallVelocity = -gravity * Time.deltaTime;  // Si el jugador est� en el suelo, ajusta la velocidad de ca�da
            movePlayer.y = fallVelocity;  // Aplica la velocidad de ca�da al movimiento
        }
        else
        {
            fallVelocity -= gravity * Time.deltaTime;  // Si el jugador est� en el aire, aumenta la velocidad de ca�da
            movePlayer.y = fallVelocity;  // Aplica la velocidad de ca�da al movimiento
        }

        slideDown();  // Controla el deslizamiento en pendientes
    }

    public void PlayersKills()
    {
        if (player.isGrounded && Input.GetButtonDown("Jump"))
        {
            fallVelocity = jumpForce;  // Si el jugador est� en el suelo y presiona el bot�n de salto, aplica la fuerza de salto
            movePlayer.y = fallVelocity;  // Aplica la fuerza de salto al movimiento
        }
       
    }
    //video 5
    public void slideDown()
    {
        isOnSlope = Vector3.Angle(Vector3.up, hitNormal) >= player.slopeLimit;  // Verifica si el jugador est� en una pendiente

        if (isOnSlope)
        {
            movePlayer.x += ((1f - hitNormal.y) * hitNormal.x) * slideVelocity;  // Ajusta el movimiento en x seg�n el deslizamiento
            movePlayer.z += ((1f - hitNormal.y) * hitNormal.z) * slideVelocity;  // Ajusta el movimiento en z seg�n el deslizamiento
            movePlayer.y += slopeForceDown;  // A�ade una fuerza hacia abajo en la pendiente
        }
    }

  
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;  // Almacena la direcci�n normal del punto de colisi�n
    }
}