using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject abilityTabUI;

    InputAction pauseAction;
    InputAction abilityTabAction; 

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
            pauseMenuUI.SetActive(true); 
            Time.timeScale = 0;
        }

        if (abilityTabAction.WasPerformedThisFrame())
        {
            abilityTabUI.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        abilityTabUI.SetActive(false);
        TimeStart();
    }

   public void TimeStart()
   {
        Time.timeScale = 1;
   }
    
}
