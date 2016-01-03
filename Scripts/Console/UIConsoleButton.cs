using UnityEngine;
using UnityEngine.UI;

public class UIConsoleButton : MonoBehaviour
{
    [SerializeField]
    private Text Text;

    private Toggle Toggle;

    private IConsoleElement ConsoleElement;

    public void SetText(string text)
    {
        Text.text = text;
    }

    public void Connect(IConsoleElement consoleElement)
    {
        ConsoleElement = consoleElement;
    }

    private void UpdateToggle(bool isOn)
    {
        if (isOn == true)
        {
            ConsoleElement.Show();
        }
        else
        {
            ConsoleElement.Hide();
        }
    }

    #region MonoBehaviour

    private void Awake()
    {
        Toggle = GetComponent<Toggle>();
        Toggle.onValueChanged.AddListener(UpdateToggle);
    }

    #endregion

}
