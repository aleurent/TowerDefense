// System includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        /// <param name="towers">list of towers read from the xml configuration file</param>
        public Panel(ContentManager content, Vector2 position, IEnumerable<XElement> towers)
            : base(position)
        {
            // Define the texture of the main panel
            _texture = content.Load<Texture2D>("Content\\Graphics\\Panel\\Panel");

            // Define the game management button to manage the start/stop/resume
            Texture2D startTexture = content.Load<Texture2D>("Content\\Graphics\\Panel\\start");
            Vector2 startPosition = new Vector2((Position.X - startTexture.Width) / 2, (_texture.Height - startTexture.Height) / 2);
            _startButton = new GameStarter(content, startTexture, startPosition);

            // Load the font used for the text display in the panel
            _font = content.Load<SpriteFont>("Content\\gameInfo");

            // Define the instance of the game information part
            _gameInfo = new GameInformation(content, _font, new Vector2 (Position.X + 2, 5));

            // Fill the list of tower the user can build
            FillTowerList(content, towers);
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
            text = string.Format("===== Towers Info =====");
            spriteBatch.DrawString(_font, text, new Vector2(_position.X + 10, _texture.Height - 90), Color.White);
            foreach (TowerButton button in _towerButton)
                button.Draw(spriteBatch, _font);
        }

        /// <summary>
        /// Method to fill the list of tower the user can build
        /// </summary>
        /// <param name="content">Instance of the current contentManager</param>
        /// <param name="towers">list of towers read from the xml configuration file</param>
        private void FillTowerList(ContentManager content, IEnumerable<XElement> towers)
        {
            // Initial position of the first tower
            Vector2 position = new Vector2(_position.X + 20, _position.Y + 160);

            // Read the list of towers from the input xml file
            foreach (XElement tower in towers)
            {
                // Define the position depending the previous position and the size of the panel
                if(position.X + 50 >= _position.X + _texture.Width - 20)
                {
                    position.X = _position.X + 20;
                    position.Y = position.Y + 70;
                }

                // Retrieve the texture name of the tower
                string towerTexture = tower.Element("Texture").Value;

                // Create the tower button
                TowerButton newTower = new TowerButton(content, position, towerTexture, Tower.Tower.GenerateToolTip(tower));
                _towerButton.Add(newTower);

                // Update the position for next tower
                position.X = position.X + 70;
            }
        }
    }
}
