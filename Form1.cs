using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace AKOUTool
{
    public partial class Form1 : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
        int nLeftRect,     // x-coordinate of upper-left corner
        int nTopRect,      // y-coordinate of upper-left corner
        int nRightRect,    // x-coordinate of lower-right corner
        int nBottomRect,   // y-coordinate of lower-right corner
        int nWidthEllipse, // height of ellipse
        int nHeightEllipse // width of ellipse
        );
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        public Form1()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            this.StartPosition = FormStartPosition.CenterScreen;
            RegisterHotKey(this.Handle, 1, 0x0004, (uint)Keys.F1);
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312 && !this.Visible) // WM_HOTKEY
            {
                // 按下了快捷鍵，顯示窗口
                this.Show();
                textBox1.Text = "";
            }
            else if (m.Msg == 0x0312 && this.Visible)
            {
                this.Hide();
            }
            base.WndProc(ref m);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //icon
            NotifyIcon notifyIcon = new NotifyIcon();
            notifyIcon.Icon = new Icon("1.ico");
            notifyIcon.Text = "AKOU Tool";
            notifyIcon.Visible = true;
            //menu
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem name = new ToolStripMenuItem("AKOU TOOL");
            ToolStripMenuItem open = new ToolStripMenuItem("Open");
            ToolStripMenuItem exit = new ToolStripMenuItem("Exit");
            name.Enabled = false;
            contextMenuStrip.Items.Add(name);
            contextMenuStrip.Items.Add(open);
            contextMenuStrip.Items.Add(exit);
            notifyIcon.ContextMenuStrip = contextMenuStrip;
            //event
            notifyIcon.Click += NotifyIcon_Click;
            open.Click += Open_Click;
            exit.Click += Exit_Click;
            this.TopMost = true;
        }
        private void Open_Click(object? sender, EventArgs e)
        {
            this.Show();
        }

        private void Exit_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void NotifyIcon_Click(object? sender, EventArgs e)
        {
            if (!this.Visible) this.Show();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            textBox1.ImeMode = ImeMode.Disable;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            search();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                search();
                e.Handled = true;
                Trace.Write("enter");
            }
        }

        private void search()
        {
            var text = textBox1.Text;
            if(text.Length > 0)
            {
                try
                {
                    if (text.StartsWith("yt "))
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = $"https://www.youtube.com/results?search_query={text.Substring(3)}",
                            UseShellExecute = true
                        });
                    }
                    else if (text == "my")
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = $"https://akkou.github.io",
                            UseShellExecute = true
                        });
                    }
                    else if (text.StartsWith("mc "))
                    {
                        if (text.Substring(3) == "start")
                        {
                            Process.Start(new ProcessStartInfo
                            {
                                FileName = "C:/Users/SHALI/Documents/minecraft/",
                                UseShellExecute = true
                            });
                        }
                    }
                    else if (text.StartsWith("-"))
                    {
                        var name = text.Substring(1);
                        switch (name)
                        {
                            case "g":
                                name = "chrome.exe";
                                break;
                            case "vscode":
                                name = "code";
                                break;
                        }

                        Process.Start(new ProcessStartInfo
                        {
                            FileName = name,
                            UseShellExecute = true
                        });
                    }
                    else if (text == "git")
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = "https://github.com/",
                            UseShellExecute = true
                        });
                    }
                    else
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = $"https://www.google.com/search?q={text}",
                            UseShellExecute = true
                        });
                    }
                    textBox1.Text = "";
                    this.Hide();
                }
                catch(Exception ex)
                {
                    Trace.WriteLine(ex.ToString());
                }
            }
        }
    }
}
