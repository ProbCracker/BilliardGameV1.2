using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaPutih : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 posisiAwal;

    public float batasBawah = -5f;
    public float kekuatanTembakan = 10f; // Kekuatan tembakan bola
    public LineRenderer lineRenderer;    // Menyimpan referensi ke Line Renderer

    public float kecepatanHenti = 0.05f; // Kecepatan di bawah ini akan membuat bola berhenti

    private bool bolaDitembak = false;   // Status untuk melacak apakah bola sudah ditembak
    private Vector3 arahTembakan;        // Arah tembakan bola

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        posisiAwal = transform.position;

        // Mengatur Line Renderer
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer.startWidth = 0.02f;  // Lebar lebih kecil
        lineRenderer.endWidth = 0.02f;    // Lebar lebih kecil
        lineRenderer.startColor = Color.white; // Warna garis putih
        lineRenderer.endColor = Color.white;
        lineRenderer.positionCount = 2;   // Hanya dua titik untuk garis lurus
        lineRenderer.enabled = false;     // Matikan garis awalnya

        rb.mass = 1f;
        rb.drag = 0.05f;
        rb.angularDrag = 0.1f;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void Update()
    {
        if (transform.position.y < batasBawah)
        {
            ResetBolaKePosisiAwal();
        }

        if (!bolaDitembak)
        {
            UpdateLineRenderer();
        }

        StopBolaJikaLambat();
    }

    void OnMouseDown()
    {
        lineRenderer.enabled = true;
    }

    void OnMouseDrag()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Plane plane = new Plane(Vector3.up, posisiAwal);
        float distance;

        if (plane.Raycast(ray, out distance))
        {
            Vector3 worldMousePosition = ray.GetPoint(distance);
            arahTembakan = worldMousePosition - posisiAwal;

            // Cek objek terdekat menggunakan raycasting
            RaycastHit hit;
            if (Physics.Raycast(posisiAwal, arahTembakan.normalized, out hit, 5f))
            {
                lineRenderer.SetPosition(1, hit.point); // Set garis berhenti di objek yang kena
            }
            else
            {
                lineRenderer.SetPosition(1, posisiAwal + arahTembakan.normalized * 5f); // Garis tetap panjang
            }
        }
    }

    void OnMouseUp()
    {
        if (!bolaDitembak)
        {
            TembakBola();
        }

        lineRenderer.enabled = false;
    }

    void TembakBola()
    {
        rb.AddForce(arahTembakan.normalized * kekuatanTembakan, ForceMode.Impulse);
        bolaDitembak = true;
    }

    void ResetBolaKePosisiAwal()
    {
        transform.position = posisiAwal;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        bolaDitembak = false;
        lineRenderer.enabled = false;
    }

    void UpdateLineRenderer()
    {
        lineRenderer.SetPosition(0, posisiAwal);
    }

    void StopBolaJikaLambat()
    {
        if (rb.velocity.magnitude < kecepatanHenti && bolaDitembak)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            bolaDitembak = false;
        }
    }
}
