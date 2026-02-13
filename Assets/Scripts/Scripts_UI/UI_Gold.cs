using TMPro;
using UnityEngine;

public class UI_Gold : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private GoldManager goldManager;
    [SerializeField] private DropResourceManager resourceManager;

    [SerializeField] private TextMeshProUGUI copperResourceText;
    [SerializeField] private TextMeshProUGUI ironResourceText;
    [SerializeField] private TextMeshProUGUI mythrilResourceText;
    [SerializeField] private TextMeshProUGUI goldResourceText;


    private void Update()
    {
        if (goldManager != null)
        {
            goldText.text = goldManager.PlayerGold.ToString();
            copperResourceText.text = ": " + resourceManager.GetResourceType(upgradeResourceType.Copper).ToString();
            ironResourceText.text = ": " + resourceManager.GetResourceType(upgradeResourceType.Iron).ToString();
            mythrilResourceText.text = ": " + resourceManager.GetResourceType(upgradeResourceType.Mithril).ToString();
            goldResourceText.text = ": " + resourceManager.GetResourceType(upgradeResourceType.Gold).ToString();
        }
    }
}