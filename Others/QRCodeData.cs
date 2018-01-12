using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using ThoughtWorks.QRCode.Codec.Util;

namespace ThoughtWorks.QRCode.Codec.Data
{

    public class QRCodeBitmapImage : QRCodeImage
    {
        private Bitmap image;

        public QRCodeBitmapImage(Bitmap image)
        {
            this.image = image;
        }

        public virtual int Width
        {
            get
            {
                return this.image.Width;
            }
        }

        public virtual int Height
        {
            get
            {
                return this.image.Height;
            }
        }

        public virtual int getPixel(int x, int y)
        {
            return this.image.GetPixel(x, y).ToArgb();
        }
    }

    public interface QRCodeImage
    {
        int Width { get; }

        int Height { get; }

        int getPixel(int x, int y);
    }

    public class DebugCanvasAdapter : DebugCanvas
    {
        public virtual void println(string string_Renamed)
        {
        }

        public virtual void drawPoint(Point point, int color)
        {
        }

        public virtual void drawCross(Point point, int color)
        {
        }

        public virtual void drawPoints(Point[] points, int color)
        {
        }

        public virtual void drawLine(Line line, int color)
        {
        }

        public virtual void drawLines(Line[] lines, int color)
        {
        }

        public virtual void drawPolygon(Point[] points, int color)
        {
        }

        public virtual void drawMatrix(bool[][] matrix)
        {
        }
    }
    public interface DebugCanvas
    {
        void println(string str);

        void drawPoint(Point point, int color);

        void drawCross(Point point, int color);

        void drawPoints(Point[] points, int color);

        void drawLine(Line line, int color);

        void drawLines(Line[] lines, int color);

        void drawPolygon(Point[] points, int color);

        void drawMatrix(bool[][] matrix);
    }

    public class Line
    {
        internal int x1;
        internal int y1;
        internal int x2;
        internal int y2;

        public virtual bool Horizontal
        {
            get
            {
                return this.y1 == this.y2;
            }
        }

        public virtual bool Vertical
        {
            get
            {
                return this.x1 == this.x2;
            }
        }

        public virtual Point Center
        {
            get
            {
                return new Point((this.x1 + this.x2) / 2, (this.y1 + this.y2) / 2);
            }
        }

        public virtual int Length
        {
            get
            {
                int num1 = Math.Abs(this.x2 - this.x1);
                int num2 = Math.Abs(this.y2 - this.y1);
                return QRCodeUtility.sqrt(num1 * num1 + num2 * num2);
            }
        }

        public Line()
        {
            this.x1 = this.y1 = this.x2 = this.y2 = 0;
        }

        public Line(int x1, int y1, int x2, int y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }

        public Line(Point p1, Point p2)
        {
            this.x1 = p1.X;
            this.y1 = p1.Y;
            this.x2 = p2.X;
            this.y2 = p2.Y;
        }

        public virtual Point getP1()
        {
            return new Point(this.x1, this.y1);
        }

        public virtual Point getP2()
        {
            return new Point(this.x2, this.y2);
        }

        public virtual void setLine(int x1, int y1, int x2, int y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }

        public virtual void setP1(Point p1)
        {
            this.x1 = p1.X;
            this.y1 = p1.Y;
        }

        public virtual void setP1(int x1, int y1)
        {
            this.x1 = x1;
            this.y1 = y1;
        }

        public virtual void setP2(Point p2)
        {
            this.x2 = p2.X;
            this.y2 = p2.Y;
        }

        public virtual void setP2(int x2, int y2)
        {
            this.x2 = x2;
            this.y2 = y2;
        }

        public virtual void translate(int dx, int dy)
        {
            this.x1 += dx;
            this.y1 += dy;
            this.x2 += dx;
            this.y2 += dy;
        }

        public static bool isNeighbor(Line line1, Line line2)
        {
            return Math.Abs(line1.getP1().X - line2.getP1().X) < 2 && Math.Abs(line1.getP1().Y - line2.getP1().Y) < 2 && (Math.Abs(line1.getP2().X - line2.getP2().X) < 2 && Math.Abs(line1.getP2().Y - line2.getP2().Y) < 2);
        }

        public static bool isCross(Line line1, Line line2)
        {
            if (line1.Horizontal && line2.Vertical)
            {
                if (line1.getP1().Y > line2.getP1().Y && line1.getP1().Y < line2.getP2().Y && line2.getP1().X > line1.getP1().X && line2.getP1().X < line1.getP2().X)
                    return true;
            }
            else if (line1.Vertical && line2.Horizontal && (line1.getP1().X > line2.getP1().X && line1.getP1().X < line2.getP2().X && line2.getP1().Y > line1.getP1().Y && line2.getP1().Y < line1.getP2().Y))
                return true;
            return false;
        }

        public static Line getLongest(Line[] lines)
        {
            Line line = new Line();
            for (int index = 0; index < lines.Length; ++index)
            {
                if (lines[index].Length > line.Length)
                    line = lines[index];
            }
            return line;
        }

        public override string ToString()
        {
            return "(" + Convert.ToString(this.x1) + "," + Convert.ToString(this.y1) + ")-(" + Convert.ToString(this.x2) + "," + Convert.ToString(this.y2) + ")";
        }
    }


    public class Point
    {
        public const int RIGHT = 1;
        public const int BOTTOM = 2;
        public const int LEFT = 4;
        public const int TOP = 8;
        internal int x;
        internal int y;

        public virtual int X
        {
            get
            {
                return this.x;
            }
            set
            {
                this.x = value;
            }
        }

        public virtual int Y
        {
            get
            {
                return this.y;
            }
            set
            {
                this.y = value;
            }
        }

        public Point()
        {
            this.x = 0;
            this.y = 0;
        }

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public virtual void translate(int dx, int dy)
        {
            this.x += dx;
            this.y += dy;
        }

        public virtual void set_Renamed(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return "(" + Convert.ToString(this.x) + "," + Convert.ToString(this.y) + ")";
        }

        public static Point getCenter(Point p1, Point p2)
        {
            return new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
        }

        public bool equals(Point compare)
        {
            return this.x == compare.x && this.y == compare.y;
        }

        public virtual int distanceOf(Point other)
        {
            int vx = other.X;
            int vy = other.Y;
            return QRCodeUtility.sqrt((this.x - vx) * (this.x - vx) + (this.y - vy) * (this.y - vy));
        }
    }

    [Serializable]
    public class DecodingFailedException : ArgumentException
    {
        internal string message = null;

        public override string Message
        {
            get
            {
                return this.message;
            }
        }

        public DecodingFailedException(string message)
        {
            this.message = message;
        }
    }


    [Serializable]
    public class SymbolNotFoundException : ArgumentException
    {
        internal string message = null;

        public override string Message
        {
            get
            {
                return this.message;
            }
        }

        public SymbolNotFoundException(string message)
        {
            this.message = message;
        }
    }

    [Serializable]
    public class InvalidDataBlockException : ArgumentException
    {
        internal string message = (string)null;

        public override string Message
        {
            get
            {
                return this.message;
            }
        }

        public InvalidDataBlockException(string message)
        {
            this.message = message;
        }
    }
    [Serializable]
    public class VersionInformationException : ArgumentException
    {
    }
    [Serializable]
    public class FinderPatternNotFoundException : Exception
    {
        internal string message = (string)null;

        public override string Message
        {
            get
            {
                return this.message;
            }
        }

        public FinderPatternNotFoundException(string message)
        {
            this.message = message;
        }
    }
    [Serializable]
    public class AlignmentPatternNotFoundException : ArgumentException
    {
        internal string message = (string)null;

        public override string Message
        {
            get
            {
                return this.message;
            }
        }

        public AlignmentPatternNotFoundException(string message)
        {
            this.message = message;
        }
    }
    [Serializable]
    public class InvalidVersionException : VersionInformationException
    {
        internal string message;

        public override string Message
        {
            get
            {
                return this.message;
            }
        }

        public InvalidVersionException(string message)
        {
            this.message = message;
        }
    }
    [Serializable]
    public class InvalidVersionInfoException : VersionInformationException
    {
        internal string message = (string)null;

        public override string Message
        {
            get
            {
                return this.message;
            }
        }

        public InvalidVersionInfoException(string message)
        {
            this.message = message;
        }
    }
    public class QRCodeSymbol
    {
        internal int[][] numErrorCollectionCode = new int[40][]
        {
      new int[4]{ 7, 10, 13, 17 },
      new int[4]{ 10, 16, 22, 28 },
      new int[4]{ 15, 26, 36, 44 },
      new int[4]{ 20, 36, 52, 64 },
      new int[4]{ 26, 48, 72, 88 },
      new int[4]{ 36, 64, 96, 112 },
      new int[4]{ 40, 72, 108, 130 },
      new int[4]{ 48, 88, 132, 156 },
      new int[4]{ 60, 110, 160, 192 },
      new int[4]{ 72, 130, 192, 224 },
      new int[4]{ 80, 150, 224, 264 },
      new int[4]{ 96, 176, 260, 308 },
      new int[4]{ 104, 198, 288, 352 },
      new int[4]{ 120, 216, 320, 384 },
      new int[4]{ 132, 240, 360, 432 },
      new int[4]{ 144, 280, 408, 480 },
      new int[4]{ 168, 308, 448, 532 },
      new int[4]{ 180, 338, 504, 588 },
      new int[4]{ 196, 364, 546, 650 },
      new int[4]{ 224, 416, 600, 700 },
      new int[4]{ 224, 442, 644, 750 },
      new int[4]{ 252, 476, 690, 816 },
      new int[4]{ 270, 504, 750, 900 },
      new int[4]{ 300, 560, 810, 960 },
      new int[4]{ 312, 588, 870, 1050 },
      new int[4]{ 336, 644, 952, 1110 },
      new int[4]{ 360, 700, 1020, 1200 },
      new int[4]{ 390, 728, 1050, 1260 },
      new int[4]{ 420, 784, 1140, 1350 },
      new int[4]{ 450, 812, 1200, 1440 },
      new int[4]{ 480, 868, 1290, 1530 },
      new int[4]{ 510, 924, 1350, 1620 },
      new int[4]{ 540, 980, 1440, 1710 },
      new int[4]{ 570, 1036, 1530, 1800 },
      new int[4]{ 570, 1064, 1590, 1890 },
      new int[4]{ 600, 1120, 1680, 1980 },
      new int[4]{ 630, 1204, 1770, 2100 },
      new int[4]{ 660, 1260, 1860, 2220 },
      new int[4]{ 720, 1316, 1950, 2310 },
      new int[4]{ 750, 1372, 2040, 2430 }
        };
        internal int[][] numRSBlocks = new int[40][]
        {
      new int[4]{ 1, 1, 1, 1 },
      new int[4]{ 1, 1, 1, 1 },
      new int[4]{ 1, 1, 2, 2 },
      new int[4]{ 1, 2, 2, 4 },
      new int[4]{ 1, 2, 4, 4 },
      new int[4]{ 2, 4, 4, 4 },
      new int[4]{ 2, 4, 6, 5 },
      new int[4]{ 2, 4, 6, 6 },
      new int[4]{ 2, 5, 8, 8 },
      new int[4]{ 4, 5, 8, 8 },
      new int[4]{ 4, 5, 8, 11 },
      new int[4]{ 4, 8, 10, 11 },
      new int[4]{ 4, 9, 12, 16 },
      new int[4]{ 4, 9, 16, 16 },
      new int[4]{ 6, 10, 12, 18 },
      new int[4]{ 6, 10, 17, 16 },
      new int[4]{ 6, 11, 16, 19 },
      new int[4]{ 6, 13, 18, 21 },
      new int[4]{ 7, 14, 21, 25 },
      new int[4]{ 8, 16, 20, 25 },
      new int[4]{ 8, 17, 23, 25 },
      new int[4]{ 9, 17, 23, 34 },
      new int[4]{ 9, 18, 25, 30 },
      new int[4]{ 10, 20, 27, 32 },
      new int[4]{ 12, 21, 29, 35 },
      new int[4]{ 12, 23, 34, 37 },
      new int[4]{ 12, 25, 34, 40 },
      new int[4]{ 13, 26, 35, 42 },
      new int[4]{ 14, 28, 38, 45 },
      new int[4]{ 15, 29, 40, 48 },
      new int[4]{ 16, 31, 43, 51 },
      new int[4]{ 17, 33, 45, 54 },
      new int[4]{ 18, 35, 48, 57 },
      new int[4]{ 19, 37, 51, 60 },
      new int[4]{ 19, 38, 53, 63 },
      new int[4]{ 20, 40, 56, 66 },
      new int[4]{ 21, 43, 59, 70 },
      new int[4]{ 22, 45, 62, 74 },
      new int[4]{ 24, 47, 65, 77 },
      new int[4]{ 25, 49, 68, 81 }
        };
        internal int version;
        internal int errorCollectionLevel;
        internal int maskPattern;
        internal int dataCapacity;
        internal bool[][] moduleMatrix;
        internal int width;
        internal int height;
        internal Point[][] alignmentPattern;

        public virtual int NumErrorCollectionCode
        {
            get
            {
                return this.numErrorCollectionCode[this.version - 1][this.errorCollectionLevel];
            }
        }

        public virtual int NumRSBlocks
        {
            get
            {
                return this.numRSBlocks[this.version - 1][this.errorCollectionLevel];
            }
        }

        public virtual int Version
        {
            get
            {
                return this.version;
            }
        }

        public virtual string VersionReference
        {
            get
            {
                return Convert.ToString(this.version) + "-" + (object)new char[4]
                {
          'L',
          'M',
          'Q',
          'H'
                }[this.errorCollectionLevel];
            }
        }

        public virtual Point[][] AlignmentPattern
        {
            get
            {
                return this.alignmentPattern;
            }
        }

        public virtual int DataCapacity
        {
            get
            {
                return this.dataCapacity;
            }
        }

        public virtual int ErrorCollectionLevel
        {
            get
            {
                return this.errorCollectionLevel;
            }
        }

        public virtual int MaskPatternReferer
        {
            get
            {
                return this.maskPattern;
            }
        }

        public virtual string MaskPatternRefererAsString
        {
            get
            {
                string str = Convert.ToString(this.MaskPatternReferer, 2);
                int length = str.Length;
                for (int index = 0; index < 3 - length; ++index)
                    str = "0" + str;
                return str;
            }
        }

        public virtual int Width
        {
            get
            {
                return this.width;
            }
        }

        public virtual int Height
        {
            get
            {
                return this.height;
            }
        }

        public virtual int[] Blocks
        {
            get
            {
                int width = this.Width;
                int height = this.Height;
                int num1 = width - 1;
                int num2 = height - 1;
                ArrayList arrayList1 = ArrayList.Synchronized(new ArrayList(10));
                ArrayList arrayList2 = ArrayList.Synchronized(new ArrayList(10));
                int num3 = 0;
                int num4 = 7;
                int num5 = 0;
                bool flag1 = true;
                bool flag2 = false;
                bool flag3 = flag1;
                do
                {
                    arrayList1.Add((object)this.getElement(num1, num2));
                    if (this.getElement(num1, num2))
                        num3 += 1 << num4;
                    --num4;
                    if (num4 == -1)
                    {
                        arrayList2.Add((object)num3);
                        num4 = 7;
                        num3 = 0;
                    }
                    do
                    {
                        if (flag3 == flag1)
                        {
                            if ((num1 + num5) % 2 == 0)
                                --num1;
                            else if (num2 > 0)
                            {
                                ++num1;
                                --num2;
                            }
                            else
                            {
                                --num1;
                                if (num1 == 6)
                                {
                                    --num1;
                                    num5 = 1;
                                }
                                flag3 = flag2;
                            }
                        }
                        else if ((num1 + num5) % 2 == 0)
                            --num1;
                        else if (num2 < height - 1)
                        {
                            ++num1;
                            ++num2;
                        }
                        else
                        {
                            --num1;
                            if (num1 == 6)
                            {
                                --num1;
                                num5 = 1;
                            }
                            flag3 = flag1;
                        }
                    }
                    while (this.isInFunctionPattern(num1, num2));
                }
                while (num1 != -1);
                int[] numArray = new int[arrayList2.Count];
                for (int index = 0; index < arrayList2.Count; ++index)
                {
                    int num6 = (int)arrayList2[index];
                    numArray[index] = num6;
                }
                return numArray;
            }
        }

        public virtual bool getElement(int x, int y)
        {
            return this.moduleMatrix[x][y];
        }

        public QRCodeSymbol(bool[][] moduleMatrix)
        {
            this.moduleMatrix = moduleMatrix;
            this.width = moduleMatrix.Length;
            this.height = moduleMatrix[0].Length;
            this.initialize();
        }

        internal virtual void initialize()
        {
            this.version = (this.width - 17) / 4;
            Point[][] pointArray1 = new Point[1][];
            for (int index = 0; index < 1; ++index)
                pointArray1[index] = new Point[1];
            int[] numArray = new int[1];
            if (this.version >= 2 && this.version <= 40)
            {
                numArray = LogicalSeed.getSeed(this.version);
                Point[][] pointArray2 = new Point[numArray.Length][];
                for (int index = 0; index < numArray.Length; ++index)
                    pointArray2[index] = new Point[numArray.Length];
                pointArray1 = pointArray2;
            }
            for (int index1 = 0; index1 < numArray.Length; ++index1)
            {
                for (int index2 = 0; index2 < numArray.Length; ++index2)
                    pointArray1[index2][index1] = new Point(numArray[index2], numArray[index1]);
            }
            this.alignmentPattern = pointArray1;
            this.dataCapacity = this.calcDataCapacity();
            this.decodeFormatInformation(this.readFormatInformation());
            this.unmask();
        }

        internal virtual bool[] readFormatInformation()
        {
            bool[] source = new bool[15];
            for (int y = 0; y <= 5; ++y)
                source[y] = this.getElement(8, y);
            source[6] = this.getElement(8, 7);
            source[7] = this.getElement(8, 8);
            source[8] = this.getElement(7, 8);
            for (int index = 9; index <= 14; ++index)
                source[index] = this.getElement(14 - index, 8);
            int number = 21522;
            for (int bits = 0; bits <= 14; ++bits)
            {
                bool flag = (SystemUtils.URShift(number, bits) & 1) == 1;
                source[bits] = source[bits] != flag;
            }
            bool[] flagArray1 = new BCH15_5(source).correct();
            bool[] flagArray2 = new bool[5];
            for (int index = 0; index < 5; ++index)
                flagArray2[index] = flagArray1[10 + index];
            return flagArray2;
        }

        internal virtual void unmask()
        {
            bool[][] maskPattern = this.generateMaskPattern();
            int width = this.Width;
            for (int y = 0; y < width; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    if (maskPattern[x][y])
                        this.reverseElement(x, y);
                }
            }
        }

        internal virtual bool[][] generateMaskPattern()
        {
            int maskPatternReferer = this.MaskPatternReferer;
            int width = this.Width;
            int height = this.Height;
            bool[][] flagArray = new bool[width][];
            for (int index = 0; index < width; ++index)
                flagArray[index] = new bool[height];
            for (int targetY = 0; targetY < height; ++targetY)
            {
                for (int targetX = 0; targetX < width; ++targetX)
                {
                    if (!this.isInFunctionPattern(targetX, targetY))
                    {
                        switch (maskPatternReferer)
                        {
                            case 0:
                                if ((targetY + targetX) % 2 == 0)
                                {
                                    flagArray[targetX][targetY] = true;
                                    break;
                                }
                                break;
                            case 1:
                                if (targetY % 2 == 0)
                                {
                                    flagArray[targetX][targetY] = true;
                                    break;
                                }
                                break;
                            case 2:
                                if (targetX % 3 == 0)
                                {
                                    flagArray[targetX][targetY] = true;
                                    break;
                                }
                                break;
                            case 3:
                                if ((targetY + targetX) % 3 == 0)
                                {
                                    flagArray[targetX][targetY] = true;
                                    break;
                                }
                                break;
                            case 4:
                                if ((targetY / 2 + targetX / 3) % 2 == 0)
                                {
                                    flagArray[targetX][targetY] = true;
                                    break;
                                }
                                break;
                            case 5:
                                if (targetY * targetX % 2 + targetY * targetX % 3 == 0)
                                {
                                    flagArray[targetX][targetY] = true;
                                    break;
                                }
                                break;
                            case 6:
                                if ((targetY * targetX % 2 + targetY * targetX % 3) % 2 == 0)
                                {
                                    flagArray[targetX][targetY] = true;
                                    break;
                                }
                                break;
                            case 7:
                                if ((targetY * targetX % 3 + (targetY + targetX) % 2) % 2 == 0)
                                {
                                    flagArray[targetX][targetY] = true;
                                    break;
                                }
                                break;
                        }
                    }
                }
            }
            return flagArray;
        }

        private int calcDataCapacity()
        {
            int version = this.Version;
            int num1 = version > 6 ? 67 : 31;
            int num2 = version / 7 + 2;
            return (this.width * this.width - ((version == 1 ? 192 : 192 + (num2 * num2 - 3) * 25) + 8 * version + 2 - (num2 - 2) * 10) - num1) / 8;
        }

        internal virtual void decodeFormatInformation(bool[] formatInformation)
        {
            this.errorCollectionLevel = formatInformation[4] ? (!formatInformation[3] ? 3 : 2) : (!formatInformation[3] ? 1 : 0);
            for (int index = 2; index >= 0; --index)
            {
                if (formatInformation[index])
                    this.maskPattern += 1 << index;
            }
        }

        public virtual void reverseElement(int x, int y)
        {
            this.moduleMatrix[x][y] = !this.moduleMatrix[x][y];
        }

        public virtual bool isInFunctionPattern(int targetX, int targetY)
        {
            if (targetX < 9 && targetY < 9 || targetX > this.Width - 9 && targetY < 9 || targetX < 9 && targetY > this.Height - 9 || this.version >= 7 && (targetX > this.Width - 12 && targetY < 6 || targetX < 6 && targetY > this.Height - 12) || (targetX == 6 || targetY == 6))
                return true;
            Point[][] alignmentPattern = this.AlignmentPattern;
            int length = alignmentPattern.Length;
            for (int index1 = 0; index1 < length; ++index1)
            {
                for (int index2 = 0; index2 < length; ++index2)
                {
                    if ((index2 != 0 || index1 != 0) && (index2 != length - 1 || index1 != 0) && (index2 != 0 || index1 != length - 1) && (Math.Abs(alignmentPattern[index2][index1].X - targetX) < 3 && Math.Abs(alignmentPattern[index2][index1].Y - targetY) < 3))
                        return true;
                }
            }
            return false;
        }
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct Color_Fields
    {
        public static readonly int GRAY = 11184810;
        public static readonly int LIGHTGRAY = 12303291;
        public static readonly int DARKGRAY = 4473924;
        public static readonly int BLACK = 0;
        public static readonly int WHITE = 16777215;
        public static readonly int BLUE = 8947967;
        public static readonly int GREEN = 8978312;
        public static readonly int LIGHTBLUE = 12303359;
        public static readonly int LIGHTGREEN = 12320699;
        public static readonly int RED = 267946120;
        public static readonly int ORANGE = 16777096;
        public static readonly int LIGHTRED = 16759739;
    }


    public class Axis
    {
        internal int sin;
        internal int cos;
        internal int modulePitch;
        internal Point origin;

        public virtual Point Origin
        {
            set
            {
                this.origin = value;
            }
        }

        public virtual int ModulePitch
        {
            set
            {
                this.modulePitch = value;
            }
        }

        public Axis(int[] angle, int modulePitch)
        {
            this.sin = angle[0];
            this.cos = angle[1];
            this.modulePitch = modulePitch;
            this.origin = new Point();
        }

        public virtual Point translate(Point offset)
        {
            return this.translate(offset.X, offset.Y);
        }

        public virtual Point translate(Point origin, Point offset)
        {
            this.Origin = origin;
            return this.translate(offset.X, offset.Y);
        }

        public virtual Point translate(Point origin, int moveX, int moveY)
        {
            this.Origin = origin;
            return this.translate(moveX, moveY);
        }

        public virtual Point translate(Point origin, int modulePitch, int moveX, int moveY)
        {
            this.Origin = origin;
            this.modulePitch = modulePitch;
            return this.translate(moveX, moveY);
        }

        public virtual Point translate(int moveX, int moveY)
        {
            long decimalPoint = (long)QRCodeImageReader.DECIMAL_POINT;
            Point point = new Point();
            int num1 = moveX == 0 ? 0 : this.modulePitch * moveX >> (int)decimalPoint;
            int num2 = moveY == 0 ? 0 : this.modulePitch * moveY >> (int)decimalPoint;
            point.translate(num1 * this.cos - num2 * this.sin >> (int)decimalPoint, num1 * this.sin + num2 * this.cos >> (int)decimalPoint);
            point.translate(this.origin.X, this.origin.Y);
            return point;
        }
    }
    
}
