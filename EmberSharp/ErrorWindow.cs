using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Windows.Forms;

namespace EmberSharp
{
    public partial class ErrorWindow : Form
    {
        private readonly Exception e;
        private readonly IList<string> logs;
        private readonly string version;

        public ErrorWindow(Exception e, IList<string> logs, string version = "unknown version")
        {
            InitializeComponent();
            this.e = e;
            this.logs = logs;
            this.version = version;
            listBox1.Items.AddRange(BuildReport().ToArray());
        }

        private static string Lineifier(string str)
        {
            if (str.Length >= 50)
            {
                return str;
            }

            str = str.PadLeft(str.Length + (50 - str.Length) / 2, '-');
            str = str.PadRight(50, '-');
            return str;
        }

        private static string[] StackTraceSplitter(string str)
        {
            var splits = str.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            return splits;
        }

        private List<string> BuildReport()
        {
            var list = new List<string>();
            list.Add(Lineifier("EMM Error Report"));
            list.Add("");
            list.Add(Lineifier("Exception"));

            if (e != null)
            {
                list.Add("Exception: " + e.GetType().Name);
                list.Add("Msg: " + e.Message);
                list.Add("Stacktrace:");
                list.AddRange(StackTraceSplitter(e.StackTrace));
                if (e.InnerException != null)
                {
                    list.Add("Inner Exception: " + e.InnerException.GetType().Name);
                    list.Add("Msg: " + e.InnerException.Message);
                    list.Add("Stacktrace:");
                    list.AddRange(StackTraceSplitter(e.InnerException.StackTrace));
                }
            }

            list.Add(Lineifier("Log"));

            if (logs != null)
            {
                foreach (var log in logs)
                {
                    var splits = log.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                    list.AddRange(splits);
                }
            }
            list.Add(Lineifier("The End"));
            return list;
        }

        private void buttonClipboard_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(string.Join(Environment.NewLine, BuildReport()));
        }

        private void buttonSaveFile_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                saveFileDialog.Title = "Save your file";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllLines(saveFileDialog.FileName, BuildReport());
                }
            }
        }

        private void buttonReportIssue_Click(object sender, EventArgs e)
        {
            //var body = HttpUtility.UrlEncode(string.Join(Environment.NewLine, BuildReport()));
            var versUrlified = HttpUtility.UrlEncode(version);
            var body = HttpUtility.UrlEncode($"What version I was using: {versUrlified}\r\n\r\nWhat I was doing when the exception occured:\r\n\r\nPaste the error log:");
            var url = "https://github.com/dyster/Ember-MM-Newscraper/issues/new?title=Unhandled+Exception+Report&body=" + body;
            Process.Start(url);
        }
    }
}