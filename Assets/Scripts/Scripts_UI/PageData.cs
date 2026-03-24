using UnityEngine;

[CreateAssetMenu(fileName = "PageData", menuName = "Scriptable Objects/PageData")]
public class PageData : ScriptableObject
{
    public string pageTitle;
    [TextArea(5, 10)]
    public string pageDescription;
    public Sprite pageImage;
}
