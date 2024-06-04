using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Macro_Plot.Draggable
{
    public class AttachableCanvas : Canvas
    {
        static AttachableCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AttachableCanvas), new FrameworkPropertyMetadata(typeof(AttachableCanvas)));
        }


    }
}
