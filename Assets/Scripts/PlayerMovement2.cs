using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//OLD PLAYER MOVEMENT SCRIPT MOVED HERE
// (pre-walljump attempt)

public class PlayerMovement2 : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float runSpeed = 2f;
    
    // Jumping
    [SerializeField] private float jumpSpeed = 3f;
    private bool isGrounded;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform groundCheckL;
    [SerializeField] private Transform groundCheckR;
    // [SerializeField] private Transform wallCheckL;
    // [SerializeField] private Transform wallCheckLTop;
    // [SerializeField] private Transform wallCheckR;
    // [SerializeField] private Transform wallCheckRTop;
    private float coyoteTime = 0.1f;
    private float coyoteTimeCounter;
    private float jumpBufferTime = 0.05f;
    private float jumpBufferCounter;

    // Sound effects
    public AudioSource jumpAudio;
    public AudioSource walkAudio;

    private GameMaster gm;

    public Vector2 playerDeathLoc;

    public Animator playerAnimator;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        //transform.position = gm.lastCheckPointPos;
    }


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate()
    {
        // Left/Right Movement
        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            rb.velocity = new Vector2(runSpeed,rb.velocity.y);
            if (!walkAudio.isPlaying)
            {
                walkAudio.Play();
            }
        }
        else if (Input.GetKey("a") || Input.GetKey("left"))
        {
            rb.velocity = new Vector2(-runSpeed,rb.velocity.y);
            if (!walkAudio.isPlaying)
            {
                walkAudio.Play();
            }
        }
        else
        {
            rb.velocity = new Vector2(0,rb.velocity.y);
        }


        // JUMPING
        // Checks if player is grounded
        // or has hit the wall
        if (Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"))
            || Physics2D.Linecast(transform.position, groundCheckL.position, 1 << LayerMask.NameToLayer("Ground"))
            || Physics2D.Linecast(transform.position, groundCheckR.position, 1 << LayerMask.NameToLayer("Ground")))
            // || Physics2D.Linecast(transform.position, wallCheckL.position, 1 << LayerMask.NameToLayer("Ground"))
            // || Physics2D.Linecast(transform.position, wallCheckLTop.position, 1 << LayerMask.NameToLayer("Ground"))
            // || Physics2D.Linecast(transform.position, wallCheckR.position, 1 << LayerMask.NameToLayer("Ground"))
            // || Physics2D.Linecast(transform.position, wallCheckRTop.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            //play landing animation
            if (isGrounded == false)
            {
                playerAnimator.SetTrigger("Landing");
            }

            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        // if (Physics2D.Linecast(transform.position, wallCheckL.position, 1 << LayerMask.NameToLayer("Ground"))
        //     || Physics2D.Linecast(transform.position, wallCheckLTop.position, 1 << LayerMask.NameToLayer("Ground"))
        //     || Physics2D.Linecast(transform.position, wallCheckR.position, 1 << LayerMask.NameToLayer("Ground"))
        //     || Physics2D.Linecast(transform.position, wallCheckRTop.position, 1 << LayerMask.NameToLayer("Ground")))
        // {
        //     Debug.Log("Wallcheck detected.");
        // }

        // Coyote Time & Jump Buffering
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetKey("w") || Input.GetKey("space") || Input.GetKey("up"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // The jump itself
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);


            jumpAudio.Play();

            playerAnimator.SetTrigger("Jump");

            coyoteTimeCounter = 0f;
            jumpBufferCounter = 0f;
        }

        playerDeathLoc = new Vector2(rb.position.x, rb.position.y + 0.8f);
    }
}
