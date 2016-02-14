using EasyFullScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace EasyFullScreen {
    public partial class Form1 : Form {
        #region 變數
        IntPtr desktop = user32.GetDesktopWindow();
        IntPtr target = user32.GetDesktopWindow();
        uint preStyle;
        bool m_bFullScreen = false;
        user32.WINDOWPLACEMENT m_wpPrev;
        user32.RECT m_rcFullScreenRect = new user32.RECT();
        #endregion

        #region 視窗生成Code與視窗callback
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            if (target == IntPtr.Zero) {
                MessageBox.Show("No Windows Found");
                return;
            }
            TrimBorder();
        }

        private void button2_Click(object sender, EventArgs e) {
            user32.HideCaret(target);
        }

        private void button3_MouseDown(object sender, MouseEventArgs e) {
            button3.BackColor = Color.Red;
            timer1.Enabled = true;
        }

        private void button3_MouseUp(object sender, MouseEventArgs e) {
            button3.BackColor = Control.DefaultBackColor;
            timer1.Enabled = false;
        }
        private void timer1_Tick(object sender, EventArgs e) {
            Point pret = new Point();
            user32.GetCursorPos(ref pret);
            IntPtr newtarget = user32.WindowFromPoint(pret);
            if (newtarget != target) {
                target = newtarget;
                m_bFullScreen = false;
            }
            StringBuilder WindowText = new StringBuilder(50);
            user32.GetWindowText(target, WindowText, WindowText.Capacity);
            StringBuilder ClassName = new StringBuilder(50);
            user32.GetClassName(target, ClassName, ClassName.Capacity);
            textBox1.Text = WindowText.ToString() + " (" + ClassName + ")";
        }

        private void button2_Click_1(object sender, EventArgs e) {
            FullScreenView();
        }
        #endregion

        #region 自定funciton

        private void SetPostion() { // Set Postion to 0,0
            user32.SetWindowPos(target, 0, 0, 0, 0, 0, user32.SWP_NOZORDER | user32.SWP_NOSIZE | user32.SWP_SHOWWINDOW);
        }

        private void TopMost() {
            uint dwExStyle = user32.GetWindowLong(target, user32.GWL_EXSTYLE);
            if ((dwExStyle & user32.WS_EX_TOPMOST) != 0) {
                user32.SetWindowPos(target, user32.HWND_NOTOPMOST, 0, 0, 0, 0, user32.SWP_NOMOVE | user32.SWP_NOSIZE | user32.SWP_SHOWWINDOW);
            } else {
                user32.SetWindowPos(target, user32.HWND_TOPMOST, 0, 0, 0, 0, user32.SWP_NOMOVE | user32.SWP_NOSIZE | user32.SWP_SHOWWINDOW);
            }
        }

        private void TrimBorder() {
            uint lStyle = user32.GetWindowLong(target, user32.GWL_STYLE);
            if ((lStyle & user32.WS_CAPTION) == 0) {
                lStyle |= (user32.WS_CAPTION | user32.WS_THICKFRAME | user32.WS_SYSMENU);
            } else {
                lStyle &= ~(user32.WS_CAPTION | user32.WS_THICKFRAME | user32.WS_SYSMENU);
            }
            user32.SetWindowLong(target, user32.GWL_STYLE, lStyle);
        }

        private void FullScreenView() {
            user32.RECT rectDesktop;
            user32.WINDOWPLACEMENT wpNew;
            if (!IsFullScreen()) {
                user32.GetWindowPlacement(target, out m_wpPrev);
                user32.GetWindowRect(new HandleRef(this, user32.GetDesktopWindow()), out rectDesktop);
                uint style = user32.GetWindowLong(target, user32.GWL_STYLE);
                uint exstyle = user32.GetWindowLong(target, user32.GWL_EXSTYLE);
                // user32.AdjustWindowRectEx(ref rectDesktop, style, false, exstyle);
                m_rcFullScreenRect = rectDesktop;
                Console.WriteLine(rectDesktop);
                wpNew = m_wpPrev;
                wpNew.ShowCmd = user32.ShowWindowCommands.Normal;
                wpNew.NormalPosition = rectDesktop;
                m_bFullScreen = true;
            } else {
                m_bFullScreen = false;
                wpNew = m_wpPrev;
            }
            user32.SetWindowPlacement(target, ref wpNew);
        }

        private bool IsFullScreen() {
            return m_bFullScreen;
        }

        #endregion

        private void button5_Click(object sender, EventArgs e) {
            ProcessList p = new ProcessList();
            p.ShowDialog();
            if (p.selectProcess == null) return;
            target = p.selectProcess.MainWindowHandle;
            StringBuilder WindowText = new StringBuilder(50);
            user32.GetWindowText(target, WindowText, WindowText.Capacity);
            StringBuilder ClassName = new StringBuilder(50);
            user32.GetClassName(target, ClassName, ClassName.Capacity);
            textBox1.Text = WindowText.ToString() + " (" + ClassName + ")";
        }

        private void button6_Click(object sender, EventArgs e) {
            SetPostion();
        }

        private void button8_Click(object sender, EventArgs e) {
            TopMost();
        }

        private void button7_Click(object sender, EventArgs e) {
            if (checkBox1.Checked) {
                TrimBorder();
                FullScreenView();
            } else {
                FullScreenView();
                TrimBorder();
            }
        }

        private void button4_Click(object sender, EventArgs e) {
            if (checkBox1.Checked) {
                TrimBorder();
                FullScreenView();
                if (m_bFullScreen) {
                    SetPostion();
                }
            } else {
                FullScreenView();
                if (m_bFullScreen) {
                    SetPostion();
                }
                TrimBorder();
            }
        }
    }
}
