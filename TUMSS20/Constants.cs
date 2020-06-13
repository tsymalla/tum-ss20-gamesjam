using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TUMSS20
{
    public class Constants
    {
        public static Color ColorFromHex(string hex)
        {
            var drawColor = System.Drawing.ColorTranslator.FromHtml(hex);
            return new Color(drawColor.R, drawColor.G, drawColor.B);
        }

        public static Color GAME_BACKGROUND_COLOR = ColorFromHex("#C4CFA1");
        public static Color GAME_FOREGROUND_COLOR = ColorFromHex("#4D533C");

        public enum ELEMENT
        {
            FIRE = 0,
            ICE = 1,
            WOOD = 2,
            STONE = 3,
            WIND = 4
        }

        public static int ELEMENT_COUNT = 5;

        public static Dictionary<ELEMENT, string> ELEMENT_NAMES = new Dictionary<ELEMENT, string>
        {
            { ELEMENT.FIRE, "Fire" },
            { ELEMENT.ICE, "Ice" },
            { ELEMENT.WOOD, "Wood" },
            { ELEMENT.STONE, "Stone" },
            { ELEMENT.WIND, "Wind" }
        };

        // mapping table: which element needs to be chosen to go on
        public static Dictionary<ELEMENT, List<ELEMENT>> ELEMENT_MAPPING = new Dictionary<ELEMENT, List<ELEMENT>>
        {
            { ELEMENT.FIRE, new List<ELEMENT>{ ELEMENT.ICE, ELEMENT.STONE } },
            { ELEMENT.ICE, new List<ELEMENT>{ ELEMENT.FIRE } },
            { ELEMENT.WOOD, new List<ELEMENT>{ ELEMENT.FIRE, ELEMENT.WIND } },
            { ELEMENT.STONE, new List<ELEMENT>{ ELEMENT.ICE, ELEMENT.WOOD } },
            { ELEMENT.WIND, new List<ELEMENT>{ ELEMENT.STONE } }
        };

        public static Dictionary<ELEMENT, Color> ELEMENT_COLORS = new Dictionary<ELEMENT, Color>
        {
            { ELEMENT.FIRE, Color.Red },
            { ELEMENT.ICE, Color.LightBlue },
            { ELEMENT.WOOD, Color.Brown },
            { ELEMENT.STONE, Color.Gray },
            { ELEMENT.WIND, Color.Turquoise }
        };

        public static ELEMENT ChoseElement(bool useElement, ELEMENT currentElement)
        {
            int newIndex = new Random().Next(0, ELEMENT_COUNT - 1);
            if (useElement)
            {
                while (newIndex == (int)currentElement)
                {
                    newIndex = new Random().Next(0, ELEMENT_COUNT - 1);
                }
            }

            return (ELEMENT)newIndex;
        }
    }
}
