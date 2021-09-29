using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocket4Net;
using System.Diagnostics;

namespace Twitchコメビュ
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.IsUpgrade == false)
            {
                // Upgradeを実行する
                Properties.Settings.Default.Upgrade();

                // 「Upgradeを実行した」という情報を設定する
                Properties.Settings.Default.IsUpgrade = true;

                // 現行バージョンの設定を保存する
                Properties.Settings.Default.Save();
            }
            //大きさとかの調節
            this.Width = Properties.Settings.Default.w;
            this.Height = Properties.Settings.Default.h;
            this.DoubleBuffered = true;

            //読み取り専用
            dataGridView1.ReadOnly = true;

            //左っ側の余白を消す
            dataGridView1.RowHeadersVisible = false;

            //カラムの数設定
            dataGridView1.ColumnCount = 2;

            //隠し列設定
            dataGridView1.Columns[1].Visible = false;

            
            //端に来たら改行する
            dataGridView1.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            //端まで引き伸ばす
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            //よくわからん
            dataGridView1.AllowUserToAddRows = false;

            //色設定
            dataGridView1.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;

            

            dataGridView1.ContextMenuStrip = this.contextMenuStrip1;

            dataGridView1.RowsDefaultCellStyle.Font = Properties.Settings.Default.font;

            if(Properties.Settings.Default.oauth == "")
            {
                Form f = new Form2();
                f.ShowDialog();
            }
            System.Type dgvtype = typeof(DataGridView);
            // プロパティ設定の取得
            System.Reflection.PropertyInfo dgvPropertyInfo =
            dgvtype.GetProperty(
            "DoubleBuffered", System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.NonPublic);


            // 対象のDataGridViewにtrueをセットする
            dgvPropertyInfo.SetValue(dataGridView1, true, null);
        }
        private void GetWebtest()
        {
            var ws = new WebSocket("wss://irc-ws.chat.twitch.tv/");
            ws.MessageReceived += Ws_MessageReceived;
            ws.Opened += Ws_Opened;
            ws.Closed += Ws_Closed;
            ws.Open();
            while (true)
            {
                if(ws.State == WebSocketState.Open)
                {
                    //ws.Send("CAP REQ :twitch.tv/tags twitch.tv/commands");
                    ws.Send("PASS " + Properties.Settings.Default.oauth);
                    Console.WriteLine("t1");
                    
                    
                    ws.Send("NICK " + Properties.Settings.Default.name);
                    Console.WriteLine("t2");
                    Task.Delay(40000);
                    ws.Send("JOIN #" + textBox1.Text);
                    Console.WriteLine("t3");
                    break;
                }
            }
            Console.WriteLine("cone");
            while (true)
            {
                if(ws.State == WebSocketState.Open)
                {

                }
                else
                {

                }
                if(button1.Text == "接続")
                {
                    break;
                }
            }
            ws.Close();
        }

        private void Ws_Closed(object sender, EventArgs e)
        {
            if (button1.Text == "切断")
            {

                for (int i = 1; i < 10; i++)
                {
                    toolStripStatusLabel1.Text = "再接続を試みます。";
                    try
                    {
                        button1.Text = "切断";
                        Task tk = Task.Run(() =>
                        {
                            GetWebtest();
                        });
                        toolStripStatusLabel1.Text = "再接続しました";
                        break;
                    }
                    catch (Exception)
                    {
                        toolStripStatusLabel1.Text = "失敗しました" + i + "回目";
                    }

                }
                //MessageBox.Show("切断されました、再接続します。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else { 

            }
        }

        private void Ws_Opened(object sender, EventArgs e)
        {
            
        }

        private void Ws_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Task tsk = Task.Run(() =>
            {
                if(e.Message.Contains("\r\n:"))
                {
                    int inde = e.Message.IndexOf("\r\n");
                    string m1 = e.Message.Substring(0, inde);
                    string m2 = e.Message.Substring(inde);
                    add(m1);
                    add(m2);
                }
                else
                {
                    add(e.Message.Replace("\r\n",""));
                }
                Console.WriteLine(e.Message.Replace("\n",""));
            });
        }
        private void add(string m)
        {
            Task Tt = Task.Run(() =>
            {
            if (m.Contains("PRIVMSG"))
            {
                int nameindex = m.IndexOf("!");
                string name = m.Substring(1,nameindex - 1);
                Console.WriteLine(name);
                int intIndexParseSign = m.IndexOf(" :");
                string m2 = m.Substring(intIndexParseSign + 2);
                Task tk = Task.Run(() =>
                {
                    
                    this.Invoke((MethodInvoker)(() => dataGridView1.Rows.Add(m2,name)));
                    //this.Invoke((MethodInvoker)(() => dataGridView1.Rows.Add(m2,name)));
                    if (!(スクロールの一時停止ToolStripMenuItem.Checked))
                    {
                        this.Invoke((MethodInvoker)(() => dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1));
                    }
                });
            }

            });

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(button1.Text == "接続")
            {
                textBox1.Enabled = false;
                button1.Text = "切断";
                Task tss = Task.Run(() =>
                {
                    GetWebtest();
                });
            }
            else
            {
                textBox1.Enabled = true;
                button1.Text = "接続";
            }
            
        }

        private void 終了XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Form2();
            f.ShowDialog();

        }

        private void 最新までスクロールToolStripMenuItem_Click(object sender, EventArgs e)
        {
            スクロールの一時停止ToolStripMenuItem.Checked = false;
            スクロールの一時停止ToolStripMenuItem.Text = "スクロールの一時停止";
            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
        }

        private void スクロールの一時停止ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (スクロールの一時停止ToolStripMenuItem.Checked)
            {
                スクロールの一時停止ToolStripMenuItem.Checked = false;
                スクロールの一時停止ToolStripMenuItem.Text = "スクロールの一時停止";
            }
            else
            {
                スクロールの一時停止ToolStripMenuItem.Checked = true;
                スクロールの一時停止ToolStripMenuItem.Text = "スクロールの再開";
                
            }
        }
        private void コピーCToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Clipboard.SetText(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                if(e.ColumnIndex >= 0 && e.RowIndex >= 0)
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[e.RowIndex].Selected = true;
                }
            }

        }

        private void twitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://twitter.com/messages/compose?recipient_id=957429065190817795");
        }

        private void 配布ページToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://mahiro.ml/twitchcv/");
        }

        private void mailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("mailto://mahiropc.ch@gmail.com");
        }

        private void twitterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Process.Start("https://twitter.com/mahiro_m4a");
        }

        private void twitchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://twitch.tv/mahiro_m4a");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.w = this.Width;
            Properties.Settings.Default.h = this.Height;
            Properties.Settings.Default.Save();

        }

        private void ユーザーページを開くToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.twitch.tv/" + dataGridView1.SelectedRows[0].Cells[1].Value.ToString());
        }
    }
}
