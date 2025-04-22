using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo Instance;

    public string PlayerName = "dear friend";

    public Transform Player;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;

        Player = transform.parent;
    }

    public List<string> EventMessages = new List<string>();

    public void AddEventMessage(string message)
    {
        EventMessages.Add(message);
    }

    public void AddPlayerChoice(PlayerChoiceKey key, PlayerChoice value)
    {
        Choices[key] = value;
    }

    public PlayerChoice GetPlayerChoice(PlayerChoiceKey key)
    {
        return Choices.GetValueOrDefault(key);
    }

    private Dictionary<PlayerChoiceKey, PlayerChoice> Choices = new Dictionary<PlayerChoiceKey, PlayerChoice>();
}

public class PlayerChoice
{
    public string Choice;
    public SideTriggers.Side DecisionSide;

    public PlayerChoice(string choice, SideTriggers.Side side)
    {
        Choice = choice;
        DecisionSide = side;
    }
}

public enum PlayerChoiceKey
{
    Name,
    Hunger,
    LetMeIn,
    Butterflies,
    Storm
}