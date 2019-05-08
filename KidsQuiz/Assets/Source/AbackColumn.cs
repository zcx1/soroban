using UnityEngine;

namespace Source
{
    public class AbackColumn : MonoBehaviour
    {
        [SerializeField] private Bone[] _bones;

        private void Update()
        {
            ControlBonesMove();
        }

        public void SetBoniesFor(int number)
        {
            if (number >= 5)
            {
                number -= 5;
                _bones[4].SetActivePosition();
            }

            for (var i = 0; i < number; i++)
            {
                _bones[i].SetActivePosition();
            }
        }

        public void ActivateInteractiveBone()
        {
            foreach (var bone in _bones)
            {
                bone.IsInteractive = true;
            }
        }

        public int GetColumnNumber()
        {
            var number = 0;

            for (var i = 0; i < _bones.Length; i++)
            {
                if (_bones[i].CheckDefaultPosition) continue;
                if (i != 4)
                {
                    number += 1;
                    continue;
                }

                number += 5;
            }

            return number;
        }

        private void ControlBonesMove()
        {
            for (var i = 0; i < _bones.Length - 1; i++)
            {
                if (i == 0)
                {
                    _bones[i].IsInteractive = !_bones[i + 1].CheckActivePosition;
                    continue;
                }
                else if (i == _bones.Length - 2)
                {
                    _bones[i].IsInteractive = _bones[i - 1].CheckActivePosition;
                    continue;
                }

                _bones[i].IsInteractive = _bones[i + 1].CheckDefaultPosition && _bones[i - 1].CheckActivePosition;
            }
        }
    }
}