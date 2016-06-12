// System includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Xna includes
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game1.UIControls
{
    public class Panel : Sprite
    {
        // -------------- Fields ---------------
        // Button represented the start part
        private GameStarter _startButton;

        // Panel part containing the useful information for the game
        private GameInformation _gameInfo;

        // Panel part containing the list of available tower
        private List<TowerButton> _towerButton = new List<TowerButton>();

        // Font used for the text display
        private SpriteFont _font;

        // -------------- Properties --------------
        // Size of the main Panel
        public int Width { get { return _texture.Width; } }
        public int Height { get { return _texture.Height; } }

        // Delegate to handle the tower button event
        public delegate void TowerButtonCallback(Object sender, string towerName);
        public TowerButtonCallback Clicked = null;

        /// <summary>
        /// Constructor of the main panel of the game
        /// </summary>
        /// <param name="content">The content manager ot retrieve the textures of our button</param>
        /// <param name="position">The current position of my main panel</param>
        public Panel(ContentManager content, Vector2 position)
            : base(position)
        {
            // Define the texture of the main panel
            _texture = content.Load<Texture2D>("Content\\Graphics\\Panel\\Panel");

            // Define the game management button to manage the start/stop/resume
            Texture2D startTexture = content.Load<Texture2D>("Content\\Graphics\\Panel\\start");
            Vector2 startPosition = new Vector2((Position.X + ((_texture.Width - startTexture.Width)/2)), 
                                                (_texture.Height - startTexture.Height));
            _startButton = new GameStarter(content, startTexture, startPosition);

            // Load the font used for the text display in the panel
            _font = content.Load<SpriteFont>("Content\\gameInfo");

            // Define the instance of the game information part
            _gameInfo = new GameInformation(content, _font, new Vector2 (Position.X + 10, 5));

            // Fill the list of tower the user can build
            fillTowerList(content);
        }

        /// <summary>
        /// Method to update the main Panel. It will calls the update of each part of the panel
        /// </summary>
        /// <param name="gametime">The current gametime</param>
        public override void Update(GameTime gametime)
        {
            // Update the parent
            base.Update(gametime);

            // Update the start button
            _startButton.Update(gametime);

            // Update each tower button
            string activatedTowerName = "";
            foreach (TowerButton button in _towerButton)
                button.Update(gametime, ref activatedTowerName);

            // Send an event in case there is a tower button activated
            if (!activatedTowerName.Equals(""))
                Clicked?.Invoke(this, activatedTowerName);
        }

        /// <summary>
        /// Method to draw the main Panel. It will calls the draw of each part of the panel
        /// </summary>
        /// <param name="spriteBatch">A SpriteBatch that has been started</param>
        /// <param name="player">Instance of the current player</param>
        public void Draw(SpriteBatch spriteBatch, Player player)
        {
            // Draw the panel
            base.Draw(spriteBatch);

            // Draw the start button
            _startButton.Draw(spriteBatch);

            // Draw the game information
            _gameInfo.Draw(spriteBatch, player);

            // Draw the list of towers
            string text = string.Format("======= Towers =======");
            spriteBatch.DrawString(_font, text, new Vector2(_position.X + 10, 130), Color.White);
            foreach (TowerButton button in _towerButton)
                button.Draw(spriteBatch, _font);
        }

        /// <summary>
        /// Method to fill the list of tower the user can build
        /// </summary>
        /// <param name="content">Instance of the current contentManager</param>
        private void fillTowerList(ContentManager content)
        {
            // weakest towers
            TowerButton newTower = new TowerButton(content, 
                                                   new Vector2(Position.X + 45, Position.Y + 160), 
                                                   "Content\\Graphics\\Tower\\paratroopa", "ParatroopaTower");
            _towerButton.Add(newTower);
            newTower = new TowerButton(content,
                                       new Vector2(Position.X + 140, Position.Y + 160),
                                       "Content\\Graphics\\Tower\\toad", "ToadTower");
            _towerButton.Add(newTower);

            // Middle strength towers
            newTower = new TowerButton(content,
                                       new Vector2(Position.X + 45, Position.Y + 230),
                                       "Content\\Graphics\\Tower\\bones", "BonesTower");
            _towerButton.Add(newTower);
            newTower = new TowerButton(content,
                                       new Vector2(Position.X + 140, Position.Y + 230),
                                       "Content\\Graphics\\Tower\\marioYoshi", "MarioYoshiTower");
            _towerButton.Add(newTower);

            // Strongest tower
            newTower = new TowerButton(content,
                                       new Vector2(Position.X + 5, Position.Y + 305),
                                       "Content\\Graphics\\Tower\\donkeykong", "DonkeyKongTower");
            _towerButton.Add(newTower);
            newTower = new TowerButton(content,
                                       new Vector2(Position.X + 80, Position.Y + 305),
                                       "Content\\Graphics\\Tower\\browser", "BrowserTower");
            _towerButton.Add(newTower);
            newTower = new TowerButton(content,
                                       new Vector2(Position.X + 160, Position.Y + 305),
                                       "Content\\Graphics\\Tower\\wario", "WarioTower");
            _towerButton.Add(newTower);
        }
    }
}
