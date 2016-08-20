﻿using System;
using UnityEngine;

public class TurnChangeEvent : EventArgs
{
    public TurnChangeEvent (int endIdx, int startIdx)
    {

    }

    public int EndingPlayerIdx { get; private set; }
    public int StartingPlayerIdx { get; private set; }
}

public class GameManager : MonoBehaviour
{
    private Player[] players;
    private Player currentPlayer;
    private Navigatable[] navObjs;
    private int turnIdx = 0;
    private int totalTurns = 0;

    public HexGrid grid;
    public event EventHandler<TurnChangeEvent> TurnChanged;

    public int TurnNumber { get; private set; }

    // Use this for initialization
    private void Start()
    {
        if (grid == null)
            grid = FindObjectOfType<HexGrid>();
        if (grid == null)
            Debug.LogError("No grid found");

        navObjs = FindObjectsOfType<Navigatable>();
        if (navObjs == null || navObjs.Length < 1)
            Debug.LogError("No navigatable objects found in scene.");

        players = FindObjectsOfType<Player>();
        if (players == null || players.Length < 1)
            Debug.LogError("No players found in scene.");
        else
        {
            for (int idx = 0; idx < players.Length; idx++)
            {
                players[idx].playerNumber = idx;
            }
        }

        currentPlayer = players[0];
        OnTurnChanged(-1);
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public MoveType GetMoveType(HexCoordinates destination, out Vector3? pos)
    {
        pos = grid.ParseMove(destination);
        if (!pos.HasValue)
            return MoveType.Invalid;

        foreach (var item in navObjs)
        {
            if (item.Position == pos)
            {
                return MoveType.Attacking;
            }
        }

        return MoveType.Movement;
    }

    public void EndTurn(Player player)
    {
        if (players[turnIdx] != player || (currentPlayer != null && currentPlayer != player))
            return;

        int oldIdx = turnIdx;
        if (++turnIdx >= players.Length)
            turnIdx = 0;

        currentPlayer = players[turnIdx];
        OnTurnChanged(oldIdx);
        ++totalTurns;

        Debug.Log("Total turns: " + totalTurns);
    }

    private void OnTurnChanged(int oldIdx)
    {
        var handler = TurnChanged;
        if (handler != null)
        {
            handler(this, new TurnChangeEvent(oldIdx, turnIdx));
        }
    }
}