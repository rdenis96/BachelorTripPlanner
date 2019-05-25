using System;

namespace DataLayer.Enums
{
    [Flags]
    public enum WeatherEnum
    {
        None = 0,
        Rainy = 1 << 0,
        Stormy = 1 << 1,
        Sunny = 1 << 2,
        Cloudy = 1 << 3,
        Hot = 1 << 4,
        Cold = 1 << 5,
        Dry = 1 << 6,
        Wet = 1 << 7,
        Windy = 1 << 8,
        Hurricanes = 1 << 9,
        Typhoons = 1 << 10,
        SandStorms = 1 << 11,
        SnowStorms = 1 << 12,
        Tornados = 1 << 13,
        Humid = 1 << 14,
        Foggy = 1 << 15,
        Snow = 1 << 16,
        Thundersnow = 1 << 17,
        Hail = 1 << 18,
        Sleet = 1 << 19,
        Drought = 1 << 20,
        Wildfire = 1 << 21,
        Blizzard = 1 << 22,
        Avalanche = 1 << 23,
        Mist = 1 << 24,
        Gloomy = 1 << 25,

        All = Rainy | Stormy | Sunny | Cloudy | Hot | Cold | Dry | Wet | Windy
            | Hurricanes | Typhoons | SandStorms | SnowStorms | Tornados | Humid | Foggy
            | Snow | Thundersnow | Hail | Sleet | Drought | Wildfire | Blizzard | Avalanche | Mist | Gloomy
    }
}