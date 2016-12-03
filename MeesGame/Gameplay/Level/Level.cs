﻿using Microsoft.Xna.Framework;
using System;

namespace MeesGame
{
    class Level : GameObjectList
    {
        protected int numRows = 12;
        protected int numColumns = 22;
        protected const int CELL_HEIGHT = 64;
        protected const int CELL_WIDTH = 64;
        protected TimeSpan timeBetweenActions = TimeSpan.FromMilliseconds(200);
        protected Point start;
        protected Player player;
        protected TileFieldView tiles;
        public Level(int levelindex = 0)
        {
            start = new Point(1, 1);
            
            TileField tileField = new TileField(numRows, numColumns, 0, "tiles");
            tileField.CellHeight = CELL_HEIGHT;
            tileField.CellWidth = CELL_WIDTH;

            //Temporary initialisation of empty tiles
            for (int x = 0; x < numColumns; x++)
            {
                for (int y = 0; y < numRows; y++)
                {
                    tileField.Add(new Tile("floorTile"), x, y);
                }
            }

            this.player = new Player(this,tileField,start);
            this.tiles = new TileFieldView(player, tileField);
            Add(this.tiles);
            Add(this.player);
        }

        public TileFieldView Tiles
        {
            get
            {
                return tiles;
            }
        }

        public TimeSpan TimeBetweenActions
        {
            get
            {
                return timeBetweenActions;
            }
        }
    }
}
