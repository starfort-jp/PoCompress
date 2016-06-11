using System;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PoCompress
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<string> xListString = new List<string>();
        BitArray xBitArray = new BitArray(65536);

        private void mark_lines()
        {
            int xLineNumber, xLp, yLp;
            string Line, xLine, xCheckString, xChr;
            int xCheckLength, xAreaStart, xAreaStop;
            bool xFlag, xCheck;

            if (radioButton1.Checked == true)
            {
                xCheckString = "#: " + radioButton1.Text;
            }
            else
            {
                xCheckString = "#: " + radioButton2.Text;
            }
            xCheckLength = xCheckString.Length;

            xLineNumber = xListString.Count;
            for (xLp = 0; xLp < 65536; xLp++)
            {
                xBitArray[xLp] = false;
            }

            xFlag = false;
            xCheck = false;
            xAreaStart = 0;
            xAreaStop = 0;
            xLp = 0;
            for (xLp = 0; xLp < xLineNumber; xLp++)
            {
                //                Line = richTextBox1.Lines[xLp];
                Line = xListString[xLp];
                if (Line.Length > 4)
                {
                    xChr = Line.Substring(0, 3);
                    if (xChr == @"#: ")
                    {
                        if (xFlag == false)
                        {
                            xAreaStart = xLp;
                            xFlag = true;
                            xCheck = false;
                        }
                        if (Line.Length > xCheckLength)
                        {
                            xLine = Line.Substring(0, xCheckLength);
                            if (xLine == xCheckString)
                            {
                                xBitArray[xLp] = true;
                            }
                            else
                            {
                                xCheck = true;
                            }
                        }
                    }
                    else
                    {
                        xAreaStop = xLp;
                        if (xCheck == false)
                        {
                            for (yLp = xAreaStart; yLp < xAreaStop; yLp++)
                            {
                                xBitArray[yLp] = false;
                            }
                        }
                        xFlag = false;
                    }
                }

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string line;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
                StreamReader sr = new StreamReader(textBox1.Text, Encoding.UTF8);
                if (File.Exists(textBox1.Text))
                {
                    xListString.Clear();
                    while ((line = sr.ReadLine()) != null) //Read line of text file 'til end
                    {
                        xListString.Add(line);
                        line += "\r\n";
                        sb.Append(line);
                    }
                    richTextBox1.Text = sb.ToString();
                }
                sr.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int xLineNumber, xLp, yLp;
            string Line, xLine, xPath, xCh;
            int xLength;
            bool xEnable, xFull;

            radioButton1.Text = "";
            radioButton2.Text = "";
            xPath = "";
//            xLineNumber = richTextBox1.Lines.Length;
            xLineNumber = xListString.Count;
            xFull = false;
            for (xLp = 0; xLp < xLineNumber; xLp++)
            {
                if (xFull == false)
                {
                    //                    xLength = richTextBox1.Lines[xLp].Length;
                    //                    Line = richTextBox1.Lines[xLp];
                    xLength = xListString[xLp].Length;
                    Line = xListString[xLp];
                    if (Line.Length >= 20)
                    {
                        xLine = Line.Substring(1, 19);
                        if (xLine == @"X-Poedit-SearchPath")
                        {
                            xEnable = false;
                            xPath = "";
                            for (yLp = 19; yLp < xLength; yLp++)
                            {
                                xCh = Line.Substring(yLp, 1);
                                if (xCh == @"/")
                                {
                                    xEnable = false;
                                }
                                else
                                {
                                    if (xEnable == true)
                                    {
                                        xPath += xCh;
                                    }
                                    if (xCh == @" ")
                                    {
                                        xEnable = true;
                                    }
                                }
                            }
                            if (radioButton1.Text == "")
                            {
                                radioButton1.Text = xPath;
                            }
                            else
                            {
                                if (radioButton1.Text != xPath)
                                {
                                    if (radioButton2.Text == "")
                                    {
                                        radioButton2.Text = xPath;
                                        xFull = true;
                                    }
                                }
                            }
                        }
                        xLength = 0;
                    }

                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int xLineNumber, xLp;
            int xStart, xLength, xPos;

            mark_lines();

            MessageBox.Show("This feature is debug version only.");
/*
            xLineNumber = richTextBox1.Lines.Length;
            xPos = 0;
            for (xLp = 0; xLp < xLineNumber; xLp++)
            {
                richTextBox1.SelectionLength = 0;
                xStart = xPos;
                xLength = richTextBox1.Lines[xLp].Length;
                richTextBox1.SelectionStart = xStart;
                richTextBox1.SelectionLength = xLength;
                if (xBitArray[xLp] == true)
                {
                    richTextBox1.SelectionColor = Color.Red;
                }
                xPos += xLength + 1;
                xLength = 0;
            }
*/
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int xLineNumber, xLp;
            string xLine, xMessage;

//            Stopwatch sw = new Stopwatch();   //Start counting.
//            sw.Start();

            mark_lines();

            xLineNumber = xListString.Count();
            List<string> xItemList = new List<string>();
            for (xLp = 0; xLp < xLineNumber; xLp++)
            {
                if (xBitArray[xLp] == false)
                {
                    xItemList.Add(xListString[xLp]);
                }
            }

            xLineNumber = xItemList.Count;
            richTextBox1.Clear();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (xLp = 0; xLp < xLineNumber; xLp++)
            {
                xLine = xItemList[xLp];
                xLine += "\r\n";
                sb.Append(xLine);
            }
            richTextBox1.Text = sb.ToString();

//            sw.Stop();
//            xMessage = string.Format("処理終了 : {0:d} msec", sw.ElapsedMilliseconds);    //Report process time.
//            MessageBox.Show(xMessage);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
//                System.Text.Encoding enc = System.Text.Encoding.GetEncoding("UTF-8");          //UTF-8 with BOM
//                System.IO.File.WriteAllText(saveFileDialog1.FileName, richTextBox1.Text, enc); // 
                System.IO.File.WriteAllText(saveFileDialog1.FileName, richTextBox1.Text);        //UTF-8 without BOM
            }
        }
    }
}
