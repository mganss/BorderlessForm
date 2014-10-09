using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BorderlessForm
{
    public partial class MainForm : FormBase
    {
        private FormWindowState previousWindowState;

        private Color hoverTextColor = Color.FromArgb(62, 109, 181);

        public Color HoverTextColor
        {
            get { return hoverTextColor; }
            set { hoverTextColor = value; }
        }

        private Color downTextColor = Color.FromArgb(25, 71, 138);

        public Color DownTextColor
        {
            get { return downTextColor; }
            set { downTextColor = value; }
        }

        private Color hoverBackColor = Color.FromArgb(213, 225, 242);

        public Color HoverBackColor
        {
            get { return hoverBackColor; }
            set { hoverBackColor = value; }
        }

        private Color downBackColor = Color.FromArgb(163, 189, 227);

        public Color DownBackColor
        {
            get { return downBackColor; }
            set { downBackColor = value; }
        }

        private Color normalBackColor = Color.White;

        public Color NormalBackColor
        {
            get { return normalBackColor; }
            set { normalBackColor = value; }
        }

        public enum MouseState 
        {
            Normal,
            Hover,
            Down
        }

        protected void SetLabelColors(Control control, MouseState state)
        {
            if (!Focused) return;

            var textColor = ActiveTextColor;
            var backColor = NormalBackColor;

            switch (state)
            {
                case MouseState.Hover:
                    textColor = HoverTextColor;
                    backColor = HoverBackColor;
                    break;
                case MouseState.Down:
                    textColor = DownTextColor;
                    backColor = DownBackColor;
                    break;
            }

            control.ForeColor = textColor;
            control.BackColor = backColor;
        }

        public MainForm()
        {
            InitializeComponent();

            Activated += MainForm_Activated;
            Deactivate += MainForm_Deactivate;

            foreach (var control in new[] { SystemLabel, MinimizeLabel, MaximizeLabel, CloseLabel })
            {
                control.MouseEnter += (s, e) => SetLabelColors((Control)s, MouseState.Hover);
                control.MouseLeave += (s, e) => SetLabelColors((Control)s, MouseState.Normal);
                control.MouseDown += (s, e) => SetLabelColors((Control)s, MouseState.Down);
            }

            TopLeftCornerPanel.MouseDown += (s, e) => DecorationMouseDown(e, HitTestValues.HTTOPLEFT);
            TopRightCornerPanel.MouseDown += (s, e) => DecorationMouseDown(e, HitTestValues.HTTOPRIGHT);
            BottomLeftCornerPanel.MouseDown += (s, e) => DecorationMouseDown(e, HitTestValues.HTBOTTOMLEFT);
            BottomRightCornerPanel.MouseDown += (s, e) => DecorationMouseDown(e, HitTestValues.HTBOTTOMRIGHT);

            TopBorderPanel.MouseDown += (s, e) => DecorationMouseDown(e, HitTestValues.HTTOP);
            LeftBorderPanel.MouseDown += (s, e) => DecorationMouseDown(e, HitTestValues.HTLEFT);
            RightBorderPanel.MouseDown += (s, e) => DecorationMouseDown(e, HitTestValues.HTRIGHT);
            BottomBorderPanel.MouseDown += (s, e) => DecorationMouseDown(e, HitTestValues.HTBOTTOM);

            SystemLabel.MouseDown += SystemLabel_MouseDown;
            SystemLabel.MouseUp += SystemLabel_MouseUp;

            TitleLabel.MouseDown += TitleLabel_MouseDown;
            TitleLabel.MouseUp += (s, e) => { if (e.Button == MouseButtons.Right && TitleLabel.ClientRectangle.Contains(e.Location)) ShowSystemMenu(MouseButtons); };
            TitleLabel.Text = Text;
            TextChanged += (s, e) => TitleLabel.Text = Text;

            var marlett = new Font("Marlett", 8.5f);

            MinimizeLabel.Font = marlett;
            MaximizeLabel.Font = marlett;
            CloseLabel.Font = marlett;
            SystemLabel.Font = marlett;

            MinimizeLabel.MouseClick += (s, e) => { if (e.Button == MouseButtons.Left) WindowState = FormWindowState.Minimized; };
            MaximizeLabel.MouseClick += (s, e) => { if (e.Button == MouseButtons.Left) ToggleMaximize(); };
            previousWindowState = MinMaxState;
            SizeChanged += MainForm_SizeChanged;
            CloseLabel.MouseClick += (s, e) => Close(e);
        }

        void SystemLabel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) ShowSystemMenu(MouseButtons);
        }

        private DateTime systemClickTime = DateTime.MinValue;
        private DateTime systemMenuCloseTime = DateTime.MinValue;

        void SystemLabel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var clickTime = (DateTime.Now - systemClickTime).TotalMilliseconds;
                if (clickTime < SystemInformation.DoubleClickTime)
                    Close();
                else
                {
                    systemClickTime = DateTime.Now;
                    if ((systemClickTime - systemMenuCloseTime).TotalMilliseconds > 200)
                    {
                        SetLabelColors(SystemLabel, MouseState.Normal);
                        ShowSystemMenu(MouseButtons, PointToScreen(new Point(8, 32)));
                        systemMenuCloseTime = DateTime.Now;
                    }
                }
            }
        }

        void Close(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) Close();
        }

        void DecorationMouseDown(MouseEventArgs e, HitTestValues h)
        {
            if (e.Button == MouseButtons.Left) DecorationMouseDown(h);
        }

        private Color activeBorderColor = Color.FromArgb(43, 87, 154);

        public Color ActiveBorderColor
        {
            get { return activeBorderColor; }
            set { activeBorderColor = value; }
        }

        private Color inactiveBorderColor = Color.FromArgb(131, 131, 131);

        public Color InactiveBorderColor
        {
            get { return inactiveBorderColor; }
            set { inactiveBorderColor = value; }
        }

        void MainForm_Deactivate(object sender, EventArgs e)
        {
            SetBorderColor(InactiveBorderColor);
            SetTextColor(InactiveTextColor);
        }

        void MainForm_Activated(object sender, EventArgs e)
        {
            SetBorderColor(ActiveBorderColor);
            SetTextColor(ActiveTextColor);
        }

        private Color activeTextColor = Color.FromArgb(68, 68, 68);

        public Color ActiveTextColor
        {
            get { return activeTextColor; }
            set { activeTextColor = value; }
        }

        private Color inactiveTextColor = Color.FromArgb(177, 177, 177);

        public Color InactiveTextColor
        {
            get { return inactiveTextColor; }
            set { inactiveTextColor = value; }
        }
        
        protected void SetBorderColor(Color color)
        {
            TopLeftCornerPanel.BackColor = color;
            TopBorderPanel.BackColor = color;
            TopRightCornerPanel.BackColor = color;
            LeftBorderPanel.BackColor = color;
            RightBorderPanel.BackColor = color;
            BottomLeftCornerPanel.BackColor = color;
            BottomBorderPanel.BackColor = color;
            BottomRightCornerPanel.BackColor = color;
        }

        protected void SetTextColor(Color color)
        {
            SystemLabel.ForeColor = color;
            TitleLabel.ForeColor = color;
            MinimizeLabel.ForeColor = color;
            MaximizeLabel.ForeColor = color;
            CloseLabel.ForeColor = color;
        }

        void MainForm_SizeChanged(object sender, EventArgs e)
        {
            var maximized = MinMaxState == FormWindowState.Maximized;
            MaximizeLabel.Text = maximized ? "2" : "1";

            var panels = new[] { TopLeftCornerPanel, TopRightCornerPanel, BottomLeftCornerPanel, BottomRightCornerPanel,
                TopBorderPanel, LeftBorderPanel, RightBorderPanel, BottomBorderPanel };

            foreach (var panel in panels)
            {
                panel.Visible = !maximized;
            }

            if (previousWindowState != MinMaxState)
            {
                if (maximized)
                {
                    SystemLabel.Left = 0;
                    SystemLabel.Top = 0;
                    CloseLabel.Left += RightBorderPanel.Width;
                    CloseLabel.Top = 0;
                    MaximizeLabel.Left += RightBorderPanel.Width;
                    MaximizeLabel.Top = 0;
                    MinimizeLabel.Left += RightBorderPanel.Width;
                    MinimizeLabel.Top = 0;
                    TitleLabel.Left -= LeftBorderPanel.Width;
                    TitleLabel.Width += LeftBorderPanel.Width + RightBorderPanel.Width;
                    TitleLabel.Top = 0;
                }
                else if (previousWindowState == FormWindowState.Maximized)
                {
                    SystemLabel.Left = LeftBorderPanel.Width;
                    SystemLabel.Top = TopBorderPanel.Height;
                    CloseLabel.Left -= RightBorderPanel.Width;
                    CloseLabel.Top = TopBorderPanel.Height;
                    MaximizeLabel.Left -= RightBorderPanel.Width;
                    MaximizeLabel.Top = TopBorderPanel.Height;
                    MinimizeLabel.Left -= RightBorderPanel.Width;
                    MinimizeLabel.Top = TopBorderPanel.Height;
                    TitleLabel.Left += LeftBorderPanel.Width;
                    TitleLabel.Width -= LeftBorderPanel.Width + RightBorderPanel.Width;
                    TitleLabel.Top = TopBorderPanel.Height;
                }

                previousWindowState = MinMaxState;
            }
        }

        private FormWindowState ToggleMaximize()
        {
            return WindowState = WindowState == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized;
        }

        private DateTime titleClickTime = DateTime.MinValue;

        void TitleLabel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
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
}
