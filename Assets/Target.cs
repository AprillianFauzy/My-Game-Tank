using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public Color hitColor = Color.red; // Warna saat terkena peluru
    private Color originalColor; // Warna awal

    private Renderer targetRenderer; // Renderer untuk mengubah warna

    private void Start()
    {
        targetRenderer = GetComponent<Renderer>();
        originalColor = targetRenderer.material.color; // Simpan warna awal
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            ChangeColor(hitColor);
        }
    }

    private void ChangeColor(Color newColor)
    {
        targetRenderer.material.color = newColor;
    }

    // Optional: Untuk mengembalikan warna setelah beberapa waktu
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            StartCoroutine(ResetColor());
        }
    }

    private IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(2f); // Tunggu 2 detik
        targetRenderer.material.color = originalColor;
    }
}
