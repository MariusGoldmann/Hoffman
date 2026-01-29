using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;

    InputAction pauseAction;

    void Start()
    {
        pauseMenuUI.SetActive(false);

        pauseAction = InputSystem.actions.FindAction("Pause");
    }

    void Update()
    {
        if (pauseAction.WasPerformedThisFrame())
        {
            pauseMenuUI.SetActive(true); 
            Time.timeScale = 0;
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        TimeStart();
    }

   public void TimeStart()
   {
        Time.timeScale = 1;
   }
    
}
