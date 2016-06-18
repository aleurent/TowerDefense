// System includes
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

// Xna includes
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        // ------------ Fields -------------
        GraphicsDeviceManager _graphics = null;
        SpriteBatch _spriteBatch = null;
        Level _level = null;
        Player _player = null;
        UIControls.Panel _mainPanel = null;
        Enemy.WaveManager _waveManager = null;
        Texture2D _winTexture = null;
        Texture2D _looseTexture = null;
        bool _endGame = false;

        /// <summary>
        /// Constructor of the game
        /// </summary>
        public Game1()
        {
            // Instantiate the graphics
            _graphics = new GraphicsDeviceManager(this);
            if (_graphics == null)
                System.Environment.Exit(0);

            // Let the mouse visible on the game
            IsMouseVisible = true;

            // Set the name of the window
            Window.Title = "Mario Tower Defense";

            // Instantiate the waveManager
            _waveManager = Enemy.WaveManager.GetInstance();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Call parent class
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the xml file
            XElement xelement = XElement.Load("Content\\MarioConfig.xml");
            if (xelement == null)
                System.Environment.Exit(0);

            // Read the enemy textures and save them into the waveManager
            foreach (XElement xEnemy in xelement.Descendants("Enemy"))
            {
                if(xEnemy.Element("Texture") != null)
                {
                    var textureName = xEnemy.Element("Texture").Value;
                    var enemyTexture = Content.Load<Texture2D>("Content\\Graphics\\Enemy\\" + textureName);
                    _waveManager.AddEnemyTexture(enemyTexture);
                }
            }

            // Initialise the level of the game
            _level = new Level(Content);

            // Instantiate the player
            IEnumerable<XElement> xTower = xelement.Descendants("Tower");
            Texture2D bulletTexture = Content.Load<Texture2D>("Content\\Graphics\\Tower\\fireball");
            _player = new Player(xTower, Content, bulletTexture, _level);
            _player.LooseGame += EndGame;

            // Init the waveManager
            _waveManager.Initialise(_level, _player);
            _waveManager.WinGame += EndGame;

            // Define the Main Panel of the game
            _mainPanel = new UIControls.Panel(Content, new Vector2(_level.Width * _level.TextureWidth, 0), xTower);
            _mainPanel.Clicked += TowerButtonCallback;

            // Define the size of the window
            _graphics.PreferredBackBufferWidth = _level.Width * _level.TextureWidth + _mainPanel.Width;
            _graphics.PreferredBackBufferHeight = _level.Height * _level.TextureHeight;
            _graphics.ApplyChanges();

            // Load the winner and loose images
            _looseTexture = Content.Load<Texture2D>("Content\\Graphics\\gameover");
            _winTexture = Content.Load<Texture2D>("Content\\Graphics\\winner");
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!_endGame)
            {
                // Update the main Panel
                _mainPanel.Update(gameTime);

                // Update the player
                _player.Update(gameTime, _waveManager.Enemies);

                // Update the wave
                _waveManager.Update(gameTime);

                base.Update(gameTime);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Start drawing
            _spriteBatch.Begin();

            if (!_endGame)
            {
                // Draw the game
                _level.Draw(_spriteBatch);
                _waveManager.Draw(_spriteBatch);
                _player.Draw(_spriteBatch);
                _mainPanel.Draw(_spriteBatch, _player);
            }
            else
            {
                if (_player.CurrentLives <= 0)
                {
                    Vector2 position = new Vector2((_graphics.PreferredBackBufferWidth - _looseTexture.Width) / 2, (_graphics.PreferredBackBufferHeight - _looseTexture.Height) / 2);
                    _spriteBatch.Draw(_looseTexture, position, null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                }
                else
                {
                    Vector2 position = new Vector2((_graphics.PreferredBackBufferWidth - _winTexture.Width) / 2, (_graphics.PreferredBackBufferHeight - _winTexture.Height) / 2);
                    _spriteBatch.Draw(_winTexture, position, null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                }
            }

            // Stop drawing
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Callback when a tower button is pressed
        /// </summary>
        /// <param name="e">The sender of the event</param>
        /// <param name="activatedTowerName">The name of the tower that has been pressed</param>
        private void TowerButtonCallback(Object e, string activatedTowerName)
        {
            _player.TowerType = activatedTowerName;
        }

        /// <summary>
        /// Callback when the game is finished (win or loose)
        /// </summary>
        /// <param name="e">The sender of the event</param>
        /// <param name="argument">The event argument</param>
        private void EndGame(Object e, EventArgs argument)
        {
            _endGame = true;
        }
    }
}
