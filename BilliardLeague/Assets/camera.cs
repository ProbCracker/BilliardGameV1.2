using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public GameObject povCam; // Kamera sudut pandang pemain (POV Cam)
    public GameObject topCam; // Kamera pandangan atas (Top Cam)
    public GameObject bolaPutih; // Bola putih

    private Rigidbody rb;
    private Vector3 posisiAwal;
    private bool bolaDitembak = false;

    public float kecepatanHenti = 0.05f; // Kecepatan di bawah ini akan dianggap sebagai berhenti

    void Start()
    {
        rb = bolaPutih.GetComponent<Rigidbody>();
        posisiAwal = bolaPutih.transform.position;

        // Set kamera awal ke POV Cam
        ActivatePOVCam();
    }

    void Update()
    {
        // Cek jika bola di bawah kecepatan tertentu (artinya bola sudah berhenti)
        if (rb.velocity.magnitude < kecepatanHenti && bolaDitembak)
        {
            ActivatePOVCam();
            bolaDitembak = false;
        }

        // Cek jika bola sudah mulai bergerak
        if (rb.velocity.magnitude >= kecepatanHenti && bolaDitembak)
        {
            ActivateTopCam();
        }

        // Cek jika bola kembali ke posisi awal (misalnya jatuh ke lubang)
        if (bolaPutih.transform.position.y < -5f)
        {
            ResetBolaKePosisiAwal();
            ActivatePOVCam();
        }
    }

    public void BolaDitembak() // Panggil fungsi ini ketika bola ditembak
    {
        bolaDitembak = true;
        ActivateTopCam();
    }

    public void ActivatePOVCam()
    {
        povCam.SetActive(true);
        topCam.SetActive(false);
    }

    public void ActivateTopCam()
    {
        povCam.SetActive(false);
        topCam.SetActive(true);
    }

    void ResetBolaKePosisiAwal()
    {
        bolaPutih.transform.position = posisiAwal;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        bolaDitembak = false;
    }
}
