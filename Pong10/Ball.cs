using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong10;

public class Ball
{
    public required Vector2 Position { get; set; }
    public required Vector2 Velocity { get; set; }
    public required int Width { get; set; }
    public required int Height { get; set; }
    static readonly Random random = new();

    public Ball()
    {
        Reset();
    }

    public void Update(GameTime gameTime)
    {
        Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public bool Collides(Paddle paddle)
    {
        if (Position.X > paddle.X + paddle.Width || Position.X + Width < paddle.X)
        {
            return false;
        }

        if (Position.Y > paddle.Y + paddle.Height || Position.Y + Height < paddle.Y)
        {
            return false;
        }

        return true;
    }

    public void Reset()
    {
        Position = new Vector2(Game1.VIRTUAL_WIDTH / 2 - Width / 2, Game1.VIRTUAL_HEIGHT / 2 - Height / 2);
        Velocity = new Vector2(random.Next(2) == 1 ? 100 : -100, random.Next(-50, 51));
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Game1.Texture, new Rectangle((int)Position.X, (int)Position.Y, Width, Height), Color.White);
    }
}