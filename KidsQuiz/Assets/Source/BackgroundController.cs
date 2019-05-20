using System;
using Source.Helpers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source
{
    public class BackgroundController: Singleton<BackgroundController> ,IDestroyableSingleton
    {
        #region InspectorFields

        [SerializeField] private GameObject[] _bgPrefs; 

        #endregion

        #region Properties

        public bool CanDestroyed => false;

        #endregion

        #region PrivateFields

        private GameObject _bgGO;

        #endregion

        #region PublicMethods

        public void ChangeBackground()
        {
            if (_bgGO != null)
            {
                Destroy(_bgGO);
            }
            var index = Random.RandomRange(0, _bgPrefs.Length - 1);
            _bgGO = Instantiate(_bgPrefs[index], transform);
        }

        #endregion

    }
}
