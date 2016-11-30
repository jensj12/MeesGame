using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;
using MeesGame;

namespace MeesGame
{
    public class FileExplorer : UIList
    {
        private readonly Color BACKGROUND = Color.Wheat;

        private ContentManager content;

        private int buttonDistance = 10;

        private String fileExtension;
        private Texture2D fileExplorerBackground;

        private ScrollBar scrollBar;

        //returns the selected variable
        private int selected = 0;

        //for now this is one directory, but it should be well doable to allow for a folder structure
        private String currentDirectory;


        public FileExplorer(Vector2 location, Vector2 dimensions, UIObject parent, ContentManager content, String fileExtension, string path) : base(location, dimensions, parent)
        {
            currentDirectory = path;
            this.content = content;
            this.fileExtension = fileExtension;

            generateFileList();
            if (children.Count > 0)
            {
                scrollBar = new ScrollBar(this, ((ListButton)(children[children.Count - 1])).Rectangle.Bottom, MoveDistanceDown);
            }
            else
            {
                scrollBar = new ScrollBar(this, 0, MoveDistanceDown);
            }
        }

        public void MoveDistanceDown(int distance)
        {
            elementsOffset = distance;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            scrollBar.Draw(gameTime, spriteBatch);
        }

        public override void DrawSelf(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (fileExplorerBackground == null)
            {
                Color[] colordata = new Color[1];
                colordata[0] = BACKGROUND;
                fileExplorerBackground = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                fileExplorerBackground.SetData(colordata);
            }

            spriteBatch.Draw(fileExplorerBackground, Rectangle, Color.White);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (Rectangle.Contains(inputHelper.MousePosition))
            {
                if (scrollBar.BarRectangle.Contains(inputHelper.MousePosition) || scrollBar.BeingDragged)
                {
                    scrollBar.HandleInput(inputHelper);
                }
                else
                    children.HandleInput(inputHelper);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            scrollBar.Update(gameTime);
        }

        public void generateFileList()
        {
            children.Reset();
            String[] tmpFileList = Directory.GetFiles(currentDirectory);
            int index = 0;
            for (int i = 0; i < tmpFileList.Length; i++)
            {
                if (tmpFileList[i].EndsWith("." + fileExtension))
                {
                    if (i == 0)
                    {
                        children.Add(new ListButton(new Vector2(0, 0), Dimensions, this, content, tmpFileList[i].Substring(currentDirectory.Length + 1, tmpFileList[i].Length - currentDirectory.Length - fileExtension.Length - 2), index, ItemSelect));
                    }
                    else
                    {
                        children.Add(new ListButton(new Vector2(0, buttonDistance + ((ListButton)children[index - 1]).Rectangle.Height + (int)((ListButton)children[index - 1]).RelativeLocation.Y), Dimensions, this, content, tmpFileList[i].Substring(currentDirectory.Length + 1, tmpFileList[i].Length - currentDirectory.Length - fileExtension.Length - 2), index, ItemSelect));
                    }
                    index++;
                }
            }
        }

        public void ItemSelect(Object o)
        {
            ListButton listbutton = (ListButton)o;
            ((ListButton)children[selected]).Selected = false;
            listbutton.Selected = true;
            selected = listbutton.Index;
        }
    }
}
