using UnityEngine;
using System.Collections.Generic;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private GameObject interActionGroup;
    [SerializeField] private GameObject towerUpgrades;

    // Future-proof: keep all groups in a list
    private List<GameObject> uiGroups;

    private void Awake()
    {
        // Initialize the list with all groups
        uiGroups = new List<GameObject> { interActionGroup, towerUpgrades };
    }

    /// <summary>
    /// Activates only the given UI group, disables all others.
    /// </summary>
    public void FocusUI(GameObject targetGroup)
    {
        foreach (var group in uiGroups)
        {
            if (group != null)
                group.SetActive(group == targetGroup);
        }
    }

    // Convenience wrappers for specific groups
    public void FocusInterActionGroup()
    {
        FocusUI(interActionGroup);
    }

    public void FocusTowerUpgrades()
    {
        FocusUI(towerUpgrades);
    }

    /// <summary>
    /// Add new UI groups dynamically if needed.
    /// </summary>
    public void RegisterUIGroup(GameObject newGroup)
    {
        if (!uiGroups.Contains(newGroup))
            uiGroups.Add(newGroup);
    }
}