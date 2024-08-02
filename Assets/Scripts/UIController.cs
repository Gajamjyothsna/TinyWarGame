using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    #region Private Variables
    [Header("Player Event UI Prefabs")]
    [SerializeField] private EventUiView _playerAEventPrefab;
    [SerializeField] private EventUiView _playerBEventPrefab;
    [Header("Content Position")]
    [SerializeField] private Transform _eventUIContentParent;
    [Header("Events Panel")]
    [SerializeField] private GameObject _eventsPanel;
    [SerializeField] private GameObject _buttonPanel;

    private List<EventData> _eventDataList = new List<EventData>();
    #endregion

    #region Event Data Class
    [Serializable]
    private class EventData
    {
        public string playerName;
        public string message;
        public DateTime eventTime;
        public UnitType unitType;

        public EventData(string playerName, string message, DateTime eventTime, UnitType unitType)
        {
            this.playerName = playerName;
            this.message = message;
            this.eventTime = eventTime;
            this.unitType = unitType;
        }
    }
    #endregion

    #region MonoBehaviour Methods
    private void OnEnable()
    {
        UnitEvents.OnUnitSpawned += AddEventData;
        UnitEvents.OnUnitHealthUpdate += AddEventData;
        UnitEvents.OnUnitDie += AddEventData;
    }

    private void OnDisable()
    {
        UnitEvents.OnUnitSpawned -= AddEventData;
        UnitEvents.OnUnitHealthUpdate -= AddEventData;
        UnitEvents.OnUnitDie -= AddEventData;
    }

    private void AddEventData(string playerName, string message, DateTime time, UnitType unitType)
    {
        EventData newEvent = new EventData(playerName, message, time, unitType);
        _eventDataList.Add(newEvent);
    }

    public void OpenEventsPanel()
    {
        _eventsPanel.SetActive(true);
        _buttonPanel.SetActive(false);
        UpdateUI();
    }

    public void CloseEventsPanel()
    {
        foreach (Transform child in _eventUIContentParent)
        {
            Destroy(child.gameObject);
        }
        _eventsPanel.SetActive(false);
        _buttonPanel.SetActive(true);
    }

    #endregion

    #region Private Methods
    private void UpdateUI(string playerName, string message, DateTime timeAgo, UnitType unitType)
    {
        if (unitType == UnitType.Blue)
        {
            EventUiView _playerEventUIView = Instantiate(_playerAEventPrefab, _eventUIContentParent);
            _playerEventUIView.Init(playerName, message, timeAgo);
        }
        else if (unitType == UnitType.Red)
        {
            EventUiView _playerEventUIView = Instantiate(_playerBEventPrefab, _eventUIContentParent);
            _playerEventUIView.Init(playerName, message, timeAgo);
        }

    }
    private void UpdateUI()
    {
        foreach (EventData eventData in _eventDataList)
        {
            EventUiView _playerEventUIView;
            if (eventData.unitType == UnitType.Blue)
            {
                _playerEventUIView = Instantiate(_playerAEventPrefab, _eventUIContentParent);
            }
            else
            {
                _playerEventUIView = Instantiate(_playerBEventPrefab, _eventUIContentParent);
            }
            _playerEventUIView.Init(eventData.playerName, eventData.message, eventData.eventTime);
        }
    }
    #endregion
}
