using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera mainCam;       // Kamera utama (MainCam)
    public Camera topCam;        // Kamera atas (TopCam)
    public GameObject whiteBall; // Bola putih (WhiteBall)
    public float ballStopThreshold = 0.1f; // Threshold untuk menganggap bola berhenti
    private Rigidbody ballRb;    // Komponen Rigidbody dari bola putih
    private Vector3 initialPosition; // Posisi awal bola putih
    private bool ballIsMoving = false; // Status pergerakan bola

    void Start()
    {
        // Setel kamera utama di awal
        mainCam.enabled = true;
        topCam.enabled = false;

        // Ambil Rigidbody dari bola putih
        ballRb = whiteBall.GetComponent<Rigidbody>();

        // Simpan posisi awal bola putih
        initialPosition = whiteBall.transform.position;
    }

    void Update()
    {
        // Jika bola mulai bergerak, beralih ke topCam
        if (!ballIsMoving && ballRb.velocity.magnitude > ballStopThreshold)
        {
            ballIsMoving = true;
            SwitchToTopCam();
        }

        // Jika bola berhenti atau kembali ke posisi awal, beralih ke mainCam
        if (ballIsMoving && (ballRb.velocity.magnitude < ballStopThreshold || whiteBall.transform.position == initialPosition))
        {
            ballIsMoving = false;
            SwitchToMainCam();
        }
    }

    void SwitchToTopCam()
    {
        mainCam.enabled = false;
        topCam.enabled = true;
    }

    void SwitchToMainCam()
    {
        mainCam.enabled = true;
        topCam.enabled = false;
    }
}
