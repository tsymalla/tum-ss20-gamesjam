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
        private int nextRefreshPointX;

        public Wall(ContentManager contentManager, int screenWidth, int screenHeight, bool inverted)
        {
            this.inverted = inverted;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            nextRefreshPointX = screenWidth / 2;

            wall = contentManager.Load<Texture2D>("wall");

            GenerateTiles(true, screenWidth / wall.Width, 4);
        }

        private void GenerateTiles(bool refresh, int count, int maxHeight)
        {
            // generate amount of heights
            Random rnd = new Random();

            if (refresh)
            {
                heights = new List<int>(count);
            }

            for (int index = 0; index < count; index++)
            {
                heights.Add(rnd.Next(1, maxHeight));
            }
        }

        public void Update(Vector2 playerPosition, int playerWidth, GameTime time, int currentScore)
        {
            // only regenerate the walls if there are too little of them.
            if (playerPosition.X >= nextRefreshPointX)
            {
                int halfScreenWidth = screenWidth / 2;
                int newCount = halfScreenWidth / wall.Width;

                // TODO use score to create larger walls
                int newMaxHeight = new Random().Next(1, 5);// Math.Max(Math.Max(1, currentScore / 4), 3);
                GenerateTiles(false, newCount, newMaxHeight);

                nextRefreshPointX += halfScreenWidth;
            }

            // TODO
            // remove all non-visible elements to prevent leaking memory
            //heights.RemoveRange(0, (int)playerPosition.X / 2 / wall.Width);
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
            int tileIndex = (int)position.X / wall.Width;
            int height = heights[tileIndex] * wall.Height;

            int posY = (int)position.Y;
            if (posY > height && posY + collidableHeight < screenHeight - height)
            {
                return false;
            }

            return true;
        }
    }
}
