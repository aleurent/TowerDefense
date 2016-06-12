// System includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Xna includes
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Game1
{
    public class Level
    {
        // -------------- Fields -----------
        // Map that allows the program to compute the level
        int[,] _levelMap;

        // Define the list of available texture
        private List<Texture2D> _texturesList = new List<Texture2D>();

        // Define the queue containing tehe list of points on the map
        private Queue<Vector2> _waypoints;

        //---------- Properties ---------------
        public Queue<Vector2> Waypoints { get { return _waypoints; } }
        public int Width { get { return _levelMap.GetLength(1); } }
        public int Height { get { return _levelMap.GetLength(0); } }
        public int TextureWidth {  get { return _texturesList.First().Width;  } }
        public int TextureHeight { get { return _texturesList.First().Height; } }

        /// <summary>
        /// Constructor of a level
        /// </summary>
        /// <param name="content">Instance of content manager we need to load texture</param>
        public Level(ContentManager content)
        {
            _texturesList.Add(content.Load<Texture2D>("Content\\Graphics\\Level\\road"));
            _texturesList.Add(content.Load<Texture2D>("Content\\Graphics\\Level\\grass"));
            _texturesList.Add(content.Load<Texture2D>("Content\\Graphics\\Level\\ground"));

            // Init the map and the queue of waypoints
            initFirstMap();
        }

        /// <summary>
        /// Method to draw our level
        /// </summary>
        /// <param name="batch">Instance of the psrite we use for the drawing</param>
        public void Draw(SpriteBatch batch)
        {
            // We loop on the map and depending on the value we search for the texture to draw
            for (int x = 0; x < _levelMap.GetLength(1); ++x)
            {
                for (int y = 0; y < _levelMap.GetLength(0); ++y)
                {
                    Texture2D texture = _texturesList[_levelMap[y, x]];
                    batch.Draw(texture, 
                               new Rectangle(x * TextureWidth, y * TextureHeight, TextureWidth, TextureHeight), 
                               Color.White);
                }
            }
        }

        /// <summary>
        /// Method to retrieve the map index from a cell in the map
        /// </summary>
        /// <param name="cellX">X coordinate of the cell</param>
        /// <param name="cellY">Y coordinate of the cell</param>
        /// <returns>The index value or 0 if the cell is out of the map</returns>
        public int GetIndex(int cellX, int cellY)
        {
            if (cellX < 0 || cellX >= Width - 1 || cellY < 0 || cellY >= Height - 1)
                return 0;

            return _levelMap[cellY, cellX];
        }

        /// <summary>
        /// Method to init our map level and waypoints
        /// </summary>
        private void initFirstMap()
        {
            // Define the map of the level
            _levelMap = new int[,]
            {
            {2,2,2,0,2,2,2,2,2,0,2,2,2,},
            {2,0,0,0,2,0,0,0,2,0,2,2,2,},
            {2,0,2,2,2,0,2,0,2,0,2,2,2,},
            {2,0,0,0,0,0,2,0,2,0,0,0,2,},
            {2,2,2,2,2,2,2,0,2,2,2,0,2,},
            {2,0,0,0,0,0,0,0,2,2,2,0,2,},
            {2,0,2,2,2,2,2,2,2,2,2,0,2,},
            {2,0,0,0,0,0,0,0,0,0,0,0,2,},
            {2,2,2,2,2,2,2,2,2,2,2,2,2,},
            };

            // Define the queue of points
            _waypoints = new Queue<Vector2>();
            _waypoints.Enqueue(new Vector2(3, 0) * TextureWidth);
            _waypoints.Enqueue(new Vector2(3, 1) * TextureWidth);
            _waypoints.Enqueue(new Vector2(1, 1) * TextureWidth);
            _waypoints.Enqueue(new Vector2(1, 3) * TextureWidth);
            _waypoints.Enqueue(new Vector2(5, 3) * TextureWidth);
            _waypoints.Enqueue(new Vector2(5, 1) * TextureWidth);
            _waypoints.Enqueue(new Vector2(7, 1) * TextureWidth);
            _waypoints.Enqueue(new Vector2(7, 5) * TextureWidth);
            _waypoints.Enqueue(new Vector2(1, 5) * TextureWidth);
            _waypoints.Enqueue(new Vector2(1, 7) * TextureWidth);
            _waypoints.Enqueue(new Vector2(11, 7) * TextureWidth);
            _waypoints.Enqueue(new Vector2(11, 3) * TextureWidth);
            _waypoints.Enqueue(new Vector2(9, 3) * TextureWidth);
            _waypoints.Enqueue(new Vector2(9, 0) * TextureWidth);
        }
    }
}
