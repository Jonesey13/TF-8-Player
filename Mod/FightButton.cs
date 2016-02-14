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
    public class MyFightButton : FightButton
    {
        public MyFightButton(Vector2 position, Vector2 tweenFrom) : base(position, tweenFrom)
        {
            this.text = "VERSUS";
            this.playersText = "2-8 PLAYERS";
        }
    }
}
