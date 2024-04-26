using System.Drawing;
using System.Windows.Forms;

namespace win_cursor_splasher {
    public partial class Splash : Form {
        private static readonly Color mask = Color.Gainsboro;

        public Splash(int r, Pen pen) {
            InitializeComponent();
            this.TransparencyKey = this.BackColor = mask;
            this.Size = new Size(r, r);
            this.ShowInTaskbar = false;
            this.Icon = Properties.Resources.icons;

            this.Paint += (s, ea) => {
                ea.Graphics.DrawEllipse(
                    pen, pen.Width / 2, pen.Width / 2, r - pen.Width, r - pen.Width
                );
            };
        }
    }
}
