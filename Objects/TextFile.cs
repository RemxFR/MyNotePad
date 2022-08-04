using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace MyNotePad.Objects
{
    public class TextFile
    {
        [XmlAttribute(AttributeName = "FileName")]
        /// <summary> Chemin d'accès et nom du fichier.
        /// 
        /// </summary>
        /// <value></value>
        public string FileName { get ; set; }

        [XmlAttribute(AttributeName = "BackUpFileName")]
        /// <summary> Chemin d'accès et nom du fichier backup.
        /// 
        /// </summary>
        /// <value></value>
        public string BackUpFileName { get; set; } = string.Empty;

        [XmlIgnore()]
        /// <summary> Nom et extension du fichier. Le nom du fichier n'inclut pas le chemin d'accès.
        /// 
        /// </summary>
        /// <value></value>
        public string SafeFileName { get; set; }
        
        [XmlIgnore()]
        /// <summary> Nom et extension du fichier backup. Le nom du fichier n'inclut pas le chemin d'accès.
       /// 
       /// </summary>
       /// <value></value>
        public string SafeBackUpFileName { get; set; }
        
        [XmlIgnore()]
        /// <summary> Contenu du fichier.
        /// 
        /// </summary>
        /// <value></value>
        public string Contents { get; set; } = string.Empty;
        /// <summary> COnstructeur de la classe TextFile.
        /// 
        /// </summary>
        public TextFile() 
        {
            
        }
        /// <summary> Constructeur de la classe TextFile.
        /// 
        /// </summary>
        /// <param name="fileName">Chemin d'accès et nom du fichier</param>
        public TextFile(String fileName)
        {
            FileName = fileName;
            SafeFileName = Path.GetFileName(fileName);

            if (FileName.StartsWith("sans titre"))
            {
                SafeBackUpFileName = $"{FileName}@{DateTime.Now:dd-MM-yyyy-HH-mm-ss}";
                BackUpFileName = Path.Combine(Session.BackupPath, SafeBackUpFileName);
            }
        }

       
    }


}