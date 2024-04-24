using Diplom.Commands;
using Diplom.Properties;
using Diplom.Views;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Diplom
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Properties
        private const int _maxConnectionAttempts = 3;
        private const int _connectionAttemptDelay = 2000;
        private Task _task;

        private string ipAddress;
        public string IpAddress
        {
            get { return ipAddress; }
            set
            {
                ipAddress = value;
                OnPropertyChanged(nameof(IpAddress));

                Settings.Default.ArduinoIp = ipAddress;
                Settings.Default.Save();
            }
        }

        private bool isConnected;
        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                isConnected = value;
                IsConnecting = false;
                OnPropertyChanged(nameof(IsConnected));
                OnPropertyChanged(nameof(IsConnecting));
            }
        }

        private bool isConnecting;
        public bool IsConnecting
        {
            get { return isConnecting; }
            set
            {
                isConnecting = value;
                OnPropertyChanged(nameof(IsConnecting));
            }
        }

        private bool connectEnabled;
        public bool ConnectEnabled
        {
            get { return connectEnabled; }
            set
            {
                connectEnabled = value;
                OnPropertyChanged(nameof(ConnectEnabled));
            }
        }

        private string humidity;
        public string Humidity
        {
            get { return humidity; }
            set
            {
                humidity = value;
                OnPropertyChanged(nameof(Humidity));
            }
        }

        private string temperature;
        public string Temperature
        {
            get { return temperature; }
            set
            {
                temperature = value;
                OnPropertyChanged(nameof(Temperature));
            }
        }

        private string pressure;
        public string Pressure
        {
            get { return pressure; }
            set
            {
                pressure = value;
                OnPropertyChanged(nameof(Pressure));
            }
        }

        private string lux;
        public string Lux
        {
            get { return lux; }
            set
            {
                lux = value;
                OnPropertyChanged(nameof(Lux));
            }
        }
        #endregion
        public MainWindowViewModel()
        {
            ConnectEnabled = true;
            ReadSettings();
        }
        private async Task StartDataUpdate()
        {
            await DataUpdaterWorker_DoWorkAsync();
        }
        private async Task DataUpdaterWorker_DoWorkAsync()
        {
            Console.WriteLine("Starting data update...");
            string ipAddress = IpAddress;
            int port = 5653;

            while (true) // Бесконечный цикл для постоянных запросов данных
            {
                try
                {
                    using (TcpClient client = new TcpClient())
                    {
                        await client.ConnectAsync(ipAddress, port);
                        Console.WriteLine("Connected to server.");

                        NetworkStream stream = client.GetStream();
                        string request = "GET /data HTTP/1.1\r\nHost: " + ipAddress + "\r\n\r\n";
                        byte[] requestBytes = Encoding.ASCII.GetBytes(request);
                        await stream.WriteAsync(requestBytes, 0, requestBytes.Length);

                        byte[] responseBytes = new byte[client.ReceiveBufferSize];
                        int bytesRead = await stream.ReadAsync(responseBytes, 0, client.ReceiveBufferSize);
                        string response = Encoding.ASCII.GetString(responseBytes, 0, bytesRead);

                        var jsonData = GetDataFromServer(ipAddress, port);
                        // Обновляем данные и отправляем их в UI
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            if (jsonData != null)
                            {
                                UpdateUIWithData(jsonData);
                                IsConnected = true; 
                            }
                            else
                            {
                                Console.WriteLine("Failed to connect to Arduino server. No data received.");
                            }
                        });

                        await Task.Delay(TimeSpan.FromSeconds(30)); 
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to connect to Arduino server: " + ex.Message);
                    await Task.Delay(_connectionAttemptDelay);
                }
            }
        }

        // Обработчик для обновления UI из фоновой задачи


        private RelayCommand connectClick;
        public RelayCommand ConnectClick
        {
            get
            {
                return connectClick ??
                  (connectClick = new RelayCommand(async obj =>
                  {
                      bool isIpValid = IPAddress.TryParse(IpAddress, out _);
                      if (!isIpValid)
                      {
                          MessageBox.Show("Невірна IP адресса!");
                          return;
                      }

                      int port = 5653;

                      ConnectEnabled = false;
                      IsConnected = false;
                      IsConnecting = true;
                      int connectionAttempts = 0;

                      while (!IsConnected && connectionAttempts < _maxConnectionAttempts)
                      {
                          try
                          {
                              var jsonData = await Task.Run(() => GetDataFromServer(IpAddress, port));

                              if (jsonData != null)
                              {
                                  UpdateUIWithData(jsonData);
                                  IsConnected = true;
                              }
                              else
                              {
                                  Console.WriteLine("Failed to connect to Arduino server. No data received.");
                              }
                          }
                          catch (Exception ex)
                          {
                              Console.WriteLine("Failed to connect to Arduino server: " + ex.Message);
                              await Task.Delay(_connectionAttemptDelay);
                          }

                          connectionAttempts++;
                      }

                      if (!IsConnected)
                      {
                          IsConnecting = false;
                          Console.WriteLine("Max connection attempts reached. Failed to connect to Arduino server.");
                          MessageBox.Show("Невдалося під'єднатися до станції");
                      }

                      ConnectEnabled = true;

                      _task = new Task(async () => { await StartDataUpdate(); });

                      _task.Start();

                      //StartDataUpdate();
                  }));
            }
        }


        private RelayCommand updateClick;
        public RelayCommand UpdateClick
        {
            get
            {
                return updateClick ??
                  (updateClick = new RelayCommand(async obj =>
                  {
                      // TODO
                  }
                  ));
            }
        }

        private RelayCommand analiticClick;
        public RelayCommand AnaliticClick
        {
            get
            {
                return analiticClick ??
                  (analiticClick = new RelayCommand(obj =>
                  {
                      AnalyticWindow analyticWindow = new AnalyticWindow();
                      analyticWindow.Show();
                  }
                  ));
            }
        }

        private string GetDataFromServer(string ipAddress, int port)
        {/*
            return "10;20;30;40";*/

            try
            {
                // Создание TCP-клиента
                int serverPort = 5653;

                using (TcpClient client = new TcpClient())
                {
                    try
                    {
                        client.Connect(ipAddress, serverPort);
                        Console.WriteLine("Connected to server.");

                        NetworkStream stream = client.GetStream();
                        string request = "GET /data HTTP/1.1\r\nHost: " + ipAddress + "\r\n\r\n";
                        byte[] requestBytes = Encoding.ASCII.GetBytes(request);
                        stream.Write(requestBytes, 0, requestBytes.Length);

                        byte[] responseBytes = new byte[client.ReceiveBufferSize];
                        int bytesRead = stream.Read(responseBytes, 0, client.ReceiveBufferSize);
                        string response = Encoding.ASCII.GetString(responseBytes, 0, bytesRead);

                        JObject jsonData = JObject.Parse(response);
                        Console.WriteLine(jsonData);
                        float temperature = (float)jsonData["Temperature"];
                        float humidity = (float)jsonData["Humidity"];
                        float pressure = (float)jsonData["Pressure"];
                        int lux = (int)jsonData["Lux"];

                        // Теперь у вас есть отдельные переменные с данными
                        Console.WriteLine("Temperature: " + temperature + "°C");
                        Console.WriteLine("Humidity: " + humidity + "%");
                        Console.WriteLine("Pressure: " + pressure + "hPa");
                        Console.WriteLine("Lux: " + lux);
                        string Data = temperature + ";" + humidity + ";" + pressure + ";" + lux;
                        return Data;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Connection attempt failed. Retrying in " + _connectionAttemptDelay + "ms...");

                Thread.Sleep(_connectionAttemptDelay);
            }
            return null;
        }

        private void UpdateUIWithData(string responseData)
        {
            if (responseData == null)
            {
                Console.WriteLine("Failed to connect to Arduino server. No data received.");
                IsConnected = false;
                return;
            }

            string[] dataValues = responseData.Split(';');

            if (dataValues.Length != 4)
            {
                Console.WriteLine("Invalid data received from Arduino server.");
                return;
            }

            if (!float.TryParse(dataValues[0], out float temperature) ||
                !float.TryParse(dataValues[1], out float humidity) ||
                !float.TryParse(dataValues[2], out float pressure) ||
                !int.TryParse(dataValues[3], out int lux))
            {
                Console.WriteLine("Error parsing data received from Arduino server.");
                return;
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                Humidity = $"{humidity}%";
                Temperature = $"{temperature}°C";
                Pressure = $"{pressure} hPa";
                Lux = $"{lux}";

                Console.WriteLine("Data received successfully. Temperature: " + temperature + "°C, Humidity: " + humidity + "%, Pressure: " + pressure + " hPa, Lux: " + lux);
            });
        }

        private void ReadSettings()
        {
            IpAddress = Settings.Default.ArduinoIp;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}