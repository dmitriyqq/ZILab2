using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZILab2Lib;

namespace ZILab2
{
    public partial class Form1 : Form
    {
        EncodingService encodingSerivice { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private string GetKey()
        {

            var key = keyTextBox.Text;
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new Exception("key should not be null");
            }

            return key;
        }

        private byte[] ReadAllBytes(OpenFileDialog dialog)
        {
            try
            {
                var filename = dialog.FileName;
                return File.ReadAllBytes(filename);
            } 
            catch
            {
                throw new Exception("Can't read file");
            }
        }

        private void encodeButton_Click(object sender, EventArgs e)
        {
            encodeFileDialog.ShowDialog();
        }

        private void decodeButton_Click(object sender, EventArgs e)
        {
            decodeFileDialog.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            encodingSerivice = new EncodingService();
        }

        private void encodeFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                var bytes = ReadAllBytes(encodeFileDialog);
                var encodedBytes = encodingSerivice.Encode(bytes, GetKey());
                var filename = encodeFileDialog.FileName;
                File.WriteAllBytes($"{filename}.{Guid.NewGuid()}.enc", encodedBytes);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void decodeFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                var bytes = ReadAllBytes(decodeFileDialog);
                var decodedBytes = encodingSerivice.Decode(bytes, GetKey());
                var filename = decodeFileDialog.FileName;
                File.WriteAllBytes($"{filename}.{Guid.NewGuid()}.dec", decodedBytes);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
    }
}
