using UnityEngine;

public class playerStateMachine : MonoBehaviour
{
    public MovingStates movingState;

    PlayerMovement playerMovement;

    void Start()
    {
        movingState = MovingStates.Normal;

        playerMovement = GetComponent<PlayerMovement>();
    }
    
 
  

    public enum MovingStates
    {
        Normal,
        InAir,
        
    }

}
