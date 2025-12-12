using UnityEngine;

public class FocusUI : MonoBehaviour
{
    // === Player Movement Control ===
    public void FocusInterActionGroup() => GameManager.Instance.UIManager?.FocusMainUIGroup();

    public void FocusTowerUpgrades() => GameManager.Instance.UIManager?.FocusTowerUpgrades();
}
