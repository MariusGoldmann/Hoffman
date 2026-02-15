using UnityEngine;

public class playerStateMachine : MonoBehaviour
{
    public MovingStates movingState;

    PlayerMovement playerMovement;

    void Start()
    {
        movingState = MovingStates.Idle;

        playerMovement = GetComponent<PlayerMovement>();
    }
    
 
  

    public enum MovingStates
    {
        Idle,
        OneLegIdle,
        Walking,
        OneLegWalking,
        Running,
        Jumping,
        Falling,
        Crouching,
    }

}
