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
    #endregion

    #region MonoBehaviour Methods
    private void OnEnable()
    {
        UnitEvents.OnUnitSpawned += UpdateUI;
        UnitEvents.OnUnitHealthUpdate += UpdateUI;
        UnitEvents.OnUnitDie += UpdateUI;
    }

    private void OnDisable()
    {
        UnitEvents.OnUnitSpawned -= UpdateUI;
        UnitEvents.OnUnitHealthUpdate -= UpdateUI;
        UnitEvents.OnUnitDie -= UpdateUI;

    }
    #endregion

    #region Private Methods
    private void UpdateUI(string playerName, string message, string timeAgo, UnitType unitType)
    {
        if(unitType == UnitType.Blue)
        {
            EventUiView _playerEventUIView = Instantiate(_playerAEventPrefab, _eventUIContentParent);
            _playerEventUIView.Init(playerName, message, timeAgo);
        }
        else if(unitType == UnitType.Red)
        {
            EventUiView _playerEventUIView = Instantiate(_playerBEventPrefab, _eventUIContentParent);
            _playerEventUIView.Init(playerName, message, timeAgo);
        }

    }
    #endregion
}
