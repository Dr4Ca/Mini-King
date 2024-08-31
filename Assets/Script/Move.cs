using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Move : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    BoxCollider2D boxCollider;
    PolygonCollider2D playerFeet;

    [SerializeField] float movementSpeed; 
    [SerializeField] float jumpSpeed;
    [SerializeField] float climbSpeed;

    float _gravityScale;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerFeet = GetComponent<PolygonCollider2D>();

        _gravityScale = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        _run();
        _jump();
        _climb();
    }

    // Isi kode untuk lompat
    private void _jump()
    {
        if (!playerFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        bool isJumping = CrossPlatformInputManager.GetButtonDown("Jump");

        if (isJumping)
        {
            Vector2 jumpVelocity = new Vector2(rb.velocity.x, jumpSpeed);
            rb.velocity = jumpVelocity;
        }
        
    }

    // Isi kode untuk manjat (gak digunain, soalnya mengganggu gameplay)
    private void _climb()
    {
        if (boxCollider.IsTouchingLayers(LayerMask.GetMask("Climb")))
        {
            float climb = CrossPlatformInputManager.GetAxis("Jump");

            Vector2 climbingVelocity = new Vector2(rb.velocity.x, climb * climbSpeed);
            rb.velocity = climbingVelocity;

            anim.SetBool("Climbing", true);

            rb.gravityScale = 0f;
        }
        else
        {
            anim.SetBool("Climbing", false);
            rb.gravityScale = _gravityScale;
        }
    }

    // Isi kode buat lari
    private void _run()
    {
        float move = CrossPlatformInputManager.GetAxis("Horizontal");

        Vector2 playerVelovity = new Vector2(move * movementSpeed, rb.velocity.y);
        rb.velocity = playerVelovity;
        
        _flip();
        _runAnim();
    }

    // Kode buat nge flip player, kalo dia nengok kanan-kiri
    private void _flip()
    {
        bool runningHorizontal = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

        if (runningHorizontal)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }
    }

    // Ngeplay animasi pas player jalan
    private void _runAnim()
    {
        bool runningHorizontal = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        anim.SetBool("Running", runningHorizontal);
    }
}
