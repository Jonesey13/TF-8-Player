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
    public class MyVariantToggle : VariantToggle
    {
        public MyVariantToggle(TowerFall.Variant variant, bool canActivate) : base(variant, canActivate)
        {
        }

        public override void Render()
        {
            Patcher.Patcher.CallRealBase();
            base.DrawSelection(this.Variant.Value ? VariantItem.ActiveSelection : VariantItem.NormalSelection);
            if (base.Selected)
            {
                bool perPlayer = this.Variant.PerPlayer;
            }
            if (this.Variant.Value && (base.Alpha >= 0.8f))
            {
                if (!base.Selected)
                {
                    Draw.HollowRect(base.X - 10f, base.Y - 10f, 20f, 20f, (Color)(VariantItem.ActiveSelection * base.Alpha));
                }
                if (this.Variant.PerPlayer)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (TFGame.Players[i] && this.Variant[i])
                        {
                            Draw.TextureCentered(TFGame.MenuAtlas["variants/playerBanner"], new Vector2((base.X - 8f) + (5 * i), base.Y + 9f), ArcherData.GetColorA(i, Allegiance.Neutral));
                        }
                    }
                    for (int i = 4; i < 8; i++)
                    {
                        if (TFGame.Players[i] && this.Variant[i])
                        {
                            Draw.TextureCentered(TFGame.MenuAtlas["variants/playerBanner"], new Vector2((base.X - 8f) + (5 * (i - 4)), base.Y + 18f), ArcherData.GetColorA(i, Allegiance.Neutral));
                        }
                    }
                }
            }
            if (this.taggedNew)
            {
                Draw.TextureCentered(TFGame.MenuAtlas["variants/newVariantsTagSmall"], new Vector2(base.X + 6f, base.Y - 6f), (Color)(Color.White * base.Alpha), Math.Max((float)1f, (float)(1f + (0.5f * this.newSine.Value))), 0f);
            }
            base.DrawBubble();
            base.DrawExplain();
        }
    }
}
