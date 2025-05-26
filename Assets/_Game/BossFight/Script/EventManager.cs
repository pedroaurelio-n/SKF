using System;
using UnityEngine;

public static class EventManager
{
    public static Action OnPlayerDeath { get; internal set; }
    public static Action OnAllWavesDefeated { get; internal set; }
    public static Action<int, int> OnWaveStarted { get; internal set; }

    // Eventos de Waves
    public static event Action<int, int> WaveStarted;      // (wave atual, total de waves)
    public static event Action<int> WaveDefeated;          // (índice da wave finalizada)
    public static event Action AllWavesDefeated;           // Todas as waves derrotadas

    // Eventos de Boss
    public static event Action BossFightStarted;           // Quando o boss começa
    public static event Action BossDefeated;               // Quando o boss morre

    // Métodos para disparar eventos
    public static void TriggerWaveStarted(int current, int total)
    {
        WaveStarted?.Invoke(current, total);
    }

    public static void TriggerWaveDefeated(int index)
    {
        WaveDefeated?.Invoke(index);
    }

    public static void TriggerAllWavesDefeated()
    {
        AllWavesDefeated?.Invoke();
    }

    public static void TriggerBossFightStarted()
    {
        BossFightStarted?.Invoke();
    }

    public static void TriggerBossDefeated()
    {
        BossDefeated?.Invoke();
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
