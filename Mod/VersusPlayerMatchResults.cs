using Microsoft.Xna.Framework;
using Monocle;
using Patcher;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TowerFall;

namespace Mod
{
    [Patch]
    public class MyVersusPlayerMatchResults : VersusPlayerMatchResults
    {
        public MyVersusPlayerMatchResults(Session session, VersusMatchResults matchResults, int playerIndex, Vector2 tweenFrom, Vector2 tweenTo, List<AwardInfo> awards) : base(session, matchResults, playerIndex, tweenFrom, tweenTo, awards)
        {
            this.session = session;
            this.matchResults = matchResults;
            this.playerIndex = playerIndex;
            this.tweenFrom = base.Position = tweenFrom;
            this.tweenTo = tweenTo;
            this.awards = awards;
            this.characterIndex = TFGame.Characters[playerIndex];
            this.won = session.MatchStats[playerIndex].Won;
            this.bg = new Image(TFGame.MenuAtlas[this.won ? "versusResults/winnerbg" : "versusResults/bg"], null);
            this.bg.CenterOrigin();
            this.bg.Zoom = 0.5f;
            base.Add(this.bg);
            ArcherData data = ArcherData.Get(TFGame.Characters[playerIndex], TFGame.AltSelect[playerIndex]);
            if (this.won)
            {
                this.portrait = new Image(data.Portraits.Win, null);
            }
            else
            {
                this.portrait = new Image(data.Portraits.Lose, null);
            }
            this.portrait.CenterOrigin();
            this.portrait.Zoom = 0.5f;
            this.portrait.Position = this.bg.Position + ((Vector2)(Vector2.UnitY * (((-this.bg.Height / 2f) + (this.portrait.Height / 2f)) + 15f)));
            DrawRectangle component = new DrawRectangle((this.portrait.X - (this.portrait.Width / 4f)) - 1f, (this.portrait.Y - (this.portrait.Height / 4f)) - 1f, (this.portrait.Width / 2f) + 2f, (this.portrait.Height / 2f) + 2f, this.won ? WinBorder : LoseBorder);
            base.Add(component);
            base.Add(this.portrait);
            this.gem = TFGame.MenuSpriteData.GetSpriteString(data.Gems.Menu);
            if (this.won)
            {
                this.gem.Play("on", false);
            }
            else
            {
                this.gem.Play("off", false);
            }
            this.gem.Position = (Vector2)((-Vector2.UnitY * this.bg.Height) / 2f);
            this.gem.Visible = false;
            this.gem.Zoom = 0.5f;
            base.Add(this.gem);
            if (this.won)
            {
                switch (this.characterIndex)
                {
                    case 0:
                        this.particlesAt = new Vector2(21f, 11f);
                        this.particlesRange = new Vector2(11f, 7f);
                        return;

                    case 1:
                        this.particlesAt = new Vector2(29f, 8f);
                        this.particlesRange = new Vector2(14f, 6f);
                        return;

                    case 2:
                        this.particlesAt = new Vector2(29f, 10f);
                        this.particlesRange = new Vector2(10f, 8f);
                        return;

                    case 3:
                        this.particlesAt = new Vector2(25f, 14f);
                        this.particlesRange = new Vector2(15f, 6f);
                        break;

                    default:
                        return;
                }
            }
        }
    }
}
