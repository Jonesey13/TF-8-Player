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
    public class MyVersusMatchResults: VersusMatchResults
    {

        public MyVersusMatchResults(Session session, VersusRoundResults roundResults) : base(session, roundResults)
        {
            Vector2[] vectorArray;
            Vector2[] vectorArray2;
            this.session = session;
            this.roundResults = roundResults;
            base.Position = new Vector2(0f, 240f);
            if (TFGame.PlayerAmount == 2)
            {
                vectorArray = new Vector2[] { new Vector2(-160f, 120f), new Vector2(480f, 120f) };
                vectorArray2 = new Vector2[] { new Vector2(110f, 120f), new Vector2(210f, 120f) };
            }
            else if (TFGame.PlayerAmount == 3)
            {
                vectorArray = new Vector2[] { new Vector2(-160f, 120f), new Vector2(160f, 360f), new Vector2(480f, 120f) };
                vectorArray2 = new Vector2[] { new Vector2(80f, 120f), new Vector2(160f, 120f), new Vector2(240f, 120f) };
            }
            else
            {
                vectorArray = new Vector2[] { new Vector2(-320f, 120f), new Vector2(-280f, 120f), new Vector2(-240f, 120f), new Vector2(-200f, 120f), new Vector2(-160f, 120f), new Vector2(-120f, 120f), new Vector2(-80f, 120f), new Vector2(-40f, 120f) };
                vectorArray2 = new Vector2[] { new Vector2(20f, 120f), new Vector2(60f, 120f), new Vector2(100f, 120f), new Vector2(140f, 120f), new Vector2(180f, 120f), new Vector2(220f, 120f), new Vector2(260f, 120f), new Vector2(300f, 120f) };
            }
            int winner = session.GetWinner();
            for (int i = 0; i < 8; i++)
            {
                if (TFGame.Players[i] && (winner == session.GetScoreIndex(i)))
                {
                    session.MatchStats[i].Won = true;
                    SaveData.Instance.Stats.Wins[TFGame.Characters[i]] += (ulong)1L;
                    SessionStats.RegisterArcherWin(i);
                }
                else
                {
                    session.MatchStats[i].Won = false;
                }
            }
            List<AwardInfo>[] awards = VersusAwards.GetAwards(session.MatchSettings, session.MatchStats);
            SessionStats.RegisterArcherPlays();
            playerResults = new List<VersusPlayerMatchResults>();
            for (int j = 0; j < 8; j++)
            {
                if (TFGame.Players[j])
                {
                    var entity = new VersusPlayerMatchResults(session, this, j, vectorArray[playerResults.Count], vectorArray2[playerResults.Count], awards[j]);
                    session.CurrentLevel.Add(entity);
                    playerResults.Add(entity);
                }
            }
        }
    }
}
