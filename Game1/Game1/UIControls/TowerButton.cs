// System includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Xna includes
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game1.UIControls
{
    public class TowerButton : Button
    {
        // ---------- Fields -------------
        // Name associated to the tower
        private string _towerName;

        // Tower properties
        private string _tooltipText = "";
        private SpriteFont _toolTipFont;

        /// <summary>
        /// Constructs a new button to manage the start/pause/resume of the game.
        /// </summary>
        /// <param name="content">The content manager ot retrieve the textures of our button</param>
        /// <param name="position">The position where the button will be drawn.</param>
        /// <param name="contentName">The root name of the content to load.</param>
        /// <param name="toolTip">The tooltip of the button</param>
        public TowerButton(ContentManager content, Vector2 position, string towerName, string toolTip)
            : base(position)
        {
            // The "Normal" texture for the start button.
            _texture = content.Load<Texture2D>("Content\\Graphics\\Tower\\" + towerName);
            // The "MouseOver" texture for the start button.
            _hoverTexture = content.Load<Texture2D>("Content\\Graphics\\Tower\\" + towerName + "_over"); 
            // The "Pressed" texture for the start button.
            _pressedTexture = content.Load<Texture2D>("Content\\Graphics\\Tower\\" + towerName + "_pressed");

            // Define the bound of the button
            _bounds = new Rectangle((int)position.X, (int)position.Y, _texture.Width, _texture.Height);

            // Save the tower name
            _towerName = towerName;

            // Load the SpriteFont for the display of the tooltip
            _toolTipFont = content.Load<SpriteFont>("Content\\toolTip");
            _tooltipText = toolTip;

        }

        /// <summary>
        /// Updates the buttons state.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        /// <param name="activatedTowerName">The name of the tower that is currently activated</param>
        public void Update(GameTime gameTime, ref string activatedTowerName)
        {
            base.Update(gameTime);

            // Trigger the event if needed
            if (_state == ButtonStatus.Pressed && Mouse.GetState().LeftButton == ButtonState.Released)
                activatedTowerName = _towerName;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            // Draw the button
            base.Draw(spriteBatch);

            // Draw the tooltip if needed
            if(_state == ButtonStatus.MouseOver && !_tooltipText.Equals(""))
                spriteBatch.DrawString(_toolTipFont, _tooltipText, new Vector2(670, 385), Color.White);
        }
    }
}
