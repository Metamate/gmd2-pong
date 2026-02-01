using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong6;

public class Game1 : Game
{
    private const int WINDOW_WIDTH = 1280;
    private const int WINDOW_HEIGHT = 720;
    private Matrix _screenScaleMatrix;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteFont _font;
    private KeyboardState _oldKeyboardState;
    private Paddle _paddle1;
    private Paddle _paddle2;
    private Ball _ball;
    private string _gameState;

    public const int VIRTUAL_WIDTH = 432;
    public const int VIRTUAL_HEIGHT = 243;
    public static Texture2D Texture { get; private set; }

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
        Texture = new(GraphicsDevice, 1, 1);
        Texture.SetData([Color.White]);
        _oldKeyboardState = Keyboard.GetState();
        _paddle1 = new()
        {
            X = 5,
            Y = 30,
            PaddleIndex = 1,
            Width = 5,
            Height = 20,
        };
        _paddle2 = new()
        {
            X = VIRTUAL_WIDTH - 10,
            Y = VIRTUAL_HEIGHT - 50,
            PaddleIndex = 2,
            Width = 5,
            Height = 20,
        };
        Random random = new();
        _ball = new()
        {
            Position = new(VIRTUAL_WIDTH / 2 - 2, VIRTUAL_HEIGHT / 2 - 2),
            Velocity = new(random.Next(2) == 1 ? 100 : -100, random.Next(-50, 51)),
            Width = 4,
            Height = 4,
        };
        _gameState = "start";
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _font = Content.Load<SpriteFont>("font");
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        UpdateInput();

        if (_gameState == "play")
        {
            _ball.Update(gameTime);
        }

        _paddle1.Update(gameTime);
        _paddle2.Update(gameTime);

        base.Update(gameTime);
    }

    private void UpdateInput()
    {
        var keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.Enter) && !_oldKeyboardState.IsKeyDown(Keys.Enter))
        {
            if (_gameState == "start")
            {
                _gameState = "play";
            }
            else
            {
                _gameState = "start";
                _ball.Reset();
            }
        }

        _oldKeyboardState = keyboardState;
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new(40, 45, 52, 255));
        string output;

        if (_gameState == "start")
        {
            output = "Start State!";
        }
        else
        {
            output = "Play State!";
        }

        Vector2 fontSize = _font.MeasureString(output);
        Vector2 position = new((float)VIRTUAL_WIDTH / 2 - fontSize.X / 2, (float)VIRTUAL_HEIGHT / 4 - fontSize.Y / 2);

        _spriteBatch.Begin(transformMatrix: _screenScaleMatrix, samplerState: SamplerState.PointClamp);
        _spriteBatch.DrawString(_font, output, position, Color.White);
        _paddle1.Draw(_spriteBatch);
        _paddle2.Draw(_spriteBatch);
        _ball.Draw(_spriteBatch);
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
