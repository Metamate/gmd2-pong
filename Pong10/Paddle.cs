using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong10;

public class Paddle
{
    public const int SPEED = 200;

    public required float X { get; set; }
    public required float Y { get; set; }
    public required int PaddleIndex { get; init; }
    public required int Width { get; init; }
    public required int Height { get; init; }

    public void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (PaddleIndex == 1)
        {
            if (keyboardState.IsKeyDown(Keys.W))
                Y = Math.Max(0, Y - SPEED * deltaTime);
            if (keyboardState.IsKeyDown(Keys.S))
                Y = Math.Min(Game1.VIRTUAL_HEIGHT - 20, Y + SPEED * deltaTime);
        }
        else if (PaddleIndex == 2)
        {
            if (keyboardState.IsKeyDown(Keys.Up))
                Y = Math.Max(0, Y - SPEED * deltaTime);
            if (keyboardState.IsKeyDown(Keys.Down))
                Y = Math.Min(Game1.VIRTUAL_HEIGHT - 20, Y + SPEED * deltaTime);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Game1.Texture, new Rectangle((int)X, (int)Y, Width, Height), Color.White);
    }
}