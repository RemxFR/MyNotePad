using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyNotePad.controls
{
    
    /// <summary> Création de la classe MainTabControl, barre d'onglet du document pour la RicheTextBox hérite de la classe TabControl.
    /// 
    /// </summary>
    public class MainTabControl : TabControl
    {
        /// <summary> Création de la variable du nom de la MainTabControl.
        /// 
        /// </summary>
        private const string NAME = "MainTabControl";
        private Form1 _form1;
        /// <summary> Création d'une instance de la classe Context Menu Strip.
        /// 
        /// </summary>
        private ContextMenuStrip _contextMenuStrip;

        /// <summary> Instance de la classe Main Tab Control.
        /// 
        /// </summary>
        public MainTabControl()
        {

            /// <summary> Création d'un nouvel objet de la classe TabControlContextMenuStrip.
            /// 
            /// </summary>
            /// <returns></returns>
            _contextMenuStrip = new TabControlContextMenuStrip();

            /// <summary> Nom de la MainTabControl.
            /// 
            /// </summary>
            Name = NAME;

            /// <summary> Contrôles associés au Context Menu Strip, pour fermer les onglets.
            /// 
            /// </summary>
            ContextMenuStrip = _contextMenuStrip;

            /// <summary> Postion de la Main Tab Control.
            /// 
            /// </summary>
            Dock = DockStyle.Fill;

            HandleCreated += (s, e) =>
            {
                _form1 = FindForm() as Form1;
            };

            SelectedIndexChanged += (s, e) =>
            {
                _form1.CurrentFile = _form1.Session.TextFiles[SelectedIndex];
                _form1.RichTextBox = (CustomRichTextBox)_form1.MainTabControl.TabPages[SelectedIndex].Controls.Find("RtbTextFileContents", true).First();
                _form1.Text = $"{_form1.CurrentFile.FileName} - NotePad.NET";
            }; 

            MouseUp += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    for (int i = 0; i < TabCount; i++)
                    {
                        var rect = GetTabRect(i);

                        if (rect.Contains(e.Location))
                        {
                            SelectedIndex = i;
                            break;
                        }
                    }
                }
            };
        }
    }
}