using System;
using System.Reflection;
using System.Text;
using System.Drawing;
using System.IO;
using ThoughtWorks.QRCode.Codec.Data;
using ThoughtWorks.QRCode.Codec.Util;
using QRCodeUtility = ThoughtWorks.QRCode.Codec.Util.QRCodeUtility;

namespace ThoughtWorks.QRCode.Codec
{

    public class QRCodeEncoder
    {
        internal QRCodeEncoder.ERROR_CORRECTION qrcodeErrorCorrect;
        internal QRCodeEncoder.ENCODE_MODE qrcodeEncodeMode;
        internal int qrcodeVersion;
        internal int qrcodeStructureappendN;
        internal int qrcodeStructureappendM;
        internal int qrcodeStructureappendParity;
        internal System.Drawing.Color qrCodeBackgroundColor;
        internal System.Drawing.Color qrCodeForegroundColor;
        internal int qrCodeScale;
        internal string qrcodeStructureappendOriginaldata;

        public QRCodeEncoder()
        {
            this.qrcodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            this.qrcodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            this.qrcodeVersion = 7;
            this.qrcodeStructureappendN = 0;
            this.qrcodeStructureappendM = 0;
            this.qrcodeStructureappendParity = 0;
            this.qrcodeStructureappendOriginaldata = "";
            this.qrCodeScale = 4;
            this.qrCodeBackgroundColor = System.Drawing.Color.White;
            this.qrCodeForegroundColor = System.Drawing.Color.Black;
        }

        public virtual QRCodeEncoder.ERROR_CORRECTION QRCodeErrorCorrect
        {
            get
            {
                return this.qrcodeErrorCorrect;
            }
            set
            {
                this.qrcodeErrorCorrect = value;
            }
        }

        public virtual int QRCodeVersion
        {
            get
            {
                return this.qrcodeVersion;
            }
            set
            {
                if (value < 0 || value > 40)
                    return;
                this.qrcodeVersion = value;
            }
        }

        public virtual QRCodeEncoder.ENCODE_MODE QRCodeEncodeMode
        {
            get
            {
                return this.qrcodeEncodeMode;
            }
            set
            {
                this.qrcodeEncodeMode = value;
            }
        }

        public virtual int QRCodeScale
        {
            get
            {
                return this.qrCodeScale;
            }
            set
            {
                this.qrCodeScale = value;
            }
        }

        public virtual System.Drawing.Color QRCodeBackgroundColor
        {
            get
            {
                return this.qrCodeBackgroundColor;
            }
            set
            {
                this.qrCodeBackgroundColor = value;
            }
        }

        public virtual System.Drawing.Color QRCodeForegroundColor
        {
            get
            {
                return this.qrCodeForegroundColor;
            }
            set
            {
                this.qrCodeForegroundColor = value;
            }
        }

        public virtual void setStructureappend(int m, int n, int p)
        {
            if (n <= 1 || n > 16 || (m <= 0 || m > 16) || p < 0 || p > (int)byte.MaxValue)
                return;
            this.qrcodeStructureappendM = m;
            this.qrcodeStructureappendN = n;
            this.qrcodeStructureappendParity = p;
        }

        public virtual int calStructureappendParity(sbyte[] originaldata)
        {
            int index = 0;
            int length = originaldata.Length;
            int num;
            if (length > 1)
            {
                num = 0;
                for (; index < length; ++index)
                    num ^= (int)originaldata[index] & (int)byte.MaxValue;
            }
            else
                num = -1;
            return num;
        }

        public virtual bool[][] calQrcode(byte[] qrcodeData)
        {
            int index1 = 0;
            int length1 = qrcodeData.Length;
            int[] data = new int[length1 + 32];
            sbyte[] bits = new sbyte[length1 + 32];
            if (length1 <= 0)
                return new bool[1][] { new bool[1] };
            if (this.qrcodeStructureappendN > 1)
            {
                data[0] = 3;
                bits[0] = (sbyte)4;
                data[1] = this.qrcodeStructureappendM - 1;
                bits[1] = (sbyte)4;
                data[2] = this.qrcodeStructureappendN - 1;
                bits[2] = (sbyte)4;
                data[3] = this.qrcodeStructureappendParity;
                bits[3] = (sbyte)8;
                index1 = 4;
            }
            bits[index1] = (sbyte)4;
            int[] numArray1;
            int index2;
            int index3;
            switch (this.qrcodeEncodeMode)
            {
                case QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC:
                    numArray1 = new int[41]
                    {
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            2,
            2,
            2,
            2,
            2,
            2,
            2,
            2,
            2,
            2,
            2,
            2,
            2,
            2,
            2,
            2,
            2,
            4,
            4,
            4,
            4,
            4,
            4,
            4,
            4,
            4,
            4,
            4,
            4,
            4,
            4
                    };
                    data[index1] = 2;
                    int index4 = index1 + 1;
                    data[index4] = length1;
                    bits[index4] = (sbyte)9;
                    index2 = index4;
                    int index5 = index4 + 1;
                    for (int index6 = 0; index6 < length1; ++index6)
                    {
                        char ch = (char)qrcodeData[index6];
                        sbyte num = 0;
                        if ((int)ch >= 48 && (int)ch < 58)
                            num = (sbyte)((int)ch - 48);
                        else if ((int)ch >= 65 && (int)ch < 91)
                        {
                            num = (sbyte)((int)ch - 55);
                        }
                        else
                        {
                            if ((int)ch == 32)
                                num = (sbyte)36;
                            if ((int)ch == 36)
                                num = (sbyte)37;
                            if ((int)ch == 37)
                                num = (sbyte)38;
                            if ((int)ch == 42)
                                num = (sbyte)39;
                            if ((int)ch == 43)
                                num = (sbyte)40;
                            if ((int)ch == 45)
                                num = (sbyte)41;
                            if ((int)ch == 46)
                                num = (sbyte)42;
                            if ((int)ch == 47)
                                num = (sbyte)43;
                            if ((int)ch == 58)
                                num = (sbyte)44;
                        }
                        if (index6 % 2 == 0)
                        {
                            data[index5] = (int)num;
                            bits[index5] = (sbyte)6;
                        }
                        else
                        {
                            data[index5] = data[index5] * 45 + (int)num;
                            bits[index5] = (sbyte)11;
                            if (index6 < length1 - 1)
                                ++index5;
                        }
                    }
                    index3 = index5 + 1;
                    break;
                case QRCodeEncoder.ENCODE_MODE.NUMERIC:
                    numArray1 = new int[41]
                    {
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            2,
            2,
            2,
            2,
            2,
            2,
            2,
            2,
            2,
            2,
            2,
            2,
            2,
            2,
            2,
            2,
            2,
            4,
            4,
            4,
            4,
            4,
            4,
            4,
            4,
            4,
            4,
            4,
            4,
            4,
            4
                    };
                    data[index1] = 1;
                    int index7 = index1 + 1;
                    data[index7] = length1;
                    bits[index7] = (sbyte)10;
                    index2 = index7;
                    int index8 = index7 + 1;
                    for (int index6 = 0; index6 < length1; ++index6)
                    {
                        if (index6 % 3 == 0)
                        {
                            data[index8] = (int)qrcodeData[index6] - 48;
                            bits[index8] = (sbyte)4;
                        }
                        else
                        {
                            data[index8] = data[index8] * 10 + ((int)qrcodeData[index6] - 48);
                            if (index6 % 3 == 1)
                            {
                                bits[index8] = (sbyte)7;
                            }
                            else
                            {
                                bits[index8] = (sbyte)10;
                                if (index6 < length1 - 1)
                                    ++index8;
                            }
                        }
                    }
                    index3 = index8 + 1;
                    break;
                default:
                    numArray1 = new int[41]
                    {
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            8,
            8,
            8,
            8,
            8,
            8,
            8,
            8,
            8,
            8,
            8,
            8,
            8,
            8,
            8,
            8,
            8,
            8,
            8,
            8,
            8,
            8,
            8,
            8,
            8,
            8,
            8,
            8,
            8,
            8,
            8
                    };
                    data[index1] = 4;
                    int index9 = index1 + 1;
                    data[index9] = length1;
                    bits[index9] = (sbyte)8;
                    index2 = index9;
                    int num1 = index9 + 1;
                    for (int index6 = 0; index6 < length1; ++index6)
                    {
                        data[index6 + num1] = (int)qrcodeData[index6] & (int)byte.MaxValue;
                        bits[index6 + num1] = (sbyte)8;
                    }
                    index3 = num1 + length1;
                    break;
            }
            int num2 = 0;
            for (int index6 = 0; index6 < index3; ++index6)
                num2 += (int)bits[index6];
            int index10;
            switch (this.qrcodeErrorCorrect)
            {
                case QRCodeEncoder.ERROR_CORRECTION.L:
                    index10 = 1;
                    break;
                case QRCodeEncoder.ERROR_CORRECTION.Q:
                    index10 = 3;
                    break;
                case QRCodeEncoder.ERROR_CORRECTION.H:
                    index10 = 2;
                    break;
                default:
                    index10 = 0;
                    break;
            }
            int[][] numArray2 = new int[4][]
            {
        new int[41]
        {
          0,
          128,
          224,
          352,
          512,
          688,
          864,
          992,
          1232,
          1456,
          1728,
          2032,
          2320,
          2672,
          2920,
          3320,
          3624,
          4056,
          4504,
          5016,
          5352,
          5712,
          6256,
          6880,
          7312,
          8000,
          8496,
          9024,
          9544,
          10136,
          10984,
          11640,
          12328,
          13048,
          13800,
          14496,
          15312,
          15936,
          16816,
          17728,
          18672
        },
        new int[41]
        {
          0,
          152,
          272,
          440,
          640,
          864,
          1088,
          1248,
          1552,
          1856,
          2192,
          2592,
          2960,
          3424,
          3688,
          4184,
          4712,
          5176,
          5768,
          6360,
          6888,
          7456,
          8048,
          8752,
          9392,
          10208,
          10960,
          11744,
          12248,
          13048,
          13880,
          14744,
          15640,
          16568,
          17528,
          18448,
          19472,
          20528,
          21616,
          22496,
          23648
        },
        new int[41]
        {
          0,
          72,
          128,
          208,
          288,
          368,
          480,
          528,
          688,
          800,
          976,
          1120,
          1264,
          1440,
          1576,
          1784,
          2024,
          2264,
          2504,
          2728,
          3080,
          3248,
          3536,
          3712,
          4112,
          4304,
          4768,
          5024,
          5288,
          5608,
          5960,
          6344,
          6760,
          7208,
          7688,
          7888,
          8432,
          8768,
          9136,
          9776,
          10208
        },
        new int[41]
        {
          0,
          104,
          176,
          272,
          384,
          496,
          608,
          704,
          880,
          1056,
          1232,
          1440,
          1648,
          1952,
          2088,
          2360,
          2600,
          2936,
          3176,
          3560,
          3880,
          4096,
          4544,
          4912,
          5312,
          5744,
          6032,
          6464,
          6968,
          7288,
          7880,
          8264,
          8920,
          9368,
          9848,
          10288,
          10832,
          11408,
          12016,
          12656,
          13328
        }
            };
            int num3 = 0;
            if (this.qrcodeVersion == 0)
            {
                this.qrcodeVersion = 1;
                for (int index6 = 1; index6 <= 40; ++index6)
                {
                    if (numArray2[index10][index6] >= num2 + numArray1[this.qrcodeVersion])
                    {
                        num3 = numArray2[index10][index6];
                        break;
                    }
                    ++this.qrcodeVersion;
                }
            }
            else
                num3 = numArray2[index10][this.qrcodeVersion];
            int num4 = num2 + numArray1[this.qrcodeVersion];
            bits[index2] = (sbyte)((int)bits[index2] + numArray1[this.qrcodeVersion]);
            int maxCodewords = new int[41]
            {
        0,
        26,
        44,
        70,
        100,
        134,
        172,
        196,
        242,
        292,
        346,
        404,
        466,
        532,
        581,
        655,
        733,
        815,
        901,
        991,
        1085,
        1156,
        1258,
        1364,
        1474,
        1588,
        1706,
        1828,
        1921,
        2051,
        2185,
        2323,
        2465,
        2611,
        2761,
        2876,
        3034,
        3196,
        3362,
        3532,
        3706
            }[this.qrcodeVersion];
            int num5 = 17 + (this.qrcodeVersion << 2);
            int[] numArray3 = new int[41]
            {
        0,
        0,
        7,
        7,
        7,
        7,
        7,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        3,
        3,
        3,
        3,
        3,
        3,
        3,
        4,
        4,
        4,
        4,
        4,
        4,
        4,
        3,
        3,
        3,
        3,
        3,
        3,
        3,
        0,
        0,
        0,
        0,
        0,
        0
            };
            int length2 = numArray3[this.qrcodeVersion] + (maxCodewords << 3);
            sbyte[] target1 = new sbyte[length2];
            sbyte[] target2 = new sbyte[length2];
            sbyte[] target3 = new sbyte[length2];
            sbyte[] target4 = new sbyte[15];
            sbyte[] target5 = new sbyte[15];
            sbyte[] target6 = new sbyte[1];
            sbyte[] target7 = new sbyte[128];
            try
            {
                // ISSUE: reference to a compiler-generated method
                var resKey = "qrv" + Convert.ToString(qrcodeVersion) + "_" + Convert.ToString(index10);
                byte[] qrdata = ReadData(resKey);
                MemoryStream memoryStream = new MemoryStream(qrdata);
                BufferedStream bufferedStream = new BufferedStream(memoryStream);
                SystemUtils.ReadInput((Stream)bufferedStream, target1, 0, target1.Length);
                SystemUtils.ReadInput((Stream)bufferedStream, target2, 0, target2.Length);
                SystemUtils.ReadInput((Stream)bufferedStream, target3, 0, target3.Length);
                SystemUtils.ReadInput((Stream)bufferedStream, target4, 0, target4.Length);
                SystemUtils.ReadInput((Stream)bufferedStream, target5, 0, target5.Length);
                SystemUtils.ReadInput((Stream)bufferedStream, target6, 0, target6.Length);
                SystemUtils.ReadInput((Stream)bufferedStream, target7, 0, target7.Length);
                bufferedStream.Close();
                memoryStream.Close();
            }
            catch (Exception ex)
            {
                SystemUtils.WriteStackTrace(ex, Console.Error);
            }
            sbyte num6 = 1;
            for (sbyte index6 = 1; (int)index6 < 128; ++index6)
            {
                if ((int)target7[(int)index6] == 0)
                {
                    num6 = index6;
                    break;
                }
            }
            sbyte[] rsBlockOrder = new sbyte[(int)num6];
            Array.Copy((Array)target7, 0, (Array)rsBlockOrder, 0, (int)(byte)num6);
            sbyte[] numArray4 = new sbyte[15]
            {
        (sbyte) 0,
        (sbyte) 1,
        (sbyte) 2,
        (sbyte) 3,
        (sbyte) 4,
        (sbyte) 5,
        (sbyte) 7,
        (sbyte) 8,
        (sbyte) 8,
        (sbyte) 8,
        (sbyte) 8,
        (sbyte) 8,
        (sbyte) 8,
        (sbyte) 8,
        (sbyte) 8
            };
            sbyte[] numArray5 = new sbyte[15]
            {
        (sbyte) 8,
        (sbyte) 8,
        (sbyte) 8,
        (sbyte) 8,
        (sbyte) 8,
        (sbyte) 8,
        (sbyte) 8,
        (sbyte) 8,
        (sbyte) 7,
        (sbyte) 5,
        (sbyte) 4,
        (sbyte) 3,
        (sbyte) 2,
        (sbyte) 1,
        (sbyte) 0
            };
            int maxDataCodewords = num3 >> 3;
            int length3 = 4 * this.qrcodeVersion + 17;
            sbyte[] target8 = new sbyte[length3 * length3 + length3];
            try
            {
                // ISSUE: reference to a compiler-generated method
                byte[] qrdata = ReadData("qrvfr" + Convert.ToString(this.qrcodeVersion));
                MemoryStream memoryStream = new MemoryStream(qrdata);
                BufferedStream bufferedStream = new BufferedStream((Stream)memoryStream);
                SystemUtils.ReadInput((Stream)bufferedStream, target8, 0, target8.Length);
                bufferedStream.Close();
                memoryStream.Close();
            }
            catch (Exception ex)
            {
                SystemUtils.WriteStackTrace(ex, Console.Error);
            }
            if (num4 <= num3 - 4)
            {
                data[index3] = 0;
                bits[index3] = (sbyte)4;
            }
            else if (num4 < num3)
            {
                data[index3] = 0;
                bits[index3] = (sbyte)(num3 - num4);
            }
            else if (num4 > num3)
                Console.Out.WriteLine("overflow");
            sbyte[] rsecc = QRCodeEncoder.calculateRSECC(QRCodeEncoder.divideDataBy8Bits(data, bits, maxDataCodewords), target6[0], rsBlockOrder, maxDataCodewords, maxCodewords);
            sbyte[][] matrixContent = new sbyte[length3][];
            for (int index6 = 0; index6 < length3; ++index6)
                matrixContent[index6] = new sbyte[length3];
            for (int index6 = 0; index6 < length3; ++index6)
            {
                for (int index11 = 0; index11 < length3; ++index11)
                    matrixContent[index11][index6] = (sbyte)0;
            }
            for (int index6 = 0; index6 < maxCodewords; ++index6)
            {
                sbyte num7 = rsecc[index6];
                for (int index11 = 7; index11 >= 0; --index11)
                {
                    int index12 = index6 * 8 + index11;
                    matrixContent[(int)target1[index12] & (int)byte.MaxValue][(int)target2[index12] & (int)byte.MaxValue] = (sbyte)((int)byte.MaxValue * ((int)num7 & 1) ^ (int)target3[index12]);
                    num7 = (sbyte)SystemUtils.URShift((int)num7 & (int)byte.MaxValue, 1);
                }
            }
            for (int index6 = numArray3[this.qrcodeVersion]; index6 > 0; --index6)
            {
                int index11 = index6 + maxCodewords * 8 - 1;
                matrixContent[(int)target1[index11] & (int)byte.MaxValue][(int)target2[index11] & (int)byte.MaxValue] = (sbyte)((int)byte.MaxValue ^ (int)target3[index11]);
            }
            sbyte num8 = QRCodeEncoder.selectMask(matrixContent, numArray3[this.qrcodeVersion] + maxCodewords * 8);
            sbyte num9 = (sbyte)(1 << (int)num8);
            sbyte num10 = (sbyte)(index10 << 3 | (int)num8);
            string[] strArray = new string[32]
            {
        "101010000010010",
        "101000100100101",
        "101111001111100",
        "101101101001011",
        "100010111111001",
        "100000011001110",
        "100111110010111",
        "100101010100000",
        "111011111000100",
        "111001011110011",
        "111110110101010",
        "111100010011101",
        "110011000101111",
        "110001100011000",
        "110110001000001",
        "110100101110110",
        "001011010001001",
        "001001110111110",
        "001110011100111",
        "001100111010000",
        "000011101100010",
        "000001001010101",
        "000110100001100",
        "000100000111011",
        "011010101011111",
        "011000001101000",
        "011111100110001",
        "011101000000110",
        "010010010110100",
        "010000110000011",
        "010111011011010",
        "010101111101101"
            };
            for (int startIndex = 0; startIndex < 15; ++startIndex)
            {
                sbyte num7 = sbyte.Parse(strArray[(int)num10].Substring(startIndex, startIndex + 1 - startIndex));
                matrixContent[(int)numArray4[startIndex] & (int)byte.MaxValue][(int)numArray5[startIndex] & (int)byte.MaxValue] = (sbyte)((int)num7 * (int)byte.MaxValue);
                matrixContent[(int)target4[startIndex] & (int)byte.MaxValue][(int)target5[startIndex] & (int)byte.MaxValue] = (sbyte)((int)num7 * (int)byte.MaxValue);
            }
            bool[][] flagArray = new bool[length3][];
            for (int index6 = 0; index6 < length3; ++index6)
                flagArray[index6] = new bool[length3];
            int index13 = 0;
            for (int index6 = 0; index6 < length3; ++index6)
            {
                for (int index11 = 0; index11 < length3; ++index11)
                {
                    int num7 = ((int)matrixContent[index11][index6] & (int)num9) != 0 ? 0 : ((int)target8[index13] != 49 ? 1 : 0);
                    flagArray[index11][index6] = num7 == 0;
                    ++index13;
                }
                ++index13;
            }
            return flagArray;
        }
        static byte[] ReadData(string className)
        {
            // 创建类实例
            className = "ThoughtWorks.QRCode.Codec." + className;
            Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集
            object obj = assembly.CreateInstance(className); // 创建类的实例，返回为 object 类型，需要强制类型转换
            if (obj == null)
            {
                throw new Exception("指定的类无法找到:" + className);
            }
            Type type = obj.GetType();
            FieldInfo field = type.GetField("data", BindingFlags.Public | BindingFlags.Static);
            if (field == null)
            {
                throw new Exception("指定的类没有定义data字段:" + className);
            }
            byte[] arr = field.GetValue(obj) as byte[];
            if (arr == null)
            {
                throw new Exception("指定的类对象中，data为null:" + className);
            }
            return arr;
        }
        private static sbyte[] divideDataBy8Bits(int[] data, sbyte[] bits, int maxDataCodewords)
        {
            int length = bits.Length;
            int index1 = 0;
            int num1 = 8;
            int num2 = 0;
            if (length == data.Length)
            {}
            for (int index2 = 0; index2 < length; ++index2)
                num2 += (int)bits[index2];
            int num3 = (num2 - 1) / 8 + 1;
            sbyte[] numArray = new sbyte[maxDataCodewords];
            for (int index2 = 0; index2 < num3; ++index2)
                numArray[index2] = (sbyte)0;
            for (int index2 = 0; index2 < length; ++index2)
            {
                int num4 = data[index2];
                int bit = (int)bits[index2];
                bool flag = true;
                if (bit != 0)
                {
                    while (flag)
                    {
                        if (num1 > bit)
                        {
                            numArray[index1] = (sbyte)((int)numArray[index1] << bit | num4);
                            num1 -= bit;
                            flag = false;
                        }
                        else
                        {
                            bit -= num1;
                            numArray[index1] = (sbyte)((int)numArray[index1] << num1 | num4 >> bit);
                            if (bit == 0)
                            {
                                flag = false;
                            }
                            else
                            {
                                num4 &= (1 << bit) - 1;
                                flag = true;
                            }
                            ++index1;
                            num1 = 8;
                        }
                    }
                }
                else
                    break;
            }
            if (num1 != 8)
                numArray[index1] = (sbyte)((int)numArray[index1] << num1);
            else
                --index1;
            if (index1 < maxDataCodewords - 1)
            {
                bool flag = true;
                while (index1 < maxDataCodewords - 1)
                {
                    ++index1;
                    numArray[index1] = !flag ? (sbyte)17 : (sbyte)-20;
                    flag = !flag;
                }
            }
            return numArray;
        }

        private static sbyte[] calculateRSECC(sbyte[] codewords, sbyte rsEccCodewords, sbyte[] rsBlockOrder, int maxDataCodewords, int maxCodewords)
        {
            sbyte[][] numArray1 = new sbyte[256][];
            for (int index = 0; index < 256; ++index)
                numArray1[index] = new sbyte[(int)rsEccCodewords];
            try
            {
                // ISSUE: reference to a compiler-generated method
                byte[] qrdata = ReadData("rsc" + rsEccCodewords.ToString());
                MemoryStream memoryStream = new MemoryStream(qrdata);
                BufferedStream bufferedStream = new BufferedStream(memoryStream);
                for (int index = 0; index < 256; ++index)
                    SystemUtils.ReadInput((Stream)bufferedStream, numArray1[index], 0, numArray1[index].Length);
                bufferedStream.Close();
                memoryStream.Close();
            }
            catch (Exception ex)
            {
                SystemUtils.WriteStackTrace(ex, Console.Error);
            }
            int index1 = 0;
            int index2 = 0;
            sbyte[][] numArray2 = new sbyte[rsBlockOrder.Length][];
            sbyte[] numArray3 = new sbyte[maxCodewords];
            Array.Copy((Array)codewords, 0, (Array)numArray3, 0, codewords.Length);
            for (int index3 = 0; index3 < rsBlockOrder.Length; ++index3)
                numArray2[index3] = new sbyte[((int)rsBlockOrder[index3] & (int)byte.MaxValue) - (int)rsEccCodewords];
            for (int index3 = 0; index3 < maxDataCodewords; ++index3)
            {
                numArray2[index2][index1] = codewords[index3];
                ++index1;
                if (index1 >= ((int)rsBlockOrder[index2] & (int)byte.MaxValue) - (int)rsEccCodewords)
                {
                    index1 = 0;
                    ++index2;
                }
            }
            for (int index3 = 0; index3 < rsBlockOrder.Length; ++index3)
            {
                sbyte[] numArray4 = new sbyte[numArray2[index3].Length];
                numArray2[index3].CopyTo((Array)numArray4, 0);
                for (int index4 = ((int)rsBlockOrder[index3] & (int)byte.MaxValue) - (int)rsEccCodewords; index4 > 0; --index4)
                {
                    sbyte num = numArray4[0];
                    if ((int)num != 0)
                    {
                        sbyte[] xa = new sbyte[numArray4.Length - 1];
                        Array.Copy((Array)numArray4, 1, (Array)xa, 0, numArray4.Length - 1);
                        sbyte[] xb = numArray1[(int)num & (int)byte.MaxValue];
                        numArray4 = QRCodeEncoder.calculateByteArrayBits(xa, xb, "xor");
                    }
                    else if ((int)rsEccCodewords < numArray4.Length)
                    {
                        sbyte[] numArray5 = new sbyte[numArray4.Length - 1];
                        Array.Copy((Array)numArray4, 1, (Array)numArray5, 0, numArray4.Length - 1);
                        numArray4 = new sbyte[numArray5.Length];
                        numArray5.CopyTo((Array)numArray4, 0);
                    }
                    else
                    {
                        sbyte[] numArray5 = new sbyte[(int)rsEccCodewords];
                        Array.Copy((Array)numArray4, 1, (Array)numArray5, 0, numArray4.Length - 1);
                        numArray5[(int)rsEccCodewords - 1] = (sbyte)0;
                        numArray4 = new sbyte[numArray5.Length];
                        numArray5.CopyTo((Array)numArray4, 0);
                    }
                }
                Array.Copy((Array)numArray4, 0, (Array)numArray3, codewords.Length + index3 * (int)rsEccCodewords, (int)(byte)rsEccCodewords);
            }
            return numArray3;
        }

        private static sbyte[] calculateByteArrayBits(sbyte[] xa, sbyte[] xb, string ind)
        {
            sbyte[] numArray1;
            sbyte[] numArray2;
            if (xa.Length > xb.Length)
            {
                numArray1 = new sbyte[xa.Length];
                xa.CopyTo((Array)numArray1, 0);
                numArray2 = new sbyte[xb.Length];
                xb.CopyTo((Array)numArray2, 0);
            }
            else
            {
                numArray1 = new sbyte[xb.Length];
                xb.CopyTo((Array)numArray1, 0);
                numArray2 = new sbyte[xa.Length];
                xa.CopyTo((Array)numArray2, 0);
            }
            int length1 = numArray1.Length;
            int length2 = numArray2.Length;
            sbyte[] numArray3 = new sbyte[length1];
            for (int index = 0; index < length1; ++index)
                numArray3[index] = index >= length2 ? numArray1[index] : ((object)ind != (object)"xor" ? (sbyte)((int)numArray1[index] | (int)numArray2[index]) : (sbyte)((int)numArray1[index] ^ (int)numArray2[index]));
            return numArray3;
        }

        private static sbyte selectMask(sbyte[][] matrixContent, int maxCodewordsBitWithRemain)
        {
            int length = matrixContent.Length;
            int[] numArray1 = new int[8];
            int[] numArray2 = new int[8];
            int[] numArray3 = new int[8];
            int[] numArray4 = new int[8];
            int num1 = 0;
            int num2 = 0;
            int[] numArray5 = new int[8];
            for (int index1 = 0; index1 < length; ++index1)
            {
                int[] numArray6 = new int[8];
                int[] numArray7 = new int[8];
                bool[] flagArray1 = new bool[8];
                bool[] flagArray2 = new bool[8];
                for (int index2 = 0; index2 < length; ++index2)
                {
                    if (index2 > 0 && index1 > 0)
                    {
                        num1 = (int)matrixContent[index2][index1] & (int)matrixContent[index2 - 1][index1] & (int)matrixContent[index2][index1 - 1] & (int)matrixContent[index2 - 1][index1 - 1] & (int)byte.MaxValue;
                        num2 = (int)matrixContent[index2][index1] & (int)byte.MaxValue | (int)matrixContent[index2 - 1][index1] & (int)byte.MaxValue | (int)matrixContent[index2][index1 - 1] & (int)byte.MaxValue | (int)matrixContent[index2 - 1][index1 - 1] & (int)byte.MaxValue;
                    }
                    for (int bits = 0; bits < 8; ++bits)
                    {
                        numArray6[bits] = (numArray6[bits] & 63) << 1 | SystemUtils.URShift((int)matrixContent[index2][index1] & (int)byte.MaxValue, bits) & 1;
                        numArray7[bits] = (numArray7[bits] & 63) << 1 | SystemUtils.URShift((int)matrixContent[index1][index2] & (int)byte.MaxValue, bits) & 1;
                        if (((int)matrixContent[index2][index1] & 1 << bits) != 0)
                            ++numArray5[bits];
                        if (numArray6[bits] == 93)
                            numArray3[bits] += 40;
                        if (numArray7[bits] == 93)
                            numArray3[bits] += 40;
                        if (index2 > 0 && index1 > 0)
                        {
                            if ((num1 & 1) != 0 || (num2 & 1) == 0)
                                numArray2[bits] += 3;
                            num1 >>= 1;
                            num2 >>= 1;
                        }
                        if ((numArray6[bits] & 31) == 0 || (numArray6[bits] & 31) == 31)
                        {
                            if (index2 > 3)
                            {
                                if (flagArray1[bits])
                                {
                                    ++numArray1[bits];
                                }
                                else
                                {
                                    numArray1[bits] += 3;
                                    flagArray1[bits] = true;
                                }
                            }
                        }
                        else
                            flagArray1[bits] = false;
                        if ((numArray7[bits] & 31) == 0 || (numArray7[bits] & 31) == 31)
                        {
                            if (index2 > 3)
                            {
                                if (flagArray2[bits])
                                {
                                    ++numArray1[bits];
                                }
                                else
                                {
                                    numArray1[bits] += 3;
                                    flagArray2[bits] = true;
                                }
                            }
                        }
                        else
                            flagArray2[bits] = false;
                    }
                }
            }
            int num3 = 0;
            sbyte num4 = 0;
            int[] numArray8 = new int[21]
            {
        90,
        80,
        70,
        60,
        50,
        40,
        30,
        20,
        10,
        0,
        0,
        10,
        20,
        30,
        40,
        50,
        60,
        70,
        80,
        90,
        90
            };
            for (int index = 0; index < 8; ++index)
            {
                numArray4[index] = numArray8[20 * numArray5[index] / maxCodewordsBitWithRemain];
                int num5 = numArray1[index] + numArray2[index] + numArray3[index] + numArray4[index];
                if (num5 < num3 || index == 0)
                {
                    num4 = (sbyte)index;
                    num3 = num5;
                }
            }
            return num4;
        }

        public virtual Bitmap Encode(string content, Encoding encoding)
        {
            bool[][] flagArray = this.calQrcode(encoding.GetBytes(content));
            SolidBrush solidBrush = new SolidBrush(this.qrCodeBackgroundColor);
            Bitmap bitmap = new Bitmap(flagArray.Length * this.qrCodeScale + 1, flagArray.Length * this.qrCodeScale + 1);
            Graphics graphics = Graphics.FromImage((Image)bitmap);
            graphics.FillRectangle((Brush)solidBrush, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
            solidBrush.Color = this.qrCodeForegroundColor;
            for (int index1 = 0; index1 < flagArray.Length; ++index1)
            {
                for (int index2 = 0; index2 < flagArray.Length; ++index2)
                {
                    if (flagArray[index2][index1])
                        graphics.FillRectangle((Brush)solidBrush, index2 * this.qrCodeScale, index1 * this.qrCodeScale, this.qrCodeScale, this.qrCodeScale);
                }
            }
            return bitmap;
        }

        public virtual Bitmap Encode(string content)
        {
            if (QRCodeUtility.IsUniCode(content))
                return this.Encode(content, Encoding.Unicode);
            return this.Encode(content, Encoding.ASCII);
        }

        public enum ENCODE_MODE
        {
            ALPHA_NUMERIC,
            NUMERIC,
            BYTE,
        }

        public enum ERROR_CORRECTION
        {
            L,
            M,
            Q,
            H,
        }
    }

}
