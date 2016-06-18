// Systme includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Xna includes
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game1.UIControls
{
    /// <summary>
    /// Describes the state of the button.
    /// </summary>
    public enum ButtonStatus
    {
        Normal,
        MouseOver,
        Pressed,
    }

    public class Button : Sprite
    {
        //---------- Fields ---------------
        // Store the MouseState of the last frame.
        protected MouseState _previousState;

        // Store the current state of the button.
        protected ButtonStatus _state = ButtonStatus.Normal;

        // The the different state textures.
        protected Texture2D _hoverTexture;
        protected Texture2D _pressedTexture;

        // A rectangle that covers the button.
        protected Rectangle _bounds;

        /// <summary>
        /// Constructs a new button.
        /// </summary>
        /// <param name="position">The position where the button will be drawn.</param>
        public Button(Vector2 position) : base(position) {}

        /// <summary>
        /// Updates the buttons state.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public override void Update(GameTime gameTime)
        {
            // Determine if the mouse if over the button.
            MouseState mouseState = Mouse.GetState();
            bool isMouseOver = _bounds.Contains(mouseState.X, mouseState.Y);

            // Set the button status
            if(isMouseOver)
            {
                // Check if the player releases the button.
                if (_previousState.LeftButton == ButtonState.Pressed)
                    _state = ButtonStatus.Pressed;
                else
                    _state = ButtonStatus.MouseOver;
            }
            else
                _state = ButtonStatus.Normal;

            _previousState = mouseState;
        }

        /// <summary>
        /// Draws the button.
        /// </summary>
        /// <param name="spriteBatch">A SpriteBatch that has been started</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_texture != null && _hoverTexture != null && _pressedTexture != null)
            {
                switch (_state)
                {
                    case ButtonStatus.Normal:
                        spriteBatch.Draw(_texture, _bounds, Color.White);
                        break;
                    case ButtonStatus.MouseOver:
                        spriteBatch.Draw(_hoverTexture, _bounds, Color.White);
                        break;
                    case ButtonStatus.Pressed:
                        spriteBatch.Draw(_pressedTexture, _bounds, Color.White);
                        break;
                    default:
                        spriteBatch.Draw(_texture, _bounds, Color.White);
                        break;
                }
            }
        }
    }
}
