using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM3DW_Level_Porter.Classes
{
    public class Colors
    {
        private List<Color> GetColors()
        {
            List<Color> lists = new List<Color>();
            #region SystemColors
            lists.Add(SystemColors.ActiveBorder);
            lists.Add(SystemColors.ActiveCaption);
            lists.Add(SystemColors.ActiveCaptionText);
            lists.Add(SystemColors.AppWorkspace);
            lists.Add(SystemColors.ButtonFace);
            lists.Add(SystemColors.ButtonHighlight);
            lists.Add(SystemColors.ButtonShadow);
            lists.Add(SystemColors.Control);
            lists.Add(SystemColors.ControlDark);
            lists.Add(SystemColors.ControlDarkDark);
            lists.Add(SystemColors.ControlLight);
            lists.Add(SystemColors.ControlLightLight);
            lists.Add(SystemColors.ControlText);
            lists.Add(SystemColors.Desktop);
            lists.Add(SystemColors.GradientActiveCaption);
            lists.Add(SystemColors.GradientInactiveCaption);
            lists.Add(SystemColors.GrayText);
            lists.Add(SystemColors.Highlight);
            lists.Add(SystemColors.HighlightText);
            lists.Add(SystemColors.HotTrack);
            lists.Add(SystemColors.InactiveBorder);
            lists.Add(SystemColors.InactiveCaption);
            lists.Add(SystemColors.InactiveCaptionText);
            lists.Add(SystemColors.Info);
            lists.Add(SystemColors.InfoText);
            lists.Add(SystemColors.Menu);
            lists.Add(SystemColors.MenuBar);
            lists.Add(SystemColors.MenuHighlight);
            lists.Add(SystemColors.MenuText);
            lists.Add(SystemColors.ScrollBar);
            lists.Add(SystemColors.Window);
            lists.Add(SystemColors.WindowFrame);
            lists.Add(SystemColors.WindowText);
            #endregion
            return lists;
        }

        private Color GetRandomColor(List<Color> list)
        {
            Random rnd = new Random();
            int r = rnd.Next(list.Count);
            return list[r];
        }

        public Color GetRandomColor()
        {
            return GetRandomColor(GetColors());
        }
    }
}
