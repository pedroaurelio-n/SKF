using System;

public static class EventManager
{
    public static event Action<int, int> OnPlayerHealthChanged;
    public static void TriggerPlayerHealthChanged (int current, int max)
    {
        OnPlayerHealthChanged?.Invoke(current, max);
    }
    
    public static event Action OnPlayerDeath;
    public static void TriggerPlayerDeath ()
    {
        OnPlayerDeath?.Invoke();
    }
    
    public static event Action<int, int> OnWaveStarted;
    public static void TriggerWaveStarted (int current, int total)
    {
        OnWaveStarted?.Invoke(current, total);
    }
    
    public static event Action<int> OnWaveDefeated;
    public static void TriggerWaveDefeated (int current)
    {
        OnWaveDefeated?.Invoke(current);
    }

    public static event Action OnAllWavesDefeated;
    public static void TriggerAllWavesDefeated ()
    {
        OnAllWavesDefeated?.Invoke();
    }
}