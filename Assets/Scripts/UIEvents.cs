using System;

public static class UnitEvents
{
    public static event Action<string, string, string, UnitType> OnUnitSpawned;

    public static void RaiseUnitSpawned(string playerName, string message, string timeAgo, UnitType unitType)
    {
        OnUnitSpawned?.Invoke(playerName, message, timeAgo, unitType);
    }

    public static event Action<string, string, string,UnitType> OnUnitHealthUpdate;

    public static void UpdateUnitHealth(string playerName, string message, string timeAgo, UnitType unitType)
    {
        OnUnitHealthUpdate?.Invoke(playerName, message, timeAgo, unitType);
    }

    public static event Action<string, string, string, UnitType> OnUnitDie;

    public static void RaiseUnitDie(string playerName, string message, string timeAgo, UnitType unitType)
    {
        OnUnitHealthUpdate?.Invoke(playerName, message, timeAgo, unitType);
    }

}


