using Microsoft.Xna.Framework;

namespace MeesGen
{
    partial class MazeGenerator
    {
        private double loopChance = 0;
        private double loopHoleChance = 0;
        private double splitChance = 0;
        private double portalChance = 0;
        private double doorChance = 0;
        private int numKeys = 0;
        private int numRows = 11;
        private int numCols = 11;
        private int portalScore = 10;
        private int doorScore = 50;

        private void SetDifficultyParameters(int difficulty)
        {
            difficulty = MathHelper.Clamp(difficulty, 1, 5);
            switch (difficulty)
            {
                case 1:
                    numCols = 21;
                    numRows = 13;
                    loopChance = 0.08;
                    loopHoleChance = 0.3;
                    splitChance = 0.05;
                    portalChance = 0;
                    doorChance = 0.02;
                    numKeys = 2;
                    break;
                case 2:
                    numCols = 21;
                    numRows = 25;
                    loopChance = 0.03;
                    loopHoleChance = 0.6;
                    splitChance = 0.04;
                    portalChance = 0.01;
                    doorChance = 0.01;
                    numKeys = 3;
                    break;
                case 3:
                    numCols = 31;
                    numRows = 31;
                    loopChance = 0.025;
                    loopHoleChance = 0.7;
                    splitChance = 0.03;
                    portalChance = 0.01;
                    doorChance = 0.03;
                    numKeys = 4;
                    break;
                case 4:
                    numCols = 41;
                    numRows = 41;
                    loopChance = 0.3;
                    loopHoleChance = 0.95;
                    splitChance = 0.03;
                    portalChance = 0.02;
                    doorChance = 0.05;
                    numKeys = 6;
                    break;
                case 5:
                    numCols = 61;
                    numRows = 61;
                    loopChance = 0.005;
                    loopHoleChance = 0;
                    splitChance = 0.03;
                    portalChance = 0.2;
                    doorChance = 0.08;
                    numKeys = 6;
                    break;
            }
        }
    }
}
