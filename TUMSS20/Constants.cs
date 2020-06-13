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
        public static int CurrentLevel = 1;
        public static int TotalScore = 0;

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
            LEAF = 2,
            STONE = 3,
            WIND = 4
        }

        public static int ELEMENT_COUNT = 5;
        public static int ELEMENT_IMAGE_SIZE = 32;
        public static int ELEMENT_IMAGE_SCALE = 3;

        public static Dictionary<ELEMENT, string> ELEMENT_NAMES = new Dictionary<ELEMENT, string>
        {
            { ELEMENT.FIRE, "Fire" },
            { ELEMENT.ICE, "Ice" },
            { ELEMENT.LEAF, "Leaf" },
            { ELEMENT.STONE, "Stone" },
            { ELEMENT.WIND, "Wind" }
        };
        
        // mapping table: which element needs to be chosen to go on
        public static Dictionary<ELEMENT, List<ELEMENT>> ELEMENT_MAPPING = new Dictionary<ELEMENT, List<ELEMENT>>
        {
            { ELEMENT.FIRE, new List<ELEMENT>{ ELEMENT.ICE, ELEMENT.STONE } },
            { ELEMENT.ICE, new List<ELEMENT>{ ELEMENT.FIRE } },
            { ELEMENT.LEAF, new List<ELEMENT>{ ELEMENT.FIRE, ELEMENT.WIND } },
            { ELEMENT.STONE, new List<ELEMENT>{ ELEMENT.ICE, ELEMENT.LEAF } },
            { ELEMENT.WIND, new List<ELEMENT>{ ELEMENT.STONE } }
        };

        public static Dictionary<ELEMENT, Color> ELEMENT_COLORS = new Dictionary<ELEMENT, Color>
        {
            { ELEMENT.FIRE, Color.Red },
            { ELEMENT.ICE, Color.Blue },
            { ELEMENT.LEAF, Color.Green},
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
