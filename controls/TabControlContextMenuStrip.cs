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
using System.Diagnostics;

namespace MyNotePad.controls
{
    /// <summary> Création de la classe de la fenêtre clic droit pour les onglets, qui hérite de ContextMenuStrip.
    /// 
    /// </summary>
    public class TabControlContextMenuStrip : ContextMenuStrip 
    {
        /// <summary> Création d'une const du nom de la fenêtrte clic droit pour les onglets.
        /// 
        /// </summary>
        private const string NAME = "TabControlContextMenuStrip";
        private Form1 _form1;
        /// <summary> Instance de la classe TabControlContextMenuStrip.
        /// 
        /// </summary>
        public TabControlContextMenuStrip()
        {

            /// <summary> Nom de l'objet.
            /// 
            /// </summary>
            Name = NAME;

            /// <summary> Contenu de la fenêtre clic droit des onglets.
            /// 
            /// </summary>
            /// <returns></returns>
            var closeTab = new ToolStripMenuItem("Fermer");
            var closeAllTabExceptThis = new ToolStripMenuItem("Fermer tout sauf ce fichier");
            var openInFileExplorer = new ToolStripMenuItem("Ouvrir le répertoire du fichier en cours dans l'explorateur");

            // Ajout du contenu à la fenêtre clic droit des onglets.
            Items.AddRange(new ToolStripItem[] {closeTab, closeAllTabExceptThis, openInFileExplorer});


            HandleCreated += (s, e) =>
            {
                _form1 = SourceControl.FindForm() as Form1;
            };

            closeTab.Click += (s, e) =>
            {
                var SelectedTab = _form1.MainTabControl.SelectedTab;
                
                _form1.Session.TextFiles.Remove(_form1.CurrentFile);

                if (_form1.MainTabControl.TabCount > 1)
                {
                    _form1.MainTabControl.TabPages.Remove(SelectedTab);
                    var newIndex = _form1.MainTabControl.TabCount - 1;
                    _form1.MainTabControl.SelectedIndex = newIndex;
                    _form1.CurrentFile = _form1.Session.TextFiles[newIndex];
                }
                else
                {
                    var fileName = "sans titre 1";
                    var file = new TextFile(fileName);

                    _form1.CurrentFile = file;
                    _form1.RichTextBox.Clear();

                    _form1.MainTabControl.SelectedTab.Text = file.FileName;
                    _form1.Session.TextFiles.Add(file);
                    _form1.Text = "sans titre 1 - NotePad.NET";
                }
            };

            closeAllTabExceptThis.Click += (s, e) =>
            {
                var filesToDelete = new List<TextFile>();

                if (_form1.MainTabControl.TabCount > 1)
                {
                    TabPage selectedTab = _form1.MainTabControl.SelectedTab;

                    //suppression des onglets qui ne correspondent pas à l'onglet sélectionné.
                    foreach (TabPage tabPage in _form1.MainTabControl.TabPages)
                    {
                        if (tabPage != selectedTab)
                        {
                            _form1.MainTabControl.TabPages.Remove(tabPage);
                        }
                    }

                    foreach (var file in _form1.Session.TextFiles)
                    {
                        if (file != _form1.CurrentFile)
                        {
                            filesToDelete.Add(file);
                        }
                    }

                    _form1.Session.TextFiles = _form1.Session.TextFiles.Except(filesToDelete).ToList();
                }

            };
        
                openInFileExplorer.Click += (s, e) =>
        {
            var argument = $"/select, \"{_form1.CurrentFile.FileName}\"";
            Process.Start("explorer.exe", argument);
        };
        
        }
    
        
    }
}