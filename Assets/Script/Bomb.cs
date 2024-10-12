using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bomb : MonoBehaviour
{
    [SerializeField] float radius = 3f;
    [SerializeField] float explosionForce = 100f;

    BoxCollider2D col;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void ExplodeBomb()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, radius, LayerMask.GetMask("Player"));

        if (playerCollider)
        {
            Rigidbody2D playerRigidBody = playerCollider.GetComponent<Rigidbody2D>();

            if (playerRigidBody != null)
            {
                // Hitung arah vektor dari bom ke pemain
                Vector2 direction = (playerRigidBody.position - (Vector2)transform.position).normalized;
                // Hitung jarak dari bom ke pemain
                float distance = Vector2.Distance(playerRigidBody.position, transform.position);
                // Hitung kekuatan gaya berdasarkan jarak (semakin jauh semakin lemah)
                float force = explosionForce / Mathf.Max(distance, 1f); // Mastiin gak dibagi nol

                // Impact ke pemain
                playerRigidBody.AddForce(direction *  force, ForceMode2D.Impulse);
            }

            playerCollider.GetComponent<Move>()._playerHit();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        anim.SetTrigger("Burning");
    }

    private void DestroyBomb()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
