using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace win_cursor_splasher {
    public partial class Splash : Form {
        private static readonly Color mask = Color.Gainsboro;
        private Timer t = new Timer();
        private int r;

        public Splash(int r, Pen pen) {
            this.r = r;

            InitializeComponent();
            this.TransparencyKey = this.BackColor = mask;
            this.Size = new Size(r, r);
            this.ShowInTaskbar = false;

            this.Paint += (s, ea) => {
                ea.Graphics.DrawEllipse(
                    pen, pen.Width/2, pen.Width/2, r - pen.Width, r - pen.Width
                );
            };
        }
    }
}
