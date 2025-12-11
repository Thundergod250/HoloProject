using UnityEngine;
using System.Collections.Generic;

public class UI_Interaction : MonoBehaviour
{
    [SerializeField] private GameObject interactionTabPrefab;
    [SerializeField] private Transform interactionGroup;

    private List<UI_Interaction_Tab> activeTabs = new List<UI_Interaction_Tab>();

    public void UpdateTabs(List<Interactable> interactables, int selectedIndex)
    {
        // Clear old tabs
        foreach (Transform child in interactionGroup)
            Destroy(child.gameObject);
        activeTabs.Clear();

        // Spawn new tabs
        for (int i = 0; i < interactables.Count; i++)
        {
            GameObject tabGO = Instantiate(interactionTabPrefab, interactionGroup);
            UI_Interaction_Tab tab = tabGO.GetComponent<UI_Interaction_Tab>();
            tab.Show(interactables[i].interactName);
            activeTabs.Add(tab);
        }

        ReorderTabs(selectedIndex); // Initial reorder
    }

    public void ReorderTabs(int selectedIndex)
    {
        if (activeTabs.Count == 0)
            return;

        selectedIndex = Mathf.Clamp(selectedIndex, 0, activeTabs.Count - 1);

        for (int i = 0; i < activeTabs.Count; i++)
        {
            activeTabs[i].SetHighlight(i == selectedIndex);
        }

        // Move selected tab to top of hierarchy (or middle visually)
        activeTabs[selectedIndex].transform.SetSiblingIndex(0);
    }
}