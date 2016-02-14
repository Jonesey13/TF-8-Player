using Microsoft.Xna.Framework;
using Monocle;
using Patcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TowerFall;

namespace Mod
{
    [Patch]
    public class MyMatchSettings : MatchSettings
    {
        public MyMatchSettings(TowerFall.LevelSystem levelSystem, TowerFall.Modes mode, MatchLengths matchLength) : base(levelSystem, mode, matchLength)
        {
            this.LevelSystem = levelSystem;
            this.Mode = mode;
            this.MatchLength = matchLength;
            this.Teams = new MatchTeams(Allegiance.Neutral);
            this.Variants = new MatchVariants(false);
        }

        public override int GetMaxTeamSize()
        {
            if (!this.TeamMode)
            {
                return 1;
            }
            int[] numArray = new int[2];
            for (int i = 0; i < 8; i++)
            {
                if (TFGame.Players[i] && (this.Teams[i] != Allegiance.Neutral))
                {
                    numArray[(int)this.Teams[i]]++;
                }
            }
            return Math.Max(numArray[0], numArray[1]);
        }

        public override int PlayerGoals(int p2goal, int p3goal, int p4goal)
        {
            switch (TFGame.PlayerAmount)
            {
                case 2:
                    return p2goal;

                case 3:
                    return p3goal;

                case 4:
                    return p4goal;
            }
            return p4goal;
        }

        public override int PlayerLimit
        {
            get
            {
                if (!this.SoloMode)
                {
                    return 8;
                }
                return 1;
            }
        }
    }
}
