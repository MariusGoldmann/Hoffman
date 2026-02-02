using UnityEngine;

public class Dennis : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public enum States
    {
        Idle,
        Walking,
        Dead,
        Cutscene
    }

    public States state;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case States.Idle:

                Debug.Log("Idle state");
                UpdateAnimations();
                break;

            case States.Walking:
                Debug.Log("Walking");
                UpdateAnimations();
                break;

            case States.Dead:

                break;

            case States.Cutscene:
                break;

        }
    }

    void UpdateAnimations()
    {

    }

    void MovePlayer()
    {

    }
}
