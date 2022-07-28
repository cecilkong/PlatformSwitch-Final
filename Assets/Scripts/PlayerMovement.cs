using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerMovement : MonoBehaviour
{
    // General movement
    private Rigidbody2D rb;
    [SerializeField] private float runSpeed = 2f;
    [SerializeField] private float XDirectionalInput; //tut
    private bool facingRight; //tut
    private bool isMoving; //tut
    [SerializeField] float airMoveSpeed = 10f; // tut


    // Jumping
    [SerializeField] private float jumpSpeed = 3f;

    private bool canJump; //tut
    private bool isJumping;
    private bool isGrounded;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform groundCheckL;
    [SerializeField] private Transform groundCheckR;

    // Wall Sliding
    [SerializeField] private Transform wallCheckR;
    [SerializeField] private Transform wallCheckRTop;
    private bool OnWallRight; // change to just OnWall
    private bool isSliding;
    [SerializeField] private float wallSlidingSpeed;


    // Wall Jumping
    private bool isWallJumping; // might not need?
    [SerializeField] private Vector2 wallJumpForce;
    [SerializeField] private float wallJumpDuration; // might not need?
    [SerializeField] Vector2 walljumpAngle;
    [SerializeField] private float walljumpX;
    [SerializeField] private float walljumpY;
    [SerializeField] float walljumpDirection = -1;

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
        // walljumpAngle.Normalize(); // tut
        
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        facingRight = true;
        canJump = true;
}

    void Update()
    {
        // Gets direction that player is facing
        XDirectionalInput = Input.GetAxis("Horizontal");
        walljumpX = walljumpAngle.x;
        walljumpY = walljumpAngle.y;

        // JUMPING
        // Checks if player can jump
        if ((Input.GetKeyDown("w") || Input.GetKeyDown("up")) && (isGrounded || OnWallRight))
        {
            canJump = true;
        }
        else if (!isGrounded && !OnWallRight)
        {
            canJump = false;
        }

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
        if (Physics2D.Linecast(transform.position, wallCheckR.position, 1 << LayerMask.NameToLayer("Ground"))
            || Physics2D.Linecast(transform.position, wallCheckRTop.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            Debug.Log("Wallcheck detected.");
            OnWallRight = true; // change var name later
        }
        else
        {
            OnWallRight = false;
        }
        
        // Wall Sliding
        // if (!OnWallRight || isGrounded || !Input.GetKey("left") && !Input.GetKey("a") && !Input.GetKey("right") || !Input.GetKey("d"))
        // {
        //     isSliding = false;
        // }

        if (OnWallRight && !isGrounded && rb.velocity.y < 0)
        {
            if ((XDirectionalInput < 0 && (Input.GetKey("left") || Input.GetKey("a"))) ||
                (XDirectionalInput > 0 && (Input.GetKey("right") || Input.GetKey("d"))))
            {
                isSliding = true;
                Debug.Log("sliding");
            }
            else
            {
                isSliding = false;
                // Debug.Log("stopped sliding");
            } 
        }
        else
        {
            isSliding = false;
        }
        
        // Flips player direction
        if (XDirectionalInput < 0 && facingRight)
        {
            Debug.Log("flipped left");
            Flip();
        }
        else if (XDirectionalInput > 0 && !facingRight)
        {
            Debug.Log("flipped right");
            Flip();
        }
    }

    void FixedUpdate()
        {
            //for Animation
            if (XDirectionalInput != 0)
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }

            // Left/Right Movement
            if ((Input.GetKey("d") || Input.GetKey("right"))) // took out isGrounded
            {
                rb.velocity = new Vector2(runSpeed, rb.velocity.y);
                if (!walkAudio.isPlaying)
                {
                    walkAudio.Play();
                }
            }
            else if ((Input.GetKey("a") || Input.GetKey("left"))) // took out isGrounded
            {
                rb.velocity = new Vector2(-runSpeed, rb.velocity.y);
                if (!walkAudio.isPlaying)
                {
                    walkAudio.Play();
                }
            }
            // else if (!isGrounded && (!isSliding || !OnWallRight) && XDirectionalInput != 0)
            // {
            //     rb.AddForce(new Vector2(airMoveSpeed * XDirectionalInput, 0));
            //     if (Mathf.Abs(rb.velocity.x) > runSpeed)
            //     {
            //         rb.velocity = new Vector2(XDirectionalInput * runSpeed, rb.velocity.y);
            //     }
            // }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            // Jumping
            if (canJump && isGrounded)
            {
                if ((jumpBufferCounter > 0f && coyoteTimeCounter > 0f))
                {
                        // isJumping = true;
                        // jumpAudio.Play();
                        Jump();
                }
                // Jump();
            }

            if (isSliding)
            {
                // rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed);
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            }

            // Wall Jumping
            if (isSliding && canJump)
            {
                rb.AddForce(new Vector2(wallJumpForce.x * walljumpX * walljumpDirection, wallJumpForce.y * walljumpY), ForceMode2D.Impulse);
                Flip();
                canJump = false;

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
            if (isGrounded || OnWallRight)
            {
                coyoteTimeCounter = coyoteTime;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
            }

            if (Input.GetKey("w") || Input.GetKey("up"))
            {
                jumpBufferCounter = jumpBufferTime;
            }
            else
            {
                jumpBufferCounter -= Time.deltaTime;
            }

            // Tracks location of player's death for (!) animation
            playerDeathLoc = new Vector2(rb.position.x, rb.position.y + 0.8f);
        }

        void Jump()
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed); // tut
            if (isGrounded)
            {
                playerAnimator.SetTrigger("Jump");
                coyoteTimeCounter = 0f;
                jumpBufferCounter = 0f;
                jumpAudio.Play();
            }
            // else if (isSliding)
            // {
            //     isWallJumping = true;
            //     Invoke("StopWallJump", wallJumpDuration);
            // }

            // OnWallRight = false;
            // isJumping = false;
            // isSliding = false;

            //isWallJumping = false;
            canJump = false; // tut
        }


        // void StopWallJump()
        // {
        //     isWallJumping = false;
        //     isSliding = false;
        // }

        void Flip()
        {
            if (!isSliding)
            {
                walljumpDirection *= -1;
                facingRight = !facingRight;
                transform.Rotate(0, 180, 0);
            }

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