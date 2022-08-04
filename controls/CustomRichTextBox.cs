using System.Drawing;

namespace MyNotePad.controls
{
    /// <summary> Création de la classe CustomRochTextBox, qui hérite de RichTextBox, et qui permet d'écrire su rle document.
    /// 
    /// </summary>
    public class CustomRichTextBox : RichTextBox
    {

        /// <summary> Création d'une constante du nom de l'objet.
        /// 
        /// </summary>
        private const string NAME = "RtbTextFileContents";

        /// <summary> Création de l'instance de la classe.
        /// 
        /// </summary>
        public CustomRichTextBox()
        {
            
            /// <summary> Ajout du nom à l'instance.
            /// 
            /// </summary>
            Name = NAME;

            /// <summary> Tab agis comme tab qd true, et change de Rtb quand false.
            /// 
            /// </summary>
            AcceptsTab = true;

            /// <summary> Police de la Rtb.
            /// 
            /// </summary>
            /// <returns></returns>
            Font = new Font("Arial", 12.0F, FontStyle.Regular);

            /// <summary> Position de la Rtb sur le document.
            /// 
            /// </summary>
            Dock = DockStyle.Fill;

            /// <summary> Style des bordures de la Rtb.
            /// 
            /// </summary>
            BorderStyle = BorderStyle.None;

            /// <summary> Création d'un objet pour faire apparaître une fenêtre de contrôle quand clic droit sur la Rtb.
            /// 
            /// </summary>
            /// <returns></returns>
            ContextMenuStrip = new RichTextBoxContextMenuStrip(this);
        }
    }
}