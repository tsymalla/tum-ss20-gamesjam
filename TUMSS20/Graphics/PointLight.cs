using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUMSS20.Graphics
{
    public class PointLight
    {
        public Vector2 Position
        {
            get;
            set;
        }

        public Color Color
        {
            get;
            set;
        }

        private Texture2D texture;

        public PointLight(Texture2D texture)
        {
            this.texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, Color);
        }
    }
}
