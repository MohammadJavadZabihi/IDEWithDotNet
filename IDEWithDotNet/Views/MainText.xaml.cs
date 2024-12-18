using IDECore.Service;
using Microsoft.UI.Xaml;
using System;
using System.Diagnostics;
using System.IO;

namespace IDEWithDotNet.Views
{
    public sealed partial class MainText : Window
    {
        public MainText()
        {
            this.InitializeComponent();
        }

        private void RunProject_Click(object sender, RoutedEventArgs e)
        {
            string pythonCode = CodeEditor.Text;

            if (string.IsNullOrWhiteSpace(pythonCode))
            {
                OutputConsole.Text = "Error: No code to run!";
                return;
            }

            try
            {
                string tempFilePath = Path.Combine(Path.GetTempPath(), "temp_code.py");
                File.WriteAllText(tempFilePath, pythonCode);

                ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    FileName = "python",
                    Arguments = tempFilePath,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(processInfo))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    if (!string.IsNullOrWhiteSpace(output))
                    {
                        OutputConsole.Text = output;
                    }

                    if (!string.IsNullOrWhiteSpace(error))
                    {
                        OutputConsole.Text += "\n[Error]:\n" + error;
                    }
                }
            }
            catch (Exception ex)
            {
                OutputConsole.Text = $"An error occurred: {ex.Message}";
            }
        }

        private void ClearOutput_Click(object sender, RoutedEventArgs e)
        {
            OutputConsole.Text = "";
        }

        private void FixCode_Click(object sender, RoutedEventArgs e)
        {
            CodeFixer codeFixer = new CodeFixer();
            var codFix = codeFixer.FixCode(CodeEditor.Text);

            if(codFix != CodeEditor.Text)
            {
                CodeEditor.Text = codFix;
            }
        }
    }
}
