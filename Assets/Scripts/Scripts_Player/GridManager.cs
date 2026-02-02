using UnityEngine;
public class GridAreaCell
{
    public bool isOccupied;
    public GameObject towerInstance;
    public Vector3 worldPosition;
}
public class GridManager : MonoBehaviour
{
    [Header("Grid Status")]
    [SerializeField] public bool _enableThisGrid;

    [Header("Grid Dimensions: X and Z")]
    public int width = 10;
    public int height = 10;
    [Header("Cell Size: How big a cell for the tower to be in")]
    public float cellSize = 1f;

    [Header("References: Item prefab spawner")]
    public GameObject nodePrefab;

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                // Calculate position relative to the GridManager's position
                Vector3 spawnPos = transform.position + new Vector3(x * cellSize + (cellSize / 2), 0, z * cellSize + (cellSize / 2));

                GameObject newNode = Instantiate(nodePrefab, spawnPos, Quaternion.identity, transform);

                // Since you have your own Tower Node script, you can initialize it here:
                // var nodeScript = newNode.GetComponent<TowerNode>();
                // nodeScript.Initialize(x, z); 
            }
        }
    }

    // This draws the Bounding Box and individual cells in the Scene View
    private void OnDrawGizmos()
    {
        // 1. Draw the individual cell outlines
        Gizmos.color = Color.yellow;
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 center = transform.position + new Vector3(x * cellSize + cellSize / 2, 0, z * cellSize + cellSize / 2);
                Gizmos.DrawWireCube(center, new Vector3(cellSize, 0, cellSize));
            }
        }

        // 2. Calculate the total area for the Bounding Box
        Vector3 totalSize = new Vector3(width * cellSize, 0, height * cellSize);
        Vector3 totalCenter = transform.position + (totalSize / 2);

        // 3. Draw the thick red border
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(totalCenter, totalSize);

        // 4. THE QUICK TIP: Draw a transparent "zone" floor
        // This helps you see the actual clickable area clearly
        Gizmos.color = new Color(0, 1, 0, 0.1f); // Very transparent green
        Gizmos.DrawCube(totalCenter, totalSize);
    }
}
