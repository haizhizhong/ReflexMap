﻿using System;
using System.Windows.Forms;
using Esri.ArcGISRuntime;
using ReflexMap;

namespace TestMap
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
//            var xxx = GeoServices.GetPositionFromAddress("101, 10423 178 street", "Edmonton", "AB", "Canada", "T5S 1R5");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}