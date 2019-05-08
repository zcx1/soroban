using Source.Helper;
using UnityEngine;

namespace Source
{
    public class AbackContainer : Singleton<AbackContainer>, IDestroyableSingleton
    {
        [SerializeField] private Transform _abackSpawnPoint;
        
        
        public Transform AbackSpawnPoint => _abackSpawnPoint;
        public bool CanDestroyed => true;
    }
}