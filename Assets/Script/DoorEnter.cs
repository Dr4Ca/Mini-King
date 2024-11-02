using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEnter : MonoBehaviour
{
    public AudioClip openingDoorSFX;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().SetTrigger("Open");

        StartCoroutine(CloseDelay(2f));
    }

    private IEnumerator CloseDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GetComponent<Animator>().SetTrigger("Close");
    }

    void PlayOpeningDoorSFX()
    {
        AudioSource.PlayClipAtPoint(openingDoorSFX, Camera.main.transform.position);
    }
}
