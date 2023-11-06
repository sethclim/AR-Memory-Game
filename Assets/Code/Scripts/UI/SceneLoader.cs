using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] string sceneName;
    // Call this method to load the next scene
    public void LoadNextScene()
    {
        // You can either use the build index or the scene name
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // For loading the next scene in build order
        SceneManager.LoadScene(sceneName); // Replace with your scene name
    }
}
