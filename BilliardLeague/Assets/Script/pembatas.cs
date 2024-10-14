using UnityEngine;

public class Boundary : MonoBehaviour
{
    // Saat ada objek yang menabrak pembatas ini
    private void OnCollisionEnter(Collision collision)
    {
        // Pastikan objek yang menabrak memiliki Rigidbody
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Mencetak informasi tabrakan
            Debug.Log("Objek " + collision.gameObject.name + " menabrak pembatas!");

            // Menghentikan objek dengan menghentikan kecepatan dan posisi
            rb.velocity = Vector3.zero;

            // Memantulkan objek (opsional)
            Vector3 arahPantul = Vector3.Reflect(rb.velocity, collision.contacts[0].normal);
            rb.AddForce(arahPantul * 10f, ForceMode.Impulse); // Kekuatan pantulan disesuaikan

            // Pastikan objek tetap di luar pembatas
            Vector3 offset = collision.contacts[0].normal * 0.1f; // Menggeser sedikit
            collision.transform.position += offset;
        }
    }
}
