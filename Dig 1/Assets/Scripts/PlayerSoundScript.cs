using UnityEngine;

public class PlayerSoundScript : MonoBehaviour
{
    public void StepSound()
    {
        PlayerSoundFXManager.instance.PlaySound(PlayerSoundFXManager.SoundType.WALK);
    }

    public void JumpSound()
    {
        PlayerSoundFXManager.instance.PlaySound(PlayerSoundFXManager.SoundType.JUMP);
    }
}
