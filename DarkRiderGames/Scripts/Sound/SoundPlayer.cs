using UnityEngine;
using Unify;

namespace DRG.Sound
{
    public class SoundPlayer : SingletonMonoBehaviour<SoundPlayer>
    {
        private AudioSource AudioSource;

        public void PlayOneShot(AudioClip audioClip, float volume = 1)
        {
            AudioSource.PlayOneShot(audioClip, volume);
        }

        #region MonoBehaviour

        private void Awake()
        {
            AudioSource = gameObject.AddComponent<AudioSource>();
        }

        #endregion
    }

}
