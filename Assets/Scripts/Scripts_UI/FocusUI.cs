using UnityEngine;

public class FocusUI : MonoBehaviour
{
    // === Player Movement Control ===
    public void FocusInterActionGroup() => GameManager.Instance.UIManager?.FocusInterActionGroup();

    public void FocusTowerUpgrades() => GameManager.Instance.UIManager?.FocusTowerUpgrades();
}
