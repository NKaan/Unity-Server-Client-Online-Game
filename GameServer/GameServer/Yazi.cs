using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameServer
{
    class Yazi
    {

        public static void Bilgi_Yaz(string text, Color? renk = null)
        {

            if(Sabitler.server.richTextBox2.Text.Length != 0)
            {
                Control.CheckForIllegalCrossThreadCalls = false;
                Sabitler.server.richTextBox2.AppendText(Environment.NewLine + text);
                Sabitler.server.richTextBox2.Select(Sabitler.server.richTextBox2.Text.Length - text.Length, text.Length);
                Sabitler.server.richTextBox2.SelectionColor = renk ?? Color.Green;
                Sabitler.server.richTextBox2.SelectionStart = Sabitler.server.richTextBox2.Text.Length;
                Sabitler.server.richTextBox2.ScrollToCaret();
            }

        }

        public static void Hata_Yaz(string text)
        {
            if (Sabitler.server.richTextBox2.Text.Length != 0)
            {
                Control.CheckForIllegalCrossThreadCalls = false;
                Sabitler.server.richTextBox2.AppendText(Environment.NewLine + text);
                Sabitler.server.richTextBox2.Select(Sabitler.server.richTextBox2.Text.Length - text.Length, text.Length);
                Sabitler.server.richTextBox2.SelectionColor = Color.Red;
                Sabitler.server.richTextBox2.SelectionStart = Sabitler.server.richTextBox2.Text.Length;
                Sabitler.server.richTextBox2.ScrollToCaret();
            }
        }

        public static void Log_yaz(string text, Color? renk = null)
        {

            if (Sabitler.server.richTextBox1.Text.Length != 0)
            {
                Control.CheckForIllegalCrossThreadCalls = false;
                Sabitler.server.richTextBox1.AppendText(Environment.NewLine + text);
                Sabitler.server.richTextBox1.Select(Sabitler.server.richTextBox1.Text.Length - text.Length, text.Length);
                Sabitler.server.richTextBox1.SelectionColor = renk ?? Color.Green;
                Sabitler.server.richTextBox1.SelectionStart = Sabitler.server.richTextBox1.Text.Length;
                Sabitler.server.richTextBox1.ScrollToCaret();
            }

        }

        public static void Gelen_Mesaj(string text)
        {
            if (Sabitler.server.richTextBox1.Text.Length != 0)
            {
                Control.CheckForIllegalCrossThreadCalls = false;
                Sabitler.server.richTextBox1.AppendText(Environment.NewLine + text);
                Sabitler.server.richTextBox1.Select(Sabitler.server.richTextBox1.Text.Length - text.Length, text.Length);
                Sabitler.server.richTextBox1.SelectionColor = Color.Black;
                Sabitler.server.richTextBox1.SelectionStart = Sabitler.server.richTextBox1.Text.Length;
                Sabitler.server.richTextBox1.ScrollToCaret();
            }
        }

    }
}
