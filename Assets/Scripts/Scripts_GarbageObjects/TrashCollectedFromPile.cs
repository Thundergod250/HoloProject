using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TrashCollectedFromPile : MonoBehaviour
{
    public GameObject TrashObject;
    public GrabManipulator GrabManipulatorVar;
    [SerializeField] private int totalTrashCount = 3;

    private void Update()
    {
        if(totalTrashCount <= 0)
            Destroy(this.gameObject);
    }
    public void TrashTaken()
    {
        if (totalTrashCount > 0)
        {
            //Creates Instance of Trash for player to use
            GameObject newTrash = Instantiate(TrashObject);
            GrabManipulatorVar._Grab(newTrash);
            totalTrashCount--;
        } 
    }
}
