using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontalMove;
    private float verticalMove;
    private Vector3 playerInput;

    // variable para almacenar donde se mueve nuestro jugador
    private Vector3 movePlayer;

    public CharacterController player;
    public float gravity = 9.8f;
    public float fallVelocity;
    public float jumpForce;

    public float playerSpeed;

    //funciones para la camara
    public Camera mainCamara;
    private Vector3 camForward; //fortar o delante
    private Vector3 camRight; //    derecha 


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        playerInput = new Vector3(horizontalMove, 0, verticalMove);
        playerInput = Vector3.ClampMagnitude(playerInput, 1); //que al momento de moverlo no sea mayor1 o menor que 0

        //  funcion que se mueva conforme a la camara
        CamDirection();
        
        //direccion prar moverse hacia derecha izquierda movienose hacia a camara
        movePlayer = playerInput.x * camRight + playerInput.z * camForward;
        movePlayer = movePlayer * playerSpeed;
        //mirar el jugador con el compontete tranform
        player.transform.LookAt(player.transform.position + movePlayer); // que gire hacia donde pulcemos las teclas

        SetGravity();
        PlayersKills();

        player.Move(movePlayer * Time.deltaTime);        


        Debug.Log(player.velocity.magnitude);
    }
    void CamDirection()
    {
        camForward = mainCamara.transform.forward; // direccion hacia delante de la camara principal
        camRight = mainCamara.transform.right; // derecha

        //vectores hacia la derecha y izquierda
        camForward.y = 0;
        camRight.y = 0;

        //ver una mejor direccion donde ve el vector o la camra
        camForward = camForward.normalized;
        camRight = camRight.normalized;

    }
    void SetGravity()
    {
        if (player.isGrounded)
        {
            fallVelocity = -gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }
        else
        {
            fallVelocity -= gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }
    }
    public void PlayersKills()
    {
        if (player.isGrounded && Input.GetButtonDown("Jump"))
        {
            fallVelocity = jumpForce;
            movePlayer.y = fallVelocity;
        }

    }
}