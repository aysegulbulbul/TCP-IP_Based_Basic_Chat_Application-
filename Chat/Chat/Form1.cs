using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;


namespace Chat
{
    public partial class Form1 : Form
    {
        // TcpClient sınıfı, TCP bağlantısı oluşturmak için kullanılır
        private TcpClient client;

        // StreamReader ve StreamWriter, ağ üzerinden veri okumak ve yazmak için kullanılır
        public StreamReader STR;
        public StreamWriter STW;

        // Gelen ve gönderilecek mesajları tutmak için string değişkenler
        public string recieve;
        public string TextToSend;

        // Formun oluşturulması
        public Form1()
        {
            InitializeComponent();


            // Yerel IP adreslerini alır
            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName());

            // IPv4 adresini TextBox'a yazar
            foreach (IPAddress address in localIP)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    ServerIPtextBox.Text = address.ToString();
                }
            }
        }

        // Server başlatma. (START)
        private void Startbutton_Click(object sender, EventArgs e)
        {
            // TcpListener, belirtilen IP adresi ve portta bağlantı dinlemek için kullanılır
            TcpListener listener = new TcpListener(IPAddress.Any, int.Parse(ServerPorttextBox.Text));
            listener.Start();

            // Bağlantıyı kabul et ve client nesnesine atar.
            client = listener.AcceptTcpClient();

            // Client'tan veri okumak ve yazmak için StreamReader ve StreamWriter oluşturur
            STR = new StreamReader(client.GetStream());
            STW = new StreamWriter(client.GetStream());
            STW.AutoFlush = true; // Otomatik olarak yazma işlemini temizler

            // Arka planları başlatır
            backgroundWorker1.RunWorkerAsync();
            backgroundWorker2.WorkerSupportsCancellation = true;
        }

        // Client CONNECT
        private void Connectbutton_Click(object sender, EventArgs e)
        {
            // TcpClient nesnesi oluştur
            client = new TcpClient();
            // Bağlanılacak server'ın IP adresi ve portu
            IPEndPoint Ipend = new IPEndPoint(IPAddress.Parse(ClientIPtextBox.Text), int.Parse(ClientPorttextBox.Text));

            try
            {
                // Server'a bağlan
                client.Connect(Ipend);
                // Bağlantı başarılıysa mesaj ekrana yazar
                ChatScreentextBox.AppendText("Connect to Server" + Environment.NewLine);
                // Client'tan veri okumak ve yazmak için StreamReader ve StreamWriter oluşturur
                STW = new StreamWriter(client.GetStream());
                STR = new StreamReader(client.GetStream());

                STW.AutoFlush = true; // Otomatik olarak yazma işlemini temizler
                // Arka plan işçilerini başlat
                backgroundWorker1.RunWorkerAsync();
                backgroundWorker2.WorkerSupportsCancellation = true;
            }
            catch (Exception ex)
            {
                // Hata durumunda mesaj kutusu gösterir
                MessageBox.Show(ex.Message.ToString());
            }
        }

        // BackgroundWorker: Gelen mesajları dinler
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // Client bağlantısı açıkken çalışır
            while (client.Connected)
            {
                try
                {
                    // Mesajı okur
                    recieve = STR.ReadLine();

                    // UI thread'inde kontrolün yapılması gerektiği için Invoke kullanılır.
                    this.ChatScreentextBox.Invoke(new MethodInvoker(delegate ()
                    {
                        // Mesajı Chat ekranına ekler
                        ChatScreentextBox.AppendText("Zehra: " + recieve + Environment.NewLine);
                    }));
                    recieve = ""; // Mesajı sıfırlar
                }
                catch (Exception ex)
                {
                    // Hata durumunda mesaj kutusu gösterir.
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        // BackgrundWorker: Gönderilecek mesajları işler
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            // Client bağlıysa çalışır
            if (client.Connected)
            {
                // Mesajı yazar
                STW.WriteLine(TextToSend);
                // UI thread'inde kontrolün yapılması gerektiği için Invoke kullanılır.
                this.ChatScreentextBox.Invoke(new MethodInvoker(delegate ()
                {
                    // Mesajı Chat ekranına ekler
                    ChatScreentextBox.AppendText("Ayşegül: " + TextToSend + Environment.NewLine);
                }));
            }
            else
            {
                // Bağlantı yoksa hata mesajı gösterir
                MessageBox.Show("Sending Failed");
            }

            // BackgrıundWorkerı iptal eder.
            backgroundWorker2.CancelAsync();
        }

        // SEND MESSAGE (GÖNDER) BUTTON
        private void Sendbutton_Click(object sender, EventArgs e)
        {
            // Mesaj kutusu boş değilse çalış
            if (MessageTextBox.Text != "")
            {
                // Gönderilecek mesajı al
                TextToSend = MessageTextBox.Text;
                // Arka plan işçisini çalıştır
                backgroundWorker2.RunWorkerAsync();
            }
            // Mesaj kutusunu temizle
            MessageTextBox.Text = "";
        }
    }
}
