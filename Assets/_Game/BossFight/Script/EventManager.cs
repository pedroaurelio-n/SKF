using System;
using UnityEngine;

public static class EventManager
{
    public static Action OnPlayerDeath { get; internal set; }
    public static Action OnAllWavesDefeated { get; internal set; }
    public static Action<int, int> OnWaveStarted { get; internal set; }

    // Eventos de Boss
    public static event Action OnBossFightStarted;           // Quando o boss come�a
    public static event Action OnBossDefeated;               // Quando o boss morre

    // M�todos para disparar eventos
    public static void TriggerWaveStarted(int current, int total)
    {
        OnWaveStarted?.Invoke(current, total);
    }

    public static void TriggerWaveDefeated(int index)
    {
    }

    public static void TriggerAllWavesDefeated()
    {
        OnAllWavesDefeated?.Invoke();
    }

    public static void TriggerBossFightStarted()
    {
        OnBossFightStarted?.Invoke();
    }

    public static void TriggerBossDefeated()
    {
        OnBossDefeated?.Invoke();
    }

    internal static void TriggerPlayerHealthChanged(int current, int max)
    {
        //throw new NotImplementedException();
    }

    internal static void TriggerPlayerDeath()
    {
        OnPlayerDeath?.Invoke();
    }
}
