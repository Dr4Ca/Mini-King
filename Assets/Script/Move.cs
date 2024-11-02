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
    AudioSource audioSource;

    // Gerak Motorik
    [SerializeField] float maxSpeed;
    [SerializeField] float jumpSpeed;
    float climbSpeed;

    // Impact
    [SerializeField] Vector2 hitKick = new Vector2(50f, 50f);

    // Serang
    [SerializeField] Transform hurtBox;
    [SerializeField] float attackRadius = 3f;

    //SFX
    [SerializeField] AudioClip jumpingSFX, attackSFX, walkSFX, hittedSFX;

    float _gravityScale;
    bool isHitted;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerFeet = GetComponent<PolygonCollider2D>();
        audioSource = GetComponent<AudioSource>();

        _gravityScale = rb.gravityScale;

        anim.SetTrigger("Door Out");

    }

    // Update is called once per frame
    void Update()
    {
        if (!isHitted)
        {
            _run();
            _jump();
            _attack();

            if (boxCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
            {
                _playerHit();
            }

            ExitLevel();
        }
    }

    private void ExitLevel()
    {
        if (!boxCollider.IsTouchingLayers(LayerMask.GetMask("Interactable"))) { return; }

        if (CrossPlatformInputManager.GetButtonDown("Vertical"))
        {
            anim.SetTrigger("Door In");
        }

    }

    public void LoadNextLevel()
    {
        FindObjectOfType<ExitDoor>().StartLoadingNextLevel();
        TurnOffRendered();
    }

    public void TurnOffRendered()
    {
        GetComponent<Renderer>().enabled = false;
    }

    private void _attack()
    {
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Attacking");
            audioSource.PlayOneShot(attackSFX);

            Collider2D[] enemiesToHit = Physics2D.OverlapCircleAll(hurtBox.position, attackRadius, LayerMask.GetMask("Enemy"));

            foreach(Collider2D enemy in enemiesToHit)
            {
                enemy.GetComponent<Enemy>().Dying();
            }
        }
    }

    public void _playerHit()
    {
        rb.velocity = hitKick * new Vector2(-transform.localScale.x, 1f);

        anim.SetTrigger("Hitted");
        isHitted = true;
        audioSource.PlayOneShot(hittedSFX);

        FindObjectOfType<GameSession>().ProcessPlayerDeath();

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

            audioSource.PlayOneShot(jumpingSFX);
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
        PlayWalkSFX();

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(hurtBox.position, attackRadius);
    }

    void PlayWalkSFX()
    {
        bool isMoving = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

        // Memainkan atau menghentikan suara berdasarkan pergerakan
        if (isMoving && !audioSource.isPlaying)
        {
            audioSource.clip = walkSFX;
            audioSource.Play();
        }
        else if (!isMoving && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

}
