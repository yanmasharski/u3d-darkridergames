using UnityEngine;

public interface IConsoleElement
{
    void Show();
    void Hide();
    void AddAsChild(Transform parent);
}
