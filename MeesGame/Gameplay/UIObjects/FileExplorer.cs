using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace MeesGame
{
    class FileExplorer : GameObject
    {
        private readonly Color BACKGROUND = Color.Wheat;

        private int buttonDistance;

        private GameObjectList ButtonsList;

        private ContentManager content;

        private Rectangle rectangle;
        private String fileExtension;
        private Texture2D fileExplorerBackground;

        //we store a texture in order to not constantly render the same image;
        private RenderTarget2D currentTexture;

        private TextureGenerator boxTextureGenerator;

        //returns the selected variable
        private int selected = 0;

        //for now this is one directory, but it should be well doable to allow for a folder structure
        private String currentDirectory;


        public FileExplorer(ContentManager content, Rectangle rectangle, String fileExtension, string path, int buttonDistance = 10)
        {
            ButtonsList = new GameObjectList();

            currentDirectory = path;

            this.buttonDistance = buttonDistance;
            this.content = content;
            this.rectangle = rectangle;
            this.fileExtension = fileExtension;
            fileExplorerBackground = content.Load<Texture2D>("floor");

            generateFileList();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            if (currentTexture == null)
            {
                if (boxTextureGenerator == null)
                {
                    boxTextureGenerator = new TextureGenerator(spriteBatch.GraphicsDevice, rectangle.Width, rectangle.Height, BACKGROUND);
                }
                currentTexture = boxTextureGenerator.Render(gameTime, ButtonsList.Draw);
            }
            spriteBatch.Draw(currentTexture, rectangle, Color.White);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);
            ButtonsList.HandleInput(inputHelper);
            currentTexture = null;
        }

        public void generateFileList()
        {
            ButtonsList.Reset();
            String[] tmpFileList = Directory.GetFiles(currentDirectory);
            int index = 0;
            for (int i = 0; i < tmpFileList.Length; i++)
            {
                if (tmpFileList[i].EndsWith("." + fileExtension))
                {
                    if (ButtonsList.Children.Count == 0)
                    {
                        ButtonsList.Add(new ListButton(content, tmpFileList[i].Substring(currentDirectory.Length + 1, tmpFileList[i].Length - currentDirectory.Length - fileExtension.Length - 2), new Vector2(0, 0), rectangle.Location.ToVector2(), rectangle.Width, index, ItemSelect));
                    }
                    else
                    {
                        ButtonsList.Add(new ListButton(content, tmpFileList[i].Substring(currentDirectory.Length + 1, tmpFileList[i].Length - currentDirectory.Length - fileExtension.Length - 2), new Vector2(0, buttonDistance + ((ListButton)ButtonsList.Children[index - 1]).Rectangle.Bottom), rectangle.Location.ToVector2(), rectangle.Width, index, ItemSelect));
                    }
                    index++;
                }
            }
        }

        public void ItemSelect(Object o)
        {
            ListButton listbutton = (ListButton)o;
            ((ListButton)ButtonsList.Children[selected]).Selected = false;
            listbutton.Selected = true;
            selected = listbutton.Index;
        }
    }
}
