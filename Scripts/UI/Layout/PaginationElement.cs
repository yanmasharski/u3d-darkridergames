using UnityEngine;
using UnityEngine.UI;
using DRG.Debug;

public class PaginationElement : MonoBehaviour, IPaginationElement
{
    [SerializeField]
    private Toggle Toggle;

    [SerializeField]
    private int FId;

    private Pagination Pagination;

    public void OnClick()
    {
        Pagination.OnScreenSelectorClick(FId);
    }

    #region MonoBehaviour
    private void Awake()
    {
        Pagination = GetComponentInParent<Pagination>();
        Pagination.Connect(this);
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
