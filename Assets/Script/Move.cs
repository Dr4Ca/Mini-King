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

    [SerializeField] float maxSpeed;
    [SerializeField] float jumpSpeed;
    float climbSpeed;
    [SerializeField] Vector2 hitKick = new Vector2(50f, 50f);

    float _gravityScale;
    bool isHitted;

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
        if (!isHitted)
        {
            _run();
            _jump();

            if (boxCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
            {
                _playerHit();
            }
        }
    }

    private void _playerHit()
    {
        rb.velocity = hitKick * new Vector2(-transform.localScale.x, 1f);

        anim.SetTrigger("Hitted");
        isHitted = true;

        StartCoroutine(stopHit());
    }

    IEnumerator stopHit()
    {
        yield return new WaitForSeconds(1f);

        isHitted = false;
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

    // Isi kode buat lari
    private void _run()
    {
        float move = CrossPlatformInputManager.GetAxis("Horizontal");

        Vector2 playerVelovity = new Vector2(move * maxSpeed, rb.velocity.y);
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
