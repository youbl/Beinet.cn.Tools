using System;
using System.Collections;
using System.IO;
using System.Text;
using ThoughtWorks.QRCode.Codec.Data;

namespace ThoughtWorks.QRCode.Codec.Util
{
    public class SystemUtils
    {
        public static int ReadInput(Stream sourceStream, sbyte[] target, int start, int count)
        {
            if (target.Length == 0)
                return 0;
            byte[] buffer = new byte[target.Length];
            int num = sourceStream.Read(buffer, start, count);
            if (num == 0)
                return -1;
            for (int index = start; index < start + num; ++index)
                target[index] = (sbyte)buffer[index];
            return num;
        }

        public static int ReadInput(TextReader sourceTextReader, short[] target, int start, int count)
        {
            if (target.Length == 0)
                return 0;
            char[] buffer = new char[target.Length];
            int num = sourceTextReader.Read(buffer, start, count);
            if (num == 0)
                return -1;
            for (int index = start; index < start + num; ++index)
                target[index] = (short)buffer[index];
            return num;
        }

        public static void WriteStackTrace(Exception throwable, TextWriter stream)
        {
            stream.Write(throwable.StackTrace);
            stream.Flush();
        }

        public static int URShift(int number, int bits)
        {
            if (number >= 0)
                return number >> bits;
            return (number >> bits) + (2 << ~bits);
        }

        public static int URShift(int number, long bits)
        {
            return SystemUtils.URShift(number, (int)bits);
        }

        public static long URShift(long number, int bits)
        {
            if (number >= 0L)
                return number >> bits;
            return (number >> bits) + (2L << ~bits);
        }

        public static long URShift(long number, long bits)
        {
            return SystemUtils.URShift(number, (int)bits);
        }

        public static byte[] ToByteArray(sbyte[] sbyteArray)
        {
            byte[] numArray = null;
            if (sbyteArray != null)
            {
                numArray = new byte[sbyteArray.Length];
                for (int index = 0; index < sbyteArray.Length; ++index)
                    numArray[index] = (byte)sbyteArray[index];
            }
            return numArray;
        }

        public static byte[] ToByteArray(string sourceString)
        {
            return Encoding.UTF8.GetBytes(sourceString);
        }

        public static byte[] ToByteArray(object[] tempObjectArray)
        {
            byte[] numArray = null;
            if (tempObjectArray != null)
            {
                numArray = new byte[tempObjectArray.Length];
                for (int index = 0; index < tempObjectArray.Length; ++index)
                    numArray[index] = (byte)tempObjectArray[index];
            }
            return numArray;
        }

        public static sbyte[] ToSByteArray(byte[] byteArray)
        {
            sbyte[] numArray = null;
            if (byteArray != null)
            {
                numArray = new sbyte[byteArray.Length];
                for (int index = 0; index < byteArray.Length; ++index)
                    numArray[index] = (sbyte)byteArray[index];
            }
            return numArray;
        }

        public static char[] ToCharArray(sbyte[] sByteArray)
        {
            return Encoding.UTF8.GetChars(SystemUtils.ToByteArray(sByteArray));
        }

        public static char[] ToCharArray(byte[] byteArray)
        {
            return Encoding.UTF8.GetChars(byteArray);
        }
    }


    public class QRCodeUtility
    {
        public static int sqrt(int val)
        {
            int num1 = 0;
            int num2 = 32768;
            int num3 = 15;
            do
            {
                int num4 = val;
                int num5 = (num1 << 1) + num2;
                int num6 = num3-- & 31;
                int num7;
                int num8 = num7 = num5 << num6;
                if (num4 >= num7)
                {
                    num1 += num2;
                    val -= num8;
                }
            }
            while ((num2 >>= 1) > 0);
            return num1;
        }

        public static bool IsUniCode(string value)
        {
            return QRCodeUtility.FromASCIIByteArray(QRCodeUtility.AsciiStringToByteArray(value)) != QRCodeUtility.FromUnicodeByteArray(QRCodeUtility.UnicodeStringToByteArray(value));
        }

        public static bool IsUnicode(byte[] byteData)
        {
            return AsciiStringToByteArray(FromASCIIByteArray(byteData))[0] != UnicodeStringToByteArray(FromUnicodeByteArray(byteData))[0];
        }

        public static string FromASCIIByteArray(byte[] characters)
        {
            return new ASCIIEncoding().GetString(characters);
        }

        public static string FromUnicodeByteArray(byte[] characters)
        {
            return new UnicodeEncoding().GetString(characters);
        }

        public static byte[] AsciiStringToByteArray(string str)
        {
            return new ASCIIEncoding().GetBytes(str);
        }

        public static byte[] UnicodeStringToByteArray(string str)
        {
            return new UnicodeEncoding().GetBytes(str);
        }
    }

    public class BCH15_5
    {
        internal static string[] bitName = new string[]
        {
      "c0",
      "c1",
      "c2",
      "c3",
      "c4",
      "c5",
      "c6",
      "c7",
      "c8",
      "c9",
      "d0",
      "d1",
      "d2",
      "d3",
      "d4"
        };
        internal int[][] gf16;
        internal bool[] recieveData;
        internal int numCorrectedError;

        public virtual int NumCorrectedError
        {
            get
            {
                return this.numCorrectedError;
            }
        }

        public BCH15_5(bool[] source)
        {
            this.gf16 = this.createGF16();
            this.recieveData = source;
        }

        public virtual bool[] correct()
        {
            return this.correctErrorBit(this.recieveData, this.detectErrorBitPosition(this.calcSyndrome(this.recieveData)));
        }

        internal virtual int[][] createGF16()
        {
            this.gf16 = new int[16][];
            for (int index = 0; index < 16; ++index)
                this.gf16[index] = new int[4];
            int[] numArray = new int[] { 1, 1, 0, 0 };
            for (int index = 0; index < 4; ++index)
                this.gf16[index][index] = 1;
            for (int index = 0; index < 4; ++index)
                this.gf16[4][index] = numArray[index];
            for (int index1 = 5; index1 < 16; ++index1)
            {
                for (int index2 = 1; index2 < 4; ++index2)
                    this.gf16[index1][index2] = this.gf16[index1 - 1][index2 - 1];
                if (this.gf16[index1 - 1][3] == 1)
                {
                    for (int index2 = 0; index2 < 4; ++index2)
                        this.gf16[index1][index2] = (this.gf16[index1][index2] + numArray[index2]) % 2;
                }
            }
            return this.gf16;
        }

        internal virtual int searchElement(int[] x)
        {
            int index = 0;
            while (index < 15 && (x[0] != this.gf16[index][0] || x[1] != this.gf16[index][1] || x[2] != this.gf16[index][2] || x[3] != this.gf16[index][3]))
                ++index;
            return index;
        }

        internal virtual int[] getCode(int input)
        {
            int[] numArray1 = new int[15];
            int[] numArray2 = new int[8];
            for (int index = 0; index < 15; ++index)
            {
                int num1 = numArray2[7];
                int num2;
                int num3;
                if (index < 7)
                {
                    num2 = (input >> 6 - index) % 2;
                    num3 = (num2 + num1) % 2;
                }
                else
                {
                    num2 = num1;
                    num3 = 0;
                }
                numArray2[7] = (numArray2[6] + num3) % 2;
                numArray2[6] = (numArray2[5] + num3) % 2;
                numArray2[5] = numArray2[4];
                numArray2[4] = (numArray2[3] + num3) % 2;
                numArray2[3] = numArray2[2];
                numArray2[2] = numArray2[1];
                numArray2[1] = numArray2[0];
                numArray2[0] = num3;
                numArray1[14 - index] = num2;
            }
            return numArray1;
        }

        internal virtual int addGF(int arg1, int arg2)
        {
            int[] x = new int[4];
            for (int index = 0; index < 4; ++index)
            {
                int num1 = arg1 < 0 || arg1 >= 15 ? 0 : this.gf16[arg1][index];
                int num2 = arg2 < 0 || arg2 >= 15 ? 0 : this.gf16[arg2][index];
                x[index] = (num1 + num2) % 2;
            }
            return this.searchElement(x);
        }

        internal virtual int[] calcSyndrome(bool[] y)
        {
            int[] numArray = new int[5];
            int[] x1 = new int[4];
            for (int index1 = 0; index1 < 15; ++index1)
            {
                if (y[index1])
                {
                    for (int index2 = 0; index2 < 4; ++index2)
                        x1[index2] = (x1[index2] + this.gf16[index1][index2]) % 2;
                }
            }
            int num1 = this.searchElement(x1);
            numArray[0] = num1 >= 15 ? -1 : num1;
            int[] x2 = new int[4];
            for (int index1 = 0; index1 < 15; ++index1)
            {
                if (y[index1])
                {
                    for (int index2 = 0; index2 < 4; ++index2)
                        x2[index2] = (x2[index2] + this.gf16[index1 * 3 % 15][index2]) % 2;
                }
            }
            int num2 = this.searchElement(x2);
            numArray[2] = num2 >= 15 ? -1 : num2;
            int[] x3 = new int[4];
            for (int index1 = 0; index1 < 15; ++index1)
            {
                if (y[index1])
                {
                    for (int index2 = 0; index2 < 4; ++index2)
                        x3[index2] = (x3[index2] + this.gf16[index1 * 5 % 15][index2]) % 2;
                }
            }
            int num3 = this.searchElement(x3);
            numArray[4] = num3 >= 15 ? -1 : num3;
            return numArray;
        }

        internal virtual int[] calcErrorPositionVariable(int[] s)
        {
            int[] numArray = new int[] { s[0], 0, 0, 0 };
            int num1 = (s[0] + s[1]) % 15;
            int num2 = this.addGF(s[2], num1);
            int num3 = num2 >= 15 ? -1 : num2;
            int num4 = (s[2] + s[1]) % 15;
            int num5 = this.addGF(s[4], num4);
            int num6 = num5 >= 15 ? -1 : num5;
            numArray[1] = num6 >= 0 || num3 >= 0 ? (num6 - num3 + 15) % 15 : -1;
            int num7 = (s[1] + numArray[0]) % 15;
            int num8 = this.addGF(s[2], num7);
            int num9 = (s[0] + numArray[1]) % 15;
            numArray[2] = this.addGF(num8, num9);
            return numArray;
        }

        internal virtual int[] detectErrorBitPosition(int[] s)
        {
            int[] numArray1 = this.calcErrorPositionVariable(s);
            int[] numArray2 = new int[4];
            if (numArray1[0] == -1)
                return numArray2;
            if (numArray1[1] == -1)
            {
                numArray2[0] = 1;
                numArray2[1] = numArray1[0];
                return numArray2;
            }
            for (int index = 0; index < 15; ++index)
            {
                int num1 = index * 3 % 15;
                int num2 = index * 2 % 15;
                int num3 = index;
                int num4 = (numArray1[0] + num2) % 15;
                if (this.addGF(this.addGF(num1, num4), this.addGF((numArray1[1] + num3) % 15, numArray1[2])) >= 15)
                {
                    ++numArray2[0];
                    numArray2[numArray2[0]] = index;
                }
            }
            return numArray2;
        }

        internal virtual bool[] correctErrorBit(bool[] y, int[] errorPos)
        {
            for (int index = 1; index <= errorPos[0]; ++index)
                y[errorPos[index]] = !y[errorPos[index]];
            this.numCorrectedError = errorPos[0];
            return y;
        }
    }


    public class SamplingGrid
    {
        private SamplingGrid.AreaGrid[][] grid;

        public virtual int TotalWidth
        {
            get
            {
                int num = 0;
                for (int index = 0; index < this.grid.Length; ++index)
                {
                    num += this.grid[index][0].Width;
                    if (index > 0)
                        --num;
                }
                return num;
            }
        }

        public virtual int TotalHeight
        {
            get
            {
                int num = 0;
                for (int index = 0; index < this.grid[0].Length; ++index)
                {
                    num += this.grid[0][index].Height;
                    if (index > 0)
                        --num;
                }
                return num;
            }
        }

        public SamplingGrid(int sqrtNumArea)
        {
            this.grid = new SamplingGrid.AreaGrid[sqrtNumArea][];
            for (int index = 0; index < sqrtNumArea; ++index)
                this.grid[index] = new SamplingGrid.AreaGrid[sqrtNumArea];
        }

        public virtual void initGrid(int ax, int ay, int width, int height)
        {
            this.grid[ax][ay] = new SamplingGrid.AreaGrid(this, width, height);
        }

        public virtual void setXLine(int ax, int ay, int x, Line line)
        {
            this.grid[ax][ay].setXLine(x, line);
        }

        public virtual void setYLine(int ax, int ay, int y, Line line)
        {
            this.grid[ax][ay].setYLine(y, line);
        }

        public virtual Line getXLine(int ax, int ay, int x)
        {
            return this.grid[ax][ay].getXLine(x);
        }

        public virtual Line getYLine(int ax, int ay, int y)
        {
            return this.grid[ax][ay].getYLine(y);
        }

        public virtual Line[] getXLines(int ax, int ay)
        {
            return this.grid[ax][ay].XLines;
        }

        public virtual Line[] getYLines(int ax, int ay)
        {
            return this.grid[ax][ay].YLines;
        }

        public virtual int getWidth()
        {
            return this.grid[0].Length;
        }

        public virtual int getHeight()
        {
            return this.grid.Length;
        }

        public virtual int getWidth(int ax, int ay)
        {
            return this.grid[ax][ay].Width;
        }

        public virtual int getHeight(int ax, int ay)
        {
            return this.grid[ax][ay].Height;
        }

        public virtual int getX(int ax, int x)
        {
            int num = x;
            for (int index = 0; index < ax; ++index)
                num += this.grid[index][0].Width - 1;
            return num;
        }

        public virtual int getY(int ay, int y)
        {
            int num = y;
            for (int index = 0; index < ay; ++index)
                num += this.grid[0][index].Height - 1;
            return num;
        }

        public virtual void adjust(Point adjust)
        {
            int x = adjust.X;
            int y = adjust.Y;
            for (int index1 = 0; index1 < this.grid[0].Length; ++index1)
            {
                for (int index2 = 0; index2 < this.grid.Length; ++index2)
                {
                    for (int index3 = 0; index3 < this.grid[index2][index1].XLines.Length; ++index3)
                        this.grid[index2][index1].XLines[index3].translate(x, y);
                    for (int index3 = 0; index3 < this.grid[index2][index1].YLines.Length; ++index3)
                        this.grid[index2][index1].YLines[index3].translate(x, y);
                }
            }
        }

        private class AreaGrid
        {
            private SamplingGrid enclosingInstance;
            private Line[] xLine;
            private Line[] yLine;

            private void InitBlock(SamplingGrid enclosingInstance)
            {
                this.enclosingInstance = enclosingInstance;
            }

            public virtual int Width
            {
                get
                {
                    return this.xLine.Length;
                }
            }

            public virtual int Height
            {
                get
                {
                    return this.yLine.Length;
                }
            }

            public virtual Line[] XLines
            {
                get
                {
                    return this.xLine;
                }
            }

            public virtual Line[] YLines
            {
                get
                {
                    return this.yLine;
                }
            }

            public SamplingGrid Enclosing_Instance
            {
                get
                {
                    return this.enclosingInstance;
                }
            }

            public AreaGrid(SamplingGrid enclosingInstance, int width, int height)
            {
                this.InitBlock(enclosingInstance);
                this.xLine = new Line[width];
                this.yLine = new Line[height];
            }

            public virtual Line getXLine(int x)
            {
                return this.xLine[x];
            }

            public virtual Line getYLine(int y)
            {
                return this.yLine[y];
            }

            public virtual void setXLine(int x, Line line)
            {
                this.xLine[x] = line;
            }

            public virtual void setYLine(int y, Line line)
            {
                this.yLine[y] = line;
            }
        }
    }

    public class QRCodeImageReader
    {
        public static int DECIMAL_POINT = 21;
        public const bool POINT_DARK = true;
        public const bool POINT_LIGHT = false;
        internal DebugCanvas canvas;
        internal SamplingGrid samplingGrid;
        internal bool[][] bitmap;

        public QRCodeImageReader()
        {
            this.canvas = QRCodeDecoder.Canvas;
        }

        internal virtual bool[][] applyMedianFilter(bool[][] image, int threshold)
        {
            bool[][] flagArray = new bool[image.Length][];
            for (int index = 0; index < image.Length; ++index)
                flagArray[index] = new bool[image[0].Length];
            for (int index1 = 1; index1 < image[0].Length - 1; ++index1)
            {
                for (int index2 = 1; index2 < image.Length - 1; ++index2)
                {
                    int num = 0;
                    for (int index3 = -1; index3 < 2; ++index3)
                    {
                        for (int index4 = -1; index4 < 2; ++index4)
                        {
                            if (image[index2 + index4][index1 + index3])
                                ++num;
                        }
                    }
                    if (num > threshold)
                        flagArray[index2][index1] = true;
                }
            }
            return flagArray;
        }

        internal virtual bool[][] applyCrossMaskingMedianFilter(bool[][] image, int threshold)
        {
            bool[][] flagArray = new bool[image.Length][];
            for (int index = 0; index < image.Length; ++index)
                flagArray[index] = new bool[image[0].Length];
            for (int index1 = 2; index1 < image[0].Length - 2; ++index1)
            {
                for (int index2 = 2; index2 < image.Length - 2; ++index2)
                {
                    int num = 0;
                    for (int index3 = -2; index3 < 3; ++index3)
                    {
                        if (image[index2 + index3][index1])
                            ++num;
                        if (image[index2][index1 + index3])
                            ++num;
                    }
                    if (num > threshold)
                        flagArray[index2][index1] = true;
                }
            }
            return flagArray;
        }

        internal virtual bool[][] filterImage(int[][] image)
        {
            this.imageToGrayScale(image);
            return this.grayScaleToBitmap(image);
        }

        internal virtual void imageToGrayScale(int[][] image)
        {
            for (int index1 = 0; index1 < image[0].Length; ++index1)
            {
                for (int index2 = 0; index2 < image.Length; ++index2)
                {
                    int num = ((image[index2][index1] >> 16 & (int)byte.MaxValue) * 30 + (image[index2][index1] >> 8 & (int)byte.MaxValue) * 59 + (image[index2][index1] & (int)byte.MaxValue) * 11) / 100;
                    image[index2][index1] = num;
                }
            }
        }

        internal virtual bool[][] grayScaleToBitmap(int[][] grayScale)
        {
            int[][] brightnessPerArea = this.getMiddleBrightnessPerArea(grayScale);
            int length = brightnessPerArea.Length;
            int num1 = grayScale.Length / length;
            int num2 = grayScale[0].Length / length;
            bool[][] flagArray = new bool[grayScale.Length][];
            for (int index = 0; index < grayScale.Length; ++index)
                flagArray[index] = new bool[grayScale[0].Length];
            for (int index1 = 0; index1 < length; ++index1)
            {
                for (int index2 = 0; index2 < length; ++index2)
                {
                    for (int index3 = 0; index3 < num2; ++index3)
                    {
                        for (int index4 = 0; index4 < num1; ++index4)
                            flagArray[num1 * index2 + index4][num2 * index1 + index3] = grayScale[num1 * index2 + index4][num2 * index1 + index3] < brightnessPerArea[index2][index1];
                    }
                }
            }
            return flagArray;
        }

        internal virtual int[][] getMiddleBrightnessPerArea(int[][] image)
        {
            int length = 4;
            int num1 = image.Length / length;
            int num2 = image[0].Length / length;
            int[][][] numArray1 = new int[length][][];
            for (int index1 = 0; index1 < length; ++index1)
            {
                numArray1[index1] = new int[length][];
                for (int index2 = 0; index2 < length; ++index2)
                    numArray1[index1][index2] = new int[2];
            }
            for (int index1 = 0; index1 < length; ++index1)
            {
                for (int index2 = 0; index2 < length; ++index2)
                {
                    numArray1[index2][index1][0] = (int)byte.MaxValue;
                    for (int index3 = 0; index3 < num2; ++index3)
                    {
                        for (int index4 = 0; index4 < num1; ++index4)
                        {
                            int num3 = image[num1 * index2 + index4][num2 * index1 + index3];
                            if (num3 < numArray1[index2][index1][0])
                                numArray1[index2][index1][0] = num3;
                            if (num3 > numArray1[index2][index1][1])
                                numArray1[index2][index1][1] = num3;
                        }
                    }
                }
            }
            int[][] numArray2 = new int[length][];
            for (int index = 0; index < length; ++index)
                numArray2[index] = new int[length];
            for (int index1 = 0; index1 < length; ++index1)
            {
                for (int index2 = 0; index2 < length; ++index2)
                    numArray2[index2][index1] = (numArray1[index2][index1][0] + numArray1[index2][index1][1]) / 2;
            }
            return numArray2;
        }

        public virtual QRCodeSymbol getQRCodeSymbol(int[][] image)
        {
            QRCodeImageReader.DECIMAL_POINT = 23 - QRCodeUtility.sqrt((image.Length < image[0].Length ? image[0].Length : image.Length) / 256);
            this.bitmap = this.filterImage(image);
            this.canvas.println("Drawing matrix.");
            this.canvas.drawMatrix(this.bitmap);
            this.canvas.println("Scanning Finder Pattern.");
            FinderPattern finderPattern;
            try
            {
                finderPattern = FinderPattern.findFinderPattern(this.bitmap);
            }
            catch (FinderPatternNotFoundException)
            {
                this.canvas.println("Not found, now retrying...");
                this.bitmap = this.applyCrossMaskingMedianFilter(this.bitmap, 5);
                this.canvas.drawMatrix(this.bitmap);
                int num = 0;
                while (num < 1000000000)
                    ++num;
                try
                {
                    finderPattern = FinderPattern.findFinderPattern(this.bitmap);
                }
                catch (FinderPatternNotFoundException ex2)
                {
                    throw new SymbolNotFoundException(ex2.Message);
                }
                catch (VersionInformationException ex2)
                {
                    throw new SymbolNotFoundException(ex2.Message);
                }
            }
            catch (VersionInformationException ex)
            {
                throw new SymbolNotFoundException(ex.Message);
            }
            this.canvas.println("FinderPattern at");
            this.canvas.println(finderPattern.getCenter(0).ToString() + finderPattern.getCenter(1).ToString() + finderPattern.getCenter(2).ToString());
            int[] angle = finderPattern.getAngle();
            this.canvas.println("Angle*4098: Sin " + Convert.ToString(angle[0]) + "  Cos " + Convert.ToString(angle[1]));
            int version = finderPattern.Version;
            this.canvas.println("Version: " + Convert.ToString(version));
            if (version < 1 || version > 40)
                throw new InvalidVersionException("Invalid version: " + (object)version);
            AlignmentPattern alignmentPattern;
            try
            {
                alignmentPattern = AlignmentPattern.findAlignmentPattern(this.bitmap, finderPattern);
            }
            catch (AlignmentPatternNotFoundException ex)
            {
                throw new SymbolNotFoundException(ex.Message);
            }
            int length = alignmentPattern.getCenter().Length;
            this.canvas.println("AlignmentPatterns at");
            for (int index1 = 0; index1 < length; ++index1)
            {
                string str = "";
                for (int index2 = 0; index2 < length; ++index2)
                    str += alignmentPattern.getCenter()[index2][index1].ToString();
                this.canvas.println(str);
            }
            this.canvas.println("Creating sampling grid.");
            this.samplingGrid = this.getSamplingGrid(finderPattern, alignmentPattern);
            this.canvas.println("Reading grid.");
            bool[][] qrCodeMatrix;
            try
            {
                qrCodeMatrix = this.getQRCodeMatrix(this.bitmap, this.samplingGrid);
            }
            catch (IndexOutOfRangeException)
            {
                throw new SymbolNotFoundException("Sampling grid exceeded image boundary");
            }
            return new QRCodeSymbol(qrCodeMatrix);
        }

        public virtual QRCodeSymbol getQRCodeSymbolWithAdjustedGrid(Point adjust)
        {
            if (this.bitmap == null || this.samplingGrid == null)
                throw new SystemException("This method must be called after QRCodeImageReader.getQRCodeSymbol() called");
            this.samplingGrid.adjust(adjust);
            this.canvas.println("Sampling grid adjusted d(" + (object)adjust.X + "," + (object)adjust.Y + ")");
            bool[][] qrCodeMatrix;
            try
            {
                qrCodeMatrix = this.getQRCodeMatrix(this.bitmap, this.samplingGrid);
            }
            catch (IndexOutOfRangeException)
            {
                throw new SymbolNotFoundException("Sampling grid exceeded image boundary");
            }
            return new QRCodeSymbol(qrCodeMatrix);
        }

        internal virtual SamplingGrid getSamplingGrid(FinderPattern finderPattern, AlignmentPattern alignmentPattern)
        {
            Point[][] center = alignmentPattern.getCenter();
            int version = finderPattern.Version;
            int num = version / 7 + 2;
            center[0][0] = finderPattern.getCenter(0);
            center[num - 1][0] = finderPattern.getCenter(1);
            center[0][num - 1] = finderPattern.getCenter(2);
            int sqrtNumArea = num - 1;
            SamplingGrid samplingGrid = new SamplingGrid(sqrtNumArea);
            Axis axis = new Axis(finderPattern.getAngle(), finderPattern.getModuleSize());
            for (int ay = 0; ay < sqrtNumArea; ++ay)
            {
                for (int ax = 0; ax < sqrtNumArea; ++ax)
                {
                    QRCodeImageReader.ModulePitch modulePitch = new QRCodeImageReader.ModulePitch(this);
                    Line line1 = new Line();
                    Line line2 = new Line();
                    axis.ModulePitch = finderPattern.getModuleSize();
                    Point[][] logicalCenter = AlignmentPattern.getLogicalCenter(finderPattern);
                    Point point1 = center[ax][ay];
                    Point point2 = center[ax + 1][ay];
                    Point point3 = center[ax][ay + 1];
                    Point point4 = center[ax + 1][ay + 1];
                    Point point5 = logicalCenter[ax][ay];
                    Point point6 = logicalCenter[ax + 1][ay];
                    Point point7 = logicalCenter[ax][ay + 1];
                    Point point8 = logicalCenter[ax + 1][ay + 1];
                    if (ax == 0 && ay == 0)
                    {
                        if (sqrtNumArea == 1)
                        {
                            point1 = axis.translate(point1, -3, -3);
                            point2 = axis.translate(point2, 3, -3);
                            point3 = axis.translate(point3, -3, 3);
                            point4 = axis.translate(point4, 6, 6);
                            point5.translate(-6, -6);
                            point6.translate(3, -3);
                            point7.translate(-3, 3);
                            point8.translate(6, 6);
                        }
                        else
                        {
                            point1 = axis.translate(point1, -3, -3);
                            point2 = axis.translate(point2, 0, -6);
                            point3 = axis.translate(point3, -6, 0);
                            point5.translate(-6, -6);
                            point6.translate(0, -6);
                            point7.translate(-6, 0);
                        }
                    }
                    else if (ax == 0 && ay == sqrtNumArea - 1)
                    {
                        point1 = axis.translate(point1, -6, 0);
                        point3 = axis.translate(point3, -3, 3);
                        point4 = axis.translate(point4, 0, 6);
                        point5.translate(-6, 0);
                        point7.translate(-6, 6);
                        point8.translate(0, 6);
                    }
                    else if (ax == sqrtNumArea - 1 && ay == 0)
                    {
                        point1 = axis.translate(point1, 0, -6);
                        point2 = axis.translate(point2, 3, -3);
                        point4 = axis.translate(point4, 6, 0);
                        point5.translate(0, -6);
                        point6.translate(6, -6);
                        point8.translate(6, 0);
                    }
                    else if (ax == sqrtNumArea - 1 && ay == sqrtNumArea - 1)
                    {
                        point3 = axis.translate(point3, 0, 6);
                        point2 = axis.translate(point2, 6, 0);
                        point4 = axis.translate(point4, 6, 6);
                        point7.translate(0, 6);
                        point6.translate(6, 0);
                        point8.translate(6, 6);
                    }
                    else if (ax == 0)
                    {
                        point1 = axis.translate(point1, -6, 0);
                        point3 = axis.translate(point3, -6, 0);
                        point5.translate(-6, 0);
                        point7.translate(-6, 0);
                    }
                    else if (ax == sqrtNumArea - 1)
                    {
                        point2 = axis.translate(point2, 6, 0);
                        point4 = axis.translate(point4, 6, 0);
                        point6.translate(6, 0);
                        point8.translate(6, 0);
                    }
                    else if (ay == 0)
                    {
                        point1 = axis.translate(point1, 0, -6);
                        point2 = axis.translate(point2, 0, -6);
                        point5.translate(0, -6);
                        point6.translate(0, -6);
                    }
                    else if (ay == sqrtNumArea - 1)
                    {
                        point3 = axis.translate(point3, 0, 6);
                        point4 = axis.translate(point4, 0, 6);
                        point7.translate(0, 6);
                        point8.translate(0, 6);
                    }
                    if (ax == 0)
                    {
                        point6.translate(1, 0);
                        point8.translate(1, 0);
                    }
                    else
                    {
                        point5.translate(-1, 0);
                        point7.translate(-1, 0);
                    }
                    if (ay == 0)
                    {
                        point7.translate(0, 1);
                        point8.translate(0, 1);
                    }
                    else
                    {
                        point5.translate(0, -1);
                        point6.translate(0, -1);
                    }
                    int width = point6.X - point5.X;
                    int height = point7.Y - point5.Y;
                    if (version < 7)
                    {
                        width += 3;
                        height += 3;
                    }
                    modulePitch.top = this.getAreaModulePitch(point1, point2, width - 1);
                    modulePitch.left = this.getAreaModulePitch(point1, point3, height - 1);
                    modulePitch.bottom = this.getAreaModulePitch(point3, point4, width - 1);
                    modulePitch.right = this.getAreaModulePitch(point2, point4, height - 1);
                    line1.setP1(point1);
                    line2.setP1(point1);
                    line1.setP2(point3);
                    line2.setP2(point2);
                    samplingGrid.initGrid(ax, ay, width, height);
                    for (int index = 0; index < width; ++index)
                    {
                        Line line3 = new Line(line1.getP1(), line1.getP2());
                        axis.Origin = line3.getP1();
                        axis.ModulePitch = modulePitch.top;
                        line3.setP1(axis.translate(index, 0));
                        axis.Origin = line3.getP2();
                        axis.ModulePitch = modulePitch.bottom;
                        line3.setP2(axis.translate(index, 0));
                        samplingGrid.setXLine(ax, ay, index, line3);
                    }
                    for (int index = 0; index < height; ++index)
                    {
                        Line line3 = new Line(line2.getP1(), line2.getP2());
                        axis.Origin = line3.getP1();
                        axis.ModulePitch = modulePitch.left;
                        line3.setP1(axis.translate(0, index));
                        axis.Origin = line3.getP2();
                        axis.ModulePitch = modulePitch.right;
                        line3.setP2(axis.translate(0, index));
                        samplingGrid.setYLine(ax, ay, index, line3);
                    }
                }
            }
            return samplingGrid;
        }

        internal virtual int getAreaModulePitch(Point start, Point end, int logicalDistance)
        {
            return (new Line(start, end).Length << QRCodeImageReader.DECIMAL_POINT) / logicalDistance;
        }

        internal virtual bool[][] getQRCodeMatrix(bool[][] image, SamplingGrid gridLines)
        {
            int totalWidth = gridLines.TotalWidth;
            this.canvas.println("gridSize=" + (object)totalWidth);
            Point point = (Point)null;
            bool[][] flagArray = new bool[totalWidth][];
            for (int index = 0; index < totalWidth; ++index)
                flagArray[index] = new bool[totalWidth];
            for (int ay = 0; ay < gridLines.getHeight(); ++ay)
            {
                for (int ax = 0; ax < gridLines.getWidth(); ++ax)
                {
                    ArrayList.Synchronized(new ArrayList(10));
                    for (int y1 = 0; y1 < gridLines.getHeight(ax, ay); ++y1)
                    {
                        for (int x1 = 0; x1 < gridLines.getWidth(ax, ay); ++x1)
                        {
                            int x2 = gridLines.getXLine(ax, ay, x1).getP1().X;
                            int y2 = gridLines.getXLine(ax, ay, x1).getP1().Y;
                            int x3 = gridLines.getXLine(ax, ay, x1).getP2().X;
                            int y3 = gridLines.getXLine(ax, ay, x1).getP2().Y;
                            int x4 = gridLines.getYLine(ax, ay, y1).getP1().X;
                            int y4 = gridLines.getYLine(ax, ay, y1).getP1().Y;
                            int x5 = gridLines.getYLine(ax, ay, y1).getP2().X;
                            int y5 = gridLines.getYLine(ax, ay, y1).getP2().Y;
                            int num1 = (y3 - y2) * (x4 - x5) - (y5 - y4) * (x2 - x3);
                            int num2 = (x2 * y3 - x3 * y2) * (x4 - x5) - (x4 * y5 - x5 * y4) * (x2 - x3);
                            int num3 = (x4 * y5 - x5 * y4) * (y3 - y2) - (x2 * y3 - x3 * y2) * (y5 - y4);
                            flagArray[gridLines.getX(ax, x1)][gridLines.getY(ay, y1)] = image[num2 / num1][num3 / num1];
                            if (ay == gridLines.getHeight() - 1 && ax == gridLines.getWidth() - 1 && y1 == gridLines.getHeight(ax, ay) - 1 && x1 == gridLines.getWidth(ax, ay) - 1)
                                point = new Point(num2 / num1, num3 / num1);
                        }
                    }
                }
            }
            if (point.X > image.Length - 1 || point.Y > image[0].Length - 1)
                throw new IndexOutOfRangeException("Sampling grid pointed out of image");
            this.canvas.drawPoint(point, Color_Fields.BLUE);
            return flagArray;
        }

        private class ModulePitch
        {
            public int top;
            public int left;
            public int bottom;
            public int right;
            private QRCodeImageReader enclosingInstance;

            public ModulePitch(QRCodeImageReader enclosingInstance)
            {
                this.InitBlock(enclosingInstance);
            }

            private void InitBlock(QRCodeImageReader enclosingInstance)
            {
                this.enclosingInstance = enclosingInstance;
            }

            public QRCodeImageReader Enclosing_Instance
            {
                get
                {
                    return this.enclosingInstance;
                }
            }
        }
    }


    public class LogicalSeed
    {
        private static int[][] seed = new int[40][];

        public static int[] getSeed(int version)
        {
            return LogicalSeed.seed[version - 1];
        }

        public static int getSeed(int version, int patternNumber)
        {
            return LogicalSeed.seed[version - 1][patternNumber];
        }

        static LogicalSeed()
        {
            LogicalSeed.seed[0] = new int[2] { 6, 14 };
            LogicalSeed.seed[1] = new int[2] { 6, 18 };
            LogicalSeed.seed[2] = new int[2] { 6, 22 };
            LogicalSeed.seed[3] = new int[2] { 6, 26 };
            LogicalSeed.seed[4] = new int[2] { 6, 30 };
            LogicalSeed.seed[5] = new int[2] { 6, 34 };
            LogicalSeed.seed[6] = new int[3]
            {
        6,
        22,
        38
            };
            LogicalSeed.seed[7] = new int[3]
            {
        6,
        24,
        42
            };
            LogicalSeed.seed[8] = new int[3]
            {
        6,
        26,
        46
            };
            LogicalSeed.seed[9] = new int[3]
            {
        6,
        28,
        50
            };
            LogicalSeed.seed[10] = new int[3]
            {
        6,
        30,
        54
            };
            LogicalSeed.seed[11] = new int[3]
            {
        6,
        32,
        58
            };
            LogicalSeed.seed[12] = new int[3]
            {
        6,
        34,
        62
            };
            LogicalSeed.seed[13] = new int[4]
            {
        6,
        26,
        46,
        66
            };
            LogicalSeed.seed[14] = new int[4]
            {
        6,
        26,
        48,
        70
            };
            LogicalSeed.seed[15] = new int[4]
            {
        6,
        26,
        50,
        74
            };
            LogicalSeed.seed[16] = new int[4]
            {
        6,
        30,
        54,
        78
            };
            LogicalSeed.seed[17] = new int[4]
            {
        6,
        30,
        56,
        82
            };
            LogicalSeed.seed[18] = new int[4]
            {
        6,
        30,
        58,
        86
            };
            LogicalSeed.seed[19] = new int[4]
            {
        6,
        34,
        62,
        90
            };
            LogicalSeed.seed[20] = new int[5]
            {
        6,
        28,
        50,
        72,
        94
            };
            LogicalSeed.seed[21] = new int[5]
            {
        6,
        26,
        50,
        74,
        98
            };
            LogicalSeed.seed[22] = new int[5]
            {
        6,
        30,
        54,
        78,
        102
            };
            LogicalSeed.seed[23] = new int[5]
            {
        6,
        28,
        54,
        80,
        106
            };
            LogicalSeed.seed[24] = new int[5]
            {
        6,
        32,
        58,
        84,
        110
            };
            LogicalSeed.seed[25] = new int[5]
            {
        6,
        30,
        58,
        86,
        114
            };
            LogicalSeed.seed[26] = new int[5]
            {
        6,
        34,
        62,
        90,
        118
            };
            LogicalSeed.seed[27] = new int[6]
            {
        6,
        26,
        50,
        74,
        98,
        122
            };
            LogicalSeed.seed[28] = new int[6]
            {
        6,
        30,
        54,
        78,
        102,
        126
            };
            LogicalSeed.seed[29] = new int[6]
            {
        6,
        26,
        52,
        78,
        104,
        130
            };
            LogicalSeed.seed[30] = new int[6]
            {
        6,
        30,
        56,
        82,
        108,
        134
            };
            LogicalSeed.seed[31] = new int[6]
            {
        6,
        34,
        60,
        86,
        112,
        138
            };
            LogicalSeed.seed[32] = new int[6]
            {
        6,
        30,
        58,
        86,
        114,
        142
            };
            LogicalSeed.seed[33] = new int[6]
            {
        6,
        34,
        62,
        90,
        118,
        146
            };
            LogicalSeed.seed[34] = new int[7]
            {
        6,
        30,
        54,
        78,
        102,
        126,
        150
            };
            LogicalSeed.seed[35] = new int[7]
            {
        6,
        24,
        50,
        76,
        102,
        128,
        154
            };
            LogicalSeed.seed[36] = new int[7]
            {
        6,
        28,
        54,
        80,
        106,
        132,
        158
            };
            LogicalSeed.seed[37] = new int[7]
            {
        6,
        32,
        58,
        84,
        110,
        136,
        162
            };
            LogicalSeed.seed[38] = new int[7]
            {
        6,
        26,
        54,
        82,
        110,
        138,
        166
            };
            LogicalSeed.seed[39] = new int[7]
            {
        6,
        30,
        58,
        86,
        114,
        142,
        170
            };
        }
    }


    public class ReedSolomon
    {
        internal int[] gexp = new int[512];
        internal int[] glog = new int[256];
        internal int[] ErrorLocs = new int[256];
        internal int[] ErasureLocs = new int[256];
        internal int NErasures = 0;
        internal bool correctionSucceeded = true;
        internal int[] y;
        internal int NPAR;
        internal int MAXDEG;
        internal int[] synBytes;
        internal int[] Lambda;
        internal int[] Omega;
        internal int NErrors;

        public virtual bool CorrectionSucceeded
        {
            get
            {
                return this.correctionSucceeded;
            }
        }

        public virtual int NumCorrectedErrors
        {
            get
            {
                return this.NErrors;
            }
        }

        public ReedSolomon(int[] source, int NPAR)
        {
            this.initializeGaloisTables();
            this.y = source;
            this.NPAR = NPAR;
            this.MAXDEG = NPAR * 2;
            this.synBytes = new int[this.MAXDEG];
            this.Lambda = new int[this.MAXDEG];
            this.Omega = new int[this.MAXDEG];
        }

        internal virtual void initializeGaloisTables()
        {
            int num1;
            int num2 = num1 = 0;
            int num3 = num1;
            int num4 = num1;
            int num5 = num1;
            int num6 = num1;
            int num7 = num1;
            int num8 = num1;
            int num9 = 1;
            this.gexp[0] = 1;
            this.gexp[(int)byte.MaxValue] = this.gexp[0];
            this.glog[0] = 0;
            for (int index = 1; index < 256; ++index)
            {
                int num10 = num2;
                num2 = num3;
                num3 = num4;
                num4 = num5;
                num5 = num6 ^ num10;
                num6 = num7 ^ num10;
                num7 = num8 ^ num10;
                num8 = num9;
                num9 = num10;
                this.gexp[index] = num9 + num8 * 2 + num7 * 4 + num6 * 8 + num5 * 16 + num4 * 32 + num3 * 64 + num2 * 128;
                this.gexp[index + (int)byte.MaxValue] = this.gexp[index];
            }
            for (int index1 = 1; index1 < 256; ++index1)
            {
                for (int index2 = 0; index2 < 256; ++index2)
                {
                    if (this.gexp[index2] == index1)
                    {
                        this.glog[index1] = index2;
                        break;
                    }
                }
            }
        }

        internal virtual int gmult(int a, int b)
        {
            if (a == 0 || b == 0)
                return 0;
            return this.gexp[this.glog[a] + this.glog[b]];
        }

        internal virtual int ginv(int elt)
        {
            return this.gexp[(int)byte.MaxValue - this.glog[elt]];
        }

        internal virtual void decode_data(int[] data)
        {
            for (int index1 = 0; index1 < this.MAXDEG; ++index1)
            {
                int b = 0;
                for (int index2 = 0; index2 < data.Length; ++index2)
                    b = data[index2] ^ this.gmult(this.gexp[index1 + 1], b);
                this.synBytes[index1] = b;
            }
        }

        public virtual void correct()
        {
            this.decode_data(this.y);
            this.correctionSucceeded = true;
            bool flag = false;
            for (int index = 0; index < this.synBytes.Length; ++index)
            {
                if (this.synBytes[index] != 0)
                    flag = true;
            }
            if (!flag)
                return;
            this.correctionSucceeded = this.correct_errors_erasures(this.y, this.y.Length, 0, new int[1]);
        }

        internal virtual void Modified_Berlekamp_Massey()
        {
            int[] numArray1 = new int[this.MAXDEG];
            int[] numArray2 = new int[this.MAXDEG];
            int[] numArray3 = new int[this.MAXDEG];
            int[] numArray4 = new int[this.MAXDEG];
            this.init_gamma(numArray4);
            this.copy_poly(numArray3, numArray4);
            this.mul_z_poly(numArray3);
            this.copy_poly(numArray1, numArray4);
            int num1 = -1;
            int L = this.NErasures;
            for (int nerasures = this.NErasures; nerasures < 8; ++nerasures)
            {
                int discrepancy = this.compute_discrepancy(numArray1, this.synBytes, L, nerasures);
                if (discrepancy != 0)
                {
                    for (int index = 0; index < this.MAXDEG; ++index)
                        numArray2[index] = numArray1[index] ^ this.gmult(discrepancy, numArray3[index]);
                    if (L < nerasures - num1)
                    {
                        int num2 = nerasures - num1;
                        num1 = nerasures - L;
                        for (int index = 0; index < this.MAXDEG; ++index)
                            numArray3[index] = this.gmult(numArray1[index], this.ginv(discrepancy));
                        L = num2;
                    }
                    for (int index = 0; index < this.MAXDEG; ++index)
                        numArray1[index] = numArray2[index];
                }
                this.mul_z_poly(numArray3);
            }
            for (int index = 0; index < this.MAXDEG; ++index)
                this.Lambda[index] = numArray1[index];
            this.compute_modified_omega();
        }

        internal virtual void compute_modified_omega()
        {
            int[] dst = new int[this.MAXDEG * 2];
            this.mult_polys(dst, this.Lambda, this.synBytes);
            this.zero_poly(this.Omega);
            for (int index = 0; index < this.NPAR; ++index)
                this.Omega[index] = dst[index];
        }

        internal virtual void mult_polys(int[] dst, int[] p1, int[] p2)
        {
            int[] numArray = new int[this.MAXDEG * 2];
            for (int index = 0; index < this.MAXDEG * 2; ++index)
                dst[index] = 0;
            for (int index1 = 0; index1 < this.MAXDEG; ++index1)
            {
                for (int maxdeg = this.MAXDEG; maxdeg < this.MAXDEG * 2; ++maxdeg)
                    numArray[maxdeg] = 0;
                for (int index2 = 0; index2 < this.MAXDEG; ++index2)
                    numArray[index2] = this.gmult(p2[index2], p1[index1]);
                for (int index2 = this.MAXDEG * 2 - 1; index2 >= index1; --index2)
                    numArray[index2] = numArray[index2 - index1];
                for (int index2 = 0; index2 < index1; ++index2)
                    numArray[index2] = 0;
                for (int index2 = 0; index2 < this.MAXDEG * 2; ++index2)
                    dst[index2] ^= numArray[index2];
            }
        }

        internal virtual void init_gamma(int[] gamma)
        {
            int[] numArray = new int[this.MAXDEG];
            this.zero_poly(gamma);
            this.zero_poly(numArray);
            gamma[0] = 1;
            for (int index = 0; index < this.NErasures; ++index)
            {
                this.copy_poly(numArray, gamma);
                this.scale_poly(this.gexp[this.ErasureLocs[index]], numArray);
                this.mul_z_poly(numArray);
                this.add_polys(gamma, numArray);
            }
        }

        internal virtual void compute_next_omega(int d, int[] A, int[] dst, int[] src)
        {
            for (int index = 0; index < this.MAXDEG; ++index)
                dst[index] = src[index] ^ this.gmult(d, A[index]);
        }

        internal virtual int compute_discrepancy(int[] lambda, int[] S, int L, int n)
        {
            int num = 0;
            for (int index = 0; index <= L; ++index)
                num ^= this.gmult(lambda[index], S[n - index]);
            return num;
        }

        internal virtual void add_polys(int[] dst, int[] src)
        {
            for (int index = 0; index < this.MAXDEG; ++index)
                dst[index] ^= src[index];
        }

        internal virtual void copy_poly(int[] dst, int[] src)
        {
            for (int index = 0; index < this.MAXDEG; ++index)
                dst[index] = src[index];
        }

        internal virtual void scale_poly(int k, int[] poly)
        {
            for (int index = 0; index < this.MAXDEG; ++index)
                poly[index] = this.gmult(k, poly[index]);
        }

        internal virtual void zero_poly(int[] poly)
        {
            for (int index = 0; index < this.MAXDEG; ++index)
                poly[index] = 0;
        }

        internal virtual void mul_z_poly(int[] src)
        {
            for (int index = this.MAXDEG - 1; index > 0; --index)
                src[index] = src[index - 1];
            src[0] = 0;
        }

        internal virtual void Find_Roots()
        {
            this.NErrors = 0;
            for (int index1 = 1; index1 < 256; ++index1)
            {
                int num = 0;
                for (int index2 = 0; index2 < this.NPAR + 1; ++index2)
                    num ^= this.gmult(this.gexp[index2 * index1 % (int)byte.MaxValue], this.Lambda[index2]);
                if (num == 0)
                {
                    this.ErrorLocs[this.NErrors] = (int)byte.MaxValue - index1;
                    ++this.NErrors;
                }
            }
        }

        internal virtual bool correct_errors_erasures(int[] codeword, int csize, int nerasures, int[] erasures)
        {
            this.NErasures = nerasures;
            for (int index = 0; index < this.NErasures; ++index)
                this.ErasureLocs[index] = erasures[index];
            this.Modified_Berlekamp_Massey();
            this.Find_Roots();
            if (this.NErrors > this.NPAR && this.NErrors <= 0)
                return false;
            for (int index = 0; index < this.NErrors; ++index)
            {
                if (this.ErrorLocs[index] >= csize)
                    return false;
            }
            for (int index1 = 0; index1 < this.NErrors; ++index1)
            {
                int errorLoc = this.ErrorLocs[index1];
                int a = 0;
                for (int index2 = 0; index2 < this.MAXDEG; ++index2)
                    a ^= this.gmult(this.Omega[index2], this.gexp[((int)byte.MaxValue - errorLoc) * index2 % (int)byte.MaxValue]);
                int elt = 0;
                int index3 = 1;
                while (index3 < this.MAXDEG)
                {
                    elt ^= this.gmult(this.Lambda[index3], this.gexp[((int)byte.MaxValue - errorLoc) * (index3 - 1) % (int)byte.MaxValue]);
                    index3 += 2;
                }
                int num = this.gmult(a, this.ginv(elt));
                codeword[csize - errorLoc - 1] ^= num;
            }
            return true;
        }
    }



    public class QRCodeDataBlockReader
    {
        private int[][] sizeOfDataLengthInfo = new int[3][]
        {
      new int[4]{ 10, 9, 8, 8 },
      new int[4]{ 12, 11, 16, 10 },
      new int[4]{ 14, 13, 16, 12 }
        };
        private const int MODE_NUMBER = 1;
        private const int MODE_ROMAN_AND_NUMBER = 2;
        private const int MODE_8BIT_BYTE = 4;
        private const int MODE_KANJI = 8;
        internal int[] blocks;
        internal int dataLengthMode;
        internal int blockPointer;
        internal int bitPointer;
        internal int dataLength;
        internal int numErrorCorrectionCode;
        internal DebugCanvas canvas;

        internal virtual int NextMode
        {
            get
            {
                if (this.blockPointer > this.blocks.Length - this.numErrorCorrectionCode - 2)
                    return 0;
                return this.getNextBits(4);
            }
        }

        public virtual sbyte[] DataByte
        {
            get
            {
                this.canvas.println("Reading data blocks.");
                MemoryStream memoryStream = new MemoryStream();
                try
                {
                    int nextMode;
                    while (true)
                    {
                        nextMode = this.NextMode;
                        int num;
                        switch (nextMode)
                        {
                            case 0:
                                goto label_3;
                            case 1:
                            case 2:
                            case 4:
                                num = 1;
                                break;
                            default:
                                num = nextMode == 8 ? 1 : 0;
                                break;
                        }
                        if (num != 0)
                        {
                            this.dataLength = this.getDataLength(nextMode);
                            if (this.dataLength >= 1)
                            {
                                switch (nextMode)
                                {
                                    case 1:
                                        sbyte[] sbyteArray1 = SystemUtils.ToSByteArray(SystemUtils.ToByteArray(this.getFigureString(this.dataLength)));
                                        memoryStream.Write(SystemUtils.ToByteArray(sbyteArray1), 0, sbyteArray1.Length);
                                        break;
                                    case 2:
                                        sbyte[] sbyteArray2 = SystemUtils.ToSByteArray(SystemUtils.ToByteArray(this.getRomanAndFigureString(this.dataLength)));
                                        memoryStream.Write(SystemUtils.ToByteArray(sbyteArray2), 0, sbyteArray2.Length);
                                        break;
                                    case 4:
                                        sbyte[] sbyteArray3 = this.get8bitByteArray(this.dataLength);
                                        memoryStream.Write(SystemUtils.ToByteArray(sbyteArray3), 0, sbyteArray3.Length);
                                        break;
                                    case 8:
                                        sbyte[] sbyteArray4 = SystemUtils.ToSByteArray(SystemUtils.ToByteArray(this.getKanjiString(this.dataLength)));
                                        memoryStream.Write(SystemUtils.ToByteArray(sbyteArray4), 0, sbyteArray4.Length);
                                        break;
                                }
                            }
                            else
                                goto label_10;
                        }
                        else
                            goto label_8;
                    }
                    label_3:
                    if (memoryStream.Length <= 0L)
                        throw new InvalidDataBlockException("Empty data block");
                    goto label_19;
                    label_8:
                    throw new InvalidDataBlockException("Invalid mode: " + (object)nextMode + " in (block:" + (object)this.blockPointer + " bit:" + (object)this.bitPointer + ")");
                    label_10:
                    throw new InvalidDataBlockException("Invalid data length: " + (object)this.dataLength);
                }
                catch (IndexOutOfRangeException ex)
                {
                    SystemUtils.WriteStackTrace((Exception)ex, Console.Error);
                    throw new InvalidDataBlockException("Data Block Error in (block:" + (object)this.blockPointer + " bit:" + (object)this.bitPointer + ")");
                }
                catch (IOException ex)
                {
                    throw new InvalidDataBlockException(ex.Message);
                }
                label_19:
                return SystemUtils.ToSByteArray(memoryStream.ToArray());
            }
        }

        public virtual string DataString
        {
            get
            {
                this.canvas.println("Reading data blocks...");
                string str = "";
                while (true)
                {
                    int nextMode = this.NextMode;
                    this.canvas.println("mode: " + (object)nextMode);
                    if (nextMode != 0)
                    {
                        if (nextMode == 1 || nextMode == 2 || nextMode == 4 || nextMode == 8)
                        {
                        }
                        this.dataLength = this.getDataLength(nextMode);
                        this.canvas.println(Convert.ToString(this.blocks[this.blockPointer]));
                        Console.Out.WriteLine("length: " + (object)this.dataLength);
                        switch (nextMode)
                        {
                            case 1:
                                str += this.getFigureString(this.dataLength);
                                break;
                            case 2:
                                str += this.getRomanAndFigureString(this.dataLength);
                                break;
                            case 4:
                                str += this.get8bitByteString(this.dataLength);
                                break;
                            case 8:
                                str += this.getKanjiString(this.dataLength);
                                break;
                        }
                    }
                    else
                        break;
                }
                Console.Out.WriteLine("");
                return str;
            }
        }

        public QRCodeDataBlockReader(int[] blocks, int version, int numErrorCorrectionCode)
        {
            this.blockPointer = 0;
            this.bitPointer = 7;
            this.dataLength = 0;
            this.blocks = blocks;
            this.numErrorCorrectionCode = numErrorCorrectionCode;
            if (version <= 9)
                this.dataLengthMode = 0;
            else if (version >= 10 && version <= 26)
                this.dataLengthMode = 1;
            else if (version >= 27 && version <= 40)
                this.dataLengthMode = 2;
            this.canvas = QRCodeDecoder.Canvas;
        }

        internal virtual int getNextBits(int numBits)
        {
            if (numBits < this.bitPointer + 1)
            {
                int num1 = 0;
                for (int index = 0; index < numBits; ++index)
                    num1 += 1 << index;
                int num2 = (this.blocks[this.blockPointer] & num1 << this.bitPointer - numBits + 1) >> this.bitPointer - numBits + 1;
                this.bitPointer -= numBits;
                return num2;
            }
            if (numBits < this.bitPointer + 1 + 8)
            {
                int num1 = 0;
                for (int index = 0; index < this.bitPointer + 1; ++index)
                    num1 += 1 << index;
                int num2 = (this.blocks[this.blockPointer] & num1) << numBits - (this.bitPointer + 1);
                ++this.blockPointer;
                int num3 = num2 + (this.blocks[this.blockPointer] >> 8 - (numBits - (this.bitPointer + 1)));
                this.bitPointer = this.bitPointer - numBits % 8;
                if (this.bitPointer < 0)
                    this.bitPointer = 8 + this.bitPointer;
                return num3;
            }
            if (numBits < this.bitPointer + 1 + 16)
            {
                int num1 = 0;
                int num2 = 0;
                for (int index = 0; index < this.bitPointer + 1; ++index)
                    num1 += 1 << index;
                int num3 = (this.blocks[this.blockPointer] & num1) << numBits - (this.bitPointer + 1);
                ++this.blockPointer;
                int num4 = this.blocks[this.blockPointer] << numBits - (this.bitPointer + 1 + 8);
                ++this.blockPointer;
                for (int index = 0; index < numBits - (this.bitPointer + 1 + 8); ++index)
                    num2 += 1 << index;
                int num5 = (this.blocks[this.blockPointer] & num2 << 8 - (numBits - (this.bitPointer + 1 + 8))) >> 8 - (numBits - (this.bitPointer + 1 + 8));
                int num6 = num3 + num4 + num5;
                this.bitPointer = this.bitPointer - (numBits - 8) % 8;
                if (this.bitPointer < 0)
                    this.bitPointer = 8 + this.bitPointer;
                return num6;
            }
            Console.Out.WriteLine("ERROR!");
            return 0;
        }

        internal virtual int guessMode(int mode)
        {
            switch (mode)
            {
                case 3:
                    return 1;
                case 5:
                    return 4;
                case 6:
                    return 4;
                case 7:
                    return 4;
                case 9:
                    return 8;
                case 10:
                    return 8;
                case 11:
                    return 8;
                case 12:
                    return 4;
                case 13:
                    return 4;
                case 14:
                    return 4;
                case 15:
                    return 4;
                default:
                    return 8;
            }
        }

        internal virtual int getDataLength(int modeIndicator)
        {
            int index = 0;
            while (true)
            {
                if (modeIndicator >> index != 1)
                    ++index;
                else
                    break;
            }
            return this.getNextBits(this.sizeOfDataLengthInfo[this.dataLengthMode][index]);
        }

        internal virtual string getFigureString(int dataLength)
        {
            int num1 = dataLength;
            int num2 = 0;
            string str = "";
            do
            {
                if (num1 >= 3)
                {
                    num2 = this.getNextBits(10);
                    if (num2 < 100)
                        str += "0";
                    if (num2 < 10)
                        str += "0";
                    num1 -= 3;
                }
                else if (num1 == 2)
                {
                    num2 = this.getNextBits(7);
                    if (num2 < 10)
                        str += "0";
                    num1 -= 2;
                }
                else if (num1 == 1)
                {
                    num2 = this.getNextBits(4);
                    --num1;
                }
                str += Convert.ToString(num2);
            }
            while (num1 > 0);
            return str;
        }

        internal virtual string getRomanAndFigureString(int dataLength)
        {
            int num = dataLength;
            string str = "";
            char[] chArray = new char[45]
            {
        '0',
        '1',
        '2',
        '3',
        '4',
        '5',
        '6',
        '7',
        '8',
        '9',
        'A',
        'B',
        'C',
        'D',
        'E',
        'F',
        'G',
        'H',
        'I',
        'J',
        'K',
        'L',
        'M',
        'N',
        'O',
        'P',
        'Q',
        'R',
        'S',
        'T',
        'U',
        'V',
        'W',
        'X',
        'Y',
        'Z',
        ' ',
        '$',
        '%',
        '*',
        '+',
        '-',
        '.',
        '/',
        ':'
            };
            do
            {
                if (num > 1)
                {
                    int nextBits = this.getNextBits(11);
                    int index1 = nextBits / 45;
                    int index2 = nextBits % 45;
                    str = str + Convert.ToString(chArray[index1]) + Convert.ToString(chArray[index2]);
                    num -= 2;
                }
                else if (num == 1)
                {
                    int nextBits = this.getNextBits(6);
                    str += Convert.ToString(chArray[nextBits]);
                    --num;
                }
            }
            while (num > 0);
            return str;
        }

        public virtual sbyte[] get8bitByteArray(int dataLength)
        {
            int num = dataLength;
            MemoryStream memoryStream = new MemoryStream();
            do
            {
                this.canvas.println("Length: " + (object)num);
                int nextBits = this.getNextBits(8);
                memoryStream.WriteByte((byte)nextBits);
                --num;
            }
            while (num > 0);
            return SystemUtils.ToSByteArray(memoryStream.ToArray());
        }

        internal virtual string get8bitByteString(int dataLength)
        {
            int num = dataLength;
            string str = "";
            do
            {
                int nextBits = this.getNextBits(8);
                str += (string)(object)(char)nextBits;
                --num;
            }
            while (num > 0);
            return str;
        }

        internal virtual string getKanjiString(int dataLength)
        {
            int num1 = dataLength;
            string str = "";
            do
            {
                int nextBits = this.getNextBits(13);
                int num2 = nextBits % 192;
                int num3 = (nextBits / 192 << 8) + num2;
                int num4 = num3 + 33088 > 40956 ? num3 + 49472 : num3 + 33088;
                sbyte[] sbyteArray = new sbyte[2]
                {
          (sbyte) (num4 >> 8),
          (sbyte) (num4 & (int) byte.MaxValue)
                };
                str += new string(SystemUtils.ToCharArray(SystemUtils.ToByteArray(sbyteArray)));
                --num1;
            }
            while (num1 > 0);
            return str;
        }
    }



    public class FinderPattern
    {
        internal static readonly int[] VersionInfoBit = new int[34]
        {
      31892,
      34236,
      39577,
      42195,
      48118,
      51042,
      55367,
      58893,
      63784,
      68472,
      70749,
      76311,
      79154,
      84390,
      87683,
      92361,
      96236,
      102084,
      102881,
      110507,
      110734,
      117786,
      119615,
      126325,
      127568,
      133589,
      136944,
      141498,
      145311,
      150283,
      152622,
      158308,
      161089,
      167017
        };
        internal static DebugCanvas canvas = QRCodeDecoder.Canvas;
        public const int UL = 0;
        public const int UR = 1;
        public const int DL = 2;
        internal Point[] center;
        internal int version;
        internal int[] sincos;
        internal int[] width;
        internal int[] moduleSize;

        public virtual int Version
        {
            get
            {
                return this.version;
            }
        }

        public virtual int SqrtNumModules
        {
            get
            {
                return 17 + 4 * this.version;
            }
        }

        public static FinderPattern findFinderPattern(bool[][] image)
        {
            Line[] lineCross = FinderPattern.findLineCross(FinderPattern.findLineAcross(image));
            Point[] center;
            try
            {
                center = FinderPattern.getCenter(lineCross);
            }
            catch (FinderPatternNotFoundException ex)
            {
                throw ex;
            }
            int[] angle = FinderPattern.getAngle(center);
            Point[] pointArray = FinderPattern.sort(center, angle);
            int[] width = FinderPattern.getWidth(image, pointArray, angle);
            int[] moduleSize = new int[3]
            {
        (width[0] << QRCodeImageReader.DECIMAL_POINT) / 7,
        (width[1] << QRCodeImageReader.DECIMAL_POINT) / 7,
        (width[2] << QRCodeImageReader.DECIMAL_POINT) / 7
            };
            int version = FinderPattern.calcRoughVersion(pointArray, width);
            if (version > 6)
            {
                try
                {
                    version = FinderPattern.calcExactVersion(pointArray, angle, moduleSize, image);
                }
                catch (VersionInformationException)
                {
                }
            }
            return new FinderPattern(pointArray, version, angle, width, moduleSize);
        }

        internal FinderPattern(Point[] center, int version, int[] sincos, int[] width, int[] moduleSize)
        {
            this.center = center;
            this.version = version;
            this.sincos = sincos;
            this.width = width;
            this.moduleSize = moduleSize;
        }

        public virtual Point[] getCenter()
        {
            return this.center;
        }

        public virtual Point getCenter(int position)
        {
            if (position >= 0 && position <= 2)
                return this.center[position];
            return (Point)null;
        }

        public virtual int getWidth(int position)
        {
            return this.width[position];
        }

        public virtual int[] getAngle()
        {
            return this.sincos;
        }

        public virtual int getModuleSize()
        {
            return this.moduleSize[0];
        }

        public virtual int getModuleSize(int place)
        {
            return this.moduleSize[place];
        }

        internal static Line[] findLineAcross(bool[][] image)
        {
            int num1 = 0;
            int num2 = 1;
            int length1 = image.Length;
            int length2 = image[0].Length;
            Point point = new Point();
            ArrayList arrayList = ArrayList.Synchronized(new ArrayList(10));
            int[] buffer = new int[5];
            int pointer = 0;
            int num3 = num1;
            bool flag1 = false;
            while (true)
            {
                bool flag2 = image[point.X][point.Y];
                if (flag2 == flag1)
                {
                    ++buffer[pointer];
                }
                else
                {
                    if (!flag2 && FinderPattern.checkPattern(buffer, pointer))
                    {
                        int x1;
                        int x2;
                        int y2;
                        int y1;
                        if (num3 == num1)
                        {
                            x1 = point.X;
                            for (int index = 0; index < 5; ++index)
                                x1 -= buffer[index];
                            x2 = point.X - 1;
                            y1 = y2 = point.Y;
                        }
                        else
                        {
                            x1 = x2 = point.X;
                            y1 = point.Y;
                            for (int index = 0; index < 5; ++index)
                                y1 -= buffer[index];
                            y2 = point.Y - 1;
                        }
                        arrayList.Add((object)new Line(x1, y1, x2, y2));
                    }
                    pointer = (pointer + 1) % 5;
                    buffer[pointer] = 1;
                    flag1 = !flag1;
                }
                if (num3 == num1)
                {
                    if (point.X < length1 - 1)
                        point.translate(1, 0);
                    else if (point.Y < length2 - 1)
                    {
                        point.set_Renamed(0, point.Y + 1);
                        buffer = new int[5];
                    }
                    else
                    {
                        point.set_Renamed(0, 0);
                        buffer = new int[5];
                        num3 = num2;
                    }
                }
                else if (point.Y < length2 - 1)
                    point.translate(0, 1);
                else if (point.X < length1 - 1)
                {
                    point.set_Renamed(point.X + 1, 0);
                    buffer = new int[5];
                }
                else
                    break;
            }
            Line[] lines = new Line[arrayList.Count];
            for (int index = 0; index < lines.Length; ++index)
                lines[index] = (Line)arrayList[index];
            FinderPattern.canvas.drawLines(lines, Color_Fields.LIGHTGREEN);
            return lines;
        }

        internal static bool checkPattern(int[] buffer, int pointer)
        {
            int[] numArray = new int[5] { 1, 1, 3, 1, 1 };
            int num1 = 0;
            for (int index = 0; index < 5; ++index)
                num1 += buffer[index];
            int num2 = (num1 << QRCodeImageReader.DECIMAL_POINT) / 7;
            for (int index = 0; index < 5; ++index)
            {
                int num3 = num2 * numArray[index] - num2 / 2;
                int num4 = num2 * numArray[index] + num2 / 2;
                int num5 = buffer[(pointer + index + 1) % 5] << QRCodeImageReader.DECIMAL_POINT;
                if (num5 < num3 || num5 > num4)
                    return false;
            }
            return true;
        }

        internal static Line[] findLineCross(Line[] lineAcross)
        {
            ArrayList arrayList1 = ArrayList.Synchronized(new ArrayList(10));
            ArrayList arrayList2 = ArrayList.Synchronized(new ArrayList(10));
            ArrayList arrayList3 = ArrayList.Synchronized(new ArrayList(10));
            for (int index = 0; index < lineAcross.Length; ++index)
                arrayList3.Add((object)lineAcross[index]);
            for (int index1 = 0; index1 < arrayList3.Count - 1; ++index1)
            {
                arrayList2.Clear();
                arrayList2.Add(arrayList3[index1]);
                for (int index2 = index1 + 1; index2 < arrayList3.Count; ++index2)
                {
                    if (Line.isNeighbor((Line)arrayList2[arrayList2.Count - 1], (Line)arrayList3[index2]))
                    {
                        arrayList2.Add(arrayList3[index2]);
                        Line line = (Line)arrayList2[arrayList2.Count - 1];
                        if (arrayList2.Count * 5 > line.Length && index2 == arrayList3.Count - 1)
                        {
                            arrayList1.Add(arrayList2[arrayList2.Count / 2]);
                            for (int index3 = 0; index3 < arrayList2.Count; ++index3)
                                arrayList3.Remove(arrayList2[index3]);
                        }
                    }
                    else if (FinderPattern.cantNeighbor((Line)arrayList2[arrayList2.Count - 1], (Line)arrayList3[index2]) || index2 == arrayList3.Count - 1)
                    {
                        Line line = (Line)arrayList2[arrayList2.Count - 1];
                        if (arrayList2.Count * 6 > line.Length)
                        {
                            arrayList1.Add(arrayList2[arrayList2.Count / 2]);
                            for (int index3 = 0; index3 < arrayList2.Count; ++index3)
                                arrayList3.Remove(arrayList2[index3]);
                            break;
                        }
                        break;
                    }
                }
            }
            Line[] lineArray = new Line[arrayList1.Count];
            for (int index = 0; index < lineArray.Length; ++index)
                lineArray[index] = (Line)arrayList1[index];
            return lineArray;
        }

        internal static bool cantNeighbor(Line line1, Line line2)
        {
            if (Line.isCross(line1, line2))
                return true;
            if (line1.Horizontal)
                return Math.Abs(line1.getP1().Y - line2.getP1().Y) > 1;
            return Math.Abs(line1.getP1().X - line2.getP1().X) > 1;
        }

        internal static int[] getAngle(Point[] centers)
        {
            Line[] lines = new Line[3];
            for (int index = 0; index < lines.Length; ++index)
                lines[index] = new Line(centers[index], centers[(index + 1) % lines.Length]);
            Line longest = Line.getLongest(lines);
            Point p1 = new Point();
            for (int index = 0; index < centers.Length; ++index)
            {
                if (!longest.getP1().equals(centers[index]) && !longest.getP2().equals(centers[index]))
                {
                    p1 = centers[index];
                    break;
                }
            }
            FinderPattern.canvas.println("originPoint is: " + (object)p1);
            Point point = new Point();
            Point p2 = !(p1.Y <= longest.getP1().Y & p1.Y <= longest.getP2().Y) ? (!(p1.X >= longest.getP1().X & p1.X >= longest.getP2().X) ? (!(p1.Y >= longest.getP1().Y & p1.Y >= longest.getP2().Y) ? (longest.getP1().Y >= longest.getP2().Y ? longest.getP2() : longest.getP1()) : (longest.getP1().X >= longest.getP2().X ? longest.getP2() : longest.getP1())) : (longest.getP1().Y >= longest.getP2().Y ? longest.getP1() : longest.getP2())) : (longest.getP1().X >= longest.getP2().X ? longest.getP1() : longest.getP2());
            int length = new Line(p1, p2).Length;
            return new int[2]
            {
        (p2.Y - p1.Y << QRCodeImageReader.DECIMAL_POINT) / length,
        (p2.X - p1.X << QRCodeImageReader.DECIMAL_POINT) / length
            };
        }

        internal static Point[] getCenter(Line[] crossLines)
        {
            ArrayList arrayList = ArrayList.Synchronized(new ArrayList(10));
            for (int index1 = 0; index1 < crossLines.Length - 1; ++index1)
            {
                Line crossLine1 = crossLines[index1];
                for (int index2 = index1 + 1; index2 < crossLines.Length; ++index2)
                {
                    Line crossLine2 = crossLines[index2];
                    if (Line.isCross(crossLine1, crossLine2))
                    {
                        int x;
                        int y;
                        if (crossLine1.Horizontal)
                        {
                            x = crossLine1.Center.X;
                            y = crossLine2.Center.Y;
                        }
                        else
                        {
                            x = crossLine2.Center.X;
                            y = crossLine1.Center.Y;
                        }
                        arrayList.Add((object)new Point(x, y));
                    }
                }
            }
            Point[] points = new Point[arrayList.Count];
            for (int index = 0; index < points.Length; ++index)
                points[index] = (Point)arrayList[index];
            if (points.Length != 3)
                throw new FinderPatternNotFoundException("Invalid number of Finder Pattern detected");
            FinderPattern.canvas.drawPolygon(points, Color_Fields.RED);
            return points;
        }

        internal static Point[] sort(Point[] centers, int[] angle)
        {
            Point[] pointArray = new Point[3];
            switch (FinderPattern.getURQuadant(angle))
            {
                case 1:
                    pointArray[1] = FinderPattern.getPointAtSide(centers, 1, 2);
                    pointArray[2] = FinderPattern.getPointAtSide(centers, 2, 4);
                    break;
                case 2:
                    pointArray[1] = FinderPattern.getPointAtSide(centers, 2, 4);
                    pointArray[2] = FinderPattern.getPointAtSide(centers, 8, 4);
                    break;
                case 3:
                    pointArray[1] = FinderPattern.getPointAtSide(centers, 4, 8);
                    pointArray[2] = FinderPattern.getPointAtSide(centers, 1, 8);
                    break;
                case 4:
                    pointArray[1] = FinderPattern.getPointAtSide(centers, 8, 1);
                    pointArray[2] = FinderPattern.getPointAtSide(centers, 2, 1);
                    break;
            }
            for (int index = 0; index < centers.Length; ++index)
            {
                if (!centers[index].equals(pointArray[1]) && !centers[index].equals(pointArray[2]))
                    pointArray[0] = centers[index];
            }
            return pointArray;
        }

        internal static int getURQuadant(int[] angle)
        {
            int num1 = angle[0];
            int num2 = angle[1];
            if (num1 >= 0 && num2 > 0)
                return 1;
            if (num1 > 0 && num2 <= 0)
                return 2;
            if (num1 <= 0 && num2 < 0)
                return 3;
            return num1 < 0 && num2 >= 0 ? 4 : 0;
        }

        internal static Point getPointAtSide(Point[] points, int side1, int side2)
        {
            Point point1 = new Point();
            Point point2 = new Point(side1 == 1 || side2 == 1 ? 0 : int.MaxValue, side1 == 2 || side2 == 2 ? 0 : int.MaxValue);
            for (int index = 0; index < points.Length; ++index)
            {
                switch (side1)
                {
                    case 1:
                        if (point2.X < points[index].X)
                        {
                            point2 = points[index];
                            break;
                        }
                        if (point2.X == points[index].X)
                        {
                            if (side2 == 2)
                            {
                                if (point2.Y < points[index].Y)
                                    point2 = points[index];
                            }
                            else if (point2.Y > points[index].Y)
                                point2 = points[index];
                            break;
                        }
                        break;
                    case 2:
                        if (point2.Y < points[index].Y)
                        {
                            point2 = points[index];
                            break;
                        }
                        if (point2.Y == points[index].Y)
                        {
                            if (side2 == 1)
                            {
                                if (point2.X < points[index].X)
                                    point2 = points[index];
                            }
                            else if (point2.X > points[index].X)
                                point2 = points[index];
                            break;
                        }
                        break;
                    case 4:
                        if (point2.X > points[index].X)
                        {
                            point2 = points[index];
                            break;
                        }
                        if (point2.X == points[index].X)
                        {
                            if (side2 == 2)
                            {
                                if (point2.Y < points[index].Y)
                                    point2 = points[index];
                            }
                            else if (point2.Y > points[index].Y)
                                point2 = points[index];
                            break;
                        }
                        break;
                    case 8:
                        if (point2.Y > points[index].Y)
                        {
                            point2 = points[index];
                            break;
                        }
                        if (point2.Y == points[index].Y)
                        {
                            if (side2 == 1)
                            {
                                if (point2.X < points[index].X)
                                    point2 = points[index];
                            }
                            else if (point2.X > points[index].X)
                                point2 = points[index];
                            break;
                        }
                        break;
                }
            }
            return point2;
        }

        internal static int[] getWidth(bool[][] image, Point[] centers, int[] sincos)
        {
            int[] numArray = new int[3];
            for (int index = 0; index < 3; ++index)
            {
                bool flag1 = false;
                int y = centers[index].Y;
                int x1;
                for (x1 = centers[index].X; x1 > 0; --x1)
                {
                    if (image[x1][y] && !image[x1 - 1][y])
                    {
                        if (!flag1)
                            flag1 = true;
                        else
                            break;
                    }
                }
                bool flag2 = false;
                int x2;
                for (x2 = centers[index].X; x2 < image.Length; ++x2)
                {
                    if (image[x2][y] && !image[x2 + 1][y])
                    {
                        if (!flag2)
                            flag2 = true;
                        else
                            break;
                    }
                }
                numArray[index] = x2 - x1 + 1;
            }
            return numArray;
        }

        internal static int calcRoughVersion(Point[] center, int[] width)
        {
            int decimalPoint = QRCodeImageReader.DECIMAL_POINT;
            int num1 = new Line(center[0], center[1]).Length << decimalPoint;
            int num2 = (width[0] + width[1] << decimalPoint) / 14;
            int num3 = (num1 / num2 - 10) / 4;
            if ((num1 / num2 - 10) % 4 >= 2)
                ++num3;
            return num3;
        }

        internal static int calcExactVersion(Point[] centers, int[] angle, int[] moduleSize, bool[][] image)
        {
            bool[] target = new bool[18];
            Point[] points = new Point[18];
            Axis axis = new Axis(angle, moduleSize[1]);
            axis.Origin = centers[1];
            for (int index1 = 0; index1 < 6; ++index1)
            {
                for (int index2 = 0; index2 < 3; ++index2)
                {
                    Point point = axis.translate(index2 - 7, index1 - 3);
                    target[index2 + index1 * 3] = image[point.X][point.Y];
                    points[index2 + index1 * 3] = point;
                }
            }
            FinderPattern.canvas.drawPoints(points, Color_Fields.RED);
            int num;
            try
            {
                num = FinderPattern.checkVersionInfo(target);
            }
            catch (InvalidVersionInfoException)
            {
                FinderPattern.canvas.println("Version info error. now retry with other place one.");
                axis.Origin = centers[2];
                axis.ModulePitch = moduleSize[2];
                for (int index1 = 0; index1 < 6; ++index1)
                {
                    for (int index2 = 0; index2 < 3; ++index2)
                    {
                        Point point = axis.translate(index1 - 3, index2 - 7);
                        target[index2 + index1 * 3] = image[point.X][point.Y];
                        points[index1 + index2 * 3] = point;
                    }
                }
                FinderPattern.canvas.drawPoints(points, Color_Fields.RED);
                try
                {
                    num = FinderPattern.checkVersionInfo(target);
                }
                catch (VersionInformationException ex2)
                {
                    throw ex2;
                }
            }
            return num;
        }

        internal static int checkVersionInfo(bool[] target)
        {
            int num = 0;
            int index1;
            for (index1 = 0; index1 < FinderPattern.VersionInfoBit.Length; ++index1)
            {
                num = 0;
                for (int index2 = 0; index2 < 18; ++index2)
                {
                    if (target[index2] ^ (FinderPattern.VersionInfoBit[index1] >> index2) % 2 == 1)
                        ++num;
                }
                if (num <= 3)
                    break;
            }
            if (num <= 3)
                return 7 + index1;
            throw new InvalidVersionInfoException("Too many errors in version information");
        }
    }



    public class AlignmentPattern
    {
        internal static DebugCanvas canvas = QRCodeDecoder.Canvas;
        internal const int RIGHT = 1;
        internal const int BOTTOM = 2;
        internal const int LEFT = 3;
        internal const int TOP = 4;
        internal Point[][] center;
        internal int patternDistance;

        public virtual int LogicalDistance
        {
            get
            {
                return this.patternDistance;
            }
        }

        internal AlignmentPattern(Point[][] center, int patternDistance)
        {
            this.center = center;
            this.patternDistance = patternDistance;
        }

        public static AlignmentPattern findAlignmentPattern(bool[][] image, FinderPattern finderPattern)
        {
            Point[][] logicalCenter = AlignmentPattern.getLogicalCenter(finderPattern);
            int patternDistance = logicalCenter[1][0].X - logicalCenter[0][0].X;
            return new AlignmentPattern(AlignmentPattern.getCenter(image, finderPattern, logicalCenter), patternDistance);
        }

        public virtual Point[][] getCenter()
        {
            return this.center;
        }

        public virtual void setCenter(Point[][] center)
        {
            this.center = center;
        }

        internal static Point[][] getCenter(bool[][] image, FinderPattern finderPattern, Point[][] logicalCenters)
        {
            int moduleSize = finderPattern.getModuleSize();
            Axis axis = new Axis(finderPattern.getAngle(), moduleSize);
            int length = logicalCenters.Length;
            Point[][] pointArray = new Point[length][];
            for (int index = 0; index < length; ++index)
                pointArray[index] = new Point[length];
            axis.Origin = finderPattern.getCenter(0);
            pointArray[0][0] = axis.translate(3, 3);
            AlignmentPattern.canvas.drawCross(pointArray[0][0], Color_Fields.BLUE);
            axis.Origin = finderPattern.getCenter(1);
            pointArray[length - 1][0] = axis.translate(-3, 3);
            AlignmentPattern.canvas.drawCross(pointArray[length - 1][0], Color_Fields.BLUE);
            axis.Origin = finderPattern.getCenter(2);
            pointArray[0][length - 1] = axis.translate(3, -3);
            AlignmentPattern.canvas.drawCross(pointArray[0][length - 1], Color_Fields.BLUE);
            Point p1 = pointArray[0][0];
            for (int index1 = 0; index1 < length; ++index1)
            {
                for (int index2 = 0; index2 < length; ++index2)
                {
                    if ((index2 != 0 || index1 != 0) && (index2 != 0 || index1 != length - 1) && (index2 != length - 1 || index1 != 0))
                    {
                        Point point1 = (Point)null;
                        if (index1 == 0)
                        {
                            if (index2 > 0 && index2 < length - 1)
                                point1 = axis.translate(pointArray[index2 - 1][index1], logicalCenters[index2][index1].X - logicalCenters[index2 - 1][index1].X, 0);
                            pointArray[index2][index1] = new Point(point1.X, point1.Y);
                            AlignmentPattern.canvas.drawCross(pointArray[index2][index1], Color_Fields.RED);
                        }
                        else if (index2 == 0)
                        {
                            if (index1 > 0 && index1 < length - 1)
                                point1 = axis.translate(pointArray[index2][index1 - 1], 0, logicalCenters[index2][index1].Y - logicalCenters[index2][index1 - 1].Y);
                            pointArray[index2][index1] = new Point(point1.X, point1.Y);
                            AlignmentPattern.canvas.drawCross(pointArray[index2][index1], Color_Fields.RED);
                        }
                        else
                        {
                            Point point2 = axis.translate(pointArray[index2 - 1][index1], logicalCenters[index2][index1].X - logicalCenters[index2 - 1][index1].X, 0);
                            Point point3 = axis.translate(pointArray[index2][index1 - 1], 0, logicalCenters[index2][index1].Y - logicalCenters[index2][index1 - 1].Y);
                            pointArray[index2][index1] = new Point((point2.X + point3.X) / 2, (point2.Y + point3.Y) / 2 + 1);
                        }
                        if (finderPattern.Version > 1)
                        {
                            Point precisionCenter = AlignmentPattern.getPrecisionCenter(image, pointArray[index2][index1]);
                            if (pointArray[index2][index1].distanceOf(precisionCenter) < 6)
                            {
                                AlignmentPattern.canvas.drawCross(pointArray[index2][index1], Color_Fields.RED);
                                int num1 = precisionCenter.X - pointArray[index2][index1].X;
                                int num2 = precisionCenter.Y - pointArray[index2][index1].Y;
                                AlignmentPattern.canvas.println("Adjust AP(" + (object)index2 + "," + (object)index1 + ") to d(" + (object)num1 + "," + (object)num2 + ")");
                                pointArray[index2][index1] = precisionCenter;
                            }
                        }
                        AlignmentPattern.canvas.drawCross(pointArray[index2][index1], Color_Fields.BLUE);
                        AlignmentPattern.canvas.drawLine(new Line(p1, pointArray[index2][index1]), Color_Fields.LIGHTBLUE);
                        p1 = pointArray[index2][index1];
                    }
                }
            }
            return pointArray;
        }

        internal static Point getPrecisionCenter(bool[][] image, Point targetPoint)
        {
            int x1 = targetPoint.X;
            int y1 = targetPoint.Y;
            if (x1 < 0 || y1 < 0 || (x1 > image.Length - 1 || y1 > image[0].Length - 1))
                throw new AlignmentPatternNotFoundException("Alignment Pattern finder exceeded out of image");
            if (!image[targetPoint.X][targetPoint.Y])
            {
                int num = 0;
                bool flag = false;
                while (!flag)
                {
                    ++num;
                    for (int index1 = num; index1 > -num; --index1)
                    {
                        for (int index2 = num; index2 > -num; --index2)
                        {
                            int index3 = targetPoint.X + index2;
                            int index4 = targetPoint.Y + index1;
                            if (index3 < 0 || index4 < 0 || (index3 > image.Length - 1 || index4 > image[0].Length - 1))
                                throw new AlignmentPatternNotFoundException("Alignment Pattern finder exceeded out of image");
                            if (image[index3][index4])
                            {
                                targetPoint = new Point(targetPoint.X + index2, targetPoint.Y + index1);
                                flag = true;
                            }
                        }
                    }
                }
            }
            int x2;
            int x3 = x2 = targetPoint.X;
            int x4 = x2;
            int num1 = x2;
            int y2;
            int y3 = y2 = targetPoint.Y;
            int y4 = y2;
            int num2 = y2;
            while (x4 >= 1 && !AlignmentPattern.targetPointOnTheCorner(image, x4, num2, x4 - 1, num2))
                --x4;
            while (x3 < image.Length - 1 && !AlignmentPattern.targetPointOnTheCorner(image, x3, num2, x3 + 1, num2))
                ++x3;
            while (y4 >= 1 && !AlignmentPattern.targetPointOnTheCorner(image, num1, y4, num1, y4 - 1))
                --y4;
            while (y3 < image[0].Length - 1 && !AlignmentPattern.targetPointOnTheCorner(image, num1, y3, num1, y3 + 1))
                ++y3;
            return new Point((x4 + x3 + 1) / 2, (y4 + y3 + 1) / 2);
        }

        internal static bool targetPointOnTheCorner(bool[][] image, int x, int y, int nx, int ny)
        {
            if (x < 0 || y < 0 || (nx < 0 || ny < 0) || (x > image.Length || y > image[0].Length || nx > image.Length) || ny > image[0].Length)
                throw new AlignmentPatternNotFoundException("Alignment Pattern Finder exceeded image edge");
            return !image[x][y] && image[nx][ny];
        }

        public static Point[][] getLogicalCenter(FinderPattern finderPattern)
        {
            int version = finderPattern.Version;
            Point[][] pointArray1 = new Point[1][];
            for (int index = 0; index < 1; ++index)
                pointArray1[index] = new Point[1];
            int[] numArray = new int[1];
            int[] seed = LogicalSeed.getSeed(version);
            Point[][] pointArray2 = new Point[seed.Length][];
            for (int index = 0; index < seed.Length; ++index)
                pointArray2[index] = new Point[seed.Length];
            for (int index1 = 0; index1 < pointArray2.Length; ++index1)
            {
                for (int index2 = 0; index2 < pointArray2.Length; ++index2)
                    pointArray2[index2][index1] = new Point(seed[index2], seed[index1]);
            }
            return pointArray2;
        }
    }
}
