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
            spriteBatch.DrawString(_font, text, new Vector2(_position.X + 35, 15), Color.White);

            // Draw the current amount of money
            spriteBatch.Draw(_moneyTexture, new Vector2(_position.X + 110, _position.Y + 5));
            text = string.Format(" {0}", player.Money);
            spriteBatch.DrawString(_font, text, new Vector2(_position.X + 140, 15), Color.White);

            // Draw the wave number of the finish is game at the end
            spriteBatch.Draw(_waveNumberTexture, new Vector2(_position.X + 50, _moneyTexture.Height + 20));
            text = string.Format("{0}/{1}", Enemy.WaveManager.getInstance().Round, Enemy.WaveManager.getInstance().WavesNumber);
            spriteBatch.DrawString(_font, text, new Vector2(_position.X + 90, _moneyTexture.Height + 25), Color.White);
        }
    }
}
