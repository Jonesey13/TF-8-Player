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
    public class MyPauseMenu : PauseMenu
    {
        public MyPauseMenu(TowerFall.Level level, Vector2 position, MenuType menuType, int controllerDisconnected = -1) : base(level, position, menuType, controllerDisconnected)
        {
        }

        public override void VersusArcherSelect()
        {
            Sounds.ui_clickBack.Play(160f, 1f);
            for (int i = 0; i < 8; i++)
            {
                TFGame.Players[i] = false;
            }
            Engine.Instance.Scene = new MainMenu(MainMenu.MenuState.Rollcall);
            this.level.Session.MatchSettings.LevelSystem.Dispose();
        }
    }
}
