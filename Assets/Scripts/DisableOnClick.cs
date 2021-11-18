using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnClick : MonoBehaviour
{
    private Canvas canvas;
    private void OnEnable()
    {
        EventManager.StartListening(Events.StartClick, OnStartClick);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.StartClick, OnStartClick);
    }

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    private void OnStartClick(EventParam param)
    {
        canvas.enabled = false;
    }
}
