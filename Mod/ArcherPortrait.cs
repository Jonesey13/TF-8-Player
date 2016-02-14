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
    public class MyArcherPortrait : ArcherPortrait
    {
        public MyArcherPortrait(Vector2 offset, int characterIndex, TowerFall.ArcherData.ArcherTypes altSelect, bool showTitle) : base(offset, characterIndex, altSelect, showTitle)
        {
            this.portrait.Zoom = 0.6f;
            this.portraitAlt.Zoom = 0.6f;
        }

        public override void Render()
        {
            Vector2 vector = base.Entity.Position + this.offset;
            this.portrait.Scale = this.portraitAlt.Scale = this.flash.Scale = new Vector2(1f + (this.wiggler.Value * 0.05f), 1f - (this.wiggler.Value * 0.05f));
            this.portrait.Position = this.portraitAlt.Position = this.flash.Position = this.offset + this.lastShake;
            if (this.joined)
            {
                this.gem.Scale = (Vector2)(Vector2.One * (1.5f + (0.2f * this.gemWiggler.Value)));
                this.gem.Rotation = ((0x2d * this.lastMove) * 0.01745329f) * this.gemWiggler.Value;
            }
            else
            {
                this.gem.Scale = (Vector2)(Vector2.One * (1f + (0.2f * this.gemWiggler.Value)));
                this.gem.Rotation = ((20 * this.lastMove) * 0.01745329f) * this.gemWiggler.Value;
            }
            if (!this.joined)
            {
                if (this.flipEase < 0.5f)
                {
                    this.portraitAlt.Visible = true;
                    this.portrait.Visible = false;
                    this.portraitAlt.Scale.X *= MathHelper.Lerp(1f, 0f, this.flipEase * 2f);
                }
                else
                {
                    this.portraitAlt.Visible = false;
                    this.portrait.Visible = true;
                    this.portrait.Scale.X *= MathHelper.Lerp(0f, 1f, (this.flipEase - 0.5f) * 2f);
                }
                if (this.portrait.Visible)
                {
                    this.portrait.DrawOutline(2);
                }
                else if (this.portraitAlt.Visible)
                {
                    this.portraitAlt.DrawOutline(2);
                }
            }
            if (this.joined)
            {
                Vector2 vector2 = Calc.Round(new Vector2((vector.X - ((this.portrait.Width / 2f) * this.portrait.Zoom)) - 2f, (vector.Y - ((this.portrait.Height * this.portrait.Zoom) / 2f)) - 2f));
                Vector2 vector3 = Calc.Round(new Vector2((this.portrait.Width * this.portrait.Zoom) + 4f, (this.portrait.Height * this.portrait.Zoom) + 4f));
                Draw.Rect(vector2.X, vector2.Y, vector3.X, vector3.Y, this.ArcherData.ColorA);
                this.portrait.DrawOutline(1);
            }
            base.Render();
            this.gem.DrawOutline(1, 1);
            this.gem.Render();
            if (this.ShowTitle)
            {
                float x;
                if (this.flipEase < 0.5f)
                {
                    x = this.portraitAlt.Scale.X;
                }
                else
                {
                    x = this.portrait.Scale.X;
                }
                new Vector2((vector.X + this.lastShake.X) - ((float)Math.Ceiling((double)((this.portrait.Width * 0.5f) * x))), (((vector.Y + this.lastShake.Y) + 30f) - 10f) - 1f).Floor();
                new Vector2(this.portrait.Width * x, 21f).Ceiling();
                Color color = this.joined ? this.ArcherData.ColorB : this.ArcherData.ColorA;
                Draw.OutlineTextCentered(TFGame.Font, this.ArcherData.Name0, ((vector + this.lastShake) + new Vector2(1f, 30f)) + new Vector2(0f, -4f), color, new Vector2(this.flipEase, 0.5f));
                Draw.OutlineTextCentered(TFGame.Font, this.ArcherData.Name1, ((vector + this.lastShake) + new Vector2(1f, 30f)) + new Vector2(0f, 4f), color, new Vector2(this.flipEase, 0.5f));
            }

        }
    }

    
}
