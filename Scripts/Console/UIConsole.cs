using UnityEngine;

public class UIConsole : MonoBehaviour
{
    [SerializeField]
    private GameObject Layout;

    [SerializeField]
    private GameObject LayoutButtons;

    [SerializeField]
    private GameObject LayoutMain;

    private const float HOLD_TIME = 3;
    private float Countdown = HOLD_TIME;

    public void Show()
    {
        Layout.SetActive(true);
    }

    public void Hide()
    {
        Layout.SetActive(false);
    }

    public void AddConsoleElement(string name, IConsoleElement consoleElement)
    {
        var toggle = Instantiate(Resources.Load<UIConsoleButton>("Console/UIConsoleButton"));
        toggle.transform.SetParent(LayoutButtons.transform);
        toggle.Connect(consoleElement);
        toggle.SetText(name);

        consoleElement.AddAsChild(LayoutMain.transform);
    }

    #region Singleton

    private static UIConsole FInstance;

    public static UIConsole Instance
    {
        get
        {
            if (FInstance == null)
            {
                FInstance = Instantiate(Resources.Load<UIConsole>("Console/UIConsole"));
                FInstance.name = "UIConsole";
            }

            return FInstance;
        }
    }

    #endregion

    #region MonoBehviour

    private void Update()
    {
        if (Input.GetKey(KeyCode.F1) == true)
        {
            Show();
        }

        if (Input.touchCount == 3)
        {
            if (Countdown > 0)
            {
                Countdown -= Time.deltaTime;

                if (Countdown <= 0)
                {
                    Show();
                }
            }
        }
        else
        {
            Countdown = HOLD_TIME;
        }
    }

    #endregion
}
