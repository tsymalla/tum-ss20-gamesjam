using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUMSS20.GameState
{
    public class Wall
    {
        private bool inverted;
        private int screenWidth;
        private int screenHeight;
        private Texture2D wall;
        private List<int> heights;
        private int totalScreens;
        private const int BIAS = 4;

        public Wall(ContentManager contentManager, int screenWidth, int screenHeight, bool inverted, int totalScreens)
        {
            this.inverted = inverted;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.totalScreens = totalScreens;

            wall = contentManager.Load<Texture2D>("wall");

            GenerateTiles();
        }

        private void GenerateTiles()
        {
            // generate amount of heights
            Random rnd = new Random();

            int tilesXPerScreen = screenWidth / wall.Width;
            int maxCount = tilesXPerScreen * totalScreens;
            heights = new List<int>(maxCount);
        
            for (int index = 0; index < maxCount; index++)
            {
                int maxHeight = Math.Min(rnd.Next(1, Constants.CurrentLevel), 5);
                heights.Add(rnd.Next(1, maxHeight));
            }
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            int wallWidth = wall.Width;
            for (int x = 0; x < heights.Count; x++)
            {
                int tileCountY = heights[x];

                int startY = inverted ? (screenHeight - (wall.Height * tileCountY)) / wall.Height : 0;
                int endY = inverted ? startY + tileCountY : tileCountY;
                for (int y = startY; y < endY; y++)
                {
                    spriteBatch.Draw(wall, new Vector2(x * wallWidth, y * wall.Height), Color.White);
                }
            }
        }

        public bool HasCollided(Vector2 position, int collidableHeight)
        {
            if (HasFinishedLevel(position))
            {
                return false;
            }

            int tileIndex = (int)position.X / wall.Width;
            int height = heights[tileIndex] * wall.Height;

            int posY = (int)position.Y;
            if (posY > height - BIAS && posY + collidableHeight < screenHeight - height + BIAS)
            {
                return false;
            }

            return true;
        }

        public bool HasFinishedLevel(Vector2 position)
        {
            int tileIndex = (int)position.X / wall.Width;
            if (tileIndex >= heights.Count)
            {
                return true;
            }

            return false;
        }
    }
}
