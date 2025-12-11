using UnityEngine;
using TMPro;

public class UI_Interaction : MonoBehaviour
{
    [SerializeField] private GameObject interactionTabPrefab;
    [SerializeField] private Transform interactionGroup;

    private UI_Interaction_Tab currentTab;

    public void Show(string name)
    {
        if (currentTab != null)
            Destroy(currentTab.gameObject);

        GameObject tabGO = Instantiate(interactionTabPrefab, interactionGroup);
        currentTab = tabGO.GetComponent<UI_Interaction_Tab>();
        currentTab.Show(name);
    }

    public void Hide()
    {
        if (currentTab != null)
        {
            Destroy(currentTab.gameObject);
            currentTab = null;
        }
    }
}