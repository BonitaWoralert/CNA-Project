﻿using System;
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
using Packets;


namespace CNA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Client m_client;
        string Username = "";
        public MainWindow(Client client)
        {
            InitializeComponent();
            m_client = client;
            //Nickname.Text = 
            
        }

        public void UpdateChatBox(string message)
        {
            ChatBox.Dispatcher.Invoke(() =>
            {
                ChatBox.Text += Username + ": " + message + Environment.NewLine;
                ChatBox.ScrollToEnd();
            });
        }
        private void Button_Click(object sender, EventArgs e) //send button
        {
            m_client.SendMessage(new ChatMessagePacket(MessageText.Text));
            UpdateChatBox(MessageText.Text);
            MessageText.Clear();
        }
        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e) //nickname field
        {
            Username = Nickname.Text;
        }
        #region ?
        private void sendMessageButton_Click(object sender, EventArgs e)
        {
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }


        private void TextBox_TextChanged_3(object sender, TextChangedEventArgs e)
        {

        }
        #endregion
    }
}
