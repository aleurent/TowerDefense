// Systme includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Xna includes
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Game1.Enemy
{
    class WaveManager
    {
        //-------------- Singletong Part -------------
        // Static instance of the class
        private static WaveManager _instance = null;

        // Private constructor to disable any new instance
        private WaveManager()
        {
        }

        public static WaveManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new WaveManager();
            }
            return _instance;
        }

        //----------- Fields --------------
        // Number of wave in the game
        private int _numberOfWaves;
        private float _timeSinceLastWave;
        private Queue<Wave> _wavesQueue = new Queue<Wave>();
        private List<Texture2D> _enemyTexture = new List<Texture2D>();
        private Level _level;
        private Player _player;
        private bool _gameEnd;

        //--------- Properties ----------
        public Wave CurrentWave { get { return _wavesQueue.Peek(); } }
        public List<Enemy> Enemies { get { return CurrentWave.Enemies; } }
        public int Round { get { return CurrentWave.RoundNumber + 1; } }
        public int WavesNumber { get { return _numberOfWaves; } }
        public bool GameEnd { get { return _gameEnd; } set { _gameEnd = value; } }

        // Property to manage the end of the game in case of success
        public EventHandler WinGame = null;

        /// <summary>
        /// Method to add a new enemy texture inside the list
        /// </summary>
        /// <param name="EnemyTextureName">The texture of the enmy to add</param>
        public void AddEnemyTexture(Texture2D EnemyTextureName)
        {
            _enemyTexture.Add(EnemyTextureName);
        }

        /// <summary>
        /// Method to initialise the waveManager
        /// </summary>
        /// <param name="level">Instance of the level</param>
        /// <param name="player">Instance of the player</param>
        public void Initialise(Level level, Player player)
        {
            _level = level;
            _player = player;
            _numberOfWaves = 1;
            _gameEnd = false;

            // Create the wave and add them in the queue
            for (int i = 0; i < _numberOfWaves; ++i)
            {
                Wave wave = new Wave(i, 30, level, _player, _enemyTexture.ElementAt(i%_enemyTexture.Count()));
                _wavesQueue.Enqueue(wave);
            }
        }

        /// <summary>
        /// Method to update the waveManager, it will update the current wave
        /// </summary>
        /// <param name="gameTime">Instance of the gametime</param>
        public void Update(GameTime gameTime)
        {
            // Update the current wave
            CurrentWave.Update(gameTime);

            // If the wave is finished, trigger the chrono for the next wave
            if (CurrentWave.RoundOver)
            {
                _timeSinceLastWave += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_timeSinceLastWave >= 10.0f)
                {
                    // Start the new wave
                    _wavesQueue.Dequeue();
                    StartNextWave();
                }
            }
        }

        /// <summary>
        /// Method to draw the current wave
        /// </summary>
        /// <param name="spriteBatch">Instance of the sprite we use for drawing</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentWave.Draw(spriteBatch);
        }

        /// <summary>
        /// Method to start a new wave or send an evnet in case no new wave exist
        /// </summary>
        public void StartNextWave()
        {
            if (_wavesQueue.Count > 0)
            {
                _wavesQueue.Peek().Start();
                _timeSinceLastWave = 0;
            }
            else
                WinGame?.Invoke(this, EventArgs.Empty);
        }
    }
}
