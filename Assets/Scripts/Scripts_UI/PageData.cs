using UnityEngine;

[CreateAssetMenu(fileName = "PageData", menuName = "Scriptable Objects/PageData")]
public class PageData : ScriptableObject
{
    public string pageTitle;
    public string pageDescription;
    public int pageHealth;
    public int pageDamage;
    public string pageSpeedRange;
    public Sprite pageImage;
}
