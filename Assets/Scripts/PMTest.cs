using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PMTest: MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float runSpeed = 2f;
    
    // Jumping
    [SerializeField] private float jumpSpeed = 3f;
    private bool isJumping;
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
    [SerializeField] private Vector2 wallJumpForce;
    [SerializeField] private float wallSlidingSpeed;
    [SerializeField] private float wallJumpDuration;
    
    // Coyote Time
    private float coyoteTime = 0.1f;
    private float coyoteTimeCounter;
    private float jumpBufferTime = 0.05f;
    private float jumpBufferCounter;

    // Sound effects
    public AudioSource jumpAudio;
    public AudioSource walkAudio;

    // Other
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
        
        // The jump itself
        // if (Input.GetKey("w") || Input.GetKey("space") || Input.GetKey("up"))
        // {
        //     if ((jumpBufferCounter > 0f && coyoteTimeCounter > 0f) || OnWallLeft || OnWallRight)
        //     {
        //         isJumping = true;
        //     }
        // }
        
        // Checks if player has hit a wall 
        if (Physics2D.Linecast(transform.position, wallCheckL.position, 1 << LayerMask.NameToLayer("Ground"))
            || Physics2D.Linecast(transform.position, wallCheckLTop.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            // Debug.Log("Left Wallcheck detected.");
            OnWallLeft = true;
            OnWallRight = false;
        }
        else if (Physics2D.Linecast(transform.position, wallCheckR.position, 1 << LayerMask.NameToLayer("Ground"))
                 || Physics2D.Linecast(transform.position, wallCheckRTop.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            // Debug.Log("Right Wallcheck detected.");
            OnWallRight = true;
            OnWallLeft = false;
        }
        // else
        // {
        //     OnWallRight = false;
        //     OnWallLeft = false;
        // }
        
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
        if (!isGrounded)
        {
            if ((OnWallLeft && (Input.GetKey("a") || Input.GetKey("left")))
                || (OnWallRight && (Input.GetKey("d") || Input.GetKey("right"))))
            {
                     isSliding = true;
                     Debug.Log("Sliding");
            }
        }
        else
        {
            isSliding = false;
            // rb.AddForce(transform.up * -3f);
            // Debug.Log(OnWallLeft + " " +  OnWallRight + " " + !isGrounded);
            // Debug.Log(isSliding);
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
        

        if (isSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            // rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed);
            // rb.velocity = new Vector2(rb.velocity.x, 0);
        }

        
        // if (isWallJumping)
        // {
        //     // if (OnWallLeft)
        //     // {
        //     //     rb.velocity = new Vector2(rb.velocity.x, wallJumpForce.y);
        //     // }
        //     // else if (OnWallRight)
        //     // {
        //     //     rb.velocity = new Vector2(-rb.velocity.x, wallJumpForce.y);
        //     // }
        //     if (OnWallLeft || OnWallRight)
        //     {
        //         rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        //         //rb.velocity = new Vector2(rb.velocity.x * wallJumpForce.x, wallJumpForce.y);
        //     }
        //     // OnWallRight = false;
        //     // OnWallLeft = false;
        // }
        // else
        // {
        //     rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
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

        if (((Input.GetKey("w") || Input.GetKey("space") || Input.GetKey("up")) && isGrounded)
        || ((Input.GetKey("w") || Input.GetKey("space") || Input.GetKey("up")) && (Input.GetKey("a") || Input.GetKey("left"))  && OnWallRight && isSliding)
        || ((Input.GetKey("w") || Input.GetKey("space") || Input.GetKey("up")) && (Input.GetKey("d") || Input.GetKey("right"))  && OnWallLeft && isSliding))
        {
            if ((jumpBufferCounter > 0f && coyoteTimeCounter > 0f) || isSliding)
            {
                Debug.Log("HIIIIII");
                isJumping = true;
                // jumpAudio.Play();
                Jump();
                
            }
        }
        
        // Tracks location of player's death for (!) animation
        playerDeathLoc = new Vector2(rb.position.x, rb.position.y + 0.8f);
    }

    void Jump()
    {
        Debug.Log("-------------");
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        if (isGrounded)
        {
            // rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            playerAnimator.SetTrigger("Jump");
            coyoteTimeCounter = 0f;
            jumpBufferCounter = 0f;
            jumpAudio.Play();
        }
        else if (isSliding)
        {
            isWallJumping = true;
            Invoke("StopWallJump", wallJumpDuration);
        }
        //rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        OnWallLeft = false;
        OnWallRight = false;
        isJumping = false;
        isSliding = false;
        //isWallJumping = false;
    }
    

    void StopWallJump()
    {
        isWallJumping = false;
        isSliding = false;
    }
}

// OLD



/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PMTest: MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float runSpeed = 2f;
    
    // Jumping
    [SerializeField] private float jumpSpeed = 3f;
    private bool isJumping;
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
    [SerializeField] private Vector2 wallJumpForce;
    [SerializeField] private float wallSlidingSpeed;
    [SerializeField] private float wallJumpDuration;
    
    // Coyote Time
    private float coyoteTime = 0.1f;
    private float coyoteTimeCounter;
    private float jumpBufferTime = 0.05f;
    private float jumpBufferCounter;

    // Sound effects
    public AudioSource jumpAudio;
    public AudioSource walkAudio;

    // Other
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
        
        // The jump itself
        // if (Input.GetKey("w") || Input.GetKey("space") || Input.GetKey("up"))
        // {
        //     if ((jumpBufferCounter > 0f && coyoteTimeCounter > 0f) || OnWallLeft || OnWallRight)
        //     {
        //         isJumping = true;
        //     }
        // }
        
        // Checks if player has hit a wall 
        if (Physics2D.Linecast(transform.position, wallCheckL.position, 1 << LayerMask.NameToLayer("Ground"))
            || Physics2D.Linecast(transform.position, wallCheckLTop.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            // Debug.Log("Left Wallcheck detected.");
            OnWallLeft = true;
            OnWallRight = false;
        }
        else if (Physics2D.Linecast(transform.position, wallCheckR.position, 1 << LayerMask.NameToLayer("Ground"))
                 || Physics2D.Linecast(transform.position, wallCheckRTop.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            // Debug.Log("Right Wallcheck detected.");
            OnWallRight = true;
            OnWallLeft = false;
        }
        // else
        // {
        //     OnWallRight = false;
        //     OnWallLeft = false;
        // }
        
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
        if (!isGrounded)
        {
            if ((OnWallLeft && (Input.GetKey("a") || Input.GetKey("left")))
                || (OnWallRight && (Input.GetKey("d") || Input.GetKey("right"))))
            {
                     isSliding = true;
                     Debug.Log("Sliding");
            }
        }
        else
        {
            isSliding = false;
            rb.velocity = new Vector2(rb.velocity.x, -9.81f);
            // rb.AddForce(transform.up * -3f);
            // Debug.Log(OnWallLeft + " " +  OnWallRight + " " + !isGrounded);
            // Debug.Log(isSliding);
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
        

        if (isSliding)
        {
            // rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed);
            // rb.velocity = new Vector2(rb.velocity.x, 0);
        }

        
        // if (isWallJumping)
        // {
        //     // if (OnWallLeft)
        //     // {
        //     //     rb.velocity = new Vector2(rb.velocity.x, wallJumpForce.y);
        //     // }
        //     // else if (OnWallRight)
        //     // {
        //     //     rb.velocity = new Vector2(-rb.velocity.x, wallJumpForce.y);
        //     // }
        //     if (OnWallLeft || OnWallRight)
        //     {
        //         rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        //         //rb.velocity = new Vector2(rb.velocity.x * wallJumpForce.x, wallJumpForce.y);
        //     }
        //     // OnWallRight = false;
        //     // OnWallLeft = false;
        // }
        // else
        // {
        //     rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
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

        if (((Input.GetKey("w") || Input.GetKey("space") || Input.GetKey("up")) && isGrounded)
        || ((Input.GetKey("w") || Input.GetKey("space") || Input.GetKey("up")) && (Input.GetKey("a") || Input.GetKey("left"))  && OnWallRight && isSliding)
        || ((Input.GetKey("w") || Input.GetKey("space") || Input.GetKey("up")) && (Input.GetKey("d") || Input.GetKey("right"))  && OnWallLeft && isSliding))
        {
            if ((jumpBufferCounter > 0f && coyoteTimeCounter > 0f) || isSliding)
            {
                Debug.Log("HIIIIII");
                isJumping = true;
                // jumpAudio.Play();
                Jump();
                
            }
        }
        
        // Tracks location of player's death for (!) animation
        playerDeathLoc = new Vector2(rb.position.x, rb.position.y + 0.8f);
    }

    void Jump()
    {
        Debug.Log("-------------");
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        if (isGrounded)
        {
            // rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            playerAnimator.SetTrigger("Jump");
            coyoteTimeCounter = 0f;
            jumpBufferCounter = 0f;
            jumpAudio.Play();
        }
        else if (isSliding)
        {
            isWallJumping = true;
            Invoke("StopWallJump", wallJumpDuration);
        }
        //rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        OnWallLeft = false;
        OnWallRight = false;
        isJumping = false;
        isSliding = false;
        //isWallJumping = false;
    }
    

    void StopWallJump()
    {
        isWallJumping = false;
        isSliding = false;
    }
}

*/