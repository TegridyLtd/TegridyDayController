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
using TMPro;

namespace Tegridy.DayController
{
    public class TegridyDayControllerInfo : MonoBehaviour
    { 
        public TextMeshProUGUI time;
        TegridyDayController control;

        void Awake()
        {
            control = FindObjectOfType<TegridyDayController>();
        }

        void Update()
        {
            if (time != null)
            {
                string thisText = "<b>Current Time</b><br>";
                thisText += control.displayDay + "<br>";
                thisText += control.displayDayOfMonth + " " + control.displayMonth + " " + control.currentYear + "<br>";
                thisText += control.display24Hours + ":" + control.displayMinutes + ":" + control.displaySeconds + "<br>";
                thisText += control.display12Hours + ":" + control.displayMinutes + ":" + control.displaySeconds + " " + control.displayAMPM + "<br>";
                
                thisText += "<br><b>Clock Rotations</b><br>";
                thisText += "Hours = " + control.displayHandHou + "<br>Minutes = " + control.displayHandMin + "<br>Seconds = " + control.displayHandSec + "<br>";

                thisText += "<br><b>Config</b><br>";
                thisText += "This Year Legnth = " + control.yearLength + "<br>";
                thisText += "Weekdays = " + control.weekDays.Length + "<br>";
                thisText += "Months = " + control.months.Length + "<br>";
                thisText += "Hours in Day = " + control.defaultDay.hoursInDay + "<br>";
                thisText += "Minute per Hour = " + control.defaultDay.minutesInHour + "<br>";
                thisText += "Seconds in Minute = " + control.defaultDay.secondsInMinute + "<br>";

                time.text = thisText;
            }
    }
    }
}