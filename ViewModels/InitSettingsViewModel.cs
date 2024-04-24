using Diplom.Commands;
using Diplom.Properties;
using Diplom.Repository;
using Diplom.Views;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Diplom.ViewModels
{
    public class InitSettingsViewModel : INotifyPropertyChanged
    {
        #region Properties
        private Dictionary<int, string> areas = DataRepository.GetAreas();
        public Dictionary<int, string> Areas
        {
            get { return areas; }
            set
            {
                areas = value;
                OnPropertyChanged(nameof(Areas));
            }
        }

        private int selectedAreaId;
        public int SelectedAreaId
        {
            get { return selectedAreaId; }
            set
            {
                selectedAreaId = value;
                OnPropertyChanged(nameof(SelectedAreaId));

                UpdateAreaItems();
            }
        }

        private Dictionary<int, string> areaItems = new Dictionary<int, string>();
        public Dictionary<int, string> AreaItems
        {
            get { return areaItems; }
            set
            {
                areaItems = value;
                OnPropertyChanged(nameof(AreaItems));
            }
        }

        private int selectedAreaItemId;
        public int SelectedAreaItemId
        {
            get { return selectedAreaItemId; }
            set
            {
                selectedAreaItemId = value;
                OnPropertyChanged(nameof(SelectedAreaItemId));
            }
        }
        #endregion

        public InitSettingsViewModel()
        {
            UpdateAreaItems();
        }
        /*      private RelayCommand flashClick;

              public ICommand FlashClick
              {
                  get
                  {
                      if (flashClick == null)
                      {
                          flashClick = new RelayCommand(param => FlashESP());
                      }
                      return flashClick;
                  }
              }

              private void FlashESP()
              {
                  // Путь к esptool.py и другие необходимые параметры для прошивки
                  string esptoolPath = "..\\..\\esptool-4.6.2\\esptool.py";
                  string comPort = "COM20"; // Порт, к которому подключен ESP32
                  string firmwarePath = "..\\..\\ViewModels\\telegram_bot.ino.bin"; // Путь к файлу прошивки

                  // Формирование команды для прошивки ESP32
                  string command = $"python {esptoolPath} --port {comPort} write_flash 0x10000 {firmwarePath}";

                  try
                  {
                      Process process = new Process();
                      ProcessStartInfo startInfo = new ProcessStartInfo
                      {
                          FileName = "cmd.exe",
                          Arguments = $"/C {command}",
                          RedirectStandardOutput = true,
                          RedirectStandardError = true,
                          UseShellExecute = false,
                          CreateNoWindow = true
                      };

                      process.StartInfo = startInfo;
                      process.Start();

                      string output = process.StandardOutput.ReadToEnd();
                      string error = process.StandardError.ReadToEnd();

                      process.WaitForExit();

                      if (process.ExitCode == 0)
                      {
                          Console.WriteLine("Прошивка успешно завершена");
                          // Добавьте свой код, который будет выполняться после успешной прошивки
                      }
                      else
                      {
                          Console.WriteLine($"Ошибка прошивки: {error}");
                          // Обработка ошибки прошивки
                      }
                  }
                  catch (Exception ex)
                  {
                      Console.WriteLine($"Ошибка: {ex.Message}");
                      // Обработка исключения
                  }
              }*/

        private RelayCommand saveSettingsClick;
        public RelayCommand SaveSettingsClick
        {
            get
            {
                return saveSettingsClick ??
                  (saveSettingsClick = new RelayCommand(obj =>
                  {
                      Settings.Default.AreaId = SelectedAreaId;
                      Settings.Default.AreaItemId = SelectedAreaItemId;
                      Settings.Default.Save();


                      MainWindow mainWindow = new MainWindow();
                      mainWindow.Show();

                      CloseWindow(obj);
                  }
                  ));
            }
        }

        private void UpdateAreaItems()
        {
            AreaItems = DataRepository.GetAreaItems(SelectedAreaId);
        }

        private void CloseWindow(object obj)
        {
            Window win = obj as Window;
            win.Close();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}