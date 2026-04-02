using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    [SerializeField] PlayerCombat playerCombat;

    [SerializeField] Image slashImage;
    [SerializeField] Image kickImage;
    [SerializeField] Image boomerangImage;


    void Start()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
    }

    void Update()
    {
        SlashImageUpdater();
        KickImageUpdater();
        BoomerangImageUpdater();
    }

    void SlashImageUpdater()  
    {
        slashImage.fillAmount = playerCombat.GetSlashTimer() * 2;
    }

    void KickImageUpdater()
    {
        kickImage.fillAmount = playerCombat.GetKickTimer() * 2;
    }

    void BoomerangImageUpdater()
    {
        boomerangImage.fillAmount = playerCombat.GetBoomerangTimer();
    }
}
