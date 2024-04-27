using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace win_cursor_splasher {
    public partial class Splash : Form {
        private static readonly Color mask = Color.Gainsboro;
        private readonly Pen pen;
        private readonly int radius;

        protected override CreateParams CreateParams {
            get {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00080000; // WS_EX_LAYERED
                cp.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
                cp.ExStyle |= 0x00000080; // WS_EX_TOOLWINDOW
                return cp;
            }
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const UInt32 SWP_NOSIZE = 0x0001;
        private const UInt32 SWP_NOMOVE = 0x0002;
        private const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

        public Splash(int r, Pen pen) {
            this.radius = r;
            this.pen = pen;

            InitializeComponent();

            { // form attributes
                this.TransparencyKey = this.BackColor = mask;
                this.FormBorderStyle = FormBorderStyle.None;
                this.ShowInTaskbar = false;
                this.TopMost = true;
                this.Size = new Size(
                    (int)(r + pen.Width * 2),
                    (int)(r + pen.Width * 2)
                );
                this.Icon = Properties.Resources.icons;
                SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
            }

            this.Paint += (s, ea) => {
                ea.Graphics.DrawEllipse(
                    pen,
                    pen.Width,
                    pen.Width,
                    r,
                    r
                );
            };
        }

        internal void Locate(Point p) {
            this.Location = new Point(
                (int)(p.X - radius / 2 - pen.Width),
                (int)(p.Y - radius / 2 - pen.Width)
            );
        }
    }
}
