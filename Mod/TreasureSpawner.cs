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
    public class MyTreasureSpawner : TreasureSpawner
    {
        private const float BIG_CHEST_CHANCE = 0.03f;
        public static readonly float[][] ChestChances = new float[5][];
        public static readonly bool[] DarkWorldTreasures;
        public const float DEFAULT_ARROW_CHANCE = 0.6f;
        public static readonly float[] DefaultTreasureChances = new float[20];
        public static readonly int[] FullTreasureMask = new int[20];

        static MyTreasureSpawner()
        {
            float[] numArray = new float[] { 0.9f, 0.9f, 0.2f, 0.1f };
            float[] numArray2 = new float[] { 0.9f, 0.9f, 0.8f, 0.2f, 0.1f };
            float[] numArray3 = new float[] { 0.9f, 0.9f, 0.6f, 0.8f, 0.2f, 0.1f };
            float[] numArray4 = new float[] { 0.9f, 0.9f, 0.9f, 0.6f, 0.8f, 0.2f, 0.1f };
            float[] numArray5 = new float[] { 0.9f, 0.9f, 0.9f, 0.9f, 0.6f, 0.8f, 0.2f, 0.1f };
            ChestChances[0] = numArray;
            ChestChances[1] = numArray2;
            ChestChances[2] = numArray3;
            ChestChances[3] = numArray4;
            ChestChances[4] = numArray5;
            DefaultTreasureChances = new float[] {
                1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 0.5f, 0.5f, 0.5f, 0.25f, 0.15f, 0.15f,
                0.15f, 0.15f, 0.001f, 0.1f
             };
            FullTreasureMask = new int[] {
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                1, 1, 1, 1
             };
            bool[] flagArray = new bool[20];
            flagArray[8] = true;
            flagArray[9] = true;
            DarkWorldTreasures = flagArray;
        }

        public MyTreasureSpawner(TowerFall.Session session, int[] mask, float arrowChance, bool arrowShuffle) : base(session, mask, arrowChance, arrowShuffle)
        {

        }

        public override bool CanSpawnAnotherChest(int alreadySpawnedAmount)
        {
            if (alreadySpawnedAmount >= ChestChances[Math.Min((TFGame.PlayerAmount - 2),  4)].Length)
            {
                return false;
            }
            if (this.Session.MatchSettings.Variants.MaxTreasure == null)
            {
                return this.Random.Chance(ChestChances[Math.Min((TFGame.PlayerAmount - 2), 4)][alreadySpawnedAmount]);
            }
            return true;
        }
    }
}
