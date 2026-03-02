using TMPro;
using UnityEngine;

public class Workbench_Towers : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private TextMeshProUGUI upgradeText;
    [SerializeField] private TowerCategoryData_SO offensiveTowerData;
    private bool playerInside = false;

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.F))
        {
            UnlockTowers();
        }
    }

    public void UnlockTowers()
    {
        foreach (CardInfo card in offensiveTowerData.cards)
        {
            card.islocked = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            playerInside = true;

            upgradeText.enabled = true;

            //  pickaxeRendTable.material.mainTexture = pickaxeRend.material.mainTexture;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerInside = false;

        upgradeText.enabled = false;
    }
}
