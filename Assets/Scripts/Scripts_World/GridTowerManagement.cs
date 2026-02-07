using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GridTowerManagement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private List<GameObject> _gridPlacements;
    [SerializeField] private int _activeGridCount = 0;

    [Header("Disable Grids Start of Game")]
    [SerializeField] private List<GameObject> _disableOnStart;

    [Header("Events")]
    public UnityEvent<GameObject> OnGridDisabled;
    public UnityEvent<GameObject> OnGridEnabled;

    // Static Dispatchers (The Radio Station)
    public static System.Action<GameObject> OnRequestEnable;
    public static System.Action<GameObject> OnRequestDisable;


    private void Start()
    {
        // This runs once when the game begins
        InitializeMap();
    }

    private void InitializeMap()
    {
        foreach (GameObject grid in _disableOnStart)
        {
            if (grid != null)
            {
                // We call the internal logic directly
                DisableSpecificGrid(grid);
            }
        }
    }

    private void OnEnable()
    {
        OnRequestEnable += EnableSpecificGrid;
        OnRequestDisable += DisableSpecificGrid;
    }

    private void OnDisable()
    {
        OnRequestEnable -= EnableSpecificGrid;
        OnRequestDisable -= DisableSpecificGrid;
    }

    public void EnableSpecificGrid(GameObject targetGridPlacement)
    {
        if (_gridPlacements.Contains(targetGridPlacement) && !targetGridPlacement.activeSelf)
        {
            Debug.Log($"Enabling grid: {targetGridPlacement.name}");
            targetGridPlacement.SetActive(true);
            _activeGridCount++;
            OnGridEnabled?.Invoke(targetGridPlacement);
        }
        else
        {
            Debug.LogWarning($"{targetGridPlacement.name} was not disabled. Is it in the list? {_gridPlacements.Contains(targetGridPlacement)}");
        }
    }

    public void DisableSpecificGrid(GameObject targetGridPlacement)
    {
        if (_gridPlacements.Contains(targetGridPlacement) && targetGridPlacement.activeSelf)
        {
            Debug.Log($"Disabling grid: {targetGridPlacement.name}");
            targetGridPlacement.SetActive(false);
            _activeGridCount--;
            OnGridDisabled?.Invoke(targetGridPlacement);
        }
        else
        {
            Debug.LogWarning($"{targetGridPlacement.name} was not disabled. Is it in the list? {_gridPlacements.Contains(targetGridPlacement)}");
        }
    }
}
