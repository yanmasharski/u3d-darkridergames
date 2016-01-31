namespace DRG.UI
{
    using UnityEngine;

    public class UIMonoBehaviour : MonoBehaviour
    {

        private RectTransform FRectTransform;

        public RectTransform RectTransform
        {
            get
            {
                if (FRectTransform == null)
                {
                    FRectTransform = transform as RectTransform;
                }

                return FRectTransform;
            }
        }
    }
}
