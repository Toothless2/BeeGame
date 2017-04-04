using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeeGame.Enums;

namespace BeeGame.Rooms
{
    [Serializable]
    public class World
    {
        [NonSerialized]
        private static int globalTime;
        [NonSerialized]
        private static Time time;

        [NonSerialized]
        private static Weather weather;
        [NonSerialized]
        private static int timeUntilWeatherChange = 5;

        public static Time GetTime
        {
            get
            {
                //every 5 mins it will change from night to day
                int temp = (int)UnityEngine.Time.timeSinceLevelLoad;

                if(globalTime < (temp / 300))
                {
                    globalTime = temp / 300;
                    timeUntilWeatherChange -= 1;
                    return time = time != Time.DAY ? Time.DAY : Time.NIGHT;
                }

                return time;
            }
        }

        public static Weather GetWeather
        {
            get
            {
                //before the weather is got time is updated as it is dependant on it
                time = GetTime;

                if(timeUntilWeatherChange < 1)
                {
                    timeUntilWeatherChange = new Random().Next(0, 10);
                    return weather = (Weather)new Random().Next(0, 4);
                }

                if(weather != Weather.NORMAL)
                {
                    weather = new Random().Next(0, 1000) > 997 ? Weather.NORMAL : weather;
                }

                return weather;
            }
        }
    }
}
