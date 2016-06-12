using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

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
        SpriteFont _font;

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

            // Initialise the level of the game
            _level = new Level(Content);

            // Instantiate the player
            _player = new Player(Content, _level);

            // Initialise the waveManager
            _waveManager = Enemy.WaveManager.getInstance();
            _waveManager.initialise(_level, _player, Content);

            // Define the Main Panel of the game
            _mainPanel = new UIControls.Panel(Content, new Vector2(_level.Width * _level.TextureWidth, 0));
            _mainPanel.Clicked += TowerButtonCallback;

            // Define the size of the window
            _graphics.PreferredBackBufferWidth = _level.Width * _level.TextureWidth + _mainPanel.Width;
            _graphics.PreferredBackBufferHeight = _level.Height * _level.TextureHeight;
            _graphics.ApplyChanges();

            // Load font used for the end of the game
            _font = Content.Load<SpriteFont>("Content\\Arial");
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

            if (!_waveManager.GameEnd)
            {
                // Update the wave
                _waveManager.Update(gameTime);

                // Update the player
                _player.Update(gameTime, _waveManager.Enemies);

                // Update the main Panel
                _mainPanel.Update(gameTime);

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

            if (!_waveManager.GameEnd)
            {
                // Draw the game
                _level.Draw(_spriteBatch);
                _waveManager.Draw(_spriteBatch);
                _player.Draw(_spriteBatch);
                _mainPanel.Draw(_spriteBatch, _player);
            }
            else
            {
                string text = string.Format("END OF THE GAME");
                int width = _graphics.PreferredBackBufferWidth / 4;
                int height = _graphics.PreferredBackBufferHeight / 2;
                _spriteBatch.DrawString(_font, text, new Vector2(width, height), Color.White);
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
    }
}
