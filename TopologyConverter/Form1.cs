using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TopologyConverter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonConverter_Click(object sender, EventArgs e)
        {
            OpenFileDialog selectBrite = new OpenFileDialog();
            selectBrite.Title = "Choose Brite Files";

            //choofdlog.Filter = "All Files (*.*)|*.*";
            selectBrite.Filter = "Brite files(*.brite)| *.brite";
            selectBrite.FilterIndex = 1;
            selectBrite.Multiselect = true;

            if (selectBrite.ShowDialog() == DialogResult.OK)
            {
                //string iFileName = selectBrite.FileName;
                string[] arrAllFiles = selectBrite.FileNames; //used when Multiselect = true

                FolderBrowserDialog folderDlg = new FolderBrowserDialog();
                folderDlg.ShowNewFolderButton = true;
                DialogResult result = folderDlg.ShowDialog();

                if (result == DialogResult.OK)
                {
                    String selectedFolder = folderDlg.SelectedPath;
                    try
                    {
                        //PARSE FILES
                        for (int i = 0; i < arrAllFiles.Length; i++)
                        {
                            string strParseSource = MakeParse(arrAllFiles[i], selectedFolder);
                        }

                        MessageBox.Show("Conversion Completed.");
                    }

                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message.ToString());
                    }
                }
            }

            else
            {

            }
        }

        public String MakeParse(String iFileName, String oFolderName)
        {
            //READ FILE
            string strSource = File.ReadAllText(@" " + iFileName);
            //------------------------------------------------------------

            //START, END CHARACTERS AND INDEXES
            string strStart = ")";
            string strEnd = "U";
            int Start = strSource.LastIndexOf(strStart) + strStart.Length;
            int End = strSource.LastIndexOf(strEnd) + 1;
            //------------------------------------------------------------

            //MAKE PARSE 
            strSource = strSource.Substring(Start + 1, End - Start);  //GET BETWEEN START AND END.
            strSource = strSource.TrimEnd(new char[] { '\r', '\n' }); //DELETE LAST LINE (EMPTY)

            string StrParseSource = null;

            using (StringReader reader = new StringReader(strSource))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] words = line.Split('\t');
                    StrParseSource += words[1] + " " + words[2] + '\n';
                }
            }

            StrParseSource = StrParseSource.TrimEnd(new char[] { '\r', '\n' }); //DELETE LAST LINE (EMPTY)
            //------------------------------------------------------------

            //WRITE FILE
            File.WriteAllText(createOutPutFileName(iFileName, oFolderName, ".top"), StrParseSource);

            return StrParseSource;
        }

        public String createOutPutFileName(String iFileName, String oFolderName, String extension)
        {
            //GET START-UP FOLDER(SELECTED FILE)
            string[] pathWords = iFileName.Split('\\');

            //GET NAME(SELECTED FILE)
            string[] lastFileWords = pathWords[pathWords.Length - 1].Split('.');

            //OUTPUT FILE NAME
            String oFileName = oFolderName + "\\" + lastFileWords[0].ToString() + extension;

            return oFileName;
        }

        public String createOutPutFileNameSameFolder(String iFileName, String extension)
        {
            //GET START-UP FOLDER(SELECTED FILE)
            string iFolderName = null;

            string[] pathWords = iFileName.Split('\\');
            for (int i = 0; i < pathWords.Length - 1; i++)
            {
                iFolderName += pathWords[i] + "\\";
            }

            //GET NAME(SELECTED FILE)
            string[] lastFileWords = pathWords[pathWords.Length - 1].Split('.');

            //OUTPUT FILE NAME
            String oFileName = iFolderName + lastFileWords[0].ToString() + extension;

            return oFileName;
        }

        public String createOutPutFileNameOther(String iFileName, String extension)
        {
            //GET START-UP FOLDER(SELECTED FILE)
            string[] pathWords = iFileName.Split('.');

            //NEW FILE NAME
            String oFileName = pathWords[0].ToString() + extension;

            return oFileName;
        }
    }
}
