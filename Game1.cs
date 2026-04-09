using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace group_5_assignment7;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private Texture2D _fire_boy;
    private Vector2 _fireboy_position;
    
    private Texture2D _waterdrop;
    private List<Waterdrop> drops;
    private Random rnd = new Random();

    private float _timer = 0;
    private bool _paused = false;

    private int _rounds = 1;
    //private float velocity = 2f;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        Window.TextInput += TextInputCallback;
        _fireboy_position = new Vector2(Window.ClientBounds.Width / 2f, 300f);
        drops = new List<Waterdrop>();

        base.Initialize();
    }
    
    private void TextInputCallback(object sender, TextInputEventArgs args)
    {
        
        if (args.Character == 'a' )
        {
            //left
            _fireboy_position.X = Math.Max(0, _fireboy_position.X - 20f);
        } else if (args.Character == 'd' )
        {
            //right
            //fireboy image is about 75 in width, but cant call yet bc hasnt been loaded in
            _fireboy_position.X = Math.Min(Window.ClientBounds.Width - 75f, _fireboy_position.X + 20f);
        } else if (args.Character == 'p' )
        {
            _paused = !_paused;

        } else if (args.Character == 'r' )
        {
            //restart
            Console.WriteLine("Restarting game");
            _timer = 0;
            _rounds = 1;
            _fireboy_position =  new Vector2(Window.ClientBounds.Width / 2f, 300f);
            drops.Clear();
            for (int i = 0; i < 5; i++)
            {
                drops.Add(new Waterdrop(_waterdrop, new Vector2(rnd.Next(0, Window.ClientBounds.Width -75), 0f)));
            }
            
        }

    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        
        _fire_boy = Content.Load<Texture2D>("images/fireboy");
        _waterdrop = Content.Load<Texture2D>("images/water");
        
        for (int i = 0; i < 5; i++)
        {
            drops.Add(new Waterdrop(_waterdrop, new Vector2(rnd.Next(0, Window.ClientBounds.Width - 75), 0f)));
        }
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        if (!_paused)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (Waterdrop w in drops)
            {
                w.Update(Window.ClientBounds.Width, Window.ClientBounds.Height);
            }
        }
        
        
        //NEXT ROUND after 5 seconds
        if (_timer >= 5*_rounds)
        {
            foreach (Waterdrop w in drops)
            {
                w.velocity += 1;
            }
            //included for debugging
            Console.WriteLine("completed round: "+ _rounds + ", new velocity: "+ drops[0].velocity);
            _rounds++;
        }

        base.Update(gameTime);
    }
    

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        
        _spriteBatch.Begin();
        _spriteBatch.Draw(_fire_boy,
            _fireboy_position,
            null,
            Color.White,
            rotation: 0.0f,
            origin: Vector2.Zero,
            scale: 0.05f,
            SpriteEffects.None,
            layerDepth: 0.0f);

        foreach (Waterdrop w in drops)
        {
            w.Draw(_spriteBatch);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}