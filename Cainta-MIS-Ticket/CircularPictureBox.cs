using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Cainta_MIS_Ticket
{
    public class CircularPictureBox : PictureBox
    {
        protected override void OnPaint(PaintEventArgs pe)
        {
            // Create a GraphicsPath object to define a circle
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, this.Width - 1, this.Height - 1);

            // Set the region of the PictureBox to the circular path
            this.Region = new Region(path);

            // Draw the image inside the circular region
            base.OnPaint(pe);
        }
    }
}
