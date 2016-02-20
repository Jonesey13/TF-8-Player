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
    public class MyVariantPerPlayer : VariantPerPlayer
    {
        public new string[] PlayerTitles2 = new string[] { "P1", "P2", "P3", "P4", "P5", "P6", "P7", "P8" };
        public new Vector2[] PlayerPositions2 = new Vector2[] { Vector2.UnitX * -70f, Vector2.UnitX * -50f, Vector2.UnitX * -30f, Vector2.UnitX * -10f,
                                                           Vector2.UnitX * 10f, Vector2.UnitX * 30f, Vector2.UnitX * 50f, Vector2.UnitX * 70f};

        public MyVariantPerPlayer(VariantToggle toggle, Vector2 position) : base(toggle, position)
        {
            base.Depth = -100;
            this.toggle = toggle;
            this.variant = toggle.Variant;
            this.tweenTo = base.Position;
            this.tweenFrom = base.Position + new Vector2(320f, 0f);
            this.selection = 0;
            for (int i = 0; i < 8; i++)
            {
                if (TFGame.Players[i])
                {
                    this.selection = i;
                    break;
                }
            }
            this.wigglers = new Wiggler[8];
            for (int j = 0; j < 8; j++)
            {
                if (TFGame.Players[j])
                {
                    this.wigglers[j] = Wiggler.Create(20, 5f, null, null, false, false);
                    base.Add(this.wigglers[j]);
                }
            }
            this.cursor = this.GetCursorTarget();
            this.cursorSine = new SineWave(60);
            base.Add(this.cursorSine);
        }

        public override void Render()
        {
            Patcher.Patcher.CallRealBase();
            MenuPanel.DrawPanel(base.X - 80f, base.Y - 23f, 160f, 46f);
            Draw.TextCentered(TFGame.Font, this.variant.Title, base.Position + ((Vector2)(Vector2.UnitY * -8f)), Color.White);
            for (int i = 0; i < 8; i++)
            {
                if (TFGame.Players[i])
                {
                    Color color = (this.selection == i) ? ArcherData.GetColorB(i, Allegiance.Neutral) : ArcherData.GetColorA(i, Allegiance.Neutral);
                    float num2 = (this.variant[i] ? 2f : 1f) + (this.wigglers[i].Value * 0.3f);
                    Draw.TextCentered(TFGame.Font, this.PlayerTitles2[i], (base.Position + this.PlayerPositions2[i]) + new Vector2(0f, 7f), color, (Vector2)(Vector2.One * num2), 0f);
                }
            }
            Vector2 position = (base.Position + this.cursor) + ((Vector2)(Vector2.UnitY * (30f - (4f * this.cursorSine.Value))));
            Color colorB = ArcherData.GetColorB(this.selection, Allegiance.Neutral);
            Vector2 scale = new Vector2(1f, -1f + (0.2f * this.cursorSine.Value));
            Draw.OutlineTextureJustify(TFGame.MenuAtlas["variantPerPlayerArrow"], position, colorB, scale, new Vector2(0.5f, 1f));
        }

        public override void Update()
        {
            Patcher.Patcher.CallRealBase();
            if (base.Selected)
            {
                if (MenuInput.Left)
                {
                    int index = this.selection - 1;
                    while ((index >= 0) && !TFGame.Players[index])
                    {
                        index--;
                    }
                    if ((index != this.selection) && (index >= 0))
                    {
                        this.selection = index;
                        Sounds.ui_move1.Play(160f, 1f);
                        this.HandleCursorChange();
                    }
                }
                else if (MenuInput.Right)
                {
                    int num2 = this.selection + 1;
                    while ((num2 < 8) && !TFGame.Players[num2])
                    {
                        num2++;
                    }
                    if ((num2 != this.selection) && (num2 < 8))
                    {
                        this.selection = num2;
                        Sounds.ui_move1.Play(160f, 1f);
                        this.HandleCursorChange();
                    }
                }
                if (MenuInput.Confirm)
                {
                    if (this.variant[this.selection])
                    {
                        this.variant[this.selection] = false;
                        this.wigglers[this.selection].Start();
                        Sounds.ui_subclickOff.Play(160f, 1f);
                    }
                    else
                    {
                        this.variant[this.selection] = true;
                        this.wigglers[this.selection].Start();
                        Sounds.ui_subclickOn.Play(160f, 1f);
                    }
                }
                if (MenuInput.Back || MenuInput.Alt)
                {
                    this.TweenOut();
                    Sounds.ui_clickBack.Play(160f, 1f);
                }
            }
        }

        public new Vector2 GetCursorTarget() =>(PlayerPositions2[this.selection] + new Vector2(0f, -12f));
    }
}
