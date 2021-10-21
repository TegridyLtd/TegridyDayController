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

using UnityEngine;

namespace Tegridy.DayController
{
    [System.Serializable] public class Day
    {
        public string dayName;
    }
    [System.Serializable] public class Month
    {
        public string monthName;
        public int daysInMonth;

        [Header("Leap Settings")]
        public bool leepMonth;
        public int yearsBetween;
        public int extraDays;

        [Header("Change Speed")]
        public float sunChangeDelay;
        public float sunChangeAmmount;
        public float skyboxChangeDelay;
        public float skyboxChangeAmmount;

        [Header("Day Config")]
        public DaySettings dayConfig;
        public bool configured;
    }
    [System.Serializable] public class DaySettings
    {
        public int hoursInDay;
        public int minutesInHour;
        public int secondsInMinute;

        [Header("Use 24hr clock")]
        [Range(0, 24)] public int riseTimeSun;
        [Range(0, 24)] public int setTimeSun;
        [Range(0, 24)] public int riseTimeSkyBox;
        [Range(0, 24)] public int setTimeSkyBox;

        [Header("SkyBox")]
        public Material skyboxDay;
        public Material skyboxNight;

        [Header("Colour Settings")]
        public Color32 skyMorning;
        public Color32 skyAfternoon;
        public Color32 skyboxFade;

        public Color32 sunMorning;
        public Color32 sunAfternoon;
    }
}