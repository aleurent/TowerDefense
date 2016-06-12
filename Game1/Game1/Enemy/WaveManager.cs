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

        public static WaveManager getInstance()
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
        private ContentManager _content;

        //--------- Properties ----------
        public Wave CurrentWave { get { return _wavesQueue.Peek(); } }
        public List<Enemy> Enemies { get { return CurrentWave.Enemies; } }
        public int Round { get { return CurrentWave.RoundNumber + 1; } }
        public int WavesNumber { get { return _numberOfWaves; } }
        public bool GameEnd { get { return _wavesQueue.Count() == 0; } }

        /// <summary>
        /// Method to initialise the waveManager
        /// </summary>
        /// <param name="level">Instance of the level</param>
        /// <param name="player">Instance of the player</param>
        /// <param name="content">Instance of the content manager to load enemy textures</param>
        public void initialise(Level level, Player player, ContentManager content)
        {
            _level = level;
            _player = player;
            _content = content;
            _numberOfWaves = 30;

            // Load enemy textures
            LoadEnemyTextures();

            // Create the wave and add them in the queue
            for (int i = 0; i < _numberOfWaves; ++i)
            {
                //int initialNumerOfEnemies = 15;
                //int numberModifier = (i / 2) + 1;

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
        /// Method to start a new wave
        /// </summary>
        public void StartNextWave()
        {
            if (_wavesQueue.Count > 0)
            {
                _wavesQueue.Peek().Start();
                _timeSinceLastWave = 0;
            }
        }

        private void LoadEnemyTextures()
        {
            _enemyTexture.Add(_content.Load<Texture2D>("Content\\Graphics\\Enemy\\banane"));
            _enemyTexture.Add(_content.Load<Texture2D>("Content\\Graphics\\Enemy\\goomba"));
            _enemyTexture.Add(_content.Load<Texture2D>("Content\\Graphics\\Enemy\\bobomb"));
            _enemyTexture.Add(_content.Load<Texture2D>("Content\\Graphics\\Enemy\\bulletbill"));
            _enemyTexture.Add(_content.Load<Texture2D>("Content\\Graphics\\Enemy\\goombafly"));
            _enemyTexture.Add(_content.Load<Texture2D>("Content\\Graphics\\Enemy\\blueshellcup"));
            _enemyTexture.Add(_content.Load<Texture2D>("Content\\Graphics\\Enemy\\piranhaplant"));
            _enemyTexture.Add(_content.Load<Texture2D>("Content\\Graphics\\Enemy\\redrocket"));
            _enemyTexture.Add(_content.Load<Texture2D>("Content\\Graphics\\Enemy\\goombatrol"));
            _enemyTexture.Add(_content.Load<Texture2D>("Content\\Graphics\\Enemy\\thundercloud"));
            _enemyTexture.Add(_content.Load<Texture2D>("Content\\Graphics\\Enemy\\yoshiegg"));
        }
    }
}
