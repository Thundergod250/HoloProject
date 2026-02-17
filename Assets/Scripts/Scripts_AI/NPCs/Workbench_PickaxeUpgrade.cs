using TMPro;
using UnityEngine;

public class Workbench_PickaxeUpgrade : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Renderer pickaxeRend;
    [SerializeField] private PlayerInteraction playerInteract;

    [Header("Vars")]
    [SerializeField] private Texture2D copperPick;
    [SerializeField] private Texture2D ironPick;
    [SerializeField] private Texture2D goldPick;
    [SerializeField] private TextMeshProUGUI upgradeText;

    private void Start()
    {
        upgradeText.enabled = false;
    }

    private void Update()
    {
        if(playerInteract.interactable == this.gameObject.GetComponent<Interactable>())
        {
            upgradeText.enabled = true;

            if (Input.GetKey(KeyCode.F))
            {
                
            }
        }
        else if (playerInteract.interactable == null)
        {
            upgradeText.enabled = false;
        }
    }

    public void UpgradePick()
    {
        
    }
}
