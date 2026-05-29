using UnityEngine;

public class EarPickUp : MonoBehaviour
{
    BossSpawnStart bossSpawnStartScript;

    Animator animator; 

    void Start()
    {
        bossSpawnStartScript = FindAnyObjectByType<BossSpawnStart>();

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(bossSpawnStartScript.win == true)
        {
            animator.SetTrigger("EarDown");
        }
    }
}
