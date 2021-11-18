using System;
using System.Collections.Generic;
using UnityEngine;

public enum Events
{
    StartClick,
    ObstacleCollision,
    ReachTarget,
    LevelWon
}

public class EventManager : MonoBehaviour
{
    private Dictionary<Events, Action<EventParam>> eventDictionary;

    private static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (eventManager)
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }

        set => eventManager = value;
    }

    private void OnDisable()
    {
        instance = null;
        eventDictionary.Clear();
    }

    private void Init()
    {
        eventDictionary ??= new Dictionary<Events, Action<EventParam>>();
    }

    public static void StartListening(Events eventName, Action<EventParam> listener)
    {
        if (instance == null) return;

        if (instance.eventDictionary.ContainsKey(eventName))
        {
            instance.eventDictionary[eventName] += listener;
        }
        else
        {
            instance.eventDictionary.Add(eventName, listener);
        }
    }

    public static void StopListening(Events eventName, Action<EventParam> listener)
    {
        if (instance == null) return;

        if (instance.eventDictionary.ContainsKey(eventName))
        {
            instance.eventDictionary[eventName] -= listener;
        }
    }

    public static void TriggerEvent(Events eventName, EventParam eventParam)
    {
        if (instance.eventDictionary.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent.Invoke(eventParam);
        }
    }
}

public struct EventParam
{
    public string strParam;
    public int intParam;
    public float floatParam;
    public bool boolParam;
}