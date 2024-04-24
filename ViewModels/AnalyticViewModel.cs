using Diplom.Commands;
using Diplom.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using Firebase.Database;
using System.Threading.Tasks;
using Diplom.Commands;
using Diplom.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using Diplom.Properties;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;


namespace Diplom.ViewModels
{
    public class AnalyticViewModel : INotifyPropertyChanged
    {
        private readonly FirebaseClient firebase1, firebase2, firebase3, firebase4;

        public ObservableCollection<SensorData> SensorDataCollection { get; private set; }
        public ObservableCollection<AnalyticModel> Analytics { get; private set; }

        private string textFromFunction;
        public string TextFromFunction
        {
            get { return textFromFunction; }
            set
            {
                textFromFunction = value;
                OnPropertyChanged("TextFromFunction");
            }
        }

        public AnalyticViewModel()
        {
            firebase1 = new FirebaseClient("https://diplomarduin-default-rtdb.firebaseio.com");
            firebase2 = new FirebaseClient("https://diplomarduin-default-rtdb.firebaseio.com");
            firebase3 = new FirebaseClient("https://diplomarduin-default-rtdb.firebaseio.com");
            firebase4 = new FirebaseClient("https://diplomarduin-default-rtdb.firebaseio.com");

            SensorDataCollection = new ObservableCollection<SensorData>();
            FetchDataFromFirebase();
            Analytics = new ObservableCollection<AnalyticModel>
            {
                
            };
        }
        private async void FetchDataFromFirebase()
        {
            try
            {
                var luxDataTask = firebase2.Child("lux").OnceAsync<int>();
                var temperatureDataTask = firebase1.Child("temperature").OnceAsync<float>();
                var humidityDataTask = firebase3.Child("humidity").OnceAsync<float>();
                var pressureDataTask = firebase4.Child("pressure").OnceAsync<float>();

                await Task.WhenAll(luxDataTask, temperatureDataTask, humidityDataTask, pressureDataTask);

                IReadOnlyCollection<FirebaseObject<int>> luxData = await luxDataTask;
                IReadOnlyCollection<FirebaseObject<float>> temperatureData = await temperatureDataTask;
                IReadOnlyCollection<FirebaseObject<float>> humidityData = await humidityDataTask;
                IReadOnlyCollection<FirebaseObject<float>> pressureData = await pressureDataTask;
                string result = Analytics_fun.ProcessData(luxData, temperatureData, humidityData, pressureData);

                TextFromFunction = result;

                // Далее вы можете использовать полученный текстовый результат
                Console.WriteLine(result);


                SensorDataCollection.Clear();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    // Обработка данных для узла lux
                    foreach (var item in luxData)
                {
                        SensorDataCollection.Add(new SensorData
                        {
                            Lux = item.Object.ToString()

                        }) ;
                    }

                // Добавляем данные для температуры, влажности и давления в SensorData
                // Добавляем данные для температуры, влажности и давления в SensorData
                int index = 0;
                foreach (var item in temperatureData)
                {
                    
                    SensorDataCollection[index].Temperature = item.Object.ToString();
                    index++;
                }

                index = 0;
                foreach (var item in humidityData)
                {
                    SensorDataCollection[index].Humidity = item.Object.ToString();
                        index++;
                }

                index = 0;
                foreach (var item in pressureData)
                {
                    SensorDataCollection[index].Pressure = item.Object.ToString();
                        SensorDataCollection[index].Data_time = item.Key.ToString();
                        index++;
                }
                 
                });
                for(int i = 0; i < SensorDataCollection.Count; i++)
                {
                    string[] formats = { "yyyy:MM;dd;HH:mm" };
                    string Data_time = "";
                    if (DateTime.TryParseExact(SensorDataCollection[i].Data_time, formats, null, System.Globalization.DateTimeStyles.None, out DateTime parsedDateTime))
                    {
                        Data_time = parsedDateTime.ToString("yyyy/MM/dd HH:mm");
                    }


                    Analytics.Add( new AnalyticModel() { Field0 = Data_time,  Field = SensorDataCollection[i].Temperature+ "℃", Field1 = SensorDataCollection[i].Lux+"Lux", Field2 = SensorDataCollection[i].Humidity+"%", Field3 = SensorDataCollection[i].Pressure+ "hPa" });
                }
                


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching data from Firebase: " + ex.Message);
            }
        }
        //public event PropertyChangedEventHandler PropertyChanged;

        private RelayCommand backClick;

        public RelayCommand BackClick
        {
            get
            {
                return backClick ??
                  (backClick = new RelayCommand(obj =>
                  {
                      CloseWindow(obj);
                  }));
            }
        }

        private void CloseWindow(object obj)
        {
            if (obj is Window win)
            {
                win.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class TemperatureSensorData
    {
        public float Temperature { get; set; }
        // Другие поля для температурных данных
    }

    public class HumiditySensorData
    {
        public float Humidity { get; set; }
        // Другие поля для влажностных данных
    }

    public class PressureSensorData
    {
        public float Pressure { get; set; }
        // Другие поля для данных о давлении
    }

    public class SensorData
    {
        public string Temperature { get; set; }
        public string Humidity { get; set; }
        public string Pressure { get; set; }
        public string Lux { get; set; }
        public string Data_time { get; internal set; }
    }
}
