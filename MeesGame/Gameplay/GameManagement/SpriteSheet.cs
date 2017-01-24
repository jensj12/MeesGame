using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class SpriteSheet
{
    protected Texture2D sprite;

    /// <summary>
    /// Part of the texture that should be rendered according to the sheetindex.
    /// </summary>
    RenderTarget2D spritePart;

    protected bool[] collisionMask;
    protected int sheetIndex;
    protected int sheetColumns;
    protected int sheetRows;
    protected bool mirror;

    public SpriteSheet(string assetname, int sheetIndex = 0)
    {
        // retrieve the sprite
        sprite = GameEnvironment.AssetManager.GetSprite(assetname);

        // construct the collision mask
        Color[] colorData = new Color[sprite.Width * sprite.Height];
        collisionMask = new bool[sprite.Width * sprite.Height];
        sprite.GetData(colorData);
        for (int i = 0; i < colorData.Length; ++i)
        {
            collisionMask[i] = colorData[i].A != 0;
        }

        this.sheetIndex = sheetIndex;
        sheetColumns = 1;
        sheetRows = 1;

        // see if we can extract the number of sheet elements from the asset-name
        string[] assetSplit = assetname.Split('@');
        if (assetSplit.Length <= 1)
        {
            return;
        }

        string sheetNrData = assetSplit[assetSplit.Length - 1];
        string[] colRow = sheetNrData.Split('x');
        sheetColumns = int.Parse(colRow[0]);
        if (colRow.Length == 2)
        {
            sheetRows = int.Parse(colRow[1]);
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="spriteBatch">spritebatch on which to draw</param>
    /// <param name="position">the position the sprite should be drawn at</param>
    /// <param name="origin">location the sprite should consider its reference point for drawing and rotating</param>
    /// <param name="width">width of the image</param>
    /// <param name="height">height of the sprite</param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 origin, Point? size = null, Color? drawColor = null, bool tiled = false)
    {
        Color color = drawColor ?? Color.White;
        SpriteEffects spriteEffects = (mirror) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
     
        //Draws to the native size of the texture if no size was specified. 
        if (size == null)
            spriteBatch.Draw(sprite, position, SpritePartRectangle, color,
                0.0f, origin, 1.0f, spriteEffects, 0.0f);
        //If a size was specified, stretches or tiles depending on the tiled parameter
        else
        {
            Rectangle destinationRectangle = new Rectangle(position.ToPoint(), (Point)size);

            if (tiled)
                spriteBatch.Draw(GetSpritePart(spriteBatch), destinationRectangle, destinationRectangle, color, 0.0f, origin, spriteEffects, 0.0f);
            else
            {
                spriteBatch.Draw(sprite, destinationRectangle, SpritePartRectangle, color, 0.0f, origin, spriteEffects, 0.0f);
            }
        }
    }

    public bool IsTranslucent(int x, int y)
    {
        int column_index = sheetIndex % sheetColumns;
        int row_index = sheetIndex / sheetColumns % sheetRows;

        return collisionMask[column_index * Width + x + (row_index * Height + y) * sprite.Width];
    }

    /// <summary>
    /// Rectangle containing the part of the sprite that should be rendered.
    /// </summary>
    private Rectangle SpritePartRectangle
    {
        get
        {
            int columnIndex = sheetIndex % sheetColumns;
            int rowIndex = sheetIndex / sheetColumns % sheetRows;
            return new Rectangle(columnIndex * Width, rowIndex * Height, Width, Height);
        }
    }

    /// <summary>
    /// Renders the tiledRectangle when needed
    /// </summary>
    /// <param name="spriteBatch"></param>
    private void RenderTiledTexture(SpriteBatch spriteBatch)
    {
        if (spritePart == null)
            spritePart = new RenderTarget2D(spriteBatch.GraphicsDevice, SpritePartRectangle.Width, Sprite.Height);

        spriteBatch.End();
        RenderTargetBinding[] renderTargets = spriteBatch.GraphicsDevice.GetRenderTargets();
        spriteBatch.GraphicsDevice.SetRenderTarget(spritePart);
        spriteBatch.GraphicsDevice.Clear(Color.Transparent);
        spriteBatch.Begin();
        Draw(spriteBatch, Vector2.Zero, Vector2.Zero);
        spriteBatch.End();
        spriteBatch.GraphicsDevice.SetRenderTargets(renderTargets);
        spriteBatch.Begin();
    }

    /// <summary>
    /// Returns the part of the texture that should be rendered according to the sheetindex.
    /// </summary>
    public Texture2D GetSpritePart(SpriteBatch spriteBatch)
    {
        //A spritepart is only rendered if the sprite containes spriteparts.
        if (sheetColumns == 1 && sheetRows == 1)
            return sprite;
        if (spritePart == null || spritePart.IsDisposed)
        {
            RenderTiledTexture(spriteBatch);
        }
        return spritePart;
    }

    public Texture2D Sprite
    {
        get { return sprite; }
    }

    public Vector2 Center
    {
        get { return new Vector2(Width, Height) / 2; }
    }

    public int Width
    {
        get
        { return sprite.Width / sheetColumns; }
    }

    public int Height
    {
        get
        { return sprite.Height / sheetRows; }
    }

    public bool Mirror
    {
        get { return mirror; }
        set { mirror = value; }
    }

    public int SheetRowIndex
    {
        get
        {
            return sheetIndex / NumberColumns;
        }
        set
        {
            sheetIndex = value * NumberColumns + SheetColIndex;
        }
    }

    public int SheetColIndex
    {
        get
        {
            return sheetIndex % NumberColumns;
        }
        set
        {
            sheetIndex = SheetRowIndex * NumberColumns + value % NumberColumns;
        }
    }

    public int SheetIndex
    {
        get { return sheetIndex; }
        set
        {
            if (value < sheetColumns * sheetRows && value >= 0 && value != sheetIndex)
            {
                sheetIndex = value;
                spritePart?.Dispose();
            }
        }
    }

    public int NumberColumns
    {
        get { return sheetColumns; }
    }

    public int NumberRows
    {
        get { return sheetRows; }
    }

    public int NumberSheetElements
    {
        get { return sheetColumns * sheetRows; }
    }
}