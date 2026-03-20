using UnityEngine;

public class PickaxeLevel : MonoBehaviour
{
    [Header("Pickaxe Level")]
    public bool copperPick;
    public bool ironPick;
    public bool goldPick;

    private void Start()
    {
        copperPick = true;
        ironPick = false; 
        goldPick = false;
    }
}
