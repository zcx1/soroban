using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source
{
    public class AbackController : MonoBehaviour
    {
        #region InspectorFields

        [SerializeField] private GameObject _columnPref;
        [SerializeField] private SettingsColumn[] _settingColumns;

        #endregion

        #region PrivateFields

        private int _answer;
        private List<AbackColumn> _columns;

        #endregion

        #region Properties

        public int ColumnCount { get; set; }

        #endregion

        #region PublicMethods

        public void Initialize()
        {
            ResetAback();
        }

        public void ShowAnswer()
        {
            switch (GameManager.Instance.GameState)
            {
                case EGameState.IN_MAKE_NUMBER:
                {
                    GameUIController.Instance.ShowAnswerMakeNumber(_answer);

                    break;
                }
                case EGameState.IN_SHOW_NUMBER:
                {
                    SpawnColumns();

                    break;
                }
            }

            Invoke("ShowQuestion", Configurations.ShowingTime);
        }

        public void ShowQuestion()
        {
            switch (GameManager.Instance.GameState)
            {
                case EGameState.IN_MAKE_NUMBER:
                {
                    SpawnColumns();
                    break;
                }
                case EGameState.IN_SHOW_NUMBER:
                {
                    GameUIController.Instance.ShowQuestionShowNumber();
                    break;
                }
            }
        }

        public bool CheckAnswer(int inputedAnsw = 0)
        {
            var value = false;
            switch (GameManager.Instance.GameState)
            {
                case EGameState.IN_MAKE_NUMBER:
                {
                    value = GetMadeNumber() == _answer;
                    break;
                }
                case EGameState.IN_SHOW_NUMBER:
                {
                    value = inputedAnsw == _answer;
                    break;
                }
            }

            return value;
        }

        #endregion

        #region PrivateMethods

        private void ResetAback()
        {
            RegenirationAnwer();
            ShowAnswer();
        }

        private void RegenirationAnwer()
        {
            var maxRangeString = "";
            var minRangeString = "1";
            for (var i = 0; i < ColumnCount; i++)
            {
                maxRangeString += "9";
                if (i == 0)
                    continue;
                minRangeString += "0";
            }

            var maxRange = Convert.ToInt32(maxRangeString);
            var minRange = Convert.ToInt32(minRangeString);
            _answer = Random.RandomRange(minRange, maxRange);
            Debug.Log("ANSWER: " + _answer);
        }

        private void SpawnColumns()
        {
            var answChars = _answer.ToString().ToCharArray();
            var answIntList = answChars.Select(answChar => (int) char.GetNumericValue(answChar)).ToList();
            answIntList.Reverse();
            if (_columns == null)
            {
                _columns = new List<AbackColumn>();
                foreach (var columnSpawnPoint in _settingColumns[ColumnCount - 1].ColumnsSpawns)
                {
                    var columnGO = Instantiate(_columnPref, columnSpawnPoint);
                    var column = columnGO.GetComponent<AbackColumn>();
                    _columns.Add(column);
                }
            }

            if (GameManager.Instance.GameState == EGameState.IN_SHOW_NUMBER)
            {
                SpawnColumnsForShowingNumber(answIntList);
                return;
            }

            SpawnColumnsForMakeNumber();
        }

        private void SpawnColumnsForMakeNumber()
        {
            foreach (var column in _columns)
            {
                column.ActivateInteractiveBone();
            }
        }

        private void SpawnColumnsForShowingNumber(List<int> answIntList)
        {
            for (var i = 0; i < answIntList.Count; i++)
            {
                _columns[i].SetBoniesFor(answIntList[i]);
            }

            if (answIntList.Count >= ColumnCount)
                return;
            for (var i = answIntList.Count; i < ColumnCount; i++)
            {
                _columns[i].SetBoniesFor(0);
            }
        }

        private int GetMadeNumber()
        {
            var stringNumber = string.Empty;

            foreach (var column in _columns)
            {
                stringNumber += column.GetColumnNumber().ToString();
            }


            if (stringNumber == string.Empty)
            {
                return 0;
            }

            stringNumber = ReverseString(stringNumber);

            return Convert.ToInt32(stringNumber);
        }

        private string ReverseString(string str)
        {
            var array = str.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }

        #endregion
    }

    [Serializable]
    public class SettingsColumn
    {
        public Transform[] ColumnsSpawns;
    }
}