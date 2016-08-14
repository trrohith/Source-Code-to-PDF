using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace Source_Code_to_PDF
{
    public partial class Form1 : Form
    {
        Document document = new Document();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            String path = textBox1.Text;

            PdfWriter.GetInstance(document, new FileStream(textBox3.Text, FileMode.Create));
            document.Open();

            if (File.Exists(path))
            {
                // This path is a file
                ProcessFile(path);
            }
            else if (Directory.Exists(path))
            {
                // This path is a directory
                ProcessDirectory(path);
            }
            document.Close();
            MessageBox.Show("File saved", "Info");
            button1.Enabled = false;
        }

        private Paragraph CreateFilePage(String file)
        {
            Paragraph p = new Paragraph();
            p.Add(CreateTitle(Path.GetFileNameWithoutExtension(file)));
            StreamReader rdr = new StreamReader(file);
            p.Add(new Paragraph(rdr.ReadToEnd()));
            return p;
        }
        private PdfPTable CreateTitle(String Title)
        {
            Phrase title = new Phrase(Title);
            PdfPTable table = new PdfPTable(1);
            table.AddCell(GetCellForBorderlessTable(title, Element.ALIGN_CENTER));
            return table;
        }

        private static PdfPCell GetCellForBorderlessTable(Phrase phrase, int align)
        {
            PdfPCell cell = new PdfPCell(phrase);
            cell.HorizontalAlignment = align;
            cell.PaddingBottom = 2f;
            cell.PaddingTop = 0f;
            cell.BorderWidth = PdfPCell.NO_BORDER;
            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
            return cell;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            textBox1.Text = fbd.SelectedPath.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "PDF|*.pdf";
            saveFileDialog1.Title = "Save the PDF File";
            saveFileDialog1.ShowDialog();
            textBox3.Text = saveFileDialog1.FileName.ToString();
        }


        public void ProcessDirectory(string targetDirectory)
        {
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }


        public void ProcessFile(string path)
        {
            if (path.EndsWith("." + textBox2.Text))
            {
                document.Add(CreateFilePage(path));
                document.NewPage();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(textBox1.Text) | String.IsNullOrEmpty(textBox2.Text) | String.IsNullOrEmpty(textBox3.Text)))
            {
                button1.Enabled = true;
            }
        }
    }
}