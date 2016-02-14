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
    public class MyMatchTeams : MatchTeams
    {
        public Allegiance player5Team;
        public Allegiance player6Team;
        public Allegiance player7Team;
        public Allegiance player8Team;

        public MyMatchTeams(Allegiance start) : base(start)
        {
            this.player1Team = this.player2Team = this.player3Team = this.player4Team = this.player5Team = this.player6Team = this.player7Team = this.player8Team = start;
        }

        public override int GetAmountOfPlayersOfAllegiance(Allegiance allegiance)
        {
            int num = 0;
            if ((this.player1Team == allegiance) && TFGame.Players[0])
            {
                num++;
            }
            if ((this.player2Team == allegiance) && TFGame.Players[1])
            {
                num++;
            }
            if ((this.player3Team == allegiance) && TFGame.Players[2])
            {
                num++;
            }
            if ((this.player4Team == allegiance) && TFGame.Players[3])
            {
                num++;
            }
            if ((this.player5Team == allegiance) && TFGame.Players[4])
            {
                num++;
            }
            if ((this.player6Team == allegiance) && TFGame.Players[5])
            {
                num++;
            }
            if ((this.player7Team == allegiance) && TFGame.Players[6])
            {
                num++;
            }
            if ((this.player8Team == allegiance) && TFGame.Players[7])
            {
                num++;
            }
            return num;
        }

        public override List<int> GetPlayersOfAllegiance(Allegiance allegiance)
        {
            List<int> list = new List<int>();
            if ((this.player1Team == allegiance) && TFGame.Players[0])
            {
                list.Add(0);
            }
            if ((this.player2Team == allegiance) && TFGame.Players[1])
            {
                list.Add(1);
            }
            if ((this.player3Team == allegiance) && TFGame.Players[2])
            {
                list.Add(2);
            }
            if ((this.player4Team == allegiance) && TFGame.Players[3])
            {
                list.Add(3);
            }
            if ((this.player5Team == allegiance) && TFGame.Players[4])
            {
                list.Add(4);
            }
            if ((this.player6Team == allegiance) && TFGame.Players[5])
            {
                list.Add(5);
            }
            if ((this.player7Team == allegiance) && TFGame.Players[6])
            {
                list.Add(6);
            }
            if ((this.player8Team == allegiance) && TFGame.Players[7])
            {
                list.Add(7);
            }
            return list;
        }


        public override Allegiance this[int index]
        {
            get
            {
                switch (index)
                {
                    case -1:
                        return Allegiance.Red;

                    case 0:
                        return this.player1Team;

                    case 1:
                        return this.player2Team;

                    case 2:
                        return this.player3Team;

                    case 3:
                        return this.player4Team;

                    case 4:
                        return this.player5Team;

                    case 5:
                        return this.player6Team;

                    case 6:
                        return this.player7Team;

                    case 7:
                        return this.player8Team;
                }
                throw new Exception("Index out of bounds!");
            }
            set
            {
                switch (index)
                {
                    case 0:
                        this.player1Team = value;
                        return;

                    case 1:
                        this.player2Team = value;
                        return;

                    case 2:
                        this.player3Team = value;
                        return;

                    case 3:
                        this.player4Team = value;
                        return;

                    case 4:
                        this.player5Team = value;
                        return;

                    case 5:
                        this.player6Team = value;
                        return;

                    case 6:
                        this.player7Team = value;
                        return;

                    case 7:
                        this.player8Team = value;
                        return;
                }
                throw new Exception("Index out of bounds!");
            }
        }

        public override bool ProperlyAssigned
        {
            get
            {
                bool flag = false;
                bool flag2 = false;
                for (int i = 0; i < 8; i++)
                {
                    if (TFGame.Players[i])
                    {
                        if (this[i] == Allegiance.Neutral)
                        {
                            return false;
                        }
                        if (this[i] == Allegiance.Red)
                        {
                            flag = true;
                        }
                        else if (this[i] == Allegiance.Blue)
                        {
                            flag2 = true;
                        }
                    }
                }
                return (flag && flag2);
            }
        }

    }
}
