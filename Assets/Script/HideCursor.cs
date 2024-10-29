using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCursor : MonoBehaviour
{
    void Start()
    {
        // Sembunyikan kursor dan kunci posisinya di tengah layar
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnApplicationFocus(bool hasFocus)
    {
        // Saat aplikasi kehilangan fokus (misalnya saat Alt-Tab), kursor akan muncul.
        // Ini memastikan kursor tetap tersembunyi saat kembali ke game.
        if (hasFocus)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
