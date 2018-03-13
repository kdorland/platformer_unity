using System;
using System.Collections.Generic;
using UnityEngine;

public class Dude2D : MonoBehaviour
{
    [Header("Jump and Run")]
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpForce = 2f;
    [Header("Can Dude move in the air?")]
    [SerializeField] private bool airControl = false;
    [Header("Pick same Layer used on your Tilemap")]
    [SerializeField] private LayerMask whatIsGround;

    const float GROUND_CHECK_RADIUS = .2f; // Radius of the overlap circle to determine if grounded
    private Transform groundCheck;         // A position marking where to check if the player is grounded.
    private bool grounded;                 // Whether or not the player is grounded.
    private Animator animator;             // Reference to the player's animator component.
    private Rigidbody2D rb2D;
    private bool facingRight = true;       // For determining which way the player is currently facing.
    private bool jumping;
    private List<Transform> hitList = new List<Transform>();
    private double timeHit = -3.0f;
    private SpriteRenderer sprite;
    //private GameController gameController;
    
    private void Awake()
    {
        groundCheck = transform.Find("GroundCheck");
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        //gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    private void Update()
    {
        if (!jumping)
        {
            jumping = Input.GetButtonDown("Jump");
        }
    }

    private void FixedUpdate()
    {
        if (hitList.Count > 0)
        {
            timeHit = Time.time;
            Transform enemy = hitList[0];
            hitList.RemoveAt(0);
            if (enemy)
            {
                sprite.color = Utility.HexToColor("E76D6DFF");
                int dir = (enemy.position.x - transform.position.x > 0) ? 1 : -1;
                transform.Translate(-0.05f * dir, 0.05f, 0.0f, Space.World);
                rb2D.velocity = Vector2.zero;
                rb2D.AddForce(new Vector2(-4.0f * dir, 8.0f), ForceMode2D.Impulse);
            }

        } else if (timeHit + 0.2f < Time.time)
        {
            sprite.color = Utility.HexToColor("FFFFFFFF");
            float h = Input.GetAxisRaw("Horizontal");
            Move(h, jumping);
        }

        jumping = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground.
        grounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, GROUND_CHECK_RADIUS, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            grounded = true;
        }
        animator.SetBool("Ground", grounded);

        // Limit physics
        rb2D.velocity = Vector3.ClampMagnitude(rb2D.velocity, 15.0f);
    }

    public void Move(float move, bool jump)
    {
        // Only control the player if grounded or airControl is turned on
        if (grounded || airControl)
        {
            // The Speed animator parameter is set to the absolute value of the horizontal input.
            animator.SetFloat("Speed", Mathf.Abs(move));

            // Move the character
            rb2D.velocity = new Vector2(move * runSpeed, rb2D.velocity.y);

            // Flip the character if his face is not in the direction of movement
            if (move > 0 && !facingRight || (move < 0 && facingRight))
            {
                Flip();
            }
        }
        // If the player should jump...
        if (grounded && jump)
        {
            // Add a vertical force to the player.
            grounded = false;
            animator.SetBool("Ground", false);
            rb2D.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Flip the player transform
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void AddHit(Transform enemy)
    {

        hitList.Add(enemy);
    }
}

