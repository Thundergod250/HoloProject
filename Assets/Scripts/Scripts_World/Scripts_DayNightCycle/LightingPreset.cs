using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "LightingPreset", menuName = "Scriptable/Lighting Preset", order = 1)]
public class LightingPreset : ScriptableObject
{
    [SerializeField] public Gradient _ambientColor;
    [SerializeField] public Gradient _directionalColor;
    [SerializeField] public Gradient _fogColor;
}
