using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong5;

public class Game1 : Game
{
    private const int WINDOW_WIDTH = 1280;
    private const int WINDOW_HEIGHT = 720;
    private const int VIRTUAL_WIDTH = 432;
    private const int VIRTUAL_HEIGHT = 243;
    private const int PADDLE_SPEED = 200;
    private Matrix _screenScaleMatrix;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteFont _font;
    private Texture2D _texture;
    private float _player1Y = 30;
    private float _player2Y = VIRTUAL_HEIGHT - 50;
    private KeyboardState _oldKeyboardState;
    private Vector2 _ballPosition = new(VIRTUAL_WIDTH / 2 - 2, VIRTUAL_HEIGHT / 2 - 2);
    private Vector2 _ballVelocity = new();
    private string _gameState;

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
        _texture = new(GraphicsDevice, 1, 1);
        _texture.SetData([Color.White]);
        _oldKeyboardState = Keyboard.GetState();
        Random random = new();
        _ballVelocity = new(random.Next(2) == 1 ? 100 : -100, random.Next(-50, 51));
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

        UpdateInput(gameTime);

        if (_gameState == "play")
        {
            _ballPosition += _ballVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        base.Update(gameTime);
    }

    private void UpdateInput(GameTime gameTime)
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

                _ballPosition = new(VIRTUAL_WIDTH / 2 - 2, VIRTUAL_HEIGHT / 2 - 2);
                Random random = new();
                _ballVelocity = new(random.Next(2) == 1 ? 100 : -100, random.Next(-50, 51));
            }
        }

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (keyboardState.IsKeyDown(Keys.W))
            _player1Y = Math.Max(0, _player1Y - PADDLE_SPEED * deltaTime);
        if (keyboardState.IsKeyDown(Keys.S))
            _player1Y = Math.Min(VIRTUAL_HEIGHT - 20, _player1Y + PADDLE_SPEED * deltaTime);

        if (keyboardState.IsKeyDown(Keys.Up))
            _player2Y = Math.Max(0, _player2Y - PADDLE_SPEED * deltaTime);
        if (keyboardState.IsKeyDown(Keys.Down))
            _player2Y = Math.Min(VIRTUAL_HEIGHT - 20, _player2Y + PADDLE_SPEED * deltaTime);

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
        _spriteBatch.Draw(_texture, new Rectangle(5, (int)_player1Y, 5, 20), Color.White);
        _spriteBatch.Draw(_texture, new Rectangle(VIRTUAL_WIDTH - 10, (int)_player2Y, 5, 20), Color.White);
        _spriteBatch.Draw(_texture, new Rectangle((int)_ballPosition.X, (int)_ballPosition.Y, 4, 4), Color.White);
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
