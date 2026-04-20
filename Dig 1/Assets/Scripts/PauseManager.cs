using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject optionUI;

    InputAction pauseAction;

    [SerializeField] ButtonScript buttonScript;
    bool paused;
    bool option;

    [SerializeField] bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false);
        optionUI.SetActive(false);

        pauseAction = InputSystem.actions.FindAction("Pause");
    }

    void Update()
    {
        if (pauseAction.WasPerformedThisFrame() )
        {

            if (!paused && option)
            {
                ResumeGame();
            } else
            {
                if (!paused)
                {
                    paused = true; 
                    pauseMenuUI.SetActive(true);
                    Time.timeScale = 0;
                }
                else
                {
                    ResumeGame();
                }
            }
        }

        if (pauseMenuUI.activeSelf == false && optionUI.activeSelf == false)
        {
            isPaused = false;
        }
        else
        {
            isPaused = true;
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        optionUI.SetActive(false);
        // om buttonScript inte hittas fråga Molly :D 

        paused = false;
        option = false;
        Time.timeScale = 1;
    }

   public void TimeStart()
   {
        Time.timeScale = 1;
   }

    
    public void Options()
    {
        optionUI.SetActive(true);
        option = true;  
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1; 
    }

    public bool GetIsPaused()
    {
        return isPaused;
    }
}
