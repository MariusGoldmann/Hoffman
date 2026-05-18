using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    [SerializeField] private PlayerCombat playerCombat;

    [SerializeField] private Image slashImage;
    [SerializeField] private Image kickImage;
    [SerializeField] private Image boomerangImage;


    private void Start()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
    }

    private void Update()
    {
        SlashImageUpdater();
        KickImageUpdater();
        BoomerangImageUpdater();
    }

    private void SlashImageUpdater()  
    {
        slashImage.fillAmount = playerCombat.GetSlashTimer() * 2;
    }

    private void KickImageUpdater()
    {
        kickImage.fillAmount = playerCombat.GetKickTimer() * 2;
    }

    private void BoomerangImageUpdater()
    {
        boomerangImage.fillAmount = playerCombat.GetBoomerangTimer();
    }
}
