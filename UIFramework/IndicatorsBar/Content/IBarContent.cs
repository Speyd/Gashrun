using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIFramework.IndicatorsBar.Content;
public interface IBarContent
{
    void UpdateContent(RectangleShape bar);
}
