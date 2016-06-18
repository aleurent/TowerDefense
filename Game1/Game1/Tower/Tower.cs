
// System includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

// Xna includes
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game1.Tower
{
    class Tower : Sprite
    {
        //------------- Fields -----------
        protected int _cost;
        protected int _damage;
        protected float _range;
        protected float _speed;
        protected Enemy.Enemy _target;
        protected float _bulletTimer;
        private Texture2D _bulletTexture;
        protected List<Bullet> _bulletList = new List<Bullet>();

        //------------- Properties -----------
        public Enemy.Enemy Target { get { return _target; } }
        public int Cost { get { return _cost; } }
        public int Damage { get { return _damage; } }
        public float Radius { get { return _range; } }

        /// <summary>
        /// Constructor of a Tower
        /// </summary>
        /// <param name="position">Position where to create the tower</param>
        /// <param name="bulletTexture">Texture of the bullet to display</param>
        /// <param name="tower">Tower element from the input xlm file</param>
        public Tower(Vector2 position, Texture2D bulletTexture, Texture2D texture, XElement tower) 
            : base(texture, position)
        {
            _bulletTexture = bulletTexture;
            _cost = Int32.Parse(tower.Element("cost").Value);
            _speed = float.Parse(tower.Element("speed").Value);
            _damage = Int32.Parse(tower.Element("damage").Value);
            _range = float.Parse(tower.Element("range").Value);
        }

        /// <summary>
        /// Method to update the target and the bullets
        /// </summary>
        /// <param name="gameTime">Instance of the gametime</param>
        /// <param name="enemies">List of enemies present in the game</param>
        public void Update(GameTime gameTime, List<Enemy.Enemy> enemies)
        {
            base.Update(gameTime);

            // Set the new time of the bullet
            _bulletTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update the target
            UpdateTarget(enemies);

            // If target is null reset the bulletTimer
            if (_target == null)
            {
                // Reset the bullet timer
                _bulletTimer = 0;

                // Clear the list of bullet
                _bulletList.Clear();
            }
            else
            {
                // Retrieve the new rotation to go to the target
                float rotationToTarget = GetRotationToTarget();

                if (_bulletTimer >= _speed)
                {
                    Bullet bullet = new Bullet(_bulletTexture, Vector2.Subtract(_center,
                        new Vector2(_bulletTexture.Width / 2)), rotationToTarget, 6, _damage);

                    _bulletList.Add(bullet);
                    _bulletTimer = 0;
                }

                List<Bullet> curBulletList = _bulletList.ToList();
                foreach (Bullet curBullet in curBulletList)
                {
                    if(rotationToTarget != 0.0)
                        curBullet.SetRotation(rotationToTarget);

                    curBullet.Update(gameTime);

                    // If the bullet is at destination (target), we kill it and we update the target life
                    if (_target != null &&
                        Vector2.Distance(curBullet.Center, _target.Center) < _target.Width / 2)
                    {
                        _target.CurrentHealth -= curBullet.Damage;
                        curBullet.Kill();
                        _bulletList.Remove(curBullet);
                    }
                }
            }
        }

        /// <summary>
        /// Method to draw the current tower and the associated bullets
        /// </summary>
        /// <param name="spriteBatch">instance of the spritebatch needed to draw</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            foreach (Bullet bullet in _bulletList)
                bullet.Draw(spriteBatch);
        }

        /// <summary>
        /// Method to generate a tooltip from tower properties
        /// </summary>
        /// <param name="tower"></param>
        /// <returns>The string corresponding to the tooltip</returns>
        static public string GenerateToolTip(XElement tower)
        {
            // Add the cost of the tower
            string toolTip = String.Format("Cost: {0} - Damage: {1}\n", tower.Element("cost").Value, tower.Element("damage").Value);

            // Add the speed
            string stringToAdd = "small";
            float speed = float.Parse(tower.Element("speed").Value);
            if (speed > 0.8f)
                stringToAdd = "small";
            else if (speed <= 0.2f)
                stringToAdd = "high";
            else
                stringToAdd = "medium";
            toolTip += String.Format("{0} speed - ", stringToAdd);

            // Add the range
            stringToAdd = "small";
            float range = float.Parse(tower.Element("range").Value);
            if (range <= 10)
                stringToAdd = "small";
            else if (range > 100)
                stringToAdd = "huge";
            else
                stringToAdd = "medium";
            toolTip += String.Format("{0} range\n", stringToAdd);

            return toolTip;
        }

        /// <summary>
        /// Method to know if a specific position is inside the tower range
        /// </summary>
        /// <param name="position">Position to verify if inside the range</param>
        /// <returns>return True if in the range, False otherwise</returns>
        private bool IsInRange(Vector2 position)
        {
            if (Vector2.Distance(_center, position) <= _range)
                return true;

            return false;
        }

        /// <summary>
        /// Method that allows to find the closest enemies of the tower from the list
        /// </summary>
        /// <param name="enemies">List of enemies present in the game</param>
        private void GetFirstEnemyInRange(List<Enemy.Enemy> enemies)
        {
            _target = null;

            foreach (Enemy.Enemy enemy in enemies)
            {
                if (Vector2.Distance(_center, enemy.Center) < _range)
                {
                    _target = enemy;
                    break;
                }
            }
        }

        /// <summary>
        /// Method that return the rotation needed to face the target
        /// </summary>
        /// <returns>The computed rotation</returns>
        private float GetRotationToTarget()
        {
            float rotation = 0.0f;
            if (_target != null)
            {
                Vector2 direction = _center - _target.Center;
                direction.Normalize();
                rotation = (float)Math.Atan2(-direction.X, direction.Y);
            }

            return rotation;
        }

        /// <summary>
        /// Method that update the current target if needed
        /// </summary>
        /// <param name="enemies">The list of enemies in which we have to find the new target</param>
        private void UpdateTarget(List<Enemy.Enemy> enemies)
        {
            // Check the existing target and get a new one if needed
            if (_target == null)
                GetFirstEnemyInRange(enemies);

            else if(_target != null && (!IsInRange(_target.Center) || _target.IsDead))
                GetFirstEnemyInRange(enemies);
        }
    }
}
