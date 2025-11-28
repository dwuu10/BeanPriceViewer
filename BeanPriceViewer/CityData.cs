using System;

namespace BeanPriceViewer;

public class CityData
{
    public CityData(string name, int humidity, int temperature)
    {
        cityName = name;
        humidityStat = humidity;
        temperatureStat = temperature;

        blueBeanPrice = CalculateBlueBeanPrice(temperatureStat);
        redBeanPrice = CalculateRedBeanPrice(temperatureStat);
        yellowBeanPrice = CalculateYellowBeanPrice(humidityStat);
        greenBeanPrice = CalculateGreenBeanPrice(humidityStat);
    }
    public string cityName;
    int humidityStat;
    int temperatureStat;

    public int blueBeanPrice;
    public int redBeanPrice;
    public int yellowBeanPrice;
    public int greenBeanPrice;

    public static int CalculateBlueBeanPrice(int temperature)
    {
        int price;
        price = temperature - 20;
        if (price <= 10)
        {
            price = 10;
        }
        return price;
    }

    public static int CalculateRedBeanPrice(int temperature)
    {
        int price;
        price = 140 - temperature;
        if (price <= 10)
        {
            price = 10;
        }
        return price;
    }

    public static int CalculateYellowBeanPrice(int humidity)
    {
        int price;
        price = humidity + 10;
        return price;
    }

    public static int CalculateGreenBeanPrice(int humidity)
    {
        int price;
        price = 110 - humidity;
        return price;
    }
}