using Photon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Character
{
    [SerializeField]
    private int moves = 3,
        attack = 1,
        deffence = 1;

    public Player owner;
    public string prefabName = "default",
        description = "default description";

    public int Moves { get { return moves; } }
    public int Attack { get { return attack; } }
    public int Deffence { get { return deffence; } }

    public Character(Player owner)
    {
        this.owner = owner;
    }
    public Character(Player owner, string name)
        : this (owner)
    {
        this.prefabName = name;
    }
    public Character(Player owner, string name, string description)
        : this(owner, name)
    {
        this.description = description;
    }
}
