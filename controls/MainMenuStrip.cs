using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyNotePad.Objects;
using MyNotePad.controls;
using MyNotePad.Services;


namespace MyNotePad;

    /// <summary> Créer la classe du  StripMenu pour Fichier, Edition, Affichage, View, qui hérite de la classe Menu Strip.
    /// 
    /// </summary>
    public class MainMenuStrip : MenuStrip
    {
        /// <summary> Nom du Main Menu Strip
        /// 
        /// </summary>
        private const string NAME = "MainMenuStrip";
        /// <summary> Création de l'objet Form1 pour récupérer la RicheTextBox
        /// 
        /// </summary>
        private Form1 _form1;
        /// <summary> Création d'un objet de la classe du FontDialog pour avoir accès à la mis en forme des polices d'écritures
        /// 
        /// </summary>
        private FontDialog _fontDialog;
        private OpenFileDialog _openFileDialog;
        private SaveFileDialog _saveFileDialog;

        /// <summary> Instance de la classe du Main menu strip
        /// 
        /// </summary>
        public MainMenuStrip()
        {
            /// <summary> INstanciation du nom du Main menu strip
            /// 
            /// </summary>
            Name = NAME;

            /// <summary> Emplacement du Main menu strip
            /// 
            /// </summary>
            Dock = DockStyle.Top;

            /// <summary> Instanciation d'un objet fontDialog pour la classe FontDialog
            /// 
            /// </summary>
            /// <returns></returns>
            _fontDialog = new FontDialog();

            _openFileDialog = new OpenFileDialog();

            _saveFileDialog = new SaveFileDialog();

            // Appel de la méthode Fichier
            FileDropDownMenu();
            // Appele de la méthode Edition
            EditDropDownMenu();
            // Appel de la méthode Format
            FormatDropDownMenu();
            // Appel de la méthode View
            ViewDropDownMenu();

          
            
            HandleCreated += (e, s) =>
            {
                _form1 = (Form1)FindForm();
            };


        }

        /// <summary> Méthode pour Fichier
        /// 
        /// </summary>
        public void FileDropDownMenu()
        {

            /// <summary> Onglet Fichier
            /// 
            /// </summary>
            /// <returns></returns>
            var fileDropDownMenu = new ToolStripMenuItem("Fichier");

            /// <summary> Contenus de "fichier" + raccourcis claviers
            /// 
            /// </summary>
            /// <param name="Keys.N"></param>
            /// <returns></returns>
            var newFile = new ToolStripMenuItem("Nouveau", null, null, Keys.Control | Keys.N);
            var openFile = new ToolStripMenuItem("Ouvrir...", null, null, Keys.Control | Keys.O);
            var save = new ToolStripMenuItem("Enregistrer", null, null, Keys.Control | Keys.S);
            var saveAs = new ToolStripMenuItem("Enregistrer sous...", null, null, Keys.Control | Keys.Shift | Keys.S);
            var quit = new ToolStripMenuItem("Quitter", null, null, Keys.Alt | Keys.F4);
            /// <summary> String pour afficher le raccourcis clavier
            /// 
            /// </summary>
            saveAs.ShortcutKeyDisplayString = "Ctrl+Maj+S";

            newFile.Click += (s, e) =>
            {
                var tabControl = _form1.MainTabControl;
                var tabPagesCount = tabControl.TabPages.Count;
                

                var fileName = $"sans titre {tabPagesCount + 1}";
                var file = new TextFile(fileName);
                var rtb = new CustomRichTextBox();

                tabControl.TabPages.Add(file.SafeFileName);

                var newTabPage = tabControl.TabPages[tabPagesCount];

                newTabPage.Controls.Add(rtb);
                
                _form1.Session.TextFiles.Add(file);
                tabControl.SelectedTab = newTabPage;

                _form1.CurrentFile = file;
                _form1.RichTextBox = rtb;
            };

            openFile.Click += async (s, e) =>
            {
               if(_openFileDialog.ShowDialog() == DialogResult.OK)
               {
                   var tabControl = _form1.MainTabControl;
                   var tabCount = tabControl.TabCount;
               
                    var file = new TextFile(_openFileDialog.FileName);

                    var rtb = new CustomRichTextBox();

                    _form1.Text = $"{file.FileName} - Notepad.NET";

                    using (StreamReader reader = new StreamReader(file.FileName))
                    {
                        file.Contents = await reader.ReadToEndAsync();
                    }

                    rtb.Text = file.Contents;

                    tabControl.TabPages.Add(file.SafeFileName);
                    tabControl.TabPages[tabCount].Controls.Add(rtb);

                    _form1.Session.TextFiles.Add(file);
                    _form1.RichTextBox = rtb;
                    _form1.CurrentFile = file;
                    tabControl.SelectedTab = tabControl.TabPages[tabCount];

                }

            };

            save.Click += async (s, e) =>
            {
                var currentFile = _form1.CurrentFile;
                var currentRtbText = _form1.RichTextBox.Text;

                if (currentFile.Contents != currentRtbText)
                {
                    if (File.Exists(currentFile.FileName))
                    {
                        // using streamwriter pour enregistrer le fichier avec son contenu.                
                        using (StreamWriter writer = File.CreateText(currentFile.FileName))
                    {
                        await writer.WriteAsync(currentFile.Contents);
                    }
                        currentFile.Contents = currentRtbText;
                        _form1.Text = currentFile.FileName;
                        _form1.MainTabControl.SelectedTab.Text = currentFile.SafeFileName;
                    }
                    else 
                    {
                        saveAs.PerformClick();
                    }
                }
            };

            saveAs.Click += async (s, e) =>
            {
                if (_saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var newFileName = _saveFileDialog.FileName;
                    var alreadyExists = false;

                    foreach (var file in _form1.Session.TextFiles)
                    {
                        if (file.FileName == newFileName)
                        {
                            MessageBox.Show("Ce fichier est déjà ouvert dans NotePad.NET", "ERREUR", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                            alreadyExists = true;
                            break;
                        }
                    }
                    //Si le fichier à enregistrer n'existe pas déjà.
                    if (!alreadyExists)
                    {
                        //initialisation d'une propriété après la var pour enregister le contenu de la rtb.
                        var file = new TextFile(newFileName) { Contents = _form1.RichTextBox.Text};
                        // Where() marche avec using.System.Linq. x = 1 fichier et first() car on souhaite que ce soit le premier sur lequel nous sommes. 
                        var oldFile = _form1.Session.TextFiles.Where(x => x.FileName == _form1.CurrentFile.FileName).First();
                       
                        _form1.Session.TextFiles.Replace(oldFile, file);
                    // using streamwriter pour enregistrer le fichier avec son contenu.                
                    using (StreamWriter writer = File.CreateText(file.FileName))
                    {
                        await writer.WriteAsync(file.Contents);
                    }

                    _form1.MainTabControl.SelectedTab.Text = file.SafeFileName;
                    _form1.Text = file.FileName;
                    _form1.CurrentFile = file;
                    }
                    
                }

            };
            
            quit.Click += (s, e) =>
            {
                Application.Exit();
            };

            /// <summary> Ajouter le contenu de "Fichier" à "Fichier"
            /// 
            /// </summary>
            /// <value></value>
            fileDropDownMenu.DropDownItems.AddRange(new ToolStripItem[] {newFile, openFile, save, saveAs, quit });
            // Ajout de "Fichier" au Main Strip Menu
            Items.Add(fileDropDownMenu);
        }

        /// <summary> Méthode pour Edition
        /// 
        /// </summary>    
        public void EditDropDownMenu()
        {
            /// <summary> Onglet Edition
            /// 
            /// </summary>
            /// <returns></returns>
            var editDropDownMenu = new ToolStripMenuItem("Edition");

            /// <summary> Contenu d'Edition + shortcuts
            /// 
            /// </summary>
            /// <param name="Keys.Z"></param>
            /// <returns></returns>
            var undo = new ToolStripMenuItem("Annuler", null, null, Keys.Control | Keys.Z);
            var redo = new ToolStripMenuItem("Restaurer", null, null, Keys.Control | Keys.Y);

            /// <summary> programmation de undo et redo, cf restore zoom pour voir prog en static.
            /// 
            /// </summary>
            /// <returns></returns>
            undo.Click += (s, e) => { if (_form1.RichTextBox.CanUndo) _form1.RichTextBox.Undo(); };
            redo.Click += (s, e) => { if (_form1.RichTextBox.CanRedo) _form1.RichTextBox.Redo(); };

            /// <summary> Ajouter le contenu d'Edition à Edition
            /// 
            /// </summary>
            /// <value></value>
            editDropDownMenu.DropDownItems.AddRange(new ToolStripItem[] {undo, redo });

            // Ajout de "Edition" au Main strip menu
            Items.Add(editDropDownMenu);
        }
    
        /// <summary> Méthode pour "Format".
        /// 
        /// </summary>
        public void FormatDropDownMenu()
        {
            /// <summary> Onglet "Format".
            /// 
            /// </summary>
            /// <returns></returns>
            var formatDropDown = new ToolStripMenuItem("Format");

            /// <summary> Contenu de "Format".
            /// 
            /// </summary>
            /// <returns></returns>
            var font = new ToolStripMenuItem("font");
            
            /// <summary> programmation de police.
            /// 
            /// </summary>
            /// <returns></returns>
            font.Click += (s, e) =>
            {
                /// <summary> Police = Police de la RicheTextBox du document, cf restore zoomV1 pour voir prog en static.
                /// 
                /// </summary>
                _fontDialog.Font = _form1.RichTextBox.Font;
                // Faire apparaître la fenêtre des polices.
                _fontDialog.ShowDialog();
                /// <summary> La police de la RicheTextBox du document = le choix de police dans la fenêtre Police, cf restore zoomV1 pour voir prog en static.
                /// 
                /// </summary>
                _form1.RichTextBox.Font = _fontDialog.Font;
            };

            /// <summary> Ajouter contenu de "Format" à "Format".
            /// 
            /// </summary>
            /// <value></value>
            formatDropDown.DropDownItems.AddRange(new ToolStripItem[] {font});
            // Ajout de l'onglet "Format" au Main strip menu.
            Items.Add(formatDropDown);
        }
    
        /// <summary> Méthode pour "Affichage".
        /// 
        /// </summary>
        public void ViewDropDownMenu()
        {
            /// <summary> Onglet "Affichage".
            /// 
            /// </summary>
            /// <returns></returns>
            var viewDropDown = new ToolStripMenuItem("Affichage");

            /// <summary> Sous onglet "Toujours devant"
            /// 
            /// </summary>
            /// <param name="devant...""></param>
            /// <returns></returns>
            var alwaysOntop = new ToolStripMenuItem("Toujours devant...");

            /// <summary> Sous onglet "Zoom".
            /// 
            /// </summary>
            /// <returns></returns>
            var zoomDropDown = new ToolStripMenuItem("Zoom");
            
            /// <summary> Contenu de "Zoom" + Shortcuts.
            /// 
            /// </summary>
            /// <param name="avant""></param>
            /// <param name="Keys.Add"></param>
            /// <returns></returns>
            var zoomIn = new ToolStripMenuItem("Zoom avant", null, null, Keys.Control | Keys.Add);
            var zoomOut = new ToolStripMenuItem("Zoom arrière", null, null, Keys.Control | Keys.Subtract);
            var resetZoom = new ToolStripMenuItem("Zoom par défaut", null, null, Keys.Control | Keys.Divide);

            /// <summary> String pour raccourics clavier.
            /// 
            /// </summary>
            zoomIn.ShortcutKeyDisplayString = "Ctrl+num +";
            zoomOut.ShortcutKeyDisplayString = "Ctrl+num -";
            resetZoom.ShortcutKeyDisplayString = "Ctrl+num /"; 

            /// <summary> Programmation de la cmd "Toujours devant"
            /// 
            /// </summary>
            /// <returns></returns>
            alwaysOntop.Click += (s, e) => 
            {
            if (alwaysOntop.Checked)
            {
                /// <summary> Case "Toujours devant", non-cochée.
                /// 
                /// </summary>
                alwaysOntop.Checked = false;
                /// <summary> "Toujours devant" Off.
                /// 
                /// </summary>
                Program.Form1.TopMost = false;
            }
            else
            {
                /// <summary> Case "Toujours devant", cochée.
                /// 
                /// </summary>
                alwaysOntop.Checked = true;
                /// <summary> "Toujours devant" On.
                /// 
                /// </summary>
                Program.Form1.TopMost = true;
            }
            };

            /// <summary> Programmation du Zoom In avec HandleCreated, cf Restore Zoom pour prog avec static.
            /// 
            /// </summary>
            /// <returns></returns>
            zoomIn.Click += (s, e) =>
            {
                if(_form1.RichTextBox.ZoomFactor <3F)
                {
                    _form1.RichTextBox.ZoomFactor += 0.3F;
                }
            };
            
            /// <summary> Programmation du Zoom Out avec HandleCreated, cf Restore Zoom pour prog avec static.
            /// 
            /// </summary>
            /// <returns></returns>
            zoomOut.Click += (s, e) =>
            {
                if(_form1.RichTextBox.ZoomFactor > 0.7F)
                {
                    _form1.RichTextBox.ZoomFactor -= 0.3F;
                }
            };

            /// <summary> Programmation du Restore Zoom V1 avec RicheTextBox en static.
            /// 
            /// </summary>
            /// <returns></returns>
           /// resetZoom.Click += (s, e) => {Form1.RichTextBox.ZoomFactor = 1F;};

            /// <summary> Programmation du restore Zoom avec HandleCreated.
            /// 
            /// </summary>
            /// <returns></returns>
            resetZoom.Click += (s, e) => {_form1.RichTextBox.ZoomFactor = 1F;};

            /// <summary> Ajout du contenu de "Zoom" à "Zoom".
            /// 
            /// </summary>
            /// <value></value>
            zoomDropDown.DropDownItems.AddRange(new ToolStripItem[] {zoomIn, zoomOut, resetZoom});

            /// <summary> Ajout du contenu d'"Affichage" à "Affichage".
            /// 
            /// </summary>
            /// <value></value>
            viewDropDown.DropDownItems.AddRange(new ToolStripItem[] {alwaysOntop, zoomDropDown });

            // Ajout 'd'"Affichage".
            Items.Add(viewDropDown);
        }

    }
