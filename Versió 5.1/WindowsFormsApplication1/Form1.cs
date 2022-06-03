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
    public partial class Form1 : Form
    {
        delegate void delegado_para_escribir_en_el_dataGridView(string mensaje);
        delegate void delegado_para_hacer_el_chat_visible();
        delegate void delegado_para_escribir_en_el_chat(string mensaje);
        Socket server;
        Thread atender;
        string jugador_en_contra;
        string yo;
        Form5 form5;
        public Form1(object sender, EventArgs e, Socket socket1, string jugador)
        {
            InitializeComponent();
            server = socket1;
            ThreadStart ts = delegate { atender_mensajes_servidor(sender, e); };
            atender = new Thread(ts);
            atender.Start();
            yo = jugador;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Controls.Add(dataGridView1);
            dataGridView1.ColumnCount = 1;
            dataGridView1.Name = "Conectados:";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Líder.Checked)
            {
                string mensaje = "3/";
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
            else if (Empates.Checked)
            {
                string mensaje = "4/";
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
            else if (Palizas.Checked)
            {
                string mensaje = "5/";
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
            else
                
                MessageBox.Show("Seleccione una consulta.");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Mensaje de desconexión
            string mensaje = "0/";
        
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            atender.Abort();
            // Nos desconectamos
            server.Shutdown(SocketShutdown.Both);
            server.Close();
            dataGridView1.Rows.Clear();
            for (int i = 1; i < dataGridView1.RowCount - 1; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value.ToString() == "")
                {
                    dataGridView1.Rows.RemoveAt(i);
                }
            }
        }

        private void mostrar_conecados(string lista_de_conectados)
        {
            string[] codigo = lista_de_conectados.Split('/');
            dataGridView1.Rows.Clear();
            foreach (string str in codigo)
            {
                if (str == "10")
                {
                }
                else
                    dataGridView1.Rows.Add(str);
            }
            for (int i = 1; i < dataGridView1.RowCount - 1; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value.ToString() == "")
                {
                    dataGridView1.Rows.RemoveAt(i);
                }
            }
        }

        private void atender_mensajes_servidor(object sender, EventArgs e)
        {
            while (true)
            {
                byte[] msg = new byte[80];
                server.Receive(msg);
                string mensaje = Encoding.ASCII.GetString(msg).Split('\0')[0];
                string[] codigo = mensaje.Split('/');
                if (Convert.ToInt32(codigo[0]) == 10)
                {
                    this.Invoke(new delegado_para_escribir_en_el_dataGridView(mostrar_conecados), new object[] {mensaje});
                }
                if (Convert.ToInt32(codigo[0]) == 5)
                {
                    jugador_en_contra = codigo[1];
                    Form3 partida = new Form3();
                    partida.pasar_informacion(this, codigo[1]);
                }
                if (Convert.ToInt32(codigo[0]) == 15)
                {
                    MessageBox.Show("El jugador que te envió la solicitud se ha desconectado.");
                }
                if (Convert.ToInt32(codigo[0]) == 20)
                {
                    if (codigo[1] == "1")
                    {
                        MessageBox.Show("El usuario " + codigo[2] + " ha aceptado tu solicitud de partida.");
                        jugador_en_contra = codigo[2];
                        Form5 partida = new Form5(yo, jugador_en_contra, server);
                        form5 = partida;
                        this.Invoke(new delegado_para_hacer_el_chat_visible(chat_visible), new object[] {});
                    }
                    if (codigo[1] == "0")
                    {
                        MessageBox.Show("El usuario " + codigo[2] + " no se encuentra o se ha desconectado.");
                    }
                    if (codigo[1] == "2")
                    {
                        MessageBox.Show("El usuario " + codigo[2] + " ha rechazado tu solicitud de partida.");
                    }
                }
                if (Convert.ToInt32(codigo[0]) == 25)
                {
                    MessageBox.Show("Han habido " + codigo[1] + " empates.");
                }
                if (Convert.ToInt32(codigo[0]) == 30)
                {
                    MessageBox.Show("El Líder es: " + codigo[1]);
                }
                if (Convert.ToInt32(codigo[0]) == 35)
                {
                    MessageBox.Show("En " + codigo[1] + " partidos ha habido una diferencia de mas de 30 puntos.");
                }
                if (Convert.ToInt32(codigo[0]) == 40)
                {
                    MessageBox.Show("Bienvenido " + codigo[1] + ".");
                }
                if (Convert.ToInt32(codigo[0]) == 45)
                {
                    MessageBox.Show(codigo[1]);
                }
                if (Convert.ToInt32(codigo[0]) == 50)
                {
                    string comentario;
                    comentario = codigo[1] + ": " + codigo[2];
                    this.Invoke(new delegado_para_escribir_en_el_chat(escribir_en_el_chat), new object[] { comentario });
                }
            }
        }

        public void pasar_decision (int decision, string amfitrion)
        {
            string enviar = "7/" + decision + "//" + amfitrion;
            byte[] msg1 = System.Text.Encoding.ASCII.GetBytes(enviar);
            server.Send(msg1);
            if (decision == 1)
            {
                Form5 partida = new Form5(yo, amfitrion, server);
                form5 = partida;
                this.Invoke(new delegado_para_hacer_el_chat_visible(chat_visible), new object[] { });
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string usuario = textBox1.Text;
            string mensaje = "6/" + usuario;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                string mensaje = "8/" + jugador_en_contra + "//" + textBox2.Text;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
                string comentario;
                comentario = "Yo: " + textBox2.Text;
                this.Invoke(new delegado_para_escribir_en_el_chat(escribir_en_el_chat), new object[] { comentario });
                textBox2.Clear();
            }
        }

        private void chat_visible()
        {
            groupBox3.Visible = true;
        }

        private void escribir_en_el_chat(string comentario)
        {
            if (textBox3.Text == "")
                textBox3.Text = comentario;
            else
                textBox3.Text = textBox3.Text + "\r\n" + comentario;
        }
    }
}