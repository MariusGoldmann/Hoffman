using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ReloadSceneTemporary : MonoBehaviour
{
    [SerializeField] bool reload;
    private void FixedUpdate()
    {
        if (Keyboard.current.hKey.isPressed || reload)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
