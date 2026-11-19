using UnityEngine;
using System.Collections.Generic;
using System;

public class ExitMultitaskButton : MonoBehaviour
{
    public static ExitMultitaskButton instance;
    private Stack<Action> onExit = new Stack<Action>();

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }
    public void AddAction(Action action)
    {
        onExit.Push(action);
        gameObject.SetActive(true);
    }
    public void Exit()
    {
        onExit.Pop().Invoke();
        if (onExit.Count == 0)
            gameObject.SetActive(false);
    }
}