using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUMSS20.Graphics
{
    public class SpriteSheet
    {
        private Texture2D texture;
        private int size;
        private int rows;
        private int columns;

        public SpriteSheet(Texture2D texture, int size)
        {
            this.texture = texture;
            this.size = size;

            rows = texture.Width / size;
            columns = texture.Height / size;
        }

        public void Draw(int index, Vector2 position, int scale, SpriteBatch spriteBatch)
        {
            int column = index % columns;
            int row = (int)((float)index / (float)columns);

            Rectangle sourceRectangle = new Rectangle(row * size, column * size, size, size);
            Rectangle destRectangle = new Rectangle((int)position.X, (int)position.Y, size * scale, size * scale);
            spriteBatch.Draw(texture, destRectangle, sourceRectangle, Color.White);
        }
    }
}
