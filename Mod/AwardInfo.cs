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
    /// <summary>
    /// Not currently working, mayber because it involves generics
    /// </summary>
    public class MyAwardInfo : AwardInfo
    {
        public override Sprite<int> GetSprite(bool locked)
        {
            Sprite<int> sprite;
            if (locked)
            {
                Subtexture subTexture = TFGame.MenuAtlas["awards/locked"];
                sprite = new Sprite<int>(subTexture, subTexture.Width, subTexture.Height, 0);
                sprite.Add(0, 0);
            }
            else
            {
                string dataName = this.DataName;
                if (TFGame.MenuSpriteData.Contains(dataName))
                {
                    sprite = TFGame.MenuSpriteData.GetSpriteInt(dataName);
                }
                else
                {
                    Subtexture subtexture2 = TFGame.MenuAtlas["awards/" + dataName];
                    sprite = new Sprite<int>(subtexture2, subtexture2.Width, subtexture2.Height, 0);
                    sprite.Add(0, 0);
                }
            }
            sprite.CenterOrigin();
            sprite.Zoom = 0.7f;
            sprite.Play(0, false);
            int anim = 0;
            sprite.OnAnimationComplete = delegate (Sprite<int> s) {
                if (!sprite.Looping && (anim < (sprite.TotalAnimations - 1)))
                {
                    sprite.Play(++anim, false);
                }
            };
            return sprite;
        }
    }
}
