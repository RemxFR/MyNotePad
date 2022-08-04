namespace MyNotePad.controls
{
    /// <summary> Création de la classe RtbContextMEnuStrip qui hérite de ContextMenuStrip, pour gérer la fenêtre lors d'un clic droit.
    /// 
    /// </summary>
    public class RichTextBoxContextMenuStrip : ContextMenuStrip
    {

        /// <summary> Création du nom de la fenêtre du clic droit.
        /// 
        /// </summary>
        private const string NAME = "richTextBoxContextMenuStrip";

        /// <summary> Création de la référence à la Rtb.
        /// 
        /// </summary>
        private RichTextBox _richTextBox;

        /// <summary> Instance de la classe de la RtbContextMenuStrip avec la Rtb comme paramètre.
        /// 
        /// </summary>
        /// <param name="richTextBox"></param>
        public RichTextBoxContextMenuStrip(RichTextBox richTextBox)
        {

            /// <summary> Création de la référence à la Rtb.
            /// 
            /// </summary>
            _richTextBox = richTextBox;

            /// <summary> Création du nom de la fenêtre, clic droit.
            /// 
            /// </summary>
            Name = NAME;

            /// <summary> Création contenu de la fenêtre clic droit + shortcuts.
            /// 
            /// </summary>
            /// <param name="Keys.X"></param>
            /// <returns></returns>
            var cut = new ToolStripMenuItem("Couper", null, null, Keys.Control | Keys.X);
            var copy = new ToolStripMenuItem("Copier", null, null, Keys.Control | Keys.C);
            var paste = new ToolStripMenuItem("Coller", null, null, Keys.Control | Keys.V);
            var selectAll = new ToolStripMenuItem("Tout sélectionner", null, null, Keys.Control | Keys.A);

            /// <summary> Programmation des cmd de la fenêtre cliuc droit.
            /// 
            /// </summary>
            /// <returns></returns>
            cut.Click += (s, e) => _richTextBox.Cut();
            copy.Click += (s, e) => _richTextBox.Copy();
            paste.Click += (s, e) => _richTextBox.Paste();
            selectAll.Click += (s, e) => _richTextBox.SelectAll();

            // Ajout du contenu à la fenêtre clic droit.
            Items.AddRange(new ToolStripItem[] {cut, copy, paste, selectAll});
        }
    }
}