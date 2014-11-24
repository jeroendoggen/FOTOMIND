﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
using System.IO.Ports;

namespace FotomindUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SerialPortInterface SerialPort1 = new SerialPortInterface();
        public ObservableCollection<ArduinoCommands> ArdCmds { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            listCommands.DataContext = ArdCmds;
            ArdCmds = new ObservableCollection<ArduinoCommands>()
            {
                new ArduinoCommands()
                {
                    Title = "Take Picture",
                    Command = "Cmd for taking picture with arduino"
                },
                new ArduinoCommands()
                {
                    Title = "Do something else",
                    Command = "Blablibloebla"
                }
            };
            
            
        }

        private void btnComPortRefresh_Click(object sender, RoutedEventArgs e)
        {
            cboComPort.Items.Clear();
            addComs();
        }

        private void btnOpenPort_Click(object sender, RoutedEventArgs e)
        {
            if (cboComPort.SelectedItem.ToString() != "No Ports")
            {
                SerialPort1.Open();
            }
            
        }

        private void btnClosePort_Click(object sender, RoutedEventArgs e)
        {
            SerialPort1.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            addComs();
            listCommands.DataContext = ArdCmds;
        }

        private void addComs()
        {
            // read avaiable COM Ports: 
            string[] Portnames = SerialPort.GetPortNames();
            if (Portnames == null)
            {
                MessageBox.Show("There are no Com Ports detected!");
                this.Close();
            }

            foreach (string portname in Portnames)
            {
                cboComPort.Items.Add(portname);
            }
            //cboComPort.Items.Add(Portnames);
            try
            {
                cboComPort.Text = Portnames[0];
            }
            catch (IndexOutOfRangeException)
            {
                cboComPort.Items.Add("No Ports");
            }
            if (cboBaudRate.Items.Count == 0)
            {
                foreach (int BR in SerialPortInterface.BaudRates)
                {
                    cboBaudRate.Items.Add(BR);
                }
            }
            cboBaudRate.Text = "9600";
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SerialPort1.Close();
        }

        private void cboComPort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboComPort.SelectedItem != null)
            {
                SerialPort1.PortName = cboComPort.SelectedItem.ToString();
            }
        }

        private void cboBaudRate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SerialPort1.BaudRate = Convert.ToInt32(cboBaudRate.SelectedItem);
        }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show(listCommands.SelectedItem.ToString());
            }
            catch (Exception)
            {
                
                MessageBox.Show("No command was selected");
            }

        }

        private void listCommands_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void listCommands_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            listCommands.SelectedItem = 0;
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            SerialPort1.Send(tbNewCmd.Text); //slechts tijdelijk, tot listbox werkt!
        }
    }
}