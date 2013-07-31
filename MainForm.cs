using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BorderlessForm
{
    public partial class MainForm : FormBase
    {
        public MainForm()
        {
            InitializeComponent();

            TopLeftCornerPanel.MouseDown += (s, e) => DecorationMouseDown(HitTestValues.HTTOPLEFT);
            TopRightCornerPanel.MouseDown += (s, e) => DecorationMouseDown(HitTestValues.HTTOPRIGHT);
            BottomLeftCornerPanel.MouseDown += (s, e) => DecorationMouseDown(HitTestValues.HTBOTTOMLEFT);
            BottomRightCornerPanel.MouseDown += (s, e) => DecorationMouseDown(HitTestValues.HTBOTTOMRIGHT);

            TopBorderPanel.MouseDown += (s, e) => DecorationMouseDown(HitTestValues.HTTOP);
            LeftBorderPanel.MouseDown += (s, e) => DecorationMouseDown(HitTestValues.HTLEFT);
            RightBorderPanel.MouseDown += (s, e) => DecorationMouseDown(HitTestValues.HTRIGHT);
            BottomBorderPanel.MouseDown += (s, e) => DecorationMouseDown(HitTestValues.HTBOTTOM);

            SystemLabel.Click += (s, e) => ShowSystemMenu(MouseButtons, PointToScreen(new Point(8, 32)));
            SystemLabel.DoubleClick += (s, e) => Close();

            TitleLabel.MouseDown += TitleLabel_MouseDown;
            TitleLabel.MouseUp += (s, e) => { if (e.Button == MouseButtons.Right) ShowSystemMenu(MouseButtons); };
            TitleLabel.Text = Text;
            TextChanged += (s, e) => TitleLabel.Text = Text;

            var marlett = new Font("Marlett", 8.5f);

            MinimizeLabel.Font = marlett;
            MaximizeLabel.Font = marlett;
            CloseLabel.Font = marlett;
            SystemLabel.Font = marlett;

            MinimizeLabel.Click += (s, e) => WindowState = FormWindowState.Minimized;
            MaximizeLabel.Click += (s, e) => ToggleMaximize();
            SizeChanged += (s, e) => MaximizeLabel.Text = WindowState == FormWindowState.Maximized ? "2" : "1";
            CloseLabel.Click += (s, e) => Close();
        }

        private DateTime systemClickTime = DateTime.MinValue;

        private FormWindowState ToggleMaximize()
        {
            return WindowState = WindowState == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized;
        }

        private DateTime titleClickTime = DateTime.MinValue;

        void TitleLabel_MouseDown(object sender, MouseEventArgs e)
        {
            var clickTime = (DateTime.Now - titleClickTime).TotalMilliseconds;
            if (clickTime < SystemInformation.DoubleClickTime)
                ToggleMaximize();
            else
            {
                titleClickTime = DateTime.Now;
                DecorationMouseDown(HitTestValues.HTCAPTION);
            }
        }
    }
}
