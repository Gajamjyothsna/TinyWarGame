using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class EventUiView : MonoBehaviour
{
    #region Private Variables
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI _playerNameTMP;
    [SerializeField] private TextMeshProUGUI _eventTMP;
    [SerializeField] private TextMeshProUGUI _timeTMP;

    private string timeAgo;
    #endregion

    #region Private Methods
    internal void Init (string playerName, string eventDescription, DateTime eventTime)
    {
        _playerNameTMP.text = playerName;
        _eventTMP.text = eventDescription;
        DateTime now = DateTime.Now;
        TimeSpan timeDifference = now - eventTime;
        if (timeDifference.TotalMinutes < 1)
        {
            timeAgo = "0 mins ago";
        }
        else if (timeDifference.TotalMinutes < 60)
        {
            timeAgo = $"{(int)timeDifference.TotalMinutes} mins ago";
        }
        else
        {
            timeAgo = $"{(int)timeDifference.TotalHours} hours ago";
        }
        _timeTMP.text = timeAgo;
    }
    #endregion
}
