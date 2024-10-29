using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondPickup : MonoBehaviour
{
    [SerializeField] int diamondValue = 100;
    [SerializeField] AudioClip diamondSFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioSource.PlayClipAtPoint(diamondSFX, Camera.main.transform.position);
        FindObjectOfType<GameSession>().AddToScore(diamondValue);
        Destroy(gameObject);
    }
}
