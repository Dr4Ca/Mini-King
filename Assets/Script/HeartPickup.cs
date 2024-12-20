using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [SerializeField] AudioClip heartSFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioSource.PlayClipAtPoint(heartSFX, Camera.main.transform.position);
        FindObjectOfType<GameSession>().AddToLives();
        Destroy(gameObject);
    }
}
