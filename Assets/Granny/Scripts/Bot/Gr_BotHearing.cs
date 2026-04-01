using System;
using UnityEngine;

public class Gr_BotHearing : MonoBehaviour
{
    private Gr_BotController bot;

    private void Awake()
    {
        bot = GetComponent<Gr_BotController>();
    }

    private void OnEnable()
    {
        Gr_EventManager.AddListener<NoiseEvent>(HandleNoiseEvent);
    }

    private void OnDisable()
    {
        Gr_EventManager.RemoveListener<NoiseEvent>(HandleNoiseEvent);
    }

    private void HandleNoiseEvent(NoiseEvent e)
    {
        bot.BlackBoard.HeardPosition = e.position;
        bot.BlackBoard.HasHeardSound = true;
    }
}
