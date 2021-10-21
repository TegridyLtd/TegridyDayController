/////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2021 Tegridy Ltd                                          //
// Author: Darren Braviner                                                 //
// Contact: db@tegridygames.co.uk                                          //
/////////////////////////////////////////////////////////////////////////////
//                                                                         //
// This program is free software; you can redistribute it and/or modify    //
// it under the terms of the GNU General Public License as published by    //
// the Free Software Foundation; either version 2 of the License, or       //
// (at your option) any later version.                                     //
//                                                                         //
// This program is distributed in the hope that it will be useful,         //
// but WITHOUT ANY WARRANTY.                                               //
//                                                                         //
/////////////////////////////////////////////////////////////////////////////
//                                                                         //
// You should have received a copy of the GNU General Public License       //
// along with this program; if not, write to the Free Software             //
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,              //
// MA 02110-1301 USA                                                       //
//                                                                         //
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using UnityEngine;

namespace Tegridy.DayController
{
    public class TegridyDayController : MonoBehaviour
    {
        [Header("World Objects")]
        public Skybox skyBox;
        public Light mainLight;
        public Light backupLight;

        [Header("System Config")]
        public float SecondsInADay = 120f;
        [Range(0, 1)] public float currentTime = 0;
        public float updateDelay;
        public int startingWeekDay;
        public int currentDay = 1;
        public int currentYear = 1;

        [Header("Day Config")]
        public DaySettings defaultDay;
        public Day[] weekDays;
        public Month[] months;

        [Header("Display Variables")]
        public string displaySeconds;
        public string displayMinutes;
        public string displayAMPM;
        public string display12Hours;
        public string display24Hours;
        public string displayDay;
        public string displayDayOfMonth;
        public string displayMonth;
        public string displayMonthNumber;
        public int yearLength;

        public float displayHandSec;
        public float displayHandMin;
        public float displayHandHou;

        bool skyboxChange = true; //is it morning or afternoon?
        bool sunChange = true;

        int nextDay; //used for keeping track of what day it is
        int currentMonth = 0; //used for keeping track of what month it is
        int[] daysTillNextMonth; //if nextday is greater, change month

        int secondsInDay; //for working out our time
        float secPer; //store division values for later
        float minPer;
        float houPer;

        public void Start()
        {
            secondsInDay = defaultDay.secondsInMinute * defaultDay.minutesInHour * defaultDay.hoursInDay;
            secPer = (float)defaultDay.secondsInMinute / 360;
            minPer = (float)defaultDay.minutesInHour / 360;
            houPer = (float)defaultDay.hoursInDay / 360;

            //setup the controller
            if (weekDays.Length == TegridyDayControllerLanguage.days.Length)
                for (int i = 0; i < weekDays.Length; i++)
                {
                    weekDays[i].dayName = TegridyDayControllerLanguage.days[i];
                }

            for (int i = 0; i < months.Length; i++)
            {
                if (!months[i].configured) months[i].dayConfig = defaultDay;
                if (TegridyDayControllerLanguage.months.Length == months.Length)
                    months[i].monthName = TegridyDayControllerLanguage.months[i];
            }

            SetupYear(currentYear);
            CheckMonth();

            nextDay = startingWeekDay;
            if (nextDay >= weekDays.Length) nextDay = 0;
            displayDay = weekDays[nextDay].dayName;
            if (currentMonth > 0) displayDayOfMonth = (currentDay - daysTillNextMonth[currentMonth - 1]).ToString();
            else displayDayOfMonth = currentDay.ToString();


            //check if we are starting in the morning or night
            int time = Mathf.RoundToInt((float)TimeSpan.FromDays(currentTime).TotalHours);
            if (months[currentMonth].dayConfig.riseTimeSun > time || months[currentMonth].dayConfig.setTimeSun < time)
            {
                sunChange = false;
                mainLight.color = months[currentMonth].dayConfig.sunAfternoon;
            }
            else
            {
                sunChange = true;
                mainLight.color = months[currentMonth].dayConfig.sunMorning;
            }

            if (months[currentMonth].dayConfig.riseTimeSkyBox > time || months[currentMonth].dayConfig.setTimeSkyBox < time)
            {
                skyboxChange = false;
                skyBox.material = months[currentMonth].dayConfig.skyboxNight;
                skyBox.material.SetColor("_Tint", months[currentMonth].dayConfig.skyAfternoon);
            }
            else
            {
                skyboxChange = true;
                skyBox.material = months[currentMonth].dayConfig.skyboxDay;
                skyBox.material.SetColor("_Tint", months[currentMonth].dayConfig.skyMorning);
            }
            StartCoroutine(CheckDay());
            StartCoroutine(TimeState());
        }
        void Update()
        {
            //Rotate our light source
            mainLight.transform.localRotation = Quaternion.Euler((currentTime * 360f) - 90, -100, 0);
            currentTime += (Time.deltaTime / SecondsInADay);
        }
        IEnumerator CheckDay()
        {
            yield return new WaitUntil(() => currentTime >= 1);
            currentTime = 0;
            currentDay++;
            NextDay();
            if (currentDay == yearLength)
            {
                currentDay = 0;
                currentMonth = 0;
                currentYear++;
                SetupYear(currentYear);
                EndYearCustom();
            }
            CheckMonth();
            StartCoroutine(CheckDay());
        }
        IEnumerator TimeState()
        {
            while (true)
            {
                GetTime();
                TimeSpan time = TimeSpan.FromDays(currentTime);
                int t = Mathf.RoundToInt((float)time.TotalHours);
                if (t == months[currentMonth].dayConfig.riseTimeSkyBox && skyboxChange == false)
                {
                    //start morning fade in
                    skyboxChange = true;
                    StartCoroutine(ChangeSkybox(months[currentMonth].dayConfig.skyboxDay, months[currentMonth].dayConfig.skyMorning, months[currentMonth].dayConfig.skyAfternoon,
                        months[currentMonth].skyboxChangeDelay, months[currentMonth].skyboxChangeAmmount));
                }

                if (t == months[currentMonth].dayConfig.setTimeSkyBox && skyboxChange == true)
                {
                    //start evening fade in
                    skyboxChange = false;
                    StartCoroutine(ChangeSkybox(months[currentMonth].dayConfig.skyboxNight, months[currentMonth].dayConfig.skyAfternoon, months[currentMonth].dayConfig.skyMorning,
                        months[currentMonth].skyboxChangeDelay, months[currentMonth].skyboxChangeAmmount));
                }
                if (t == months[currentMonth].dayConfig.riseTimeSun && sunChange == false)
                {
                    sunChange = true;
                    StartCoroutine(LightColour(months[currentMonth].dayConfig.sunMorning,
                        months[currentMonth].sunChangeDelay, months[currentMonth].sunChangeAmmount));
                }
                if (t == months[currentMonth].dayConfig.setTimeSun && sunChange == true)
                {
                    sunChange = false;
                    StartCoroutine(LightColour(months[currentMonth].dayConfig.sunAfternoon,
                        months[currentMonth].sunChangeDelay, months[currentMonth].sunChangeAmmount));
                }
                yield return new WaitForSeconds(updateDelay);
            }
        }
        IEnumerator LightColour(Color32 newColour, float delay, float changeAmmount)
        {
            float change = 0;
            while (change < 1)
            {
                change += changeAmmount;
                mainLight.color = Color.Lerp(mainLight.color, newColour, change);
                if (backupLight != null) backupLight.color = Color.Lerp(backupLight.color, newColour, change);
                yield return new WaitForSeconds(delay);
            }
        }
        IEnumerator ChangeSkybox(Material newSkybox, Color32 newColour, Color32 oldColour, float delay, float changeAmmount)
        {
            float change = 0;
            while (change < 1)
            {
                change += changeAmmount;
                skyBox.material.SetColor("_Tint", Color.Lerp(oldColour, months[currentMonth].dayConfig.skyboxFade, change));
                yield return new WaitForSeconds(delay);
            }
            skyBox.material = newSkybox;
            skyBox.material.SetColor("_Tint", months[currentMonth].dayConfig.skyboxFade);
            change = 0;
            while (change < 1)
            {
                change += changeAmmount;
                skyBox.material.SetColor("_Tint", Color.Lerp(months[currentMonth].dayConfig.skyboxFade, newColour, change));
                yield return new WaitForSeconds(delay);
            }
        }
        private void SetupYear(int year)
        {
            daysTillNextMonth = new int[months.Length];
            int count = 0;
            for (int i = 0; i < months.Length; i++)
            {
                count += months[i].daysInMonth;
                if (months[i].leepMonth && months[i].extraDays != 0 && months[i].yearsBetween != 0)
                {
                    //check for leap year
                    if (year % months[i].yearsBetween == 0)
                    {
                        count += months[i].extraDays;
                    }
                }
                daysTillNextMonth[i] = count;
            }
            yearLength = count;
        }
        private void CheckMonth()
        {
            for (int i = currentMonth; i < daysTillNextMonth.Length; i++)
            {
                if (daysTillNextMonth[i] <= currentDay) { currentMonth++; EndMonthCustom(); } 
                else break;
            }
            displayMonth = months[currentMonth].monthName;
            displayMonthNumber = (currentMonth + 1).ToString();
        }
        private void NextDay()
        {
            nextDay++;
            if (nextDay >= weekDays.Length) { nextDay = 0; EndWeekCustom(); }
            displayDay = weekDays[nextDay].dayName;

            if (currentMonth > 0) displayDayOfMonth = (currentDay - daysTillNextMonth[currentMonth - 1]).ToString();
            else displayDayOfMonth = currentDay.ToString();

            EndDayCustom();
        }
        private void GetTime()
        {
            float seconds = secondsInDay * currentTime;
            float minutes = seconds / defaultDay.secondsInMinute;
            float hours = minutes / defaultDay.minutesInHour;

            seconds %= defaultDay.secondsInMinute;
            minutes %= defaultDay.minutesInHour;
            hours %= defaultDay.hoursInDay;

            int minutesRounded = Convert.ToInt32(minutes + 0.5);
            int hoursRounded = Convert.ToInt32(hours - 0.5f);
            float hoursDivded = (hours % (defaultDay.hoursInDay / 2));

            displaySeconds = seconds.ToString("F0");
            displayMinutes = minutesRounded.ToString();
            display24Hours = hoursRounded.ToString();
            display12Hours = (hoursRounded % (defaultDay.hoursInDay / 2)).ToString("F0"); //(hoursRounded % (defaultDay.hoursInDay / 2)).ToString("F0");

            displaySeconds = displaySeconds.PadLeft(2, '0');
            displayMinutes = displayMinutes.PadLeft(2, '0');
            display24Hours = display24Hours.PadLeft(2, '0');
            display12Hours = display12Hours.PadLeft(2, '0');

            if (currentTime > 0.5f) displayAMPM = TegridyDayControllerLanguage.pm;
            else displayAMPM = TegridyDayControllerLanguage.am;

            displayHandSec = seconds / secPer;
            displayHandMin = minutes / minPer;
            displayHandHou = hoursDivded / (houPer / 2);
        }

        private void EndDayCustom()
        {

        }
        private void EndWeekCustom()
        {

        }
        private void EndMonthCustom()
        {

        }
        private void EndYearCustom()
        {

        }

    }
}