using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float runSpeed = 2f;
    
    // Jumping
    [SerializeField] private float jumpSpeed = 3f;
    private bool isGrounded;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform groundCheckL;
    [SerializeField] private Transform groundCheckR;
    
    // Wall Jumping
    [SerializeField] private Transform wallCheckL;
    [SerializeField] private Transform wallCheckLTop;
    [SerializeField] private Transform wallCheckR;
    [SerializeField] private Transform wallCheckRTop;
    private bool OnWallRight;
    private bool OnWallLeft;
    private bool isWallJumping;
    private bool isSliding;
    [SerializeField] private float wallJumpForce;
    [SerializeField] private float wallSlidingSpeed;
    
    // Coyote Time
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

    void Update()
    {
        // JUMPING
        // Checks if player is grounded
        if (Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"))
            || Physics2D.Linecast(transform.position, groundCheckL.position, 1 << LayerMask.NameToLayer("Ground"))
            || Physics2D.Linecast(transform.position, groundCheckR.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            //play landing animation
            if (isGrounded == false)
            {
                playerAnimator.SetTrigger("Landing");
            }

            isGrounded = true;
            // Debug.Log("Ground detected.");
        }
        else
        {
            isGrounded = false;
        }
        
        // Checks if player has hit a wall 
        if (Physics2D.Linecast(transform.position, wallCheckL.position, 1 << LayerMask.NameToLayer("Ground"))
            || Physics2D.Linecast(transform.position, wallCheckLTop.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            Debug.Log("Left Wallcheck detected.");
            OnWallLeft = true;
            OnWallRight = false;
        }
        else if (Physics2D.Linecast(transform.position, wallCheckR.position, 1 << LayerMask.NameToLayer("Ground"))
                 || Physics2D.Linecast(transform.position, wallCheckRTop.position,
                     1 << LayerMask.NameToLayer("Ground")))
        {
            Debug.Log("Right Wallcheck detected.");
            OnWallRight = true;
            OnWallLeft = false;
        }
        else
        {
            OnWallRight = false;
            OnWallLeft = false;
        }
        
        // Checks if player is walljumping
        if ((OnWallLeft || OnWallRight) && (Input.GetKey("w") || Input.GetKey("space") || Input.GetKey("up")))
        {
            isWallJumping = true;
        }
        else
        {
            isWallJumping = false;
        }

        // Checks if player is sliding down wall
        if ((OnWallLeft || OnWallRight) && !isGrounded)
        {
            isSliding = true;
            Debug.Log("Sliding");
        }
        else
        {
            isSliding = false;
        }
        
        
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

/*
        // JUMPING
        // Checks if player is grounded
        if (Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"))
            || Physics2D.Linecast(transform.position, groundCheckL.position, 1 << LayerMask.NameToLayer("Ground"))
            || Physics2D.Linecast(transform.position, groundCheckR.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            //play landing animation
            if (isGrounded == false)
            {
                playerAnimator.SetTrigger("Landing");
            }

            isGrounded = true;
            // Debug.Log("Ground detected.");
        }
        else
        {
            isGrounded = false;
        }

        // Checks if player has hit a wall 
        if (Physics2D.Linecast(transform.position, wallCheckL.position, 1 << LayerMask.NameToLayer("Ground"))
            || Physics2D.Linecast(transform.position, wallCheckLTop.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            Debug.Log("Left Wallcheck detected.");
            OnWallLeft = true;
            OnWallRight = false;
        }
        else if (Physics2D.Linecast(transform.position, wallCheckR.position, 1 << LayerMask.NameToLayer("Ground"))
                 || Physics2D.Linecast(transform.position, wallCheckRTop.position,
                     1 << LayerMask.NameToLayer("Ground")))
        {
            Debug.Log("Right Wallcheck detected.");
            OnWallRight = true;
            OnWallLeft = false;
        }
        else
        {
            OnWallRight = false;
            OnWallLeft = false;
        }
        
        // Checks if player is sliding down wall
        if ((OnWallLeft || OnWallRight) && !isGrounded)
        {
            isSliding = true;
            Debug.Log("Sliding");
        }
        else
        {
            isSliding = false;
        }
        */

        if (isSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }

        if (isWallJumping)
        {
            // rb.velocity = new Vector2(rb.velocity.x, wallJumpForce.x, wallJumpForce.y);
        }

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
        if (Input.GetKey("w") || Input.GetKey("space") || Input.GetKey("up"))
        {
            if ((jumpBufferCounter > 0f && coyoteTimeCounter > 0f) || OnWallLeft || OnWallRight)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);

                jumpAudio.Play();

                playerAnimator.SetTrigger("Jump");

                coyoteTimeCounter = 0f;
                jumpBufferCounter = 0f;
            }
        }

        // if ((jumpBufferCounter > 0f && coyoteTimeCounter > 0f) || OnWallLeft || OnWallRight)
        // {
        //     rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        //
        //     jumpAudio.Play();
        //
        //     playerAnimator.SetTrigger("Jump");
        //
        //     coyoteTimeCounter = 0f;
        //     jumpBufferCounter = 0f;
        // }
    
        // Tracks location of player's death for (!) animation
        playerDeathLoc = new Vector2(rb.position.x, rb.position.y + 0.8f);
    }
}
