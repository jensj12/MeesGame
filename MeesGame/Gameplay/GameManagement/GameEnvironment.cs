using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class GameEnvironment : Game
{
    public static event EventHandler ScreenChanged;

    protected GraphicsDeviceManager graphics;
    protected SpriteBatch spriteBatch;
    protected InputHelper inputHelper;
    protected Matrix spriteScale;
    protected Point preferredWindowSize;
    protected bool fullScreen;

    protected static Point screen;
    protected static GameStateManager gameStateManager;
    protected static Random random;
    protected static AssetManager assetManager;
    protected static GameSettingsManager gameSettingsManager;
    protected static GameEnvironment instance;

    public GameEnvironment()
    {
        graphics = new GraphicsDeviceManager(this);

        inputHelper = new InputHelper();
        gameStateManager = new GameStateManager();
        spriteScale = Matrix.CreateScale(1, 1, 1);
        random = new Random();
        assetManager = new AssetManager(Content);
        gameSettingsManager = new GameSettingsManager();
        instance = this;

        screen = new Point(1024, 586);
        preferredWindowSize = screen;

        Window.ClientSizeChanged += OnClientSizeChanged;
    }

    public Point PreferedWindowSize
    {
        get { return preferredWindowSize; }
        set { preferredWindowSize = value; }
    }

    public static Point Screen
    {
        get { return screen; }
        set
        {
            Point minScreenSize = new Point(1440, 825);
            screen = (value.X < minScreenSize.X || value.Y < minScreenSize.Y) ? minScreenSize : value;
        }
    }

    public static Random Random
    {
        get { return random; }
    }

    public static AssetManager AssetManager
    {
        get { return assetManager; }
    }

    public static GameStateManager GameStateManager
    {
        get { return gameStateManager; }
    }

    public static GameSettingsManager GameSettingsManager
    {
        get { return gameSettingsManager; }
    }

    public static GameEnvironment Instance
    {
        get { return instance; }
    }

    public bool FullScreen
    {
        get { return Window.IsBorderless; }
        set
        {
            ApplyResolutionSettings(value);
        }
    }

    public void ApplyResolutionSettings(bool fullScreen = false)
    {
        if (fullScreen)
        {
            Screen = new Point(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            graphics.PreferredBackBufferWidth = screen.X;
            graphics.PreferredBackBufferHeight = screen.Y;

            Window.Position = Point.Zero;
        }
        else
        {
            Screen = PreferedWindowSize;
            graphics.PreferredBackBufferWidth = preferredWindowSize.X;
            graphics.PreferredBackBufferHeight = preferredWindowSize.Y;


            Point windowPosition = new Point();
            windowPosition.X = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2 - preferredWindowSize.X / 2;
            windowPosition.Y = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2 - preferredWindowSize.Y / 2;
            Window.Position = windowPosition;
        }

        Window.IsBorderless = fullScreen;


        Viewport viewport = new Viewport();
        viewport.Width = Screen.X;
        viewport.Height = Screen.Y;
        GraphicsDevice.Viewport = viewport;

        graphics.ApplyChanges();

        ScreenChanged?.Invoke(this, null);
    }

    public void OnClientSizeChanged(object sender, EventArgs args)
    {
        if (screen != Point.Zero)
        {
            Vector2 scale = GetScale();

            inputHelper.Scale = scale;
            inputHelper.Offset = new Vector2(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y);

            spriteScale = Matrix.CreateScale(scale.X, scale.Y, 1.0f);
        }
    }

    public Vector2 GetScale()
    {
        float scaleX = (float)Window.ClientBounds.Width / Screen.X;
        float scaleY = (float)Window.ClientBounds.Height / Screen.Y;
        return new Vector2(scaleX, scaleY);
    }

    protected override void LoadContent()
    {
        DrawingHelper.Initialize(this.GraphicsDevice);
        spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected void HandleInput()
    {
        if (IsActive)
        {
            inputHelper.Update();
            if (inputHelper.KeyPressed(Keys.F5))
            {
                FullScreen = !FullScreen;
            }
            gameStateManager.HandleInput(inputHelper);
        }
    }

    protected override void Update(GameTime gameTime)
    {
        HandleInput();
        gameStateManager.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, spriteScale);
        gameStateManager.Draw(gameTime, spriteBatch);
        spriteBatch.End();
    }
}
