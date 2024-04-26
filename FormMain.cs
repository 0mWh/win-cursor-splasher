using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace win_cursor_splasher {
    public partial class FormMain : Form {

        private readonly Timer t = new Timer();
        private static readonly Queue<Splash> all = new Queue<Splash>();
        private readonly Pen pen = new Pen(Color.Red, 10);

        private static int outlineRadius = 200,
            outlineWidth = 10,
            outlineTimeout = 1500,
            updateInterval = 500,
            updateDistance = 150;
        private static bool borderOnly = false;

        public FormMain() {
            InitializeComponent();
            this.Icon = Properties.Resources.icons;
            this.pictureCredit.Image = Properties.Resources.icons256;

            { // get values from UI default
                dataOutlineRadius_ValueChanged(null, null);
                dataOutlineWidth_ValueChanged(null, null);
                dataOutlineTimeout_ValueChanged(null, null);
                dataUpdateDistance_ValueChanged(null, null);
                dataUpdateInterval_ValueChanged(null, null);
                dataBorderOnly_CheckedChanged(null, null);
            }

            Point last = Cursor.Position;
            Screen last_s = Screen.PrimaryScreen;

            t.Interval = updateInterval;
            t.Tick += (s, ea) => {
                Point curr = Cursor.Position;
                Screen curr_s = PointOnScreen(curr);

                { // display
                    this.dataLocation.Text = $"{curr.X}, {curr.Y}";
                    this.dataScreen.Text = $"{curr_s.DeviceName}";
                    this.dataRelative.Text = $"{curr.X - curr_s.Bounds.Left}, {curr.Y - curr_s.Bounds.Top}";
                }

                if (borderOnly) {
                    if (curr_s != last_s) {
                        Begin(FindBorder(curr_s, last_s, curr, last));
                    }
                } else {
                    if (Dist(curr, last) > updateDistance) {
                        Begin(curr);
                    }
                }

                last = curr;
                last_s = curr_s;
            };
            t.Start();
        }

        // recursively find the coordinate of the screen border between two points
        private Point FindBorder(Screen sa, Screen sb, Point pa, Point pb) {
            Point pc = new Point((pa.X + pb.X) / 2, (pa.Y + pb.Y) / 2);

            { // close enough
                if (Dist(pa, pb) < 2) {
                    return pc;
                }
            }

            { // center on border: ensure both points on different screens
                Screen sc = PointOnScreen(pc);
                if (sc == sa) {
                    return FindBorder(sc, sb, pc, pb);
                } else if (sc == sb) {
                    return FindBorder(sa, sc, pa, pc);
                }
            }

            return pc;
        }

        // place a circle on the screen at point p
        private void Begin(Point p) {
            Splash b = new Splash(outlineRadius, pen);

            { // produce
                b.Show();
                b.Location = new Point(p.X - outlineRadius / 2, p.Y - outlineRadius / 2);
            }

            { // expiry
                Timer tt = new Timer {
                    Interval = outlineTimeout
                };
                tt.Tick += (sA, eaA) => {
                    tt.Stop();
                    b.Close();
                };
                tt.Start();
            }

            //{ // mouseover?
            //    b.MouseEnter += (s, ea) => {
            //        if (Dist(Cursor.Position, b.Location) > outlineWidth) {
            //            RemoveUntil(b);
            //        }
            //    };
            //}

            { // force expiry
                while (all.Count > 2) {
                    all.Dequeue().Close();
                }
            }

            all.Enqueue(b);
        }

        // remove all circles up to and including the parameter
        public static void RemoveUntil(Splash until) {
            until.Close();
            while (all.Any() && all.Peek() != until)
                all.Dequeue().Close();
        }

        // get the screen that this coordinate lands on
        public static Screen PointOnScreen(Point mousePoint) {
            foreach (Screen screen in Screen.AllScreens)
                if (screen.Bounds.Contains(mousePoint))
                    return screen;
            return null;
        }

        // euclidean distance between two points
        public static double Dist(Point a, Point b) {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2D) + Math.Pow(a.Y - b.Y, 2D));
        }

        #region UI Setting Code

        private void linkCredits_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            System.Diagnostics.Process.Start("https://github.com/0mWh/win-cursor-splasher");
        }

        private void dataUpdateDistance_ValueChanged(object sender, EventArgs e) {
            updateDistance = (int)this.dataUpdateDistance.Value;
        }

        private void dataUpdateInterval_ValueChanged(object sender, EventArgs e) {
            updateInterval = (int)(this.dataUpdateInterval.Value * 1000);
            t.Interval = updateInterval;
        }

        private void dataOutlineWidth_ValueChanged(object sender, EventArgs e) {
            outlineWidth = (int)this.dataOutlineWidth.Value;
            pen.Width = outlineWidth;
        }

        private void dataOutlineRadius_ValueChanged(object sender, EventArgs e) {
            outlineRadius = (int)this.dataOutlineRadius.Value;
        }

        private void dataBorderOnly_CheckedChanged(object sender, EventArgs e) {
            borderOnly = (bool)this.dataBorderOnly.Checked;
            this.dataUpdateDistance.Enabled = !borderOnly;
        }

        private void dataOutlineTimeout_ValueChanged(object sender, EventArgs e) {
            outlineTimeout = (int)(this.dataOutlineTimeout.Value * 1000);
        }

        #endregion

    }
}
