using UnityEngine;
using System.Collections.Generic;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private GameObject mainUiGroup;
    [SerializeField] private GameObject towerUpgrades;

    private List<GameObject> uiGroups;

    private void Awake()
    {
        // Initialize the list with the two groups
        uiGroups = new List<GameObject> { mainUiGroup, towerUpgrades };
    }
    
    public void FocusUI(GameObject targetGroup)
    {
        foreach (var group in uiGroups)
        {
            if (group != null)
                group.SetActive(group == targetGroup);
        }
    }

    // Convenience wrappers
    public void FocusMainUIGroup()
    {
        FocusUI(mainUiGroup);
    }

    public void FocusTowerUpgrades()
    {
        FocusUI(towerUpgrades);
    }
    
    public void RegisterUIGroup(GameObject newGroup)
    {
        if (newGroup != null && !uiGroups.Contains(newGroup))
            uiGroups.Add(newGroup);
    }
}