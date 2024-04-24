using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Diplom.Views
{

    public partial class MainWindow : Window
    {
        //private readonly DispatcherTimer blinkTimer;
        //private bool isBlinking = false;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();

            //blinkTimer = new DispatcherTimer();
            //blinkTimer.Interval = TimeSpan.FromSeconds(0.5);
            //blinkTimer.Tick += BlinkTimer_Tick;
            //blinkTimer.Start();

            //Closing += MainWindow_Closing;
        }


        //private void MainWindow_Closing(object sender, CancelEventArgs e)
        //{
        //    blinkTimer.Stop();
        //}

        //private void BlinkTimer_Tick(object sender, EventArgs e)
        //{
        //    isBlinking = !isBlinking;

        //    // Обновление значений свойств в ViewModel
        //    ((ViewModel)DataContext).IsBlinking = isBlinking;
        //}
        //bool isConnected;
        //private async void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    // IP-адрес и порт Arduino сервера
        //    string ipAddress = "192.168.56.154";
        //    int port = 5653;

        //    // Помечаем кнопку как недоступную
        //    Connect_button.IsEnabled = false;

        //    isConnected = false;
        //    int connectionAttempts = 0;

        //    while (!isConnected && connectionAttempts < MaxConnectionAttempts)
        //    {
        //        try
        //        {
        //            // Получение данных в фоновом режиме
        //            var jsonData = await Task.Run(() => GetDataFromServer(ipAddress, port));

        //            if (jsonData != null)
        //            {
        //                // Обновление UI с полученными данными
        //                UpdateUIWithData(jsonData);

        //                var text = humidityTextBlock.Text;

        //                isConnected = true;
        //            }
        //            else
        //            {
        //                Console.WriteLine("Failed to connect to Arduino server. No data received.");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Failed to connect to Arduino server: " + ex.Message);
        //            // Добавим небольшую задержку перед повторной попыткой подключения
        //            Thread.Sleep(ConnectionAttemptDelay);
        //        }

        //        connectionAttempts++;
        //    }

        //    if (!isConnected)
        //    {
        //        Console.WriteLine("Max connection attempts reached. Failed to connect to Arduino server.");
        //    }

        //    // Восстанавливаем доступность кнопки
        //    Connect_button.IsEnabled = true;
        //}

        //private const int MaxConnectionAttempts = 3;
        //private const int ConnectionAttemptDelay = 2000;

        //private string GetDataFromServer(string ipAddress, int port)
        //{
        //    return "10;20;30;40";
        //    try
        //    {
        //        // Создание TCP-клиента
        //        int serverPort = 5653;

        //        using (TcpClient client = new TcpClient())
        //        {
        //            try
        //            {
        //                client.Connect(ipAddress, serverPort);
        //                Console.WriteLine("Connected to server.");

        //                NetworkStream stream = client.GetStream();
        //                string request = "GET /data HTTP/1.1\r\nHost: " + ipAddress + "\r\n\r\n";
        //                byte[] requestBytes = Encoding.ASCII.GetBytes(request);
        //                stream.Write(requestBytes, 0, requestBytes.Length);

        //                byte[] responseBytes = new byte[client.ReceiveBufferSize];
        //                int bytesRead = stream.Read(responseBytes, 0, client.ReceiveBufferSize);
        //                string response = Encoding.ASCII.GetString(responseBytes, 0, bytesRead);

        //                JObject jsonData = JObject.Parse(response);
        //                Console.WriteLine(jsonData);
        //                float temperature = (float)jsonData["Temperature"];
        //                float humidity = (float)jsonData["Humidity"];
        //                float pressure = (float)jsonData["Pressure"];
        //                int lux = (int)jsonData["Lux"];

        //                // Теперь у вас есть отдельные переменные с данными
        //                Console.WriteLine("Temperature: " + temperature + "°C");
        //                Console.WriteLine("Humidity: " + humidity + "%");
        //                Console.WriteLine("Pressure: " + pressure + "hPa");
        //                Console.WriteLine("Lux: " + lux);
        //                string Data = temperature + ";" + humidity + ";" + pressure + ";" + lux;
        //                return Data;
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine("Error: " + ex.Message);
        //            }
        //        }
        //    }
        //    catch (SocketException ex)
        //    {
        //        // Проверка количества попыток подключения
        //        Console.WriteLine("Connection attempt failed. Retrying in " + ConnectionAttemptDelay + "ms...");

        //        // Задержка перед повторной попыткой подключения
        //        Thread.Sleep(ConnectionAttemptDelay);
        //    }

        //    // Все попытки подключения были неудачными
        //    Console.WriteLine("Max connection attempts reached. Failed to connect to Arduino server.");
        //    return null;
        //}

        //private void UpdateUIWithData(string responseData)
        //{
        //    if (responseData == null)
        //    {
        //        // Обработка неудачного подключения
        //        Console.WriteLine("Failed to connect to Arduino server. No data received.");
        //        isConnected = false;
        //        return;
        //    }

        //    // Split the response data into individual values
        //    string[] dataValues = responseData.Split(';');

        //    if (dataValues.Length != 4)
        //    {
        //        Console.WriteLine("Invalid data received from Arduino server.");
        //        return;
        //    }

        //    // Parse the individual values
        //    if (!float.TryParse(dataValues[0], out float temperature) ||
        //        !float.TryParse(dataValues[1], out float humidity) ||
        //        !float.TryParse(dataValues[2], out float pressure) ||
        //        !int.TryParse(dataValues[3], out int lux))
        //    {
        //        Console.WriteLine("Error parsing data received from Arduino server.");
        //        return;
        //    }

        //    humidityTextBlock.Text = $"{humidity}%";
        //    temperatureTextBlock.Text = $"{temperature}°C";
        //    pressureTextBlock.Text = $"{pressure} hPa";
        //    luxTextBlock.Text = $"{lux}";

        //    Console.WriteLine("Data received successfully. Temperature: " + temperature + "°C, Humidity: " + humidity + "%, Pressure: " + pressure + " hPa, Lux: " + lux);
        //}
    }
    //public class ViewModel : INotifyPropertyChanged
    //{
    //    private bool isConnected;
    //    public bool IsConnected
    //    {
    //        get { return isConnected; }
    //        set
    //        {
    //            isConnected = value;
    //            OnPropertyChanged(nameof(IsConnected));
    //        }
    //    }

    //    private bool isConnecting;
    //    public bool IsConnecting
    //    {
    //        get { return isConnecting; }
    //        set
    //        {
    //            isConnecting = value;
    //            OnPropertyChanged(nameof(IsConnecting));
    //        }
    //    }

    //    private bool isBlinking;
    //    public bool IsBlinking
    //    {
    //        get { return isBlinking; }
    //        set
    //        {
    //            isBlinking = value;
    //            OnPropertyChanged(nameof(IsBlinking));
    //        }
    //    }

    //    public Brush StatusForeground
    //    {
    //        get
    //        {
    //            if (!IsConnected)
    //            {
    //                return Brushes.Red;
    //            }
    //            else if (IsConnecting)
    //            {
    //                return Brushes.Yellow;
    //            }
    //            else
    //            {
    //                return IsBlinking ? Brushes.LightGray : Brushes.Green;
    //            }
    //        }
    //    }

    //    public event PropertyChangedEventHandler PropertyChanged;

    //    protected virtual void OnPropertyChanged(string propertyName)
    //    {
    //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    //        if (propertyName == nameof(IsConnected) ||
    //            propertyName == nameof(IsConnecting) ||
    //            propertyName == nameof(IsBlinking))
    //        {
    //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StatusForeground)));
    //        }
    //    }
    //}
}
