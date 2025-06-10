using SFML.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIFramework.Sprite;
using UIFramework.Text;

namespace InteractionFramework.Dialog;
public interface IDialogObject
{
    UISprite? DialogSprite { get; set; }
    UIText? DisplayName { get; set; }
}
