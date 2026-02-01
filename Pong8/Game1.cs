using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong8;

public class Game1 : Game
{
    private const int WINDOW_WIDTH = 1280;
    private const int WINDOW_HEIGHT = 720;
    private Matrix _screenScaleMatrix;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteFont _font;
    private SpriteFont _fontBig;
    private KeyboardState _oldKeyboardState;
    private Paddle _paddle1;
    private Paddle _paddle2;
    private Ball _ball;
    private string _gameState;
    private readonly Random _random = new();
    private int _player1Score;
    private int _player2Score;

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
        _fontBig = Content.Load<SpriteFont>("font-big");
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        UpdateInput();

        if (_gameState == "play")
        {
            if (_ball.Collides(_paddle1))
            {
                _ball.Velocity = new(-_ball.Velocity.X * 1.1f, _ball.Velocity.Y);
                _ball.Position = new(_paddle1.X + _paddle1.Width, _ball.Position.Y);

                float newY = _ball.Velocity.Y < 0 ?
                    -_random.Next(80, 100) :
                    _random.Next(80, 100);

                _ball.Velocity = new(_ball.Velocity.X, newY);
            }

            if (_ball.Collides(_paddle2))
            {
                _ball.Velocity = new(-_ball.Velocity.X * 1.1f, _ball.Velocity.Y);
                _ball.Position = new(_paddle2.X - _ball.Width, _ball.Position.Y);

                float newY = _ball.Velocity.Y < 0 ?
                    -_random.Next(80, 100) :
                    _random.Next(80, 100);

                _ball.Velocity = new(_ball.Velocity.X, newY);
            }

            if (_ball.Position.Y <= 0)
            {
                _ball.Position = new(_ball.Position.X, 0);
                _ball.Velocity = new(_ball.Velocity.X, -_ball.Velocity.Y);
            }

            if (_ball.Position.Y >= VIRTUAL_HEIGHT - _ball.Height)
            {
                _ball.Position = new(_ball.Position.X, VIRTUAL_HEIGHT - _ball.Height);
                _ball.Velocity = new(_ball.Velocity.X, -_ball.Velocity.Y);
            }

            if (_ball.Position.X + _ball.Width < 0)
            {
                _player2Score++;
                _ball.Reset();
                _gameState = "start";
            }

            if (_ball.Position.X > VIRTUAL_WIDTH)
            {
                _player1Score++;
                _ball.Reset();
                _gameState = "start";
            }

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

        string output = _gameState == "start" ? "Start State!" : "Play State!";

        Vector2 fontSize = _font.MeasureString(output);
        Vector2 position = new((float)VIRTUAL_WIDTH / 2 - fontSize.X / 2, (float)VIRTUAL_HEIGHT / 4 - fontSize.Y / 2);

        _spriteBatch.Begin(transformMatrix: _screenScaleMatrix, samplerState: SamplerState.PointClamp);
        _spriteBatch.DrawString(_font, output, position, Color.White);
        _spriteBatch.DrawString(_fontBig, _player1Score.ToString(), new Vector2(VIRTUAL_WIDTH / 2 - 50, VIRTUAL_HEIGHT / 3), Color.White);
        _spriteBatch.DrawString(_fontBig, _player2Score.ToString(), new Vector2(VIRTUAL_WIDTH / 2 + 30, VIRTUAL_HEIGHT / 3), Color.White);

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
