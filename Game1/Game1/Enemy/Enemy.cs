// System includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Xna includes
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game1.Enemy
{
    public class Enemy : Sprite
    {
        // --------- Fields -------------
        protected float _startHealth, _currentHealth;
        protected float _speed;
        private bool _alive = true;
        protected int _bountyGiven;
        private Queue<Vector2> _waypoints;
        private int _width;
        private int _height;

        // --------- Properties ---------
        public float CurrentHealth
        {
            get { return _currentHealth; }
            set { _currentHealth = value; }
        }

        public bool IsDead { get { return ( _currentHealth <= 0 || !_alive); } }
        public int BountyGiven { get { return _bountyGiven; } }
        public float DistanceToDestination { get { return Vector2.Distance(_position, _waypoints.Peek()); } }
        public int Width { get { return _width; } }

        /// <summary>
        /// Constructor of an enemy
        /// </summary>
        /// <param name="texture">Current texture of the enemy</param>
        /// <param name="position">Position of the enemy to create</param>
        /// <param name="health">Total of health of a new enemy</param>
        /// <param name="bountyGiven">Number of piece we win when the enemy is dead</param>
        /// <param name="speed">Speed of the enemy</param>
        public Enemy(Texture2D texture, Vector2 position, float health, int bountyGiven, float speed)
            : base(texture, position)
        {
            _startHealth = health;
            _currentHealth = _startHealth;
            _bountyGiven = bountyGiven;
            _speed = speed;
            _width = _texture.Width;
            _height = _texture.Height;
        }

        /// <summary>
        /// Set the waypoints that we use the enemy to move along the level
        /// </summary>
        /// <param name="waypoints">Queue of points</param>
        public void SetWaypoints(Queue<Vector2> waypoints)
        {
            _waypoints = new Queue<Vector2>(waypoints);
        }

        /// <summary>
        /// Method to update the enemy
        /// </summary>
        /// <param name="gameTime">Instance of the gametime</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Update the center to have the enemy in the middle of the road
            _center = new Vector2(_center.X + Math.Abs(50 - _width) / 2, _center.Y + Math.Abs(50 - _height) / 2);

            // Verify where the enemy is, and where he shall go
            if (_waypoints.Count > 0)
            {
                if (DistanceToDestination < _speed)
                    _position = _waypoints.Dequeue();
                else
                {
                    Vector2 direction = _waypoints.Peek() - _position;
                    direction.Normalize();

                    _velocity = Vector2.Multiply(direction, _speed);

                    _position += _velocity;
                }
            }
            else
                _alive = false;
        }

        /// <summary>
        /// Method to draw the enemy
        /// </summary>
        /// <param name="spriteBatch">Instance of sprite we use to draw</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsDead)
            {
                float healthPercentage = _currentHealth / _startHealth;
                Color color = new Color(new Vector3(healthPercentage, healthPercentage, healthPercentage));

                spriteBatch.Draw(_texture, _center, null, color, _rotation, _origin, 1.0f, SpriteEffects.None, 0);
            }
        }
    }
}
