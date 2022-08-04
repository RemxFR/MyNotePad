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
    /// <summary> Création d'une classe pour la session.
    /// 
    /// </summary>
    public class Session
    {
        /// <summary> Création d'une const nom pour la session.
        /// 
        /// </summary>
        private const string FILENAME = "session.XML";

        /// <summary> Création d'un chemin d'accès pour l'appliacation.
        ///  
        /// </summary>
        /// <returns></returns>
        private static string applicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        /// <summary> Création de la combinaison du chemin d'accès et du nom de l'application.
        /// 
        /// </summary>
        /// <returns></returns>
        private static string applicationPath = Path.Combine(applicationDataPath, "NotePad.NET");

        private readonly XmlWriterSettings _writerSettings;

        public static string BackupPath = Path.Combine(applicationDataPath, "NotePad.NET", "backup");

        /// <summary> Chemin d'accès et nom du fichie représentant la session.
        /// 
        /// </summary>
        /// <returns></returns>
        public static string FileName { get; } = Path.Combine(applicationPath, FILENAME);
        [XmlAttribute(AttributeName = "Active Index")]
        /// <summary> Index active de l'onglet ouvert dans la session.
        /// 
        /// </summary>
        /// <value></value>
        public int ActiveIndex { get; set; } = 0;
        [XmlElement(ElementName = "File")]
        /// <summary> Liste des document texte dans la session.
        /// 
        /// </summary>
        /// <value></value>
        public List<TextFile> TextFiles { get; set;}

        public Session()
        {
            /// <summary> Création d'une liste TetxtFiles.
            /// 
            /// </summary>
            /// <value></value>
            TextFiles = new List<TextFile>{};

            _writerSettings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = ("\t"),
                OmitXmlDeclaration = true
            };

            if (!Directory.Exists(applicationPath))
            {
                Directory.CreateDirectory(applicationPath);
            }
        }

        public static async Task<Session> Load()

        {
            var session = new Session();

            if (File.Exists(FileName))
            {
                var serializer = new XmlSerializer(typeof(Session));
                var streamReader = new StreamReader(FileName);

                try
                {
                    session = serializer.Deserialize(streamReader) as Session;
                
                    foreach (var file in session.TextFiles)
                    {
                        var fileName = file.FileName;
                        var backUpFileName = file.BackUpFileName;

                        file.SafeFileName = Path.GetFileName(fileName);

                        // Fichier existant sur le disque.
                        if (File.Exists(fileName))
                        {
                            using (StreamReader reader = new StreamReader(fileName))
                            {
                                file.Contents = await reader.ReadToEndAsync();
                            }
                        }

                        // Fichier BACKUP du dossier BACKUP.
                        if (File.Exists(backUpFileName))
                        {
                            using (StreamReader reader = new StreamReader(backUpFileName))
                            {
                                file.Contents = await reader.ReadToEndAsync();
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    
                    System.Windows.Forms.MessageBox.Show("Une erreur s'est produite: "+ ex.Message);
                }
            
                streamReader.Close();
            }

            return session;

        }

        /// <summary> Sérialisation, pour enregistrer les documents et les réouvrir automatiquement en ouvrant l'app.
        /// 
        /// </summary>
        public void Save()
        {

            var emptyNamespace = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty});
            var serializer = new XmlSerializer(typeof(Session));
            
            using (XmlWriter writer = XmlWriter.Create(FileName, _writerSettings))
            {
                serializer.Serialize(writer, this, emptyNamespace);
            }
        }
    
        public async void BackupFile(TextFile file)
        {
            if (!Directory.Exists(BackupPath))
            {
                await Task.Run(() => Directory.CreateDirectory(BackupPath));
            }
            if (file.FileName.StartsWith("sans titre"))
            {
                using (StreamWriter writer = File.CreateText(file.BackUpFileName))
                {
                    await writer.WriteAsync(file.Contents);
                }
            }
        } 
    }
}