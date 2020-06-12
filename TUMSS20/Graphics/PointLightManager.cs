using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUMSS20.Graphics
{
    public class PointLightManager
    {
        private List<PointLight> lights;
        private BlendState blendState;
        private Texture2D lightTexture;

        public PointLightManager(ContentManager contentManager)
        {
            lights = new List<PointLight>();
            blendState = new BlendState
            {
                AlphaSourceBlend = Blend.SourceAlpha,
                AlphaDestinationBlend = Blend.InverseDestinationAlpha
            };

            lightTexture = contentManager.Load<Texture2D>("pointlight");
        }

        public void AddLight(Color color, Vector2 position)
        {
            PointLight light = new PointLight(lightTexture);
            light.Color = color;
            light.Position = position;

            lights.Add(light);
        }

        public void Clear()
        {
            lights.Clear();
        }

        // Apply a flickering effect to the point lights.
        public void Update(GameTime time)
        {
            foreach(PointLight light in lights)
            {

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            foreach(PointLight light in lights)
            {
                light.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}
