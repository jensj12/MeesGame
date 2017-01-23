using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class SpriteSheet
{
    protected Texture2D sprite;
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
    /// Creates customizable button
    /// </summary>
    /// <param name="spriteBatch">spritebatch on which to draw</param>
    /// <param name="position">the position the sprite should be drawn at</param>
    /// <param name="origin">location the sprite should consider its reference point for drawing and rotating</param>
    /// <param name="width">width of the image</param>
    /// <param name="height">height of the sprite</param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 origin, int width = -1, int height = -1, Color? drawColor = null)
    {
        Color color = drawColor ?? Color.White;
        int columnIndex = sheetIndex % sheetColumns;
        int rowIndex = sheetIndex / sheetColumns % sheetRows;
        Rectangle spritePart = new Rectangle(columnIndex * Width, rowIndex * Height, Width, Height);
        SpriteEffects spriteEffects = SpriteEffects.None;
        if (mirror)
        {
            spriteEffects = SpriteEffects.FlipHorizontally;
        }
        //checking using -1 to verify that we haven't inserted a size for the sprite
        if (width == -1 || height == -1)
            spriteBatch.Draw(sprite, position, spritePart, color,
                0.0f, origin, 1.0f, spriteEffects, 0.0f);
        else
        {
            Rectangle destinationRectangle = new Rectangle((int)position.X, (int)position.Y, width, height);
            spriteBatch.Draw(sprite, destinationRectangle, spritePart, color);
        }
    }

    public bool IsTranslucent(int x, int y)
    {
        int column_index = sheetIndex % sheetColumns;
        int row_index = sheetIndex / sheetColumns % sheetRows;

        return collisionMask[column_index * Width + x + (row_index * Height + y) * sprite.Width];
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
        get
        { return sheetIndex; }
        set
        {
            if (value < sheetColumns * sheetRows && value >= 0)
            {
                sheetIndex = value;
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