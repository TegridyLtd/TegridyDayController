# Tegridy Day Controller
## About
The Tegridy Day Controller is designed to provide a platform to easily implement
World time and date tracking within a virtual world. The system allows you to easily setup custom time schemes with user customisation for year, month, week and day lengths with further customisation for hours, minutes, and even seconds bringing a new level of immersion to your game or virtual world.
## Usage
A blueprint and scene can be found within the project already configured for the standard Gregorian Calender that can be used in your own scenes. If you require custom configuration information regarding the available settings; these can be found below.  

To implement the system into your save system you will need to store: CurrentYear, CurrentDay, CurrentTime, and set them before restarting. 

To add your own features for the end of each day/week/month/year find the TegridyDayController.cs file in your assets folder and add your code to theEndDayCustom, EndWeekCustom, EndMonthCustom, and EndYearCustom functions.

![Tegridy](./1.png)

To use your own language you will need to set the days and months strings. These are found in TegridyDayControllerLanguage.cs. If these arrays do not match the number of days and months in your configuration the system will use the editor configuration as default. A digital and analogue clock example can also be found.

![Tegridy](./2.png)

## World Objects
- **Skybox** - The camera that the sky box is attached to.
- **MainLight** - Main light source that will provide the day/shadow effects.
- **BackupLight** - Not Required – only the colours will be changed on this light, can be useful for ambience.

## System Config
- **Seconds In A Day** - The amount of time in seconds that each day should take. Make sure you allow enough time in your day to accommodate for your fade settings for the months (fade time * (1 / Change)).
- **CurrentTime** - Set this to when you would like to start your first day. 
- **UpdateDelay** - How often to update all the display variables, does not affect lighting.
- **StartingWeekDay** - This value should be set to match the index of the day in WeekDays you would like to start the controller on.
- **CurrentDay** - This value should be set to the day of the year you would like to start on(disregarding months).
- **CurrentYear** - This value should be set to the year you want to start the calender from.

![Tegridy](./3.png)

## DayConfig – Default Day – Must Be Set
- **HoursInDay** - The number of hours there is in the worlds day
- **MinutesInHour** - The number of minutes that each hour should contain.
- **SecondsInMinute** - The number of seconds that each minute should contain.
- **Rise/Set Time** - The time the skybox and sun should start to transition to its next phase, uses standard day 24hr format.
- **Skybox Day / Skybox Night** - Skybox cubemaps that will be transitioned between.
- **Sky Morning/Afternoon** - The colours for the skybox.
- **SkyboxFade** - The colour that will be used when changing between skyboxes.
- **Sun Morning/Afternoon** - The Colour the light sources should be during the day and night.

## WeekDays – Size should match your week length 
- **DayName** - Name of the week day to be displayed

![Tegridy](./4.png)

## Months – set size to your number of months
- **MonthName** - Name of the Month
- **DaysInMonth** - Set to the desired number of days for that month
- **LeapMonth** - Does this month have a leap year at some point?
- **YearsBetween** - The amount of time between leap years
- **ExtraDays** - The amount of days to be added or removed when it is a leap year
- **Sun / Skybox ChangeDelay** - The amount of time to wait between colour/material change increments,
- **Sun / Skybox ChangeAmount** - The amount to increment towards the new color/material
- **DayConfig / Configured** - If configured is not set to true this month will use the default day setting, Day config is same as described above

**Display Variables** – Use these in your custom scripts for getting data from the controller


