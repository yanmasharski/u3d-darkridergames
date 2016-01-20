using UnityEngine;
using DRG.UI;
using System.Collections.Generic;

public class Pagination : MonoBehaviour, IPagination
{
    private bool IsDirty = false;
    private List<IPaginationElement> Elements = new List<IPaginationElement>();

    public void Connect(IPaginationElement paginationElement)
    {
        Elements.Add(paginationElement);
        IsDirty = true;
    }

	public void SetSelected(int currentScreen)
    {
        if (IsDirty)
        {
            IsDirty = false;
            Elements.Sort((x, y) => x.Id.CompareTo(y.Id));
        }

        for (int i = 0; i < Elements.Count; i++)
        {
            Elements[i].IsSelected = (currentScreen == i);
        }
    }

    private void Awake()
    {
        GetComponentInParent<HorizontalScrollSnap>().Connect(this);
    }
}
