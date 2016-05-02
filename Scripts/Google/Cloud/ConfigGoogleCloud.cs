namespace DRG.Google.Cloud
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "ConfigGoogleCloud", menuName = "Google Cloud Config")]
    public class ConfigGoogleCloud : ScriptableObject
    {
        [SerializeField]
        private string keyIOS;

        [SerializeField]
        private string keyAndroid;

        public string key
        {
            get
            {
#if UNITY_IPHONE
                return keyIOS;
#elif UNITY_ANDROID
                return keyAndroid;
#endif
                return "";
            }
        }
    }
}