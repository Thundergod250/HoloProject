using UnityEngine;
using System.Collections.Generic;

public class UI_Interaction : MonoBehaviour
{
    [SerializeField] private GameObject interactionTabPrefab;
    [SerializeField] private Transform interactionGroup;

    private List<UI_Interaction_Tab> activeTabs = new List<UI_Interaction_Tab>();

    public void SyncTabs(List<Interactable> interactables, int selectedIndex)
    {
        // Ensure we have the same number of tabs as interactables
        while (activeTabs.Count < interactables.Count)
        {
            GameObject tabGO = Instantiate(interactionTabPrefab, interactionGroup);
            activeTabs.Add(tabGO.GetComponent<UI_Interaction_Tab>());
        }

        while (activeTabs.Count > interactables.Count)
        {
            Destroy(activeTabs[activeTabs.Count - 1].gameObject);
            activeTabs.RemoveAt(activeTabs.Count - 1);
        }

        // Update labels and highlight
        for (int i = 0; i < activeTabs.Count; i++)
        {
            activeTabs[i].Show(interactables[i].interactName);
            activeTabs[i].SetHighlight(i == selectedIndex);
        }
    }

    public void ReorderTabs(int selectedIndex)
    {
        if (activeTabs.Count == 0) return;

        selectedIndex = Mathf.Clamp(selectedIndex, 0, activeTabs.Count - 1);

        for (int i = 0; i < activeTabs.Count; i++)
            activeTabs[i].SetHighlight(i == selectedIndex);

        // Move selected tab to top (Vertical Layout Group handles positioning)
        activeTabs[selectedIndex].transform.SetSiblingIndex(0);
    }

    public void ClearTabs()
    {
        foreach (var tab in activeTabs)
            Destroy(tab.gameObject);
        activeTabs.Clear();
    }
}