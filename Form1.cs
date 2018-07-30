using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fibonacci
{
    public partial class Form1 : Form
    {
        ulong[] fibMemo; //List of fibonacci digits
        byte rotation = 0; //Int or enum can be used but eh
        public List<BoxWithACurve> boxes;

        int totalWidth = 100;
        int totalHeight = 100;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int length = (int)numericUpDown1.Value;

            fibMemo = new ulong[length];


            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++) // Calculate and write the fibonacci digits
            {
                ulong fibDigit = GetFib(i); //Get Nth fibonacci digit ( i is used instead of n )

                sb.Append(fibDigit.ToString()); //Write the digit
                if (i != length - 1) sb.Append(","); //Add a , if not at the end

            }
            richTextBox1.Text = sb.ToString();



            boxes = new List<BoxWithACurve>();

            rotation = (byte)numericUpDown3.Value; //Starting rotation / direction

            int boxSize = (int)numericUpDown2.Value;

            for (int i = 0; i < length; i++) //Calculate box locations and rotations. Is in a seperate loop for clarity
            {


                ulong fibDigit = fibMemo[i];

                if (fibDigit == 0) continue;

                int size = (int)fibDigit * boxSize;


                if (boxes.Count > 0)
                {
                    if (rotation == 0) //LowRight to UpLeft
                    {
                        ShiftAll(0, size);

                    }
                    else if (rotation == 1)//UpRight to LowLeft
                    {
                        ShiftAll(size, 0);

                    }
                    else if (rotation == 2)//UpLeft to LowRight
                    {
                        ShiftAll(0, -totalHeight);

                    }
                    else if (rotation == 3)//LowLeft to UpRight
                    {

                        ShiftAll(-totalWidth, 0);
                    }
                }



                boxes.Add(new BoxWithACurve(rotation, size));

                Normalize();
                GetTotalSize();


                rotation++;
                if (rotation == 4) rotation = 0;


            }



            Normalize();
            GetTotalSize();

            pictureBox1.Image = null;

            if ((totalWidth + totalHeight) <= 0) return;

            if (totalHeight > totalWidth) //Square the canvas
            {
                totalWidth = totalHeight;
            }
            else
            {
                totalHeight = totalWidth;
            }

            Bitmap bmp = new Bitmap(totalWidth, totalHeight);

            for (int x = 0; x < totalWidth; x++)
            {
                for (int y = 0; y < totalHeight; y++)
                {
                    bmp.SetPixel(x, y, Color.White);
                }
            }


            for (int i = 0; i < boxes.Count; i++) //Actually draw the thing
            {
                Color c = Color.FromKnownColor(KnownColor.Black);
                if (i == 0) c = Color.FromKnownColor(KnownColor.Red);

                if (checkBox1.Checked)
                {
                    for (int x = 0; x < boxes[i].size; x++)
                    {
                        for (int y = 0; y < boxes[i].size; y++)
                        {
                            if (x == 0 || x == (boxes[i].size - 1))
                            {
                                bmp.SetPixel(boxes[i].xPosition + x, boxes[i].yPosition + y, c);
                            }
                            else if (y == 0 || y == (boxes[i].size - 1))
                            {
                                bmp.SetPixel(boxes[i].xPosition + x, boxes[i].yPosition + y, c);
                            }
                        }
                    }
                }
                if (checkBox3.Checked)//drawing diagonals
                {
                    for (int x = 0; x < boxes[i].size; x++)
                    {


                        if (boxes[i].rotation == 0) //LowRight to UpLeft
                        {
                            bmp.SetPixel(boxes[i].xPosition + x, boxes[i].yPosition + x, c);
                        }
                        else if (boxes[i].rotation == 1)//UpRight to LowLeft
                        {
                            bmp.SetPixel(boxes[i].xPosition + (boxes[i].size - x - 1), boxes[i].yPosition + x, c);
                        }
                        else if (boxes[i].rotation == 2)//UpLeft to LowRight
                        {
                            bmp.SetPixel(boxes[i].xPosition + x, boxes[i].yPosition + x, c);
                        }
                        else if (boxes[i].rotation == 3)//LowLeft to UpRight
                        {
                            bmp.SetPixel(boxes[i].xPosition + x, boxes[i].yPosition + (boxes[i].size - x - 1), c);
                        }

                    }
                }
                if (checkBox2.Checked)//drawing circle part
                {
                    for (int x = 0; x < boxes[i].size; x++)
                    {
                        for (int y = 0; y < boxes[i].size; y++)
                        {
                            if (boxes[i].rotation == 0) //LowRight to UpLeft
                            {
                                float h = ((float)boxes[i].size);
                                float dist = (float)Math.Sqrt((float)((boxes[i].size - x) * (boxes[i].size - x) + (boxes[i].size - y) * (boxes[i].size - y)));

                                if (dist < h)
                                {

                                    if ((dist + (float)numericUpDown4.Value) > h)
                                        bmp.SetPixel(boxes[i].xPosition + (boxes[i].size - x), boxes[i].yPosition + y, c);
                                }
                            }
                            else if (boxes[i].rotation == 1)//UpRight to LowLeft
                            {
                                float h = ((float)boxes[i].size);
                                float dist = (float)Math.Sqrt((float)((boxes[i].size - x) * (boxes[i].size - x) + (boxes[i].size - y) * (boxes[i].size - y)));

                                if (dist < h)
                                {

                                    if ((dist + (float)numericUpDown4.Value) > h)
                                        bmp.SetPixel(boxes[i].xPosition + x, boxes[i].yPosition + y, c);
                                }
                            }
                            else if (boxes[i].rotation == 2)//UpLeft to LowRight
                            {
                                float h = ((float)boxes[i].size);
                                float dist = (float)Math.Sqrt((float)((boxes[i].size - x) * (boxes[i].size - x) + (boxes[i].size - y) * (boxes[i].size - y)));
                                if (dist < h)
                                {
                                    if ((dist + (float)numericUpDown4.Value) > h)
                                        bmp.SetPixel(boxes[i].xPosition + x, boxes[i].yPosition + (boxes[i].size - y), c);
                                }
                            }
                            else if (boxes[i].rotation == 3)//LowLeft to UpRight
                            {
                                float h = ((float)boxes[i].size);
                                float dist = (float)Math.Sqrt((float)((boxes[i].size - x) * (boxes[i].size - x) + (boxes[i].size - y) * (boxes[i].size - y)));
                                if (dist < h)
                                {
                                    if ((dist + (float)numericUpDown4.Value) > h)
                                        bmp.SetPixel(boxes[i].xPosition + (boxes[i].size - x), boxes[i].yPosition + (boxes[i].size - y), c);
                                }
                            }
                        }
                    }
                }
            }


            pictureBox1.Image = bmp;
            pictureBox1.Refresh();
            pictureBox1.Update();


        }

        public void Normalize()
        {
            int xSpace = 0;
            int ySpace = 0;
            for (int i = 0; i < boxes.Count; i++)
            {
                if (boxes[i].xPosition < xSpace)
                {
                    xSpace = boxes[i].xPosition;
                }
                if (boxes[i].yPosition < ySpace)
                {
                    ySpace = boxes[i].yPosition;
                }
            }
            if (xSpace != 0 || ySpace != 0)
            {
                ShiftAll(-xSpace, -ySpace);
            }
        }

        public void GetTotalSize()
        {
            totalHeight = -1;
            totalWidth = -1;

            for (int i = 0; i < boxes.Count; i++)
            {
                if (totalWidth < boxes[i].xPosition + boxes[i].size)
                {
                    totalWidth = boxes[i].xPosition + boxes[i].size;
                }
                if (totalHeight < boxes[i].yPosition + boxes[i].size)
                {
                    totalHeight = boxes[i].yPosition + boxes[i].size;
                }
            }


        }



        public void ShiftAll(int x, int y)
        {
            for (int i = 0; i < boxes.Count; i++)
            {
                boxes[i].xPosition += x;
                boxes[i].yPosition += y;
            }
        }

        public ulong GetFib(int i)
        {
            if (i > 1)
            {
                if (i < fibMemo.Length)
                {
                    if (fibMemo[i] != 0)
                    {
                        return fibMemo[i];
                    }
                    else
                    {
                        try
                        {
                            fibMemo[i] = checked(fibMemo[i - 1] + fibMemo[i - 2]); // Fibonacci formula. fibonacci digit n is equal to the sum of past two digits

                        }
                        catch (Exception ex) { MessageBox.Show("ulong overflow at i=" + i.ToString() + ", so keep it below that"); fibMemo[i] = ulong.MaxValue; }
                        return fibMemo[i];
                    }
                }
                else
                {
                    return 0;
                }

            }
            else if (i == 1)
            {
                fibMemo[1] = 1;
                return fibMemo[1];
            }
            else
            {
                fibMemo[0] = 0;
                return fibMemo[0];
            }

            return 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
            }

        }
    }

    public class BoxWithACurve
    {
        public byte rotation;
        public int xPosition;
        public int yPosition;
        public int size;

        public BoxWithACurve(byte r, int s)
        {
            rotation = r;
            size = s;
        }
        public BoxWithACurve(byte r, int s, int x, int y) : this(r, s)
        {
            this.xPosition = x;
            this.yPosition = y;
        }
    }
}
