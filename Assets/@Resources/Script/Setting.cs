using UnityEngine;
using UnityEngine.UI;

public class Setting : UI_Base
{
    public Dropdown dropdown;
    public Slider slider;
    void Start()
    {
        slider.value = GameManager.Instance.volume;
        dropdown.onValueChanged.AddListener(ChangeWindowMode);
        slider.onValueChanged.AddListener(ChangeVolume);
        
    }
    void ChangeWindowMode(int value)
    {
        switch (value)
        {
            case 0:
                Screen.SetResolution(1920,1080,true);
                break;
            case 1:
                Screen.SetResolution(1280, 720, false);
                break;
            case 2:
                Screen.SetResolution(854, 480, false);
                break;
        }
    }
    void ChangeVolume(float value)
    {
        GameManager.Instance.volume = value;
    }
}
