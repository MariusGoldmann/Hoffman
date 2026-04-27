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
    
    [SerializeField] bool isPaused = false;

    Animator animator; 
    ButtonScript buttonScript;


    void Start()
    {
        optionUI.SetActive(false);

        UIIsActive = false;  
        // No menu active

        animator = pauseMenuUI.GetComponent<Animator>();

        pauseAction = InputSystem.actions.FindAction("Pause");
        buttonScript = FindAnyObjectByType<ButtonScript>(); 
    }

    void Update()
    {
        if (UIIsActive)
        {
            Debug.Log("UI is active");
        }else
        {
            Debug.Log("UI is not active");
        }


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

                    animator.SetTrigger("IsPaused");
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
        animator.SetTrigger("IsPausedNot");
        optionUI.SetActive(false);

        UIIsActive = false;

        paused = false;
        option = false;
        Time.timeScale = 1;
    }
    
    public void Options()
    {
        optionUI.SetActive(true);
        option = true;
        animator.SetTrigger("IsPausedNot");
        Time.timeScale = 1; 
    }

    public bool GetIsPaused()
    {
        return isPaused;
    }

    public bool GetUIIsActive()
    {
        return UIIsActive;
    }

}
