using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject optionUI;

    InputAction pauseAction;

    bool paused;
    bool option;

    bool UIIsActive; 

    Animator animatorPause;
    Animator animatorOptions; 
    ButtonScript buttonScript;


    void Start()
    {

        UIIsActive = false;  
        // No menu active

        animatorPause = pauseMenuUI.GetComponent<Animator>();
        animatorOptions = optionUI.GetComponent<Animator>();

        pauseAction = InputSystem.actions.FindAction("Pause");
        buttonScript = FindAnyObjectByType<ButtonScript>(); 
    }

    void Update()
    {
        

        if (pauseAction.WasPerformedThisFrame())
        {

            if (!paused && option)
            {
                ResumeGame();
            }
            else
            {
                if (!paused)
                {
                    paused = true;
                    UIIsActive = true;

                    animatorPause.SetTrigger("IsPaused");
                    Time.timeScale = 0;
                }
                else
                {
                    ResumeGame();
                }
            }
        }


    }

    public void ResumeGame()
    {
        animatorPause.SetTrigger("IsPausedNot");
        animatorOptions.SetTrigger("OptionsUp"); 

        UIIsActive = false;

        paused = false;
        option = false;
        Time.timeScale = 1;
    }
    
    public void Options()
    {
        animatorOptions.SetTrigger("OptionsDown");
        option = true;
        animatorPause.SetTrigger("IsPausedNot");
        Time.timeScale = 1; 
    }

  

    public bool GetIsPaused()
    {
        return UIIsActive;
    }

}
