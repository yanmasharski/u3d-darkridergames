using System;

public interface IPagination
{
    void SetSelected(int currentScreen);
    Action<int> OnScreenSelectorClick { get; set; }
}
