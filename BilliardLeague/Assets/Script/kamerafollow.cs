using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Target yang diikuti (tongkat)
    public Vector3 offset; // Offset dari posisi target

    void Start()
    {
        // Set offset awal dari posisi dan rotasi
        offset = new Vector3(7.28f, 1.268f, 3.826f) - target.position; // Ubah sesuai posisi target
    }

    void LateUpdate()
    {
        // Memastikan kamera mengikuti target
        transform.position = target.position + offset;

        // Rotasi kamera agar mengikuti rotasi target
        transform.rotation = Quaternion.Euler(141.188f, 90f, 180f); // Ubah sesuai rotasi yang diinginkan
    }
}
