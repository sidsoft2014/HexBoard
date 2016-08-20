using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExitGames.Client.Photon;

public class Player : PhotonPlayer
{
    public Player(bool isLocal, int actorID, string name)
        : base(isLocal, actorID, name)
    {

    }

    protected internal Player(bool isLocal, int actorID, Hashtable properties)
        : base(isLocal, actorID, properties)
    {

    }
}
