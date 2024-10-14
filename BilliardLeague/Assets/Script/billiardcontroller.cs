using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBallShoot : MonoBehaviour
{
    public Rigidbody bolaPutihRigidbody;  // Referensi ke Rigidbody bola putih
    public float kekuatanTembakan = 5f;   // Kekuatan dorongan untuk menembak bola
    private LineRenderer lineRenderer;      // Line Renderer untuk menggambar garis panduan
    public Transform bolaPutih;             // Referensi ke bola putih untuk posisi
    public Transform stick;                 // Referensi ke stick
    public Camera camera;                   // Referensi ke kamera
    public float rotasiKecepatan = 100f;   // Kecepatan rotasi garis
    public float jarakStick = 1f;          // Jarak stick dari bola putih

    private bool bolaDitembak = false;      // Status apakah bola sudah ditembak atau belum
    private Vector3 posisiAwalStick;        // Menyimpan posisi awal stick

    void Start()
    {
        if (bolaPutihRigidbody == null)
        {
            // Cek apakah Rigidbody telah di-referensikan
            Debug.LogError("Rigidbody dari bola putih belum diatur!");
        }

        // Simpan posisi awal stick
        posisiAwalStick = stick.position;

        // Menambahkan komponen Line Renderer secara programatik
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // Setel properti awal LineRenderer
        lineRenderer.startWidth = 0.05f;   // Lebar awal garis
        lineRenderer.endWidth = 0.05f;     // Lebar akhir garis
        lineRenderer.startColor = Color.white; // Warna garis
        lineRenderer.endColor = Color.white;
        lineRenderer.positionCount = 2;    // Hanya dua titik untuk garis lurus
        lineRenderer.enabled = false;       // Matikan awalnya
    }

    void Update()
    {
        // Jika bola belum ditembak, tampilkan garis panduan
        if (!bolaDitembak)
        {
            UpdateLineRenderer();
            RotateStick();
            UpdateCameraRotation(); // Panggil fungsi untuk memperbarui rotasi kamera
        }
    }

    void OnMouseDown()
    {
        // Nyalakan garis saat mouse ditekan pada stick
        lineRenderer.enabled = true;
    }

    void OnMouseDrag()
    {
        // Mengatur arah garis berdasarkan posisi mouse di layar
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Plane plane = new Plane(Vector3.up, bolaPutih.position); // Bidang horizontal untuk memproyeksikan rotasi
        float distance;

        if (plane.Raycast(ray, out distance))
        {
            Vector3 worldMousePosition = ray.GetPoint(distance);
            Vector3 direction = worldMousePosition - bolaPutih.position;

            // Update arah garis berdasarkan mouse
            lineRenderer.SetPosition(1, bolaPutih.position + direction.normalized * 5f); // Ubah 5f sesuai panjang garis yang diinginkan
        }
    }

    void OnMouseUp()
    {
        // Fungsi ini akan dipanggil saat mouse dilepas
        if (!bolaDitembak)
        {
            TembakBola();
        }

        // Matikan garis setelah bola ditembak
        lineRenderer.enabled = false;
    }

    void TembakBola()
    {
        // Arahkan tongkat (Stick) ke arah bola putih (WhiteBall) agar bisa menembak
        Vector3 arahTembakan = (lineRenderer.GetPosition(1) - bolaPutih.position).normalized; // Menggunakan posisi akhir garis

        // Terapkan dorongan ke bola putih
        bolaPutihRigidbody.AddForce(arahTembakan * kekuatanTembakan, ForceMode.Impulse);

        // Tandai bahwa bola sudah ditembak
        bolaDitembak = true;

        // Menyembunyikan stick setelah bola ditembak
        stick.gameObject.SetActive(false);
    }

    void UpdateLineRenderer()
    {
        // Atur posisi awal garis ke posisi bola putih
        lineRenderer.SetPosition(0, bolaPutih.position);

        // Dapatkan posisi akhir garis
        Vector3 endLinePosition = lineRenderer.GetPosition(1);

        // Update rotasi stick untuk mengikuti arah dari garis
        Vector3 direction = endLinePosition - bolaPutih.position;
        if (direction != Vector3.zero) // Pastikan tidak membagi nol
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            stick.rotation = rotation; // Atur rotasi stick agar menghadap ke arah garis
        }
    }

    void RotateStick()
    {
        // Hitung arah ke bola putih dari posisi awal stick
        Vector3 directionToBola = bolaPutih.position - posisiAwalStick;

        // Rotasi stick agar menghadap bola putih
        if (directionToBola != Vector3.zero) // Pastikan tidak membagi nol
        {
            Quaternion rotation = Quaternion.LookRotation(directionToBola);
            stick.rotation = rotation; // Atur rotasi stick agar menghadap ke arah bola
        }
    }

    void UpdateCameraRotation()
    {
        // Dapatkan posisi akhir dari garis
        Vector3 endLinePosition = lineRenderer.GetPosition(1);

        // Hitung arah dari kamera ke posisi akhir garis
        Vector3 direction = endLinePosition - camera.transform.position;

        // Buat rotasi baru untuk kamera agar menghadap ke arah posisi akhir garis
        if (direction != Vector3.zero) // Pastikan tidak membagi nol
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, targetRotation, Time.deltaTime * rotasiKecepatan);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Memeriksa apakah bola berhenti setelah menembak
        if (collision.gameObject == bolaPutih.gameObject)
        {
            // Cek apakah bola berhenti
            if (bolaPutihRigidbody.velocity.magnitude < 0.1f) // Ubah threshold jika diperlukan
            {
                // Tampilkan kembali stick setelah bola berhenti
                stick.gameObject.SetActive(true);
                bolaDitembak = false; // Reset status bola ditembak
            }
        }
    }
}
