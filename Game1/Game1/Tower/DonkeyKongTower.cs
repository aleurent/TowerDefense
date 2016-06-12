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

namespace Game1.Tower
{
    class DonkeyKongTower : Tower
    {
        /// <summary>
        /// Constructor for BonesTower
        /// </summary>
        /// <param name="content">Instance of content manager to access the texture to load</param>
        /// <param name="position">The position where to create the tower</param>
        /// <param name="bulletTexture">Texture of the bullet to display</param>
        public DonkeyKongTower(ContentManager content, Vector2 position, Texture2D bulletTexture)
            : base(position, bulletTexture)
        {
            _cost = 50;
            _damage = 70;
            _range = 110;
            _speed = 0.5f;

            // Load the texture of the tower
            _texture = content.Load<Texture2D>("Content\\Graphics\\Tower\\donkeykong");
        }
    }
}
