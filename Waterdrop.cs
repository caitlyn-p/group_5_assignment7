namespace group_5_assignment7;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Waterdrop
{
    private Texture2D _water;
    private Vector2 _position = Vector2.Zero;
    private float _velocity ;
    
    public bool isAlive = true;
    
    
    
    public Waterdrop(Texture2D waterdrop, Vector2 pos, float v)
    {
        _water = waterdrop;
        _position = pos;
        _velocity = v;
        
        //Random rnd = new Random();
        _velocity = v;
        //rnd.Next(1, 50)/10f;
    }

    public void Update(float h)
    {
        //_velocity += acceleration;
        _position.Y += _velocity;

        if (_position.Y > h)
        {
            isAlive = false;
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