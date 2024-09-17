using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float movementSpeed = 10f;       // Kecepatan maju tank
    public float turnSpeed = 100f;          // Kecepatan putar badan tank
    public float turretRotationSpeed = 5f;  // Kecepatan rotasi turret mengikuti input mouse
    public Transform turret;                // Referensi ke turret tank
    public Rigidbody tankRigidbody;         // Rigidbody dari tank untuk gerakan fisik

    public GameObject bulletPrefab;         // Prefab peluru yang akan ditembakkan
    public Transform firePoint;             // Titik tembak peluru pada ujung meriam
    public float bulletForce = 20f;         // Kekuatan peluru saat ditembakkan

    // Variabel UI
    public Joystick movementJoystick;       // Joystick untuk gerakan tank
    public Button fireButton;               // Tombol untuk menembak

    private float movementInputValue;       // Input maju/mundur
    private float turnInputValue;           // Input putar badan

    private void Start()
    {
        if (!tankRigidbody)
        {
            tankRigidbody = GetComponent<Rigidbody>(); // Pastikan Rigidbody terhubung
        }

        // Setup button listener untuk menembak
        if (fireButton != null)
        {
            fireButton.onClick.AddListener(Shoot);
        }
        else
        {
            Debug.LogError("Fire Button is not assigned!");
        }
    }

    private void Update()
    {
        // Ambil input dari joystick untuk gerakan tank
        movementInputValue = movementJoystick.Vertical;   // Input maju/mundur dari joystick
        turnInputValue = movementJoystick.Horizontal;     // Input putar dari joystick

        // Ambil input untuk rotasi turret dari mouse
        RotateTurret();
    }

    private void FixedUpdate()
    {
        // Gerakkan dan putar tank
        Move();
        Turn();
    }

    private void Move()
    {
        Vector3 movement = transform.forward * movementInputValue * movementSpeed * Time.deltaTime;
        tankRigidbody.MovePosition(tankRigidbody.position + movement);
    }

    private void Turn()
    {
        float turn = turnInputValue * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        tankRigidbody.MoveRotation(tankRigidbody.rotation * turnRotation);
    }

    private void RotateTurret()
    {
        // Ambil input mouse untuk rotasi turret
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Hitung rotasi turret berdasarkan input mouse
        float turretRotation = mouseX * turretRotationSpeed * Time.deltaTime;
        turret.Rotate(Vector3.up * turretRotation);

        // Mengatur rotasi vertikal turret jika diperlukan
        float verticalRotation = -mouseY * turretRotationSpeed * Time.deltaTime;
        turret.Rotate(Vector3.left * verticalRotation);
    }

    private void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
                Debug.Log("Bullet fired!");
            }
            else
            {
                Debug.LogError("Bullet prefab does not have a Rigidbody!");
            }
        }
        else
        {
            Debug.LogError("Bullet Prefab or Fire Point is not assigned!");
        }
    }
}
