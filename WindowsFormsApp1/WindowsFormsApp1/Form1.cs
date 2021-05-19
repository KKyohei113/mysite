using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /* BMI を計算するボタンハンドラ */
        private void calculate_Click(object sender, EventArgs e)
        {
            double h, w;
            var data = new List<string>();

            h = int.Parse(tbHeight.Text);
            w = int.Parse(tbWeight.Text);

            result.Text = (w / h / h * 10000.0).ToString();

            data.Add(tbName.Text);
            data.Add(h.ToString());
            data.Add(w.ToString());
            data.Add(result.Text);
            csv_Output(data);
        }

        /* ファイル出力 */
        private void csv_Output(List<string> data)
        {
            csv_Search(data[0], true);

            using (StreamWriter sw = new StreamWriter("test.csv", true))
            {
                sw.WriteLine(String.Join(",", data));
            }
        }

        /* ファイルから読み出しまたはレコードの削除を行う */
        /* <delete> true: 削除 false: 読み出し */
        private string[] csv_Search(string name, bool delete)
        {
            string[] ret = { };
            var sl = new List<string>();

            using (StreamReader sr = new StreamReader("test.csv"))
            {
                if (delete)
                {
                    while (sr.EndOfStream == false)
                    {
                        string s;
                        string[] sa;

                        s = sr.ReadLine();
                        sa = s.Split(',');

                        if (sa[0] != name)
                        {
                            // 指定されたもの以外を書き出すことで削除を実現
                            sl.Add(s);
                        }
                    }
                }
                else
                {
                    while (sr.EndOfStream == false)
                    {
                        string[] s;

                        s = sr.ReadLine().Split(',');

                        if (s[0] == name)
                        {
                            ret = s;
                            break;
                        }
                    }
                }
            }

            if (sl.Count != 0)
            {
                using (StreamWriter sw = new StreamWriter("test.csv"))
                {
                    foreach (var item in sl)
                    {
                        sw.WriteLine(item);
                    }
                }
            }

            return ret;
        }

        /* ファイル内を検索するボタンハンドラ */
        private void search_Click(object sender, EventArgs e)
        {
            string[] s;

            s = csv_Search(tbName.Text, false);

            if (s.Length != 0)
            {
                tbName.Text = s[0];
                tbHeight.Text = s[1];
                tbWeight.Text = s[2];
                result.Text = s[3];
            }
        }
    }
}
