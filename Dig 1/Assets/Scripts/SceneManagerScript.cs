using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
   

    public void LoadNewGame()
    {
      SceneManager.LoadScene(1);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        
    }

    //  public void LoadNextScene()
    //{
    // int currentSene = SceneManager.GetActiveScene().buildIndex;
    //  SceneManager.LoadScene(currentScene + 1);
    //}
}
