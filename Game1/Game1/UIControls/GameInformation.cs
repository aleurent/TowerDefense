// System includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Xna includes
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Game1.UIControls
{
    public class GameInformation
    {
        //-------------- Fields --------------
        // Font used for the text display
        private SpriteFont _font;

        // Texture used for the display
        private Texture2D _moneyTexture;
        private Texture2D _livesTexture;
        private Texture2D _waveNumberTexture;

        // Position of the text
        private Vector2 _position;

        // Font used for the display of wave information
        private SpriteFont _waveInfoFont;

        /// <summary>
        /// Constructor of the game information display (lives, money, wave number)
        /// </summary>
        /// <param name="content">The content manager ot retrieve the textures of our button</param>
        /// <param name="font">The font used to display the game information.</param>
        /// <param name="position">The position where the text will be drawn.</param>
        public GameInformation(ContentManager content, SpriteFont font, Vector2 position)
        {
            // Load needed data
            _moneyTexture = content.Load<Texture2D>("Content\\Graphics\\Panel\\coin");
            _livesTexture = content.Load<Texture2D>("Content\\Graphics\\Panel\\life");
            _waveNumberTexture = content.Load<Texture2D>("Content\\Graphics\\Panel\\wave");
            _position = position;
            _font = font;

            // Load the SpriteFont for the display of the tooltip
            _waveInfoFont = content.Load<SpriteFont>("Content\\toolTip");
        }

        /// <summary>
        /// Method to draw the game information text.
        /// </summary>
        /// <param name="spriteBatch">A SpriteBatch that has been started</param>
        /// <param name="player">Instance of the current player</param>
        public void Draw(SpriteBatch spriteBatch, Player player)
        {
            // Draw the number of remaining lives
            spriteBatch.Draw(_livesTexture, _position);
            string text = string.Format("{0}/{1}", player.CurrentLives, player.OriginalLives);
            spriteBatch.DrawString(_font, text, new Vector2(_position.X + 30, 15), Color.White);

            // Draw the current amount of money
            spriteBatch.Draw(_moneyTexture, new Vector2(_position.X + 95, _position.Y + 5));
            text = string.Format(" {0}", player.Money);
            spriteBatch.DrawString(_font, text, new Vector2(_position.X + 120, 15), Color.White);

            // Draw the wave number
            spriteBatch.Draw(_waveNumberTexture, new Vector2(_position.X + 150, _position.Y + 5));
            text = string.Format("{0}/{1}", Enemy.WaveManager.GetInstance().Round, Enemy.WaveManager.GetInstance().WavesNumber);
            spriteBatch.DrawString(_font, text, new Vector2(_position.X + 185, 15), Color.White);

            // Draw the wave information
            text = string.Format("======= Wave Info ======");
            spriteBatch.DrawString(_font, text, new Vector2(_position.X + 10, 50), Color.White);
            var curWave = Enemy.WaveManager.GetInstance().CurrentWave;
            if (curWave != null)
            {
                text = string.Format("current wave enemy: {0}", curWave.EnemyName.Split('\\').Last());
                spriteBatch.DrawString(_waveInfoFont, text, new Vector2(_position.X + 05, 80), Color.White);
                text = string.Format("number of enemy: {0} - health: {1}", curWave.NumberEnemies, curWave.EnemyHealth);
                spriteBatch.DrawString(_waveInfoFont, text, new Vector2(_position.X + 05, 100), Color.White);
            }
        }
    }
}
