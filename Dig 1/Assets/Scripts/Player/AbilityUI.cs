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
        slashImage.fillAmount = playerCombat.GetSlashTimer();
    }

    void KickImageUpdater()
    {
        kickImage.fillAmount = playerCombat.GetKickTimer();
    }

    void BoomerangImageUpdater()
    {
        boomerangImage.fillAmount = playerCombat.GetBoomerangTimer();
    }
}
