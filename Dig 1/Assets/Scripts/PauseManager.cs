using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject abilityTabUI;

    InputAction pauseAction;
    InputAction abilityTabAction;

    bool tab;
    bool paused;

    void Start()
    {
        pauseMenuUI.SetActive(false);
        abilityTabUI.SetActive(false);

        pauseAction = InputSystem.actions.FindAction("Pause");
        abilityTabAction = InputSystem.actions.FindAction("AbilityTab");
    }

    void Update()
    {
        if (pauseAction.WasPerformedThisFrame())
        {
            paused = !paused;
            if (!paused)
            {
                pauseMenuUI.SetActive(true);
                Time.timeScale = 0;
            } else
            {
                pauseMenuUI.SetActive(false);
                Time.timeScale = 1;
            }
                
        }

        if (abilityTabAction.WasPerformedThisFrame())
        {
            tab = !tab;
            if (tab)
            {
                abilityTabUI.SetActive(true);
                Time.timeScale = 0;
            }   else
            {
                abilityTabUI.SetActive(false);
                Time.timeScale = 1;
            }
               
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        abilityTabUI.SetActive(false);
        Time.timeScale = 1;
    }

   public void TimeStart()
   {
        Time.timeScale = 1;
   }
    
}
