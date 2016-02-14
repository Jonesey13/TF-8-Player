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
    public class MyPressStartText : PressStartText
    {
        public MyPressStartText(Vector2 position) : base(position)
        {
        }

        public override void Render()
        {
            float scale = 1f - (this.wiggler.Value * 0.3f);
            Draw.OutlineTextCentered(TFGame.Font, "8 PLAYER EDITION", base.Position + new Vector2(30f, -50f), Color.White, Color.Black);
            if (TFGame.PlayerInputs[0] is KeyboardInput)
            {
                PlayerInput input = TFGame.PlayerInputs[0];
                Vector2 vector = base.Position + ((Vector2)(Vector2.UnitX * -55f));
                Vector2 position = base.Position;
                Vector2 vector3 = base.Position + ((Vector2)(Vector2.UnitX * 50f));
                Draw.TextureCentered(input.UpIcon, vector + new Vector2(0f, -7f), Color.White);
                Draw.TextureCentered(input.DownIcon, vector + new Vector2(0f, 8f), Color.White);
                Draw.TextureCentered(input.LeftIcon, vector + new Vector2(-13f, 8f), Color.White);
                Draw.TextureCentered(input.RightIcon, vector + new Vector2(13f, 8f), Color.White);
                Draw.OutlineTextCentered(TFGame.Font, "NAVIGATE", vector + new Vector2(0f, 24f), Color.White, Color.Black);
                Draw.TextureCentered(input.BackIcon, position, Color.White);
                Draw.OutlineTextCentered(TFGame.Font, "CANCEL", position + new Vector2(0f, 14f), Color.White, Color.Black);
                Draw.TextureCentered(input.ConfirmIcon, vector3, Color.White, scale, 0f);
                Draw.OutlineTextCentered(TFGame.Font, "CONFIRM", vector3 + new Vector2(0f, 14f), Color.White, Color.Black, scale);
            }
            else
            {
                float num2 = TFGame.Font.MeasureString("PRESS").X * scale;
                float num3 = MenuButtons.StartGame.Width * scale;
                float num4 = (num2 + 4f) + num3;
                Draw.OutlineTextCentered(TFGame.Font, "PRESS", base.Position + ((Vector2)(Vector2.UnitX * ((-num4 / 2f) + (num2 / 2f)))), Color.White, scale);
                Draw.TextureCentered(MenuButtons.StartGame, base.Position + ((Vector2)(Vector2.UnitX * ((num4 / 2f) - (num3 / 2f)))), Color.White, scale, 0f);
            }
        }
    }
}
