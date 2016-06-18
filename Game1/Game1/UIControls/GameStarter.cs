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
using Microsoft.Xna.Framework.Input;

namespace Game1.UIControls
{
    public class GameStarter :  Button
    {
        /// <summary>
        /// Constructs a new button to manage the start/pause/resume of the game.
        /// </summary>
        /// <param name="content">The content manager ot retrieve the textures of our button</param>
        /// <param name="texture">The normal texture of the button</param>
        /// <param name="position">The position where the button will be drawn.</param>
        public GameStarter(ContentManager content, Texture2D texture, Vector2 position)
            : base(position)
        {
            // The "Normal" texture for the start button.
            _texture = texture;
            // The "MouseOver" texture for the start button.
            _hoverTexture = content.Load<Texture2D>("Content\\Graphics\\Panel\\statOver");
            // The "Pressed" texture for the start button.
            _pressedTexture = content.Load<Texture2D>("Content\\Graphics\\Panel\\startPressed");

            // Define the bound of the button
            _bounds = new Rectangle((int)position.X, (int)position.Y, _texture.Width, _texture.Height);
        }

        /// <summary>
        /// Updates the buttons state.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Trigger the event if needed
            if (_state == ButtonStatus.Pressed && Mouse.GetState().LeftButton == ButtonState.Released)
                Enemy.WaveManager.GetInstance().StartNextWave();
        }
    }
}
