using UnityEngine;

public class SetTowerSpawn : MonoBehaviour
{
    [SerializeField] private AudioClip _buyTowerClip;

    public void SetTowerSpawnTransform(TowerNodeManager node) => GameManager.Instance.CurrentTowerNode = node;

    public void SpawnTower(GameObject obj) 
    {
        AudioManager.Instance?.PlaySFX(_buyTowerClip);

        GameManager.Instance.SpawnTower(obj); 
    }
}
