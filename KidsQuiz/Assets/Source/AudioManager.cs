using Source.Helpers;
using UnityEngine;

namespace Source
{
    public class AudioManager : Singleton<AudioManager>, IDestroyableSingleton
    {
        #region InspectorFields

        [SerializeField] private AudioSource _bgAudioSource;
        [SerializeField] private AudioSource _boneAudioSource;
        [SerializeField] private AudioClip[] _bgClips;

        #endregion

        #region PrivateFields

        private int _bgIndex = 0;

        #endregion

        #region Properties

        public bool CanDestroyed => false;

        #endregion

        #region PublicMethods

        public void Initialize()
        {
            if (Configurations.IsMusic)
            {
                UnmuteAudios();
                return;
            }

            MuteAudios();
        }

        public void PlayBoneSound()
        {
            _boneAudioSource.Play();
        }

        public void MuteAudios()
        {
            _bgAudioSource.mute = true;
            _boneAudioSource.mute = true;
            CancelInvoke("PlayNext");
        }

        #endregion

        #region PrivateMethods

        private void UnmuteAudios()
        {
            _bgAudioSource.mute = false;
            _boneAudioSource.mute = false;
            PlayNext();
        }


        private void PlayNext()
        {
            if (_bgIndex >= _bgClips.Length)
            {
                _bgIndex = 0;
            }

            _bgAudioSource.clip = _bgClips[_bgIndex];
            _bgAudioSource.Play();
            Invoke("PlayNext", _bgAudioSource.clip.length);
            _bgIndex++;
        }

        #endregion
    }
}