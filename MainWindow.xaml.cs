using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using System.IO.Ports;
using System.Windows.Threading;
using System.Diagnostics;



namespace ArduinoUploaderTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SerialPort port;
        public MainWindow()
        {
            InitializeComponent();
            getPortNames();
        }

        void getPortNames()
        {
            String[] ports = SerialPort.GetPortNames();
            foreach (String port in ports)
            {
                PortName.Items.Add(port);
            }
        }


      


        private bool isOpen { get; set; }

        private bool isGenerated { get; set; }

        private void browse_button_Click(object sender, RoutedEventArgs e)
        {
            // Create a new instance of the OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set the default file extension and filter for the dialog
            openFileDialog.DefaultExt = ".xml";
            openFileDialog.Filter = "xml files (*.xml)|*.xml";

            // Show the dialog and check if the user clicked the OK button
            if (openFileDialog.ShowDialog() == true)
            {
                // Get the selected file path from the dialog
                string filePath = openFileDialog.FileName;

                // Validate the file path and perform loading logic
                if (File.Exists(filePath))
                {
                    FileUrl.Text = filePath;
                    file_container.Text = File.ReadAllText(filePath);
                    string sourcePath = Directory.GetCurrentDirectory() + "\\lib\\bloc.jar";
                    string command = "java -jar \"" + sourcePath + "\" \"" + filePath + "\"";
                    run_cmd(command);
                    isGenerated = true;
                }
                else
                {
                    // File does not exist, throw an error
                    MessageBox.Show("Invalid file path. Please select a valid file.");
                }
            }

        }

        private  async void progressValue()
        {
            for(int i = 0; i < 100; i += 3)
            {
                progress.Value = i;
            }
        }
        private bool run_cmd(string command)
        {
            progress.Value = 0;
            Process proc = new Process();
            proc.StartInfo.FileName = "CMD.exe";
            proc.StartInfo.Arguments = "/c " + command;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo.CreateNoWindow = true;
            progressValue();
            Mouse.OverrideCursor = Cursors.Wait;
            proc.Start();
            proc.WaitForExit();
            if (proc.HasExited)
            {
                Mouse.OverrideCursor = null;
                progress.Value = 100;
                return true;
            }
            else
            {
                MessageBox.Show("Error, Please retry !");
                return false;
            }
           
            
            
    
        }

        private void open_button_Click(object sender, RoutedEventArgs e)
        {
            if (PortName.Text=="")
            {
                MessageBox.Show("Please select the right port !");

            }
            else
            {
                isOpen = true;
                open_button.IsEnabled = false;
            }
        }

        private void upload_button_Click(object sender, RoutedEventArgs e)
        {
            if (!isOpen)
            {
                MessageBox.Show("Please verify the Port !");
            }
            else
            {
                
                string filepath = Directory.GetCurrentDirectory() + "\\src-gen\\code\\code.ino";
                // Debug.WriteLine(filepath);
                Debug.WriteLine("port: " + PortName.Text);
               string command = "arduino-cli compile --upload -p " + PortName.Text + " --fqbn arduino:avr:uno \"" + filepath + "\"";
                Debug.WriteLine(command);
                if (run_cmd(command))
                    MessageBox.Show("Code uploaded successfully !");
                else
                    MessageBox.Show("Error while uploading the code to the Arduino board !");
            }
            
        }

        private void close_button_Click(object sender, RoutedEventArgs e)
        {
            if (!open_button.IsEnabled)
            {
                PortName.Text = "";
                open_button.IsEnabled=true;
                isOpen = false;
            }
        }
    }
}
