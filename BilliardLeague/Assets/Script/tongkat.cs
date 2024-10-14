using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongkatBiliar : MonoBehaviour
{
    public Transform bolaPutih; // Referensi ke bola putih
    public float jarakDariBola = 0.5f; // Jarak tongkat dari bola
    public float kecepatanRotasi = 100f; // Kecepatan rotasi tongkat
    public float kekuatanTembakan = 10f; // Kekuatan dorongan untuk menembak bola

    private bool isAiming = false; // Apakah sedang mengarahkan tembakan
    private float angle = 0f; // Sudut rotasi tongkat
    private Rigidbody bolaRigidbody; // Untuk mengakses komponen Rigidbody dari bola putih

    void Start()
    {
        // Mendapatkan komponen Rigidbody dari bola putih
        bolaRigidbody = bolaPutih.GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Jika sedang mengarahkan, tongkat mengikuti mouse
        if (isAiming)
        {
            UpdateRotasiTongkat();
        }
    }

    void OnMouseDown()
    {
        // Mulai mengarahkan tembakan
        isAiming = true; // Aktifkan mengarahkan

        // Setelah diklik, langsung tembak bola putih
        TembakBola();
    }

    void OnMouseUp()
    {
        // Berhenti mengarahkan setelah mouse dilepas
        isAiming = false;
    }

    void UpdateRotasiTongkat()
    {
        // Mendapatkan posisi mouse di layar dan mengubahnya menjadi posisi dunia
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        Plane plane = new Plane(Vector3.up, bolaPutih.position); // Membuat bidang horizontal untuk memproyeksikan rotasi
        float distance;

        // Cek apakah ray dari kamera memotong bidang
        if (plane.Raycast(ray, out distance))
        {
            // Dapatkan posisi di mana ray dari mouse bertemu dengan bidang horizontal
            Vector3 worldMousePosition = ray.GetPoint(distance);

            // Hitung arah dari bola putih ke posisi mouse
            Vector3 direction = worldMousePosition - bolaPutih.position;

            // Hitung sudut rotasi menggunakan arctan2
            angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

            // Perbarui posisi dan rotasi tongkat
            Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)) * jarakDariBola;
            transform.position = bolaPutih.position + offset; // Atur posisi tongkat
            transform.rotation = Quaternion.Euler(0, -angle, 0); // Atur rotasi tongkat
        }
    }

    void TembakBola()
    {
        // Hitung arah tembakan berdasarkan sudut tongkat
        Vector3 arahTembakan = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad));

        // Terapkan dorongan (impulse) ke bola putih
        bolaRigidbody.AddForce(arahTembakan * kekuatanTembakan, ForceMode.Impulse);

        // Setelah menembak, nonaktifkan pengaturan arah tongkat
        isAiming = false;
    }
}
