using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] float _runSpeed;

    Rigidbody2D rb;
    Collider2D col;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //Ngambil komponen rigibody
    }

    // Update is called once per frame
    void Update()
    {
        if (isFacingLeft())
        {
            rb.velocity = new Vector2(-_runSpeed, 0f);
        }
        else
        {
            rb.velocity = new Vector2(_runSpeed, 0f); //Ngebuat player bisa jalan ke arah -x (negatif)
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _flip();
    }

    private void _flip() //Otomatis ngebuat enemy putar balik pas gak ada tanah didepannya
    {
        transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
    }

    private bool isFacingLeft()
    {
        return transform.localScale.x > 0f;
    }
}
