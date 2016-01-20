using UnityEngine;
using UnityEngine.UI;
using DRG.Debug;

public class PaginationElement : MonoBehaviour, IPaginationElement
{
    [SerializeField]
    private Toggle Toggle;

    [SerializeField]
    private int FId;

    private void OnValueChanged(bool val)
    {
        Log.Message("OnValueChanged: " + val);
    }

    #region MonoBehaviour
    private void Awake()
    {
        Toggle.onValueChanged.AddListener(OnValueChanged);
        GetComponentInParent<Pagination>().Connect(this);
    }
    #endregion

    #region IPaginationElement

    public int Id
    {
        get
        {
            return FId;
        }
    }

    public bool IsSelected
    {
        get
        {
            return Toggle.isOn;
        }
        set
        {
            Toggle.isOn = value;
        }
    }

    #endregion
}
