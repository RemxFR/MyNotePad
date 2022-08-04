using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyNotePad.controls;

namespace MyNotePad;

static class Program
{
    /// <summary> Création d'une classe Form1 pour avoir accès à l'instance Form1 dans tout le programme.
    /// 
    /// </summary>
    public static Form1 Form1;
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        /// <summary> Création d'un objet Form1 dans la classe Form1.
        /// 
        /// </summary>
        /// <returns></returns>
        Form1 = new Form1();

        // Application run Form1.
        Application.Run(Form1);
    }    
}