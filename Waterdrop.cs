using System;

namespace group_5_assignment7;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Waterdrop
{
    private Texture2D _water;
    private Vector2 _position = Vector2.Zero;
    public float velocity ;
    private Random rnd = new Random();
    
    
    
    public Waterdrop(Texture2D waterdrop, Vector2 pos)
    {
        _water = waterdrop;
        _position = pos;
        velocity = 2f;
    }

    public void Update(int w, float h)
    {
        _position.Y += velocity;

        if (_position.Y > h)
        {
            _position = new Vector2(rnd.Next(0, w - 75), 0);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_water,
            _position,
            null,
            Color.White,
            rotation: 0.0f,
            origin: Vector2.Zero,
            scale: 0.5f,
            SpriteEffects.None,
            layerDepth: 0.0f);
    }
}