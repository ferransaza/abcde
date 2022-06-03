using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form4 : Form
    {
        Socket server;
        int conectado = 0;
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (conectado == 0)
            {
                IPAddress direc = IPAddress.Parse("192.168.56.102");
                IPEndPoint ipep = new IPEndPoint(direc, 9093);
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    server.Connect(ipep);//Intentamos conectar el socket
                    this.BackColor = Color.Green;
                    MessageBox.Show("Conectado.");
                    conectado = 1;
                }
                catch (SocketException)
                {
                    //Si hay excepcion imprimimos error y salimos del programa con return 
                    MessageBox.Show("No he podido conectar con el servidor");
                    return;
                }
            }

            string mensaje = "1/" + nombre.Text + "/*/" + contraseña.Text;
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            byte[] msg2 = new byte[80];
            server.Receive(msg2);
            string mensaje2 = Encoding.ASCII.GetString(msg2).Split('\0')[0];
            string[] codigo = mensaje2.Split('/');
            if (Convert.ToInt32(codigo[0]) == 40)
            {
                MessageBox.Show("Bienvenido " + codigo[1] + ".");
                this.Hide();
                Form1 a = new Form1(sender, e, server, nombre.Text);
                a.ShowDialog();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form2 b = new Form2(this);
            b.Show();
        }

        public void Registrar(string mensaje)
        {
            if (conectado == 0)
            {
                IPAddress direc = IPAddress.Parse("192.168.56.102");
                IPEndPoint ipep = new IPEndPoint(direc, 9042);
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    server.Connect(ipep);//Intentamos conectar el socket
                    this.BackColor = Color.Green;
                    MessageBox.Show("Conectado.");
                    conectado = 1;
                }
                catch (SocketException)
                {
                    //Si hay excepcion imprimimos error y salimos del programa con return 
                    MessageBox.Show("No he podido conectar con el servidor");
                    return;
                }
            }

            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            byte[] msg2 = new byte[80];
            server.Receive(msg2);
            string mensaje2 = Encoding.ASCII.GetString(msg2).Split('\0')[0];
            string[] codigo = mensaje2.Split('/');
            if (Convert.ToInt32(codigo[0]) == 45)
            {
                MessageBox.Show(codigo[1]);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (conectado == 0)
            {
                IPAddress direc = IPAddress.Parse("192.168.56.102");
                IPEndPoint ipep = new IPEndPoint(direc, 9090);
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    server.Connect(ipep);//Intentamos conectar el socket
                    this.BackColor = Color.Green;
                    MessageBox.Show("Conectado.");
                    conectado = 1;
                }
                catch (SocketException)
                {
                    //Si hay excepcion imprimimos error y salimos del programa con return 
                    MessageBox.Show("No he podido conectar con el servidor");
                    return;
                }
                if (conectado == 1)
                {
                    string mensaje = "15/" + nombre.Text + "/" + contraseña.Text;
                    // Enviamos al servidor el nombre tecleado
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                    byte[] msg2 = new byte[80];
                    server.Receive(msg2);
                    string mensaje2 = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                    string[] codigo = mensaje2.Split('/');
                    if (Convert.ToInt32(codigo[0]) == 40)
                    {
                        MessageBox.Show("Te has dado de baja.");
                    }
                }
            }
        }
    }
}