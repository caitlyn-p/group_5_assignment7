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
    
    //scoring variables
    private int _score = 0;

    private int _maxRounds = 3;
    private bool _gameWon = false;
    private bool _gameLost = false;
    
    //adding lives (can remove if we don't want this)
    private int _lives = 5;
    private int _maxLives = 5;
    
    private SpriteFont _font;

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
            
            //added some scoring reset logic'
            _lives = _maxLives;
            _score = 0;
            _gameWon = false;
            _gameLost = false;

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
        
        //adding font
        _font = Content.Load<SpriteFont>("fonts/font");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
       
        //will stop game if either game won or lost is true
        if (_gameWon || _gameLost)
        {
            return;
        }
        
        if (!_paused)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (Waterdrop w in drops)
            {
                w.Update(Window.ClientBounds.Width, Window.ClientBounds.Height);
                
                // if droplet reaches bottom, fireboy dodged it
                if (w.Position.Y <= 1f)
                {
                    _score++;
                }
                
            }
        }
        
        
        //NEXT ROUND after 15 seconds (**Kavya changed this from 5 to 15 btw)
        if (_timer >= 15*_rounds)
        {
            foreach (Waterdrop w in drops)
            {
                w.velocity += 1;
            }
            //included for debugging
            Console.WriteLine("completed round: "+ _rounds + ", new velocity: "+ drops[0].velocity);
            _rounds++;
        }
        
        if (_rounds > _maxRounds)
        {
            _gameWon = true;
        }
        
        //lose condition (if fireboy hots a water drop)
        Rectangle fireboyBox = new Rectangle(
            (int)_fireboy_position.X,
            (int)_fireboy_position.Y,
            40,
            40
        );

        foreach (Waterdrop w in drops)
        {
            Rectangle dropBox = new Rectangle(
                (int)w.Position.X,
                (int)w.Position.Y,
                30,
                30
            );

            if (fireboyBox.Intersects(dropBox))
            {
                _lives--;

                // reuse same idea as restart
                w.Position = new Vector2(
                    rnd.Next(0, Window.ClientBounds.Width - 75),
                    0f
                );

                if (_lives <= 0)
                {
                    _gameLost = true;
                }
            }
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
        
        _spriteBatch.DrawString(
            _font,
            "Score: " + _score,
            new Vector2(10, 10),
            Color.Black
        );

        _spriteBatch.DrawString(
            _font,
            "Round: " + _rounds,
            new Vector2(10, 40),
            Color.Black
        );
        
        _spriteBatch.DrawString(
            _font,
            "Lives: " + _lives,
            new Vector2(10, 70),
            Color.DarkRed
        );

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}