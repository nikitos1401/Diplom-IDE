using Diplom.Properties;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplom.Commands
{
    public class Analytics_fun
    {
        public static string ProcessData(
            IReadOnlyCollection<FirebaseObject<int>> luxData,
            IReadOnlyCollection<FirebaseObject<float>> temperatureData,
            IReadOnlyCollection<FirebaseObject<float>> humidityData,
            IReadOnlyCollection<FirebaseObject<float>> pressureData)
        {
            double avgLux = luxData.Average(x => x.Object);
            float avgTemperature = temperatureData.Average(x => x.Object);
            float avgHumidity = humidityData.Average(x => x.Object);
            float avgPressure = pressureData.Average(x => x.Object);
            string result = "Ваш текстовый ответ на основе данных";
            if (Settings.Default.AreaId == 0)
            {
                switch (Settings.Default.AreaItemId)
                {
                    case 0:
                        if (avgTemperature > 20 && avgHumidity > 60 && avgPressure > 1013.25)
                        {
                            result = "Високі середні показники температури, вологості та тиску - можливо, це негативно впливає на виробництво молока";
                        }
                        else if (avgTemperature < 15 && avgHumidity < 40 && avgPressure < 990)
                        {
                            result = "Низькі середні показники температури, вологості та тиску - можливо, це негативно впливає на виробництво молока";
                        }
                        else
                        {
                            result = "Умови в хліву неоднозначні, необхідно провести більш глибокий аналіз";
                        }
                        break;
                    case 1:
                        if (avgTemperature > 22 && avgHumidity > 65 && avgPressure > 980)
                        {
                            result = "Високі середні показники температури, вологості та тиску - можливо, це негативно впливає на комфорт свиней";
                        }
                        else if (avgTemperature < 18 && avgHumidity < 50 && avgPressure < 950)
                        {
                            result = "Низькі середні показники температури, вологості та тиску - можливо, це негативно впливає на комфорт свиней";
                        }
                        else
                        {
                            result = "Умови в хліві для свиней неоднозначні, необхідно провести більш глибокий аналіз";
                        }
                        break;
                    case 2:
                        if (avgTemperature > 20 && avgHumidity > 60 && avgPressure > 980)
                        {
                            result = "Високі середні значення температури, вологості та тиску - можливо, це негативно впливає на комфорт кіз";
                        }
                        else if (avgTemperature < 15 && avgHumidity < 50 && avgPressure < 950)
                        {
                            result = "Низькі середні значення температури, вологості та тиску - можливо, це негативно впливає на комфорт кіз";
                        }
                        else
                        {
                            result = "Умови в стайні для кіз неоднозначні, необхідно провести більш глибокий аналіз";
                        }

                        break;
                    case 3:
                        if (avgTemperature > 25 && avgHumidity > 70 && avgLux > 800)
                        {
                            result = "Високі середні значення температури, вологості та освітленості - можливо, це негативно впливає на комфорт кур";
                        }
                        else if (avgTemperature < 18 && avgHumidity < 50 && avgLux < 200)
                        {
                            result = "Низькі середні значення температури, вологості та освітленості - можливо, це негативно впливає на комфорт кур";
                        }
                        else
                        {
                            result = "Умови в курнику для кур неоднозначні, необхідно провести більш глибокий аналіз";
                        }

                        break;
                }

            }
            else
            {
                switch (Settings.Default.AreaItemId)
                {
                    case 0:
                        if (avgTemperature > 25 && avgHumidity > 70 && avgLux > 800)
                        {
                            result = "Високі середні показники температури, вологості та освітленості - можливо, це негативно впливає на ріст огірків";
                        }
                        else if (avgTemperature < 18 && avgHumidity < 50 && avgLux < 200)
                        {
                            result = "Низькі середні показники температури, вологості та освітленості - можливо, це негативно впливає на ріст огірків";
                        }
                        else
                        {
                            result = "Умови для росту огірків неоднозначні, рекомендується провести більш детальний аналіз";
                        }

                        break;
                    case 1:
                        if (avgTemperature > 25 && avgHumidity > 60 && avgLux > 800)
                        {
                            result = "Високі середні показники температури, вологості та освітленості - можливо, це негативно впливає на ріст помідорів";
                        }
                        else if (avgTemperature < 18 && avgHumidity < 50 && avgLux < 200)
                        {
                            result = "Низькі середні показники температури, вологості та освітленості - можливо, це негативно впливає на ріст помідорів";
                        }
                        else
                        {
                            result = "Умови для росту помідорів неоднозначні, рекомендується провести більш детальний аналіз";
                        }
                        break;
                    case 2:
                        if (avgTemperature > 25 && avgHumidity > 60 && avgLux > 800)
                        {
                            result = "Високі середні показники температури, вологості та освітленості - можливо, це негативно впливає на ріст перцю";
                        }
                        else if (avgTemperature < 18 && avgHumidity < 50 && avgLux < 200)
                        {
                            result = "Низькі середні показники температури, вологості та освітленості - можливо, це негативно впливає на ріст перцю";
                        }
                        else
                        {
                            result = "Умови для росту перцю неоднозначні, рекомендується провести більш детальний аналіз";
                        }

                        break;
                    case 3:
                        if (avgTemperature > 25 && avgHumidity > 60 && avgLux > 800)
                        {
                            result = "Високі середні показники температури, вологості та освітленості - можливо, це негативно впливає на ріст кабачків";
                        }
                        else if (avgTemperature < 18 && avgHumidity < 50 && avgLux < 200)
                        {
                            result = "Низькі середні показники температури, вологості та освітленості - можливо, це негативно впливає на ріст кабачків";
                        }
                        else
                        {
                            result = "Умови для росту кабачків неоднозначні, рекомендується провести більш детальний аналіз";
                        }

                        break;
                }
            }

            return result;
        }
    }
}
