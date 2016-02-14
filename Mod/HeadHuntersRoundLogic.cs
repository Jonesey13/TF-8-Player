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
    public class MyHeadhuntersRoundLogic : HeadhuntersRoundLogic
    {
        public MyHeadhuntersRoundLogic(Session session) : base(session)
        {
        }

        public override bool OtherPlayerCouldWin(int playerIndex)
        {
            if ((base.Session.Scores[playerIndex] < base.Session.MatchSettings.GoalScore) || (base.Session.GetScoreLead(playerIndex) <= 0))
            {
                return true;
            }
            int num = base.Session.GetHighestScore() - (base.Session.CurrentLevel.LivingPlayers - 1);
            for (int i = 0; i < 8; i++)
            {
                if (TFGame.Players[i] && (i != playerIndex))
                {
                    Player player = base.Session.CurrentLevel.GetPlayer(i);
                    if (((player != null) && !player.Dead) && (base.Session.Scores[i] >= num))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}