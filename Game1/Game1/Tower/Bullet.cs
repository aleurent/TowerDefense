// System includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Xna includes
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1.Tower
{
    class Bullet : Sprite
    {
        // ----------- Fields ----------
        private int _damage;
        private int _age;
        private int _speed;

        // ------------ PRoperties ----------
        public int Damage { get { return _damage; } }
        public bool IsDead() { return _age > 100; }

        /// <summary>
        /// Constructor of a bullet
        /// </summary>
        /// <param name="texture">Texture of a bullet</param>
        /// <param name="position">Current position of the bullet</param>
        /// <param name="rotation">Current rotation of the bullet</param>
        /// <param name="speed">Current speed of the bullet</param>
        /// <param name="damage">Current number of damage of the bullet</param>
        public Bullet(Texture2D texture, Vector2 position, float rotation, int speed, int damage) 
            : base(texture, position)
        {
            _rotation = rotation;
            _damage = damage;
            _speed = speed;
        }

        /// <summary>
        /// Method to kill a bullet
        /// </summary>
        public void Kill() { _age = 200; }

        /// <summary>
        /// Method to update the bullet
        /// </summary>
        /// <param name="gameTime">Instance of the gametime</param>
        public override void Update(GameTime gameTime)
        {
            ++_age;
            _position += _velocity;

            base.Update(gameTime);
        }

        /// <summary>
        /// Method to set the rotation of the bullet
        /// </summary>
        /// <param name="value">Value of the rotation to set</param>
        public void SetRotation(float value)
        {
            _rotation = value;
            _velocity = Vector2.Transform(new Vector2(0, -_speed),
                Matrix.CreateRotationZ(_rotation));
        }

        /// <summary>
        /// Method to draw the bullet
        /// </summary>
        /// <param name="spriteBatch">Instance of the sprite we use to draw</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsDead())
            {
                base.Draw(spriteBatch);
            }
        }
    }
}
