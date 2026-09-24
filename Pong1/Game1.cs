using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong1;

public class Game1 : Game
{
    private const int WINDOW_WIDTH = 1280;
    private const int WINDOW_HEIGHT = 720;
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
    }

    protected override void Initialize()
    {
        base.Initialize();
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
        Vector2 position = new((float)WINDOW_WIDTH / 2 - fontSize.X / 2, (float)WINDOW_HEIGHT / 2 - fontSize.Y / 2);

        _spriteBatch.Begin();
        _spriteBatch.DrawString(_font, output, position, Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
