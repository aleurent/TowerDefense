// SYstem includes
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
    class Wave
    {
        //------------ Fields ---------
        // Number of enemies to create for a wave
        private int _numOfEnemies;
        // Number of current wave
        private int _waveNumber;
        // Time between two enemies creation
        private float _spawnTimer = 0;
        // How many enemies have spawned
        private int _enemiesSpawned = 0;
        // Boolean we use to know when the game start
        private bool _startWave;
        private int _health;

        private Level _level; 
        private Player _player;
        private Texture2D _enemyTexture;
        private List<Enemy> _enemiesList = new List<Enemy>();

        //---------- Properties -----------
        public bool RoundOver { get { return _enemiesList.Count == 0 && _enemiesSpawned == _numOfEnemies; } }
        public int RoundNumber { get { return _waveNumber; } }
        public List<Enemy> Enemies { get { return _enemiesList; } }
        public string EnemyName {  get { return _enemyTexture.Name; } }
        public int NumberEnemies { get { return _numOfEnemies; } }
        public int EnemyHealth { get { return _health; } }

        /// <summary>
        /// Constructor of a wave
        /// </summary>
        /// <param name="waveNumber">Number of the current wave</param>
        /// <param name="nbEnemies">Number of enemies to create</param>
        /// <param name="level">Instance of the level</param>
        /// <param name="player">Instance of the player</param>
        /// <param name="enemyTexture">Texture of the enemy of the wave</param>
        public Wave(int waveNumber, int nbEnemies, Level level, Player player, Texture2D enemyTexture)
        {
            _waveNumber = waveNumber;
            _level = level;
            _player = player;
            _numOfEnemies = nbEnemies;
            _enemyTexture = enemyTexture;
            _health = 50 + (50 * _waveNumber);
            _startWave = false;
        }

        /// <summary>
        /// Method to start the wave
        /// </summary>
        public void Start()
        {
            _startWave = true;
        }

        /// <summary>
        /// Method to update the wave
        /// </summary>
        /// <param name="gameTime">Instance of the gametime</param>
        public void Update(GameTime gameTime)
        {
            // Check if we still need to create an enemy
            if(_startWave && _enemiesSpawned != _numOfEnemies)
            {
                // Update the elpase of time
                _spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Check if the elapse of time between 2 creation is reached
                if (_spawnTimer > 1.5)
                    AddEnemy();
            }

            // Update the enemiesList in case some are dead
            List<Enemy> curEnemiesList = _enemiesList.ToList();
            foreach(Enemy curEnemy in curEnemiesList)
            {
                // Update the enemy
                curEnemy.Update(gameTime);

                // Check if an enemy is dead
                if(curEnemy.IsDead)
                {
                    // Check if the enemy reach the end or if he has been killed
                    if (curEnemy.CurrentHealth > 0)
                        _player.CurrentLives -= 1;
                    else
                        _player.Money += curEnemy.BountyGiven;

                    // Remove the enemy from the list
                    _enemiesList.Remove(curEnemy);
                }
            }
        }

        /// <summary>
        /// Method to draw the wave, it will draw each enemy
        /// </summary>
        /// <param name="spriteBatch">Instance of the psrite we use for the drawing</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Enemy enemy in _enemiesList)
                enemy.Draw(spriteBatch);
        }

        /// <summary>
        /// Method to add a new enemy in the list
        /// </summary>
        private void AddEnemy()
        {
            Enemy newEnemy = new Enemy(_enemyTexture, _level.Waypoints.Peek(), _health, 2+(_waveNumber/3), 0.6f);
            newEnemy.SetWaypoints(_level.Waypoints);
            _enemiesList.Add(newEnemy);
            _spawnTimer = 0;

            ++_enemiesSpawned;
        }
    }
}
