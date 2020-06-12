using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUMSS20
{
    public class Constants
    {
        private static Color ColorFromHex(string hex)
        {
            var drawColor = System.Drawing.ColorTranslator.FromHtml(hex);
            return new Color(drawColor.R, drawColor.G, drawColor.B);
        }

        public static Color GAME_BACKGROUND_COLOR = ColorFromHex("#C4CFA1");
        public static Color GAME_FOREGROUND_COLOR = ColorFromHex("#4D533C");
    }
}
