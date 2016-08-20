using Photon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Character
{
    public string prefabName = "default",
        description = "default description";
    
    private int moves = 3,
        cost = 1,
        attack = 1,
        defence = 1,
        health = 10;
    public Character()
    {

    }
    public Character(string name)
    {
        this.prefabName = name;
    }
    public Character(string name, string description)
        : this(name)
    {
        this.description = description;
    }

    public int Attack { get { return attack; } }
    public int Cost { get { return cost; } }
    public int Defence { get { return defence; } }
    public int Health { get { return health; } }
    public int Moves { get { return moves; } }
}
