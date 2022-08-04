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
using MyNotePad.Objects;
using System.ComponentModel;

namespace MyNotePad;

public partial class Form1 : Form
{
    /// <summary> Création de l'instance RicheTextBox en Static pour être accessible partout dans le programme.
    /// 
    /// </summary>
   // public static RichTextBox RichTextBox;

   /// <summary> Autre méthode pour récupérer l'instance de la RicheTextBox via une fonction C#
   /// 
   /// </summary>
    public RichTextBox RichTextBox;

    /// <summary> Instance de la Main tab control pour créer l'objet tabcontrol dans le formulaire.
    /// 
    /// </summary>
    public TabControl MainTabControl;

    public Session Session;

    public TextFile CurrentFile;
    public Form1()
    {
        InitializeComponent();

        /// <summary> Création d'un nouvel objet menu strip dans la classe Main Menu Strip.
        /// 
        /// </summary>
        /// <returns></returns>
        var menuStrip = new MainMenuStrip();

        /// <summary> Création d'un nouvel objet main tab control dans la classe Main Tab Control.
        /// 
        /// </summary>
        /// <returns></returns>
        MainTabControl = new MainTabControl();


        /// <summary> Ajout des contenus de Controls : Main Tab Control et Menu strip.
        /// 
        /// </summary>
        /// <value></value>
        Controls.AddRange(new Control[]{MainTabControl, menuStrip});
        
        InitializeFile();

    }


    private async void InitializeFile()
    {
        Session = await Session.Load();
        

        if (Session.TextFiles.Count == 0)
        {
            var file = new TextFile("sans titre 1");

            MainTabControl.TabPages.Add(file.SafeFileName);
            //Gestion des contrôles.
            var tabPage = MainTabControl.TabPages[0];
            var rtb = new CustomRichTextBox();
            tabPage.Controls.Add(rtb);
            rtb.Select();

            Session.TextFiles.Add(file);
            CurrentFile = file;
            RichTextBox = rtb;

        } else
        {
            var activeIndex = Session.ActiveIndex;
            foreach (var file in Session.TextFiles)
            {
                if (File.Exists(file.FileName) || File.Exists(path: file.BackUpFileName))
                {
                    var rtb = new CustomRichTextBox();
                    var tabCount = MainTabControl.TabCount;

                    MainTabControl.TabPages.Add(file.SafeFileName);
                    MainTabControl.TabPages[tabCount].Controls.Add(rtb);

                    rtb.Text = file.Contents;
                }
            }
            
            CurrentFile = Session.TextFiles[activeIndex];
            RichTextBox = (CustomRichTextBox)MainTabControl.TabPages[activeIndex].Controls.Find("RtbTextFileContents", true).First();
            RichTextBox.Select();
            
            MainTabControl.SelectedIndex = activeIndex;
            Text = $"{CurrentFile.FileName} - NotePad.NET";
        }
    }

    private void Form1_FormClosing(object s, FormClosingEventArgs e)
    {
        Session.ActiveIndex = MainTabControl.SelectedIndex;
        Session.Save();       

        foreach (var file in Session.TextFiles)
        {
            var fileIndex = Session.TextFiles.IndexOf(file);
            var rtb = MainTabControl.TabPages[fileIndex].Controls.Find("RtbTextFileContents", true).First();
            
            if (file.FileName.StartsWith("sans titre"))
            {
                file.Contents = rtb.Text;
                Session.BackupFile(file);
            }
        }
    }
}
