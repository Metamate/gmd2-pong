using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong2;

public class Game1 : Game
{
    private const int WINDOW_WIDTH = 1280;
    private const int WINDOW_HEIGHT = 720;
    private const int VIRTUAL_WIDTH = 432;
    private const int VIRTUAL_HEIGHT = 243;
    private Matrix _screenScaleMatrix;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteFont _font;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = WINDOW_WIDTH,
            PreferredBackBufferHeight = WINDOW_HEIGHT
        };
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += (sender, e) => UpdateScreenScaleMatrix();
    }

    protected override void Initialize()
    {
        base.Initialize();
        UpdateScreenScaleMatrix();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _font = Content.Load<SpriteFont>("arial");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        string output = "Hello Pong!";
        Vector2 fontSize = _font.MeasureString(output);
        Vector2 position = new((float)VIRTUAL_WIDTH / 2 - fontSize.X / 2, (float)VIRTUAL_HEIGHT / 2 - fontSize.Y / 2);

        _spriteBatch.Begin(transformMatrix: _screenScaleMatrix, samplerState: SamplerState.PointClamp);
        _spriteBatch.DrawString(_font, output, position, Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void UpdateScreenScaleMatrix()
    {
        float screenWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
        float screenHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;
        float currentWidth, currentHeight;

        if (screenWidth / VIRTUAL_WIDTH > screenHeight / VIRTUAL_HEIGHT)
        {
            float aspect = screenHeight / VIRTUAL_HEIGHT;
            currentWidth = aspect * VIRTUAL_WIDTH;
            currentHeight = screenHeight;
        }
        else
        {
            float aspect = screenWidth / VIRTUAL_WIDTH;
            currentWidth = screenWidth;
            currentHeight = aspect * VIRTUAL_HEIGHT;
        }

        _screenScaleMatrix = Matrix.CreateScale(currentWidth / VIRTUAL_WIDTH, currentHeight / VIRTUAL_HEIGHT, 1);

        GraphicsDevice.Viewport = new()
        {
            X = (int)(screenWidth / 2 - currentWidth / 2),
            Y = (int)(screenHeight / 2 - currentHeight / 2),
            Width = (int)currentWidth,
            Height = (int)currentHeight,
            MinDepth = 0,
            MaxDepth = 1,
        };
    }
}
