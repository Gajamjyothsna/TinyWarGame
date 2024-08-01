using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EventUiView : MonoBehaviour
{
    #region Private Variables
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI _playerNameTMP;
    [SerializeField] private TextMeshProUGUI _eventTMP;
    [SerializeField] private TextMeshProUGUI _timeTMP;
    #endregion

    #region Private Methods
    internal void Init (string playerName, string eventDescription, string timeInfo)
    {
        _playerNameTMP.text = playerName;
        _eventTMP.text = eventDescription;
        _timeTMP.text = timeInfo;
    }
    #endregion
}
