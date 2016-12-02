using System;
using Sy3stem.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MeesGame.Gameplay.UIObjects
{
    public interface UIListObject
    {
        bool Selected { get; set; }

        int Index { get; set; }

    }

    public static class UIListObjectExtension
    {

    }
}
