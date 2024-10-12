using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] float _runSpeed; //Kecepatan si musuh

    Rigidbody2D rb;
    Collider2D col;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //Ngambil komponen rigibody
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
    }

    public void Dying()
    {
        anim.SetTrigger("Die");
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        rb.bodyType = RigidbodyType2D.Static;

        StartCoroutine(EnemyGone());
    }

    IEnumerator EnemyGone()
    {
        yield return new WaitForSeconds(5f);

        Destroy(gameObject);
    }

    private void EnemyMovement()
    {
        if (isFacingLeft())
        {
            rb.velocity = new Vector2(-_runSpeed, 0f);
        }
        else
        {
            rb.velocity = new Vector2(_runSpeed, 0f); // Ngebuat player bisa jalan ke arah -x (negatif)
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _flip();
    }

    private void _flip() // Otomatis ngebuat enemy putar balik pas gak ada tanah didepannya
    {
        transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
    }

    private bool isFacingLeft()
    {
        return transform.localScale.x > 0f;
    }
}
