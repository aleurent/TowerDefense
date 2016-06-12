// System includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Xna includes
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Game1
{
    public class Player
    {
        //----------- Fields ----------
        private int _money;
        private int _lives;
        private int _originalLives;
        private List<Tower.Tower> _towers = new List<Tower.Tower>();
        private Level _level;
        private ContentManager _content;
        private string _towerType;
        private Texture2D _bulletTexture;

        // Mouse state for the current frame
        private MouseState _mouseState;

        // Mouse state for the previous frame
        private MouseState _oldState;

        // Cell position of the mouse
        private int _cellX;
        private int _cellY;

        // Tile position of the mouse
        private int _tileX;
        private int _tileY;

        // ----------- Properties ----------
        public int Money { get { return _money; } set { _money = value; } }
        public int CurrentLives { get { return _lives; } set { _lives = value; } }
        public int OriginalLives { get { return _originalLives; } }
        public string TowerType { set { _towerType = value; } }

        /// <summary>
        /// Constructor of a player
        /// </summary>
        /// <param name="content">Instance of the content manager we use later to load data</param>
        /// <param name="level">Instance of the existing level</param>
        public Player(ContentManager content, Level level)
        {
            _level = level;
            _content = content;
            _money = 20;
            _lives = _originalLives = 20;
            _towerType = "";

            // Load the texture of the bullet
            _bulletTexture = content.Load<Texture2D>("Content\\Graphics\\Tower\\fireball");
        }

        /// <summary>
        /// Method we use to update the player: add a new tower and update them
        /// </summary>
        /// <param name="gameTime">Instance of the gametime</param>
        /// <param name="enemies">List of enemies we need to update towers</param>
        public void Update(GameTime gameTime, List<Enemy.Enemy> enemies)
        {
            // Retrieve mouse state
            _mouseState = Mouse.GetState();

            // Save the new mouse position
            _cellX = (int)(_mouseState.X / _level.TextureWidth); // Convert the position of the mouse
            _cellY = (int)(_mouseState.Y / _level.TextureHeight); // from array space to level space

            _tileX = _cellX * _level.TextureWidth; // Convert from array space to level space
            _tileY = _cellY * _level.TextureHeight; // Convert from array space to level space

            // Check if the mouse button is released, if yes check if we can create a new tower.
            if (_mouseState.LeftButton == ButtonState.Released
                && _oldState.LeftButton == ButtonState.Pressed)
            {
                if (IsCellClear() && !_towerType.Equals(""))
                    AddTower();
            }

            // Set the oldState so it becomes the state of the previous frame.
            _oldState = _mouseState; 

            // Update the created towers
            foreach (Tower.Tower tower in _towers)
                tower.Update(gameTime, enemies);
        }

        /// <summary>
        /// Method to draw to player, it will draw each tower from the list
        /// </summary>
        /// <param name="spriteBatch">Instance of the sprite we use for the drawing</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tower.Tower tower in _towers)
                tower.Draw(spriteBatch);
        }

        /// <summary>
        /// Method we use to build a new tower
        /// </summary>
        private void AddTower()
        {
            Type newTowerType = Type.GetType("Game1.Tower." + _towerType);
            if (newTowerType != null)
            {
                Tower.Tower newTower = (Tower.Tower)Activator.CreateInstance(newTowerType, 
                                                                             new Object[] { _content,
                                                                                            new Vector2(_tileX, _tileY),
                                                                                            _bulletTexture});

                // Check if the user has enough money to build the tower
                if (newTower.Cost <= _money)
                {
                    _towers.Add(newTower);
                    _money -= newTower.Cost;
                }
            }

            _towerType = "";
        }

        /// <summary>
        ///  Method to know if we can build a tower at the place (tileX, tileY)
        /// </summary>
        /// <returns>True if the twoer can be built, False otherwise</returns>
        private bool IsCellClear()
        {
            // Check if inside the limit of the map
            bool inBounds = _cellX >= 0 && _cellY >= 0 && // Make sure tower is within limits
                _cellX < _level.Width  && _cellY < _level.Height;
            
            // Check if a tower is already at this place
            bool spaceClear = true;
            foreach (Tower.Tower tower in _towers)
            {
                if (tower.Position == new Vector2(_tileX, _tileY))
                    break;
            }

            // Check the place is authorised to build a tower
            bool onPath = (_level.GetIndex(_cellX, _cellY) != 0);

            return (inBounds && spaceClear && onPath);
        }
    }
}
