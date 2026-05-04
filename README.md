This is an MVC website version of my Bean Trader Game

Add the cities you are interested in and have fun trying make as much profit as you can in the set amount of turns

Each turn, you can make only one buy/sell order, so choose wisely

Prices are calculated from real-life weather data using OpenWeatherMap's API

Features:

- 4 types of beans to trade for profit, each one is affected by either humidity or temperature

- Uses temperature and humidity data from the OpenWeatherMap API

- Customizable starting settings (changing starting cash and turn limit)

- Play with any set of cities you want (as long as they are available on OpenWeatherMap)


To Play this: 

- Clone this repo

- Open the project solution file (.sln) ideally with Visual Studio (the local website can be launched with the .exe, but will lack CSS formatting)

- Add your cities through the homepage or "Saved Cities" tab

- Navigate to "Games", and start your new game from there

- Enjoy!

IMPORTANT!!!

Using this requires an API key from OpenWeatherMap

Before you start the application:

- Go to https://openweathermap.org/api and obtain an API key from there

- Add a new .json file called "key.json" to the BeanPriceViewer folder (same folder as appsettings.json)

- Put your API key under a field called "Key"

key.json template:

{
  "Key": "your api key here"
}
