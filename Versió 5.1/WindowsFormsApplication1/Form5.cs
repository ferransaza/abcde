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
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form5 : Form
    {
        delegate void delegado_para_escribir_en_el_label(string mensaje);
        delegate void delegado_para_sacar_valor_de_el_label(int mensaje);
        delegate void delegado_para_desbloquear_los_botones();
        public class carta
        {
            public int fila;
            public int columna;
            public int contado = 0;
        }
        public class cartas_en_la_mesa
        {
            public carta[] cartas = new carta[5];
        }
        public class mis_cartas
        {
            public carta[] cartas = new carta[2];
        }
        public class cartas_a_contar
        {
            public carta[] cartas = new carta[7];
        }
        Socket server;
        Thread atender;
        string jugador_en_contra;
        string yo;
        int fichas1 = 10000;
        int fichas2 = 10000;
        int apuesta = 0;
        int mi_apuesta = 10;
        int bote = 0;
        int apuesta_mínima = 10;
        int partida;
        int que_cartas_mostrar = 0;
        int dealer;
        cartas_en_la_mesa cartas_mesa = new cartas_en_la_mesa();
        mis_cartas miscartas = new mis_cartas();
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
        }

        public Form5(string jugador1, string contrincante, Socket socket_servidor)
        {
            InitializeComponent();
            yo = jugador1;
            jugador_en_contra = contrincante;
            label7.Text = yo;
            label6.Text = jugador_en_contra;
            fichas1 = 10000;
            fichas2 = 10000;
            server = socket_servidor;
            ThreadStart ts = delegate { atender_mensajes_servidor(); };
            atender = new Thread(ts);
            atender.Start();
            this.ShowDialog();
        }

        public void pasar_cartas_en_la_mesa(string cartas)
        {
            string[] carta = cartas.Split('/');
            int i = 0;
            int j = 0;
            int foc = 0;
            int k = 0;
            foreach (string str in carta)
            {
                if ((str != "55") && (str != "60") && (str != "65"))
                {
                    if (i < 5)
                    {
                        if (foc == 0)
                        {
                            cartas_mesa.cartas[i] = new carta();
                            cartas_mesa.cartas[i].fila = Convert.ToInt32(str);
                            foc = 1;
                        }
                        else
                        {
                            cartas_mesa.cartas[i].columna = Convert.ToInt32(str);
                            foc = 0;
                            i++;
                        }
                    }
                    else if (j < 2)
                    {
                        if (foc == 0)
                        {
                            miscartas.cartas[j] = new carta();
                            miscartas.cartas[j].fila = Convert.ToInt32(str);
                            foc = 1;
                        }
                        else
                        {
                            miscartas.cartas[j].columna = Convert.ToInt32(str);
                            foc = 0;
                            j++;
                        }
                    }
                    else if (k == 0)
                    {
                        if (Convert.ToInt32(str) == 1)
                        {
                            dealer = 1;
                            label4.Text = Convert.ToString(apuesta_mínima);
                            label13.ForeColor = System.Drawing.Color.White;
                            label14.ForeColor = System.Drawing.Color.Black;
                            this.bloquear_botones();
                        }
                        else
                        {
                            dealer = 0;
                            label14.ForeColor = System.Drawing.Color.White;
                            label13.ForeColor = System.Drawing.Color.Black;
                        }
                        k = 1;
                    }
                    else if (k == 1)
                    {
                        partida = Convert.ToInt32(str);
                    }
                }
            }
        }

        public void pasar_mis_cartas(string cartas)
        {
            string[] carta = cartas.Split('/');
            int i = 0;
            int foc = 0;
            foreach (string str in carta)
            {
                if (str == "60")
                {
                }
                else
                {
                    if (foc == 0)
                    {
                        miscartas.cartas[i] = new carta();
                        miscartas.cartas[i].fila = Convert.ToInt32(str);
                        foc = 1;
                    }
                    else
                    {
                        miscartas.cartas[i].columna = Convert.ToInt32(str);
                        foc = 0;
                        i++;
                    }
                }
            }
        }

        private void mostrar_cartas()
        {
            pictureBox10.Image = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\Cartas\" + "Carta_del_revés.jpg");
            pictureBox10.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox11.Image = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\Cartas\" + "Carta_del_revés.jpg");
            pictureBox11.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox8.Image = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\Cartas\" + "Carta_del_revés.jpg");
            pictureBox8.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox7.Image = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\Cartas\" + "Carta_del_revés.jpg");
            pictureBox7.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox6.Image = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\Cartas\" + "Carta_del_revés.jpg");
            pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.Image = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\Cartas\" + "Carta_del_revés.jpg");
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\Cartas\" + "Carta_del_revés.jpg");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.Image = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\Cartas\" + "Carta_del_revés.jpg");
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.Image = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\Cartas\" + "Carta_del_revés.jpg");
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void mostrar_cartas_iniciales()
        {
            pictureBox8.Image = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\Cartas\" + "fila-" + cartas_mesa.cartas[0].fila + "-columna-" + cartas_mesa.cartas[0].columna + ".jpg");
            pictureBox8.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox7.Image = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\Cartas\" + "fila-" + cartas_mesa.cartas[1].fila + "-columna-" + cartas_mesa.cartas[1].columna + ".jpg");
            pictureBox7.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox6.Image = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\Cartas\" + "fila-" + cartas_mesa.cartas[2].fila + "-columna-" + cartas_mesa.cartas[2].columna + ".jpg");
            pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.Image = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\Cartas\" + "fila-" + miscartas.cartas[0].fila + "-columna-" + miscartas.cartas[0].columna + ".jpg");
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.Image = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\Cartas\" + "fila-" + miscartas.cartas[1].fila + "-columna-" + miscartas.cartas[1].columna + ".jpg");
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void mostrar_cuartra_carta()
        {
            pictureBox1.Image = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\Cartas\" + "fila-" + cartas_mesa.cartas[3].fila + "-columna-" + cartas_mesa.cartas[3].columna + ".jpg");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void mostrar_quinta_carta()
        {
            pictureBox3.Image = new Bitmap(Path.GetDirectoryName(Application.ExecutablePath) + @"\Cartas\" + "fila-" + cartas_mesa.cartas[4].fila + "-columna-" + cartas_mesa.cartas[4].columna + ".jpg");
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void actualizar_tabla()
        {
            apuesta = 0;
            mi_apuesta = 10;
            bote = 0;
            apuesta_mínima = 10;
            que_cartas_mostrar = 0;
            this.Invoke(new delegado_para_escribir_en_el_label(escribir_label9), new object[] { Convert.ToString(bote) });
            this.Invoke(new delegado_para_escribir_en_el_label(escribir_label8), new object[] { Convert.ToString(fichas1) });
            this.Invoke(new delegado_para_escribir_en_el_label(escribir_label11), new object[] { Convert.ToString(fichas2) });
            this.Invoke(new delegado_para_escribir_en_el_label(escribir_label10), new object[] { Convert.ToString(apuesta) });
            this.Invoke(new delegado_para_escribir_en_el_label(escribir_label4), new object[] { Convert.ToString(apuesta_mínima) });

        }

        private void atender_mensajes_servidor()
        {
            while (true)
            {
                byte[] msg = new byte[80];
                server.Receive(msg);
                string mensaje = Encoding.ASCII.GetString(msg).Split('\0')[0];
                string[] codigo = mensaje.Split('/');
                if (codigo[0] != "")
                {
                    if (Convert.ToInt32(codigo[0]) == 55)
                    {
                        this.pasar_cartas_en_la_mesa(mensaje);
                        this.mostrar_cartas();
                    }
                    if (Convert.ToInt32(codigo[0]) == 60)
                    {
                        if (Convert.ToInt32(codigo[1]) == 1)
                        {
                            partida = Convert.ToInt32(codigo[2]);
                            label4.Text = Convert.ToString(apuesta_mínima);
                            label13.ForeColor = System.Drawing.Color.White;
                            label14.ForeColor = System.Drawing.Color.Black;
                        }
                        else
                        {
                            partida = Convert.ToInt32(codigo[2]);
                            label14.ForeColor = System.Drawing.Color.White;
                            label13.ForeColor = System.Drawing.Color.Black;
                            this.bloquear_botones();
                        }
                    }
                    if (Convert.ToInt32(codigo[0]) == 70)
                    {
                        this.desbloquear_botones();
                        apuesta_mínima = 10;
                        mi_apuesta = apuesta_mínima;
                        apuesta = Convert.ToInt32(codigo[1]);
                        this.Invoke(new delegado_para_escribir_en_el_label(escribir_label10), new object[] { codigo[1] });
                        if (apuesta > apuesta_mínima)
                        {
                            this.Invoke(new delegado_para_escribir_en_el_label(escribir_label4), new object[] { codigo[1] });
                            mi_apuesta = apuesta;
                            apuesta_mínima = apuesta;
                        }
                        if (Convert.ToInt32(codigo[1]) != 0)
                        {
                            this.Invoke(new delegado_para_desbloquear_los_botones(bloquear_boton3));
                        }
                        fichas2 = Convert.ToInt32(codigo[2]);
                        this.Invoke(new delegado_para_escribir_en_el_label(escribir_label11), new object[] { codigo[2] });
                        bote = Convert.ToInt32(codigo[3]);
                        this.Invoke(new delegado_para_escribir_en_el_label(escribir_label9), new object[] { codigo[3] });
                    }
                    if (Convert.ToInt32(codigo[0]) == 75)
                    {
                        if (que_cartas_mostrar == 0)
                        {
                            this.mostrar_cartas_iniciales();
                        }
                        else if (que_cartas_mostrar == 1)
                        {
                            this.mostrar_cuartra_carta();
                        }
                        else if (que_cartas_mostrar == 2)
                        {
                            this.mostrar_quinta_carta();
                        }
                        else if (que_cartas_mostrar == 3)
                        {
                            this.contar_puntos();
                            this.bloquear_botones();
                        }
                        que_cartas_mostrar = que_cartas_mostrar + 1;
                    }
                    if (Convert.ToInt32(codigo[0]) == 97)
                    {
                        MessageBox.Show("Ha habido un empate.");
                        fichas1 = fichas1 + bote / 2;
                        fichas2 = fichas2 + bote / 2;
                        if (dealer == 1)
                        {
                            dealer = 0;
                            label13.ForeColor = System.Drawing.Color.White;
                            label14.ForeColor = System.Drawing.Color.Black;
                            this.desbloquear_botones();
                        }
                        else
                        {
                            dealer = 1;
                            label14.ForeColor = System.Drawing.Color.White;
                            label13.ForeColor = System.Drawing.Color.Black;
                            this.bloquear_botones();
                        }
                        string mensaje2 = "12/" + partida;
                        byte[] msg2 = System.Text.Encoding.ASCII.GetBytes(mensaje2);
                        server.Send(msg2);
                        actualizar_tabla();
                    }
                    if (Convert.ToInt32(codigo[0]) == 100)
                    {
                        MessageBox.Show("Felicidades, has ganado.");
                        fichas1 = fichas1 + bote;
                        if (dealer == 1)
                        {
                            dealer = 0;
                            label13.ForeColor = System.Drawing.Color.White;
                            label14.ForeColor = System.Drawing.Color.Black;
                            this.desbloquear_botones();
                        }
                        else
                        {
                            dealer = 1;
                            label14.ForeColor = System.Drawing.Color.White;
                            label13.ForeColor = System.Drawing.Color.Black;
                            this.bloquear_botones();
                        }
                        string mensaje2 = "12/" + partida;
                        byte[] msg2 = System.Text.Encoding.ASCII.GetBytes(mensaje2);
                        server.Send(msg2);
                        actualizar_tabla();
                    }
                    if (Convert.ToInt32(codigo[0]) == 95)
                    {
                        MessageBox.Show("Vaya, parece que has perdido, más suerte para la proxima.");
                        fichas2 = fichas2 + bote;
                        if (dealer == 1)
                        {
                            dealer = 0;
                            label13.ForeColor = System.Drawing.Color.White;
                            label14.ForeColor = System.Drawing.Color.Black;
                            this.desbloquear_botones();
                        }
                        else
                        {
                            dealer = 1;
                            label14.ForeColor = System.Drawing.Color.White;
                            label13.ForeColor = System.Drawing.Color.Black;
                            this.bloquear_botones();
                        }
                        string mensaje2 = "12/" + partida;
                        byte[] msg2 = System.Text.Encoding.ASCII.GetBytes(mensaje2);
                        server.Send(msg2);
                        actualizar_tabla();
                    }
                    if (Convert.ToInt32(codigo[0]) == 80)
                    {
                        apuesta = Convert.ToInt32(codigo[1]);
                        this.Invoke(new delegado_para_escribir_en_el_label(escribir_label10), new object[] { codigo[1] });
                        fichas2 = Convert.ToInt32(codigo[2]);
                        this.Invoke(new delegado_para_escribir_en_el_label(escribir_label11), new object[] { codigo[2] });
                        bote = Convert.ToInt32(codigo[3]);
                        this.Invoke(new delegado_para_escribir_en_el_label(escribir_label9), new object[] { codigo[3] });
                    }
                    if (Convert.ToInt32(codigo[0]) == 120)
                    {
                        MessageBox.Show(jugador_en_contra + "ha abandonado la partida.");
                        this.Close();
                    }
                }
            }
        }

        private void bloquear_boton1()
        {
            this.button1.Hide();
        }

        private void bloquear_boton2()
        {
            this.button2.Hide();
        }

        private void bloquear_boton3()
        {
            this.button3.Hide();
        }

        private void bloquear_boton4()
        {
            this.button4.Hide();
        }

        private void bloquear_boton5()
        {
            this.button5.Hide();
        }

        private void bloquear_boton6()
        {
            this.button6.Hide();
        }

        private void bloquear_boton7()
        {
            this.button7.Hide();
        }

        private void desbloquear_boton1()
        {
            this.button1.Show();
        }

        private void desbloquear_boton2()
        {
            this.button2.Show();
        }

        private void desbloquear_boton3()
        {
            this.button3.Show();
        }

        private void desbloquear_boton4()
        {
            this.button4.Show();
        }

        private void desbloquear_boton5()
        {
            this.button5.Show();
        }

        private void desbloquear_boton6()
        {
            this.button6.Show();
        }

        private void desbloquear_boton7()
        {
            this.button7.Show();
        }

        private void escribir_label4(string mensaje)
        {
            label4.Text = mensaje;
        }

        private void escribir_label8(string mensaje)
        {
            label8.Text = mensaje;
        }

        private void escribir_label9(string mensaje)
        {
            label9.Text = mensaje;
        }

        private void escribir_label10(string mensaje)
        {
            label10.Text = mensaje;
        }

        private void escribir_label11(string mensaje)
        {
            label11.Text = mensaje;
        }

        private void bloquear_botones()
        {
            this.Invoke(new delegado_para_desbloquear_los_botones(bloquear_boton1));
            this.Invoke(new delegado_para_desbloquear_los_botones(bloquear_boton2));
            this.Invoke(new delegado_para_desbloquear_los_botones(bloquear_boton3));
            this.Invoke(new delegado_para_desbloquear_los_botones(bloquear_boton4));
            this.Invoke(new delegado_para_desbloquear_los_botones(bloquear_boton5));
            this.Invoke(new delegado_para_desbloquear_los_botones(bloquear_boton6));
            this.Invoke(new delegado_para_desbloquear_los_botones(bloquear_boton7));
        }

        private void desbloquear_botones()
        {
            this.Invoke(new delegado_para_desbloquear_los_botones(desbloquear_boton1));
            this.Invoke(new delegado_para_desbloquear_los_botones(desbloquear_boton2));
            this.Invoke(new delegado_para_desbloquear_los_botones(desbloquear_boton3));
            this.Invoke(new delegado_para_desbloquear_los_botones(desbloquear_boton4));
            this.Invoke(new delegado_para_desbloquear_los_botones(desbloquear_boton5));
            this.Invoke(new delegado_para_desbloquear_los_botones(desbloquear_boton6));
            this.Invoke(new delegado_para_desbloquear_los_botones(desbloquear_boton7));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            apuesta = Convert.ToInt32(label10.Text);
            int miapuesta = Convert.ToInt32(label4.Text);
            if (apuesta > miapuesta)
            {
                miapuesta = apuesta;
                label4.Text = Convert.ToString(apuesta);
                mi_apuesta = miapuesta;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int miapuesta = Convert.ToInt32(label4.Text);
            int misfichas = Convert.ToInt32(label8.Text);
            if (miapuesta + 5 < misfichas)
            {
                miapuesta = miapuesta + 5;
                label4.Text = Convert.ToString(miapuesta);
                mi_apuesta = miapuesta;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int miapuesta = Convert.ToInt32(label4.Text);
            if (miapuesta - 5 >= apuesta_mínima)
            {
                miapuesta = miapuesta - 5;
                label4.Text = Convert.ToString(miapuesta);
                mi_apuesta = miapuesta;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label4.Text = Convert.ToString(0);
            mi_apuesta = 0;
            string mensaje = "9/" + "0" + "/" + label8.Text + "/" + label9.Text + "/" + partida;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            this.bloquear_botones();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string mensaje = "10/" + partida;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            this.bloquear_botones();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label4.Text = label8.Text;
            mi_apuesta = fichas1;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if ((mi_apuesta == apuesta) && (mi_apuesta != 0))
            {
                bote = bote + 2 * mi_apuesta;
                this.Invoke(new delegado_para_escribir_en_el_label(escribir_label9), new object[] { Convert.ToString(bote) });
                fichas1 = fichas1 - mi_apuesta;
                this.Invoke(new delegado_para_escribir_en_el_label(escribir_label8), new object[] { Convert.ToString(fichas1) });
                this.Invoke(new delegado_para_escribir_en_el_label(escribir_label10), new object[] { "0" });
                this.Invoke(new delegado_para_escribir_en_el_label(escribir_label4), new object[] { "10" });
                string mensaje = "9/" + mi_apuesta + "/" + fichas1 + "/" + bote + "/" + partida;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
            else
            {
                fichas1 = fichas1 - mi_apuesta;
                this.Invoke(new delegado_para_escribir_en_el_label(escribir_label8), new object[] { Convert.ToString(fichas1) });
                this.Invoke(new delegado_para_escribir_en_el_label(escribir_label10), new object[] { Convert.ToString(mi_apuesta)});
                this.Invoke(new delegado_para_escribir_en_el_label(escribir_label4), new object[] { "10" });
                apuesta = mi_apuesta;
                string mensaje = "9/" + mi_apuesta + "/" + fichas1 + "/" + bote + "/" + partida;
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
            }
            this.bloquear_botones();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string mensaje = "13/" + partida;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            this.Close();
        }

        private void contar_puntos()
        {
            int puntuacion = 0;
            cartas_a_contar ordenadas = new cartas_a_contar();
            cartas_a_contar desordenadas = new cartas_a_contar();
            for (int i = 0; i < 7; i++)
            {
                if (i < 2)
                {
                    desordenadas.cartas[i] = new carta();
                    desordenadas.cartas[i].columna = miscartas.cartas[i].columna;
                    desordenadas.cartas[i].fila = miscartas.cartas[i].fila;
                }
                else
                {
                    desordenadas.cartas[i] = new carta();
                    desordenadas.cartas[i].columna = cartas_mesa.cartas[i - 2].columna;
                    desordenadas.cartas[i].fila = cartas_mesa.cartas[i - 2].fila;
                }
            }
            for (int i = 0; i < 7; i++)
            {
                for (int j = i + 1; j < 7; j++)
                {
                    if (desordenadas.cartas[j].columna < desordenadas.cartas[i].columna)
                    {
                        ordenadas.cartas[i] = new carta();
                        ordenadas.cartas[i].columna = desordenadas.cartas[j].columna;
                        ordenadas.cartas[i].fila = desordenadas.cartas[j].fila;
                    }
                }
            }

            int contador_pareja = 0;
            int contador_trio = 0;
            int eliminar_la_doble_pareja = 0;
            int columna_pareja_eliminada = 0;
            int carta_mas_alta1 = 0, carta_mas_alta2 = 0;
            for (int i = 6; i < -1; i--)
            {
                if (i > 0)
                {
                    if (ordenadas.cartas[i].columna == ordenadas.cartas[i - 1].columna)
                    {
                        if (i > 1)
                        {
                            if (ordenadas.cartas[i - 1].columna == ordenadas.cartas[i - 2].columna)
                            {
                                if (i > 2)
                                {
                                    if (ordenadas.cartas[i - 2].columna == ordenadas.cartas[i - 3].columna)
                                    {
                                        puntuacion = 160 + ordenadas.cartas[i].columna; //POKER
                                        for (int j = 0; j < 2; j++)
                                        {
                                            if (miscartas.cartas[j].columna == ordenadas.cartas[i].columna)
                                            {
                                                miscartas.cartas[j].contado = 1;
                                            }
                                            else
                                            {
                                                miscartas.cartas[j].contado = 0;
                                            }
                                        }
                                    }
                                    else 
                                    {
                                        if (contador_trio == 0)
                                        {
                                            contador_trio++;
                                            for (int j = 0; j < 2; j++)
                                            {
                                                if (miscartas.cartas[j].columna == ordenadas.cartas[i].columna)
                                                {
                                                    miscartas.cartas[j].contado = 1;
                                                }
                                            }
                                            if (contador_pareja == 2)
                                            {
                                                puntuacion = eliminar_la_doble_pareja + puntuacion + 70 + ordenadas.cartas[i].columna;
                                                for (int j = 0; j < 2; j++)
                                                {
                                                    if (miscartas.cartas[j].columna == columna_pareja_eliminada)
                                                    {
                                                        miscartas.cartas[j].contado = 0;
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                puntuacion = puntuacion + 70 + ordenadas.cartas[i].columna; //TRIO
                                            }
                                        }
                                    }
                                }
                            }
                            else if (((contador_pareja < 2) && (contador_trio == 0))|| ((contador_pareja < 1) && (contador_trio == 1)))
                            {
                                if (eliminar_la_doble_pareja == 0)
                                {
                                    eliminar_la_doble_pareja = puntuacion + 20 + ordenadas.cartas[i].columna;
                                }
                                else
                                {
                                    columna_pareja_eliminada = ordenadas.cartas[i].columna;
                                }
                                puntuacion = puntuacion + 20 + ordenadas.cartas[i].columna; //Pareja o doble pareja
                                for(int j= 0; j < 2; j++)
                                {
                                    if (miscartas.cartas[j].columna == ordenadas.cartas[i].columna)
                                    {
                                        miscartas.cartas[j].contado = 1;
                                    }
                                }
                                contador_pareja++;
                            }
                        }
                    }
                    if (i > 3)
                    {
                        if ((ordenadas.cartas[i].columna == (ordenadas.cartas[i - 1].columna + 1)) && (ordenadas.cartas[i].columna == (ordenadas.cartas[i - 2].columna + 2)) && (ordenadas.cartas[i].columna == (ordenadas.cartas[i - 3].columna + 3)) && (ordenadas.cartas[i].columna == (ordenadas.cartas[i - 4].columna + 4)))
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                carta_mas_alta1 = 0;
                                carta_mas_alta2 = 0;
                            }
                            puntuacion = 120 + ordenadas.cartas[i].columna;
                            if ((ordenadas.cartas[i].fila == ordenadas.cartas[i - 1].fila) && (ordenadas.cartas[i].fila == ordenadas.cartas[i - 2].fila) && (ordenadas.cartas[i].fila == ordenadas.cartas[i - 3].fila) && (ordenadas.cartas[i].fila == ordenadas.cartas[i - 4].fila))
                            {
                                puntuacion = 180 + ordenadas.cartas[i].columna;
                            }
                        }
                        if ((ordenadas.cartas[i].fila == ordenadas.cartas[i - 1].fila) && (ordenadas.cartas[i].fila == ordenadas.cartas[i - 2].fila) && (ordenadas.cartas[i].fila == ordenadas.cartas[i - 3].fila) && (ordenadas.cartas[i].fila == ordenadas.cartas[i - 4].fila))
                        {
                            puntuacion = 140 + ordenadas.cartas[i].columna;
                            for (int j = 0; j < 2; j++)
                            {
                                carta_mas_alta1 = 0;
                                carta_mas_alta2 = 0;
                            }
                        }
                    }
                }
            }
            if (miscartas.cartas[0].columna > miscartas.cartas[1].columna)
            {
                if (miscartas.cartas[0].contado == 0)
                {
                    carta_mas_alta1 = miscartas.cartas[0].columna;
                    if (miscartas.cartas[1].contado == 0)
                    {
                        carta_mas_alta2 = miscartas.cartas[1].columna;
                    }
                }
                else if (miscartas.cartas[1].contado == 0)
                {
                    carta_mas_alta1 = miscartas.cartas[1].columna;
                    carta_mas_alta2 = 0;
                }
            }
            else if (miscartas.cartas[0].columna < miscartas.cartas[1].columna)
            {
                if (miscartas.cartas[1].contado == 0)
                {
                    carta_mas_alta1 = miscartas.cartas[1].columna;
                    if (miscartas.cartas[0].contado == 0)
                    {
                        carta_mas_alta2 = miscartas.cartas[0].columna;
                    }
                }
                else if (miscartas.cartas[0].contado == 0)
                {
                    carta_mas_alta1 = miscartas.cartas[0].columna;
                    carta_mas_alta2 = 0;
                }
            }
            string mensaje = "11/" + puntuacion + "/" + carta_mas_alta1 + "/" + carta_mas_alta2+ "/" + partida;
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
        }
    }
}