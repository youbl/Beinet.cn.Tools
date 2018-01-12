using System;
using System.Collections;
using System.Text;
using ThoughtWorks.QRCode.Codec.Data;
using ThoughtWorks.QRCode.Codec.Util;
using QRCodeUtility = ThoughtWorks.QRCode.Codec.Util.QRCodeUtility;

namespace ThoughtWorks.QRCode.Codec
{
    public class QRCodeDecoder
    {
        internal ArrayList lastResults = ArrayList.Synchronized(new ArrayList(10));
        internal QRCodeSymbol qrCodeSymbol;
        internal int numTryDecode;
        internal ArrayList results;
        internal static DebugCanvas canvas;
        internal QRCodeImageReader imageReader;
        internal int numLastCorrections;
        internal bool correctionSucceeded;

        public static DebugCanvas Canvas
        {
            get
            {
                return QRCodeDecoder.canvas;
            }
            set
            {
                QRCodeDecoder.canvas = value;
            }
        }

        internal virtual Point[] AdjustPoints
        {
            get
            {
                ArrayList arrayList = ArrayList.Synchronized(new ArrayList(10));
                for (int index = 0; index < 4; ++index)
                    arrayList.Add(new Point(1, 1));
                int num1 = 0;
                int num2 = 0;
                for (int index1 = 0; index1 > -4; --index1)
                {
                    for (int index2 = 0; index2 > -4; --index2)
                    {
                        if (index2 != index1 && (index2 + index1) % 2 == 0)
                        {
                            arrayList.Add(new Point(index2 - num1, index1 - num2));
                            num1 = index2;
                            num2 = index1;
                        }
                    }
                }
                Point[] pointArray = new Point[arrayList.Count];
                for (int index = 0; index < pointArray.Length; ++index)
                    pointArray[index] = (Point)arrayList[index];
                return pointArray;
            }
        }

        public QRCodeDecoder()
        {
            this.numTryDecode = 0;
            this.results = ArrayList.Synchronized(new ArrayList(10));
            QRCodeDecoder.canvas = (DebugCanvas)new DebugCanvasAdapter();
        }

        public virtual sbyte[] decodeBytes(QRCodeImage qrCodeImage)
        {
            Point[] adjustPoints = this.AdjustPoints;
            ArrayList arrayList = ArrayList.Synchronized(new ArrayList(10));
            while (this.numTryDecode < adjustPoints.Length)
            {
                try
                {
                    QRCodeDecoder.DecodeResult decodeResult = this.decode(qrCodeImage, adjustPoints[this.numTryDecode]);
                    if (decodeResult.CorrectionSucceeded)
                        return decodeResult.DecodedBytes;
                    arrayList.Add(decodeResult);
                    QRCodeDecoder.canvas.println("Decoding succeeded but could not correct");
                    QRCodeDecoder.canvas.println("all errors. Retrying..");
                }
                catch (DecodingFailedException ex)
                {
                    if (ex.Message.IndexOf("Finder Pattern") >= 0)
                        throw ex;
                }
                finally
                {
                    ++this.numTryDecode;
                }
            }
            if (arrayList.Count == 0)
                throw new DecodingFailedException("Give up decoding");
            int index1 = -1;
            int num = int.MaxValue;
            for (int index2 = 0; index2 < arrayList.Count; ++index2)
            {
                QRCodeDecoder.DecodeResult decodeResult = (QRCodeDecoder.DecodeResult)arrayList[index2];
                if (decodeResult.NumErrors < num)
                {
                    num = decodeResult.NumErrors;
                    index1 = index2;
                }
            }
            QRCodeDecoder.canvas.println("All trials need for correct error");
            QRCodeDecoder.canvas.println("Reporting #" + index1.ToString() + " that,");
            QRCodeDecoder.canvas.println("corrected minimum errors (" + num.ToString() + ")");
            QRCodeDecoder.canvas.println("Decoding finished.");
            return ((QRCodeDecoder.DecodeResult)arrayList[index1]).DecodedBytes;
        }

        public virtual string decode(QRCodeImage qrCodeImage, Encoding encoding)
        {
            sbyte[] numArray = this.decodeBytes(qrCodeImage);
            byte[] bytes = new byte[numArray.Length];
            Buffer.BlockCopy((Array)numArray, 0, (Array)bytes, 0, bytes.Length);
            return encoding.GetString(bytes);
        }

        public virtual string decode(QRCodeImage qrCodeImage)
        {
            sbyte[] numArray1 = this.decodeBytes(qrCodeImage);
            byte[] numArray2 = new byte[numArray1.Length];
            Buffer.BlockCopy((Array)numArray1, 0, (Array)numArray2, 0, numArray2.Length);
            return (!QRCodeUtility.IsUnicode(numArray2) ? Encoding.ASCII : Encoding.Unicode).GetString(numArray2);
        }

        internal virtual QRCodeDecoder.DecodeResult decode(QRCodeImage qrCodeImage, Point adjust)
        {
            try
            {
                if (this.numTryDecode == 0)
                {
                    QRCodeDecoder.canvas.println("Decoding started");
                    int[][] intArray = this.imageToIntArray(qrCodeImage);
                    this.imageReader = new QRCodeImageReader();
                    this.qrCodeSymbol = this.imageReader.getQRCodeSymbol(intArray);
                }
                else
                {
                    QRCodeDecoder.canvas.println("--");
                    QRCodeDecoder.canvas.println("Decoding restarted #" + numTryDecode.ToString());
                    this.qrCodeSymbol = this.imageReader.getQRCodeSymbolWithAdjustedGrid(adjust);
                }
            }
            catch (SymbolNotFoundException ex)
            {
                throw new DecodingFailedException(ex.Message);
            }
            QRCodeDecoder.canvas.println("Created QRCode symbol.");
            QRCodeDecoder.canvas.println("Reading symbol.");
            QRCodeDecoder.canvas.println("Version: " + this.qrCodeSymbol.VersionReference);
            QRCodeDecoder.canvas.println("Mask pattern: " + this.qrCodeSymbol.MaskPatternRefererAsString);
            int[] blocks1 = this.qrCodeSymbol.Blocks;
            QRCodeDecoder.canvas.println("Correcting data errors.");
            int[] blocks2 = this.correctDataBlocks(blocks1);
            try
            {
                return new QRCodeDecoder.DecodeResult(this, this.getDecodedByteArray(blocks2, this.qrCodeSymbol.Version, this.qrCodeSymbol.NumErrorCollectionCode), this.numLastCorrections, this.correctionSucceeded);
            }
            catch (InvalidDataBlockException ex)
            {
                QRCodeDecoder.canvas.println(ex.Message);
                throw new DecodingFailedException(ex.Message);
            }
        }

        internal virtual int[][] imageToIntArray(QRCodeImage image)
        {
            int width = image.Width;
            int height = image.Height;
            int[][] numArray = new int[width][];
            for (int index = 0; index < width; ++index)
                numArray[index] = new int[height];
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                    numArray[x][y] = image.getPixel(x, y);
            }
            return numArray;
        }

        internal virtual int[] correctDataBlocks(int[] blocks)
        {
            int num1 = 0;
            int dataCapacity = this.qrCodeSymbol.DataCapacity;
            int[] numArray1 = new int[dataCapacity];
            int errorCollectionCode = this.qrCodeSymbol.NumErrorCollectionCode;
            int numRsBlocks = this.qrCodeSymbol.NumRSBlocks;
            int NPAR = errorCollectionCode / numRsBlocks;
            if (numRsBlocks == 1)
            {
                ReedSolomon reedSolomon = new ReedSolomon(blocks, NPAR);
                reedSolomon.correct();
                int num2 = num1 + reedSolomon.NumCorrectedErrors;
                if (num2 > 0)
                    QRCodeDecoder.canvas.println(Convert.ToString(num2) + " data errors corrected.");
                else
                    QRCodeDecoder.canvas.println("No errors found.");
                this.numLastCorrections = num2;
                this.correctionSucceeded = reedSolomon.CorrectionSucceeded;
                return blocks;
            }
            int length1 = dataCapacity % numRsBlocks;
            if (length1 == 0)
            {
                int length2 = dataCapacity / numRsBlocks;
                int[][] numArray2 = new int[numRsBlocks][];
                for (int index = 0; index < numRsBlocks; ++index)
                    numArray2[index] = new int[length2];
                int[][] numArray3 = numArray2;
                for (int index1 = 0; index1 < numRsBlocks; ++index1)
                {
                    for (int index2 = 0; index2 < length2; ++index2)
                        numArray3[index1][index2] = blocks[index2 * numRsBlocks + index1];
                    ReedSolomon reedSolomon = new ReedSolomon(numArray3[index1], NPAR);
                    reedSolomon.correct();
                    num1 += reedSolomon.NumCorrectedErrors;
                    this.correctionSucceeded = reedSolomon.CorrectionSucceeded;
                }
                int num2 = 0;
                for (int index1 = 0; index1 < numRsBlocks; ++index1)
                {
                    for (int index2 = 0; index2 < length2 - NPAR; ++index2)
                        numArray1[num2++] = numArray3[index1][index2];
                }
            }
            else
            {
                int length2 = dataCapacity / numRsBlocks;
                int length3 = dataCapacity / numRsBlocks + 1;
                int length4 = numRsBlocks - length1;
                int[][] numArray2 = new int[length4][];
                for (int index = 0; index < length4; ++index)
                    numArray2[index] = new int[length2];
                int[][] numArray3 = numArray2;
                int[][] numArray4 = new int[length1][];
                for (int index = 0; index < length1; ++index)
                    numArray4[index] = new int[length3];
                int[][] numArray5 = numArray4;
                for (int index1 = 0; index1 < numRsBlocks; ++index1)
                {
                    if (index1 < length4)
                    {
                        int num2 = 0;
                        for (int index2 = 0; index2 < length2; ++index2)
                        {
                            if (index2 == length2 - NPAR)
                                num2 = length1;
                            numArray3[index1][index2] = blocks[index2 * numRsBlocks + index1 + num2];
                        }
                        ReedSolomon reedSolomon = new ReedSolomon(numArray3[index1], NPAR);
                        reedSolomon.correct();
                        num1 += reedSolomon.NumCorrectedErrors;
                        this.correctionSucceeded = reedSolomon.CorrectionSucceeded;
                    }
                    else
                    {
                        int num2 = 0;
                        for (int index2 = 0; index2 < length3; ++index2)
                        {
                            if (index2 == length2 - NPAR)
                                num2 = length4;
                            numArray5[index1 - length4][index2] = blocks[index2 * numRsBlocks + index1 - num2];
                        }
                        ReedSolomon reedSolomon = new ReedSolomon(numArray5[index1 - length4], NPAR);
                        reedSolomon.correct();
                        num1 += reedSolomon.NumCorrectedErrors;
                        this.correctionSucceeded = reedSolomon.CorrectionSucceeded;
                    }
                }
                int num3 = 0;
                for (int index1 = 0; index1 < numRsBlocks; ++index1)
                {
                    if (index1 < length4)
                    {
                        for (int index2 = 0; index2 < length2 - NPAR; ++index2)
                            numArray1[num3++] = numArray3[index1][index2];
                    }
                    else
                    {
                        for (int index2 = 0; index2 < length3 - NPAR; ++index2)
                            numArray1[num3++] = numArray5[index1 - length4][index2];
                    }
                }
            }
            if (num1 > 0)
                QRCodeDecoder.canvas.println(Convert.ToString(num1) + " data errors corrected.");
            else
                QRCodeDecoder.canvas.println("No errors found.");
            this.numLastCorrections = num1;
            return numArray1;
        }

        internal virtual sbyte[] getDecodedByteArray(int[] blocks, int version, int numErrorCorrectionCode)
        {
            QRCodeDataBlockReader codeDataBlockReader = new QRCodeDataBlockReader(blocks, version, numErrorCorrectionCode);
            sbyte[] dataByte;
            try
            {
                dataByte = codeDataBlockReader.DataByte;
            }
            catch (InvalidDataBlockException ex)
            {
                throw ex;
            }
            return dataByte;
        }

        internal virtual string getDecodedString(int[] blocks, int version, int numErrorCorrectionCode)
        {
            QRCodeDataBlockReader codeDataBlockReader = new QRCodeDataBlockReader(blocks, version, numErrorCorrectionCode);
            string dataString;
            try
            {
                dataString = codeDataBlockReader.DataString;
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new InvalidDataBlockException(ex.Message);
            }
            return dataString;
        }

        internal class DecodeResult
        {
            internal int numCorrections;
            internal bool correctionSucceeded;
            internal sbyte[] decodedBytes;
            private QRCodeDecoder enclosingInstance;

            public DecodeResult(QRCodeDecoder enclosingInstance, sbyte[] decodedBytes, int numErrors, bool correctionSucceeded)
            {
                this.InitBlock(enclosingInstance);
                this.decodedBytes = decodedBytes;
                this.numCorrections = numErrors;
                this.correctionSucceeded = correctionSucceeded;
            }

            private void InitBlock(QRCodeDecoder enclosingInstance)
            {
                this.enclosingInstance = enclosingInstance;
            }

            public virtual sbyte[] DecodedBytes
            {
                get
                {
                    return this.decodedBytes;
                }
            }

            public virtual int NumErrors
            {
                get
                {
                    return this.numCorrections;
                }
            }

            public virtual bool CorrectionSucceeded
            {
                get
                {
                    return this.correctionSucceeded;
                }
            }

            public QRCodeDecoder Enclosing_Instance
            {
                get
                {
                    return this.enclosingInstance;
                }
            }
        }
    }
}
