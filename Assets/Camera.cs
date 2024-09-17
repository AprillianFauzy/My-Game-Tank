using UnityEngine;

public class TankCameraController : MonoBehaviour
{
    public Transform turret; // Referensi ke turret
    public Transform cameraTransform; // Referensi ke transform kamera
    public Vector3 cameraOffset; // Offset dari turret ke kamera
    public float sensitivityX = 10f; // Sensitivitas rotasi horizontal
    public float rotationSpeed = 5f; // Kecepatan rotasi kamera

    public RectTransform joystickArea; // Area joystick UI dalam canvas
    private float rotationX = 0f;

    void Update()
    {
        // Jika ada sentuhan pada layar
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Cek apakah sentuhan berada di luar area joystick
            if (!IsTouchWithinRect(touch.position, joystickArea))
            {
                // Hitung rotasi horizontal berdasarkan sentuhan
                rotationX += touch.deltaPosition.x * sensitivityX * Time.deltaTime;
            }
        }
        else
        {
            // Jika tidak ada sentuhan, gunakan input mouse (untuk testing atau di editor)
            float mouseX = Input.GetAxis("Mouse X");
            rotationX += mouseX * sensitivityX;
        }

        // Menerapkan rotasi horizontal pada turret
        turret.localRotation = Quaternion.Euler(0, rotationX, 0);

        // Mengatur posisi dan rotasi kamera
        cameraTransform.position = turret.position + cameraOffset;
        cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, turret.rotation, rotationSpeed * Time.deltaTime);
    }

    // Fungsi untuk memeriksa apakah sentuhan berada dalam area tertentu
    private bool IsTouchWithinRect(Vector2 touchPosition, RectTransform rectTransform)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, touchPosition, null, out localPoint);
        return rectTransform.rect.Contains(localPoint);
    }
}
