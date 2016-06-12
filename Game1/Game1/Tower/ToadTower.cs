// System includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Xna includes
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1.Tower
{
    class ToadTower : Tower
    {
        /// <summary>
        /// ToadTower constructor
        /// </summary>
        /// <param name="content">Instance of the contentManager to load texture data</param>
        /// <param name="position">Position where to create the tower</param>
        /// <param name="bulletTexture">Texture of the bullet to display</param>
        public ToadTower(ContentManager content, Vector2 position, Texture2D bulletTexture)
            : base(position, bulletTexture)
        {
            _cost = 10;
            _damage = 5;
            _range = 60;
            _speed = 0.5f;

            // Load the texture of the tower
            _texture = content.Load<Texture2D>("Content\\Graphics\\Tower\\toad");
        }
    }
}
