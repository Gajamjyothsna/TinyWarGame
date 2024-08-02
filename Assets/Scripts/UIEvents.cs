using System;

public static class UnitEvents
{
    public static event Action<string, string, DateTime, UnitType> OnUnitSpawned;

    public static void RaiseUnitSpawned(string playerName, string message, DateTime spawnTime, UnitType unitType)
    {
        OnUnitSpawned?.Invoke(playerName, message, spawnTime, unitType);
    }

    public static event Action<string, string, DateTime, UnitType> OnUnitHealthUpdate;

    public static void UpdateUnitHealth(string playerName, string message, DateTime timeAgo, UnitType unitType)
    {
        OnUnitHealthUpdate?.Invoke(playerName, message, timeAgo, unitType);
    }

    public static event Action<string, string, DateTime, UnitType> OnUnitDie;

    public static void RaiseUnitDie(string playerName, string message, DateTime timeAgo, UnitType unitType)
    {
        OnUnitHealthUpdate?.Invoke(playerName, message, timeAgo, unitType);
    }

}


