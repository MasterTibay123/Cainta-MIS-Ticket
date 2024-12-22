using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cainta_MIS_Ticket
{
    public class CustomToolStripRenderer : ToolStripProfessionalRenderer
    {
        private ToolStripMenuItem _selectedMenuItem;
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            // Customize the background rendering
            if (e.Item.Selected || e.Item == _selectedMenuItem)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.Transparent), e.Item.ContentRectangle);
            }
            else
            {
                base.OnRenderMenuItemBackground(e);
            }
        }
    }
}
