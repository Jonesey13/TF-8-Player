using Microsoft.Xna.Framework;
using Patcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TowerFall;

namespace Mod
{
    [Patch]
    public class MyPlayer : Player
    {
        public static Monocle.Collider[] wasColliders = new Monocle.Collider[8];

        public MyPlayer(int playerIndex, Vector2 position, Allegiance allegiance, Allegiance teamColor, PlayerInventory inventory, HatStates hatState, bool frozen, bool flash, bool indicator) : base(playerIndex, position, allegiance, teamColor, inventory, hatState, frozen, flash, indicator)
        {
        }
    }
}
