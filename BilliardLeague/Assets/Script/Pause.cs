using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    // Fungsi ini akan dipanggil ketika tombol "Play" diklik
    public void PlayGame()
    {
        // Pastikan untuk mengganti "GameScene" dengan nama scene yang ingin dimuat
        SceneManager.LoadScene("Settings");
    }

    // Fungsi ini akan dipanggil ketika tombol "Quit" diklik
    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit(); // Ini hanya akan bekerja setelah di-build, bukan di editor
    }
}
