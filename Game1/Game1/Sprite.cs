// System includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Xna includes
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game1
{
    public class Sprite
    {
        //----------- Fields ----------
        protected Texture2D _texture;
        protected Vector2 _position, _velocity, _center, _origin;
        protected float _rotation;

        //---------- Properties -----------
        public Vector2 Center { get { return _center; } }
        public Vector2 Position { get { return _position; } }

        /// <summary>
        /// Constructor of a sprite
        /// </summary>
        /// <param name="texture">Texture of the sprite to create</param>
        /// <param name="position">Position of the sprite to create</param>
        public Sprite(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            _position = position;
            _velocity = Vector2.Zero;
            _center = new Vector2(position.X + texture.Width / 2, position.Y + texture.Height / 2);
            _origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        /// <summary>
        /// Constructor of a sprite
        /// </summary>
        /// <param name="position">Position of the sprite to create</param>
        public Sprite(Vector2 position)
        {
            _texture = null;
            _position = position;
            _velocity = Vector2.Zero;
            _center = Vector2.Zero;
            _origin = Vector2.Zero;
        }

        /// <summary>
        /// Method to update the sprite
        /// </summary>
        /// <param name="gameTime">Instance of the gametime</param>
        public virtual void Update(GameTime gameTime)
        {
            if (_position != null && _texture != null)
            {
                _center = new Vector2(_position.X + _texture.Width / 2, _position.Y + _texture.Height / 2);
                _origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            }
        }

        /// <summary>
        /// Method to draw a sprite
        /// </summary>
        /// <param name="spriteBatch">Instance of spritebatch we use for drawing</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (_center != null && _texture != null && _origin != null)
            {
                spriteBatch.Draw(_texture, _center, null, Color.White, _rotation, _origin, 1.0f, SpriteEffects.None, 0);
            }
        }
    }
}
