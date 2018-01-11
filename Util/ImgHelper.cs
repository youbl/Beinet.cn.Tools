using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Beinet.cn.Tools
{
    public static class ImgHelper
    {
        private static ASCIIEncoding _encoding = new ASCIIEncoding();

        private static bool IsPicture(string filePath)//filePath是文件的完整路径 
        {
            try
            {
                string fileClass;
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    byte[] b = new byte[2];
                    byte buffer = reader.ReadByte();
                    b[0] = buffer;
                    fileClass = buffer.ToString();
                    buffer = reader.ReadByte();
                    b[1] = buffer;
                    fileClass += buffer.ToString();

                }
                //255216是jpg;7173是gif;6677是BMP,13780是PNG;7790是exe,8297是rar
                if (fileClass == "255216")
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static Metadata GetExif(string imgPath)
        {
            Metadata metadata = _initMetadata();
            if (!IsPicture(imgPath))
            {
                return metadata;
            }
            metadata.IsImg = true;
            using (Image img = Image.FromFile(imgPath))
            {
                int[] arrProp = img.PropertyIdList;
                foreach (int propId in arrProp)
                {
                    PropertyItem propItem = img.GetPropertyItem(propId);
                    _countProp(ref metadata, propItem);
                }
            }

            return metadata;
        }

        private static Metadata _initMetadata()
        {
            return new Metadata
            {
                IsImg = false,
                EquipmentMake = { Hex = "10f" },
                CameraModel = { Hex = "110" },
                DatePictureTaken = { Hex = "9003" },
                ExposureTime = { Hex = "829a" },
                Fstop = { Hex = "829d" },
                ShutterSpeed = { Hex = "9201" },
                MeteringMode = { Hex = "9207" },
                Flash = { Hex = "9209" },
                FNumber = { Hex = "829d" },
                ExposureProg = { Hex = "" },
                SpectralSense = { Hex = "8824" },
                ISOSpeed = { Hex = "8827" },
                OECF = { Hex = "8828" },
                Ver = { Hex = "9000" },
                CompConfig = { Hex = "9101" },
                CompBPP = { Hex = "9102" },
                Aperture = { Hex = "9202" },
                Brightness = { Hex = "9203" },
                ExposureBias = { Hex = "9204" },
                MaxAperture = { Hex = "9205" },
                SubjectDist = { Hex = "9206" },
                LightSource = { Hex = "9208" },
                FocalLength = { Hex = "920a" },
                FPXVer = { Hex = "a000" },
                ColorSpace = { Hex = "a001" },
                FocalXRes = { Hex = "a20e" },
                FocalYRes = { Hex = "a20f" },
                FocalResUnit = { Hex = "a210" },
                ExposureIndex = { Hex = "a215" },
                SensingMethod = { Hex = "a217" },
                SceneType = { Hex = "a301" },
                CfaPattern = { Hex = "a302" }
            };
        }

        private static void _countProp(ref Metadata metadata, PropertyItem propItem)
        {
            string hexVal;
            byte[] propValue = propItem.Value;
            string strProp = propItem.Id.ToString("x");
            string strVal = "", strCodeVal = "";
            if (propValue != null)
            {
                strVal = BitConverter.ToString(propValue);
                strCodeVal = _encoding.GetString(propValue).Trim('\0');
            }
            else
            {
                propValue = new byte[0];
            }
            switch (strProp)
            {
                case "10f":
                    {
                        metadata.EquipmentMake.RawValueAsString = strVal;
                        metadata.EquipmentMake.DisplayValue = strCodeVal;
                        break;
                    }

                case "110":
                    {
                        metadata.CameraModel.RawValueAsString = strVal;
                        metadata.CameraModel.DisplayValue = strCodeVal;
                        break;
                    }

                case "9003":
                    {
                        metadata.DatePictureTaken.RawValueAsString = strVal;
                        metadata.DatePictureTaken.DisplayValue = strCodeVal;
                        break;
                    }

                case "9207":
                    {
                        metadata.MeteringMode.RawValueAsString = strVal;
                        metadata.MeteringMode.DisplayValue = LookupEXIFValue("MeteringMode", BitConverter.ToInt16(propValue, 0).ToString());
                        break;
                    }

                case "9209":
                    {
                        metadata.Flash.RawValueAsString = strVal;
                        metadata.Flash.DisplayValue = LookupEXIFValue("Flash", BitConverter.ToInt16(propValue, 0).ToString());
                        break;
                    }

                case "829a":
                    {
                        metadata.ExposureTime.RawValueAsString = strVal;
                        string StringValue = "";
                        for (int Offset = 0; Offset < propItem.Len; Offset = Offset + 4)
                        {
                            StringValue += BitConverter.ToInt32(propValue, Offset).ToString() + "/";
                        }
                        metadata.ExposureTime.DisplayValue = StringValue.Substring(0, StringValue.Length - 1);
                        break;
                    }
                case "829d":
                    {
                        metadata.Fstop.RawValueAsString = strVal;
                        int int1 = BitConverter.ToInt32(propValue, 0);
                        int int2 = BitConverter.ToInt32(propValue, 4);
                        metadata.Fstop.DisplayValue = "F/" + (int1 / int2);

                        metadata.FNumber.RawValueAsString = strVal;
                        metadata.FNumber.DisplayValue = BitConverter.ToInt16(propValue, 0).ToString();

                        break;
                    }
                case "9201":
                    {
                        metadata.ShutterSpeed.RawValueAsString = strVal;
                        string StringValue = BitConverter.ToInt32(propValue, 0).ToString();
                        metadata.ShutterSpeed.DisplayValue = "1/" + StringValue;
                        break;
                    }

                case "8822":
                    {
                        metadata.ExposureProg.RawValueAsString = strVal;
                        metadata.ExposureProg.DisplayValue = LookupEXIFValue("ExposureProg", BitConverter.ToInt16(propValue, 0).ToString());
                        break;
                    }

                case "8824":
                    {
                        metadata.SpectralSense.RawValueAsString = strVal;
                        metadata.SpectralSense.DisplayValue = strCodeVal;
                        break;
                    }
                case "8827":
                    {
                        metadata.ISOSpeed.RawValueAsString = strVal;
                        hexVal = strVal.Substring(0, 2);
                        metadata.ISOSpeed.DisplayValue = Convert.ToInt32(hexVal, 16).ToString();//_encoding.GetString(propItem); 
                        break;
                    }

                case "8828":
                    {
                        metadata.OECF.RawValueAsString = strVal;
                        metadata.OECF.DisplayValue = strCodeVal;
                        break;
                    }

                case "9000":
                    {
                        metadata.Ver.RawValueAsString = strVal;
                        metadata.Ver.DisplayValue = strCodeVal.Substring(1, 1) + "." + strCodeVal.Substring(2, 2);
                        break;
                    }

                case "9101":
                    {
                        metadata.CompConfig.RawValueAsString = strVal;
                        metadata.CompConfig.DisplayValue = LookupEXIFValue("CompConfig", BitConverter.ToInt16(propValue, 0).ToString());
                        break;
                    }

                case "9102":
                    {
                        metadata.CompBPP.RawValueAsString = strVal;
                        metadata.CompBPP.DisplayValue = BitConverter.ToInt16(propValue, 0).ToString();
                        break;
                    }

                case "9202":
                    {
                        metadata.Aperture.RawValueAsString = strVal;
                        hexVal = _convertBit(propValue);
                        metadata.Aperture.DisplayValue = hexVal;
                        break;
                    }

                case "9203":
                    {
                        metadata.Brightness.RawValueAsString = strVal;
                        hexVal = _convertBit(propValue);
                        metadata.Brightness.DisplayValue = hexVal;
                        break;
                    }

                case "9204":
                    {
                        metadata.ExposureBias.RawValueAsString = strVal;
                        metadata.ExposureBias.DisplayValue = BitConverter.ToInt16(propValue, 0).ToString();
                        break;
                    }

                case "9205":
                    {
                        metadata.MaxAperture.RawValueAsString = strVal;
                        hexVal = _convertBit(propValue);
                        metadata.MaxAperture.DisplayValue = hexVal;
                        break;
                    }

                case "9206":
                    {
                        metadata.SubjectDist.RawValueAsString = strVal;
                        metadata.SubjectDist.DisplayValue = strCodeVal;
                        break;
                    }

                case "9208":
                    {
                        metadata.LightSource.RawValueAsString = strVal;
                        metadata.LightSource.DisplayValue = LookupEXIFValue("LightSource", BitConverter.ToInt16(propValue, 0).ToString());
                        break;
                    }

                case "920a":
                    {
                        metadata.FocalLength.RawValueAsString = strVal;
                        hexVal = _convertBit(propValue);
                        metadata.FocalLength.DisplayValue = hexVal;
                        break;
                    }

                case "a000":
                    {
                        metadata.FPXVer.RawValueAsString = strVal;
                        metadata.FPXVer.DisplayValue = strCodeVal.Substring(1, 1) + "." + strCodeVal.Substring(2, 2);
                        break;
                    }

                case "a001":
                    {
                        metadata.ColorSpace.RawValueAsString = strVal;
                        if (BitConverter.ToInt16(propValue, 0).ToString() == "1")
                            metadata.ColorSpace.DisplayValue = "RGB";
                        if (BitConverter.ToInt16(propValue, 0).ToString() == "65535")
                            metadata.ColorSpace.DisplayValue = "Uncalibrated";
                        break;
                    }

                case "a20e":
                    {
                        metadata.FocalXRes.RawValueAsString = strVal;
                        metadata.FocalXRes.DisplayValue = BitConverter.ToInt16(propValue, 0).ToString();
                        break;
                    }

                case "a20f":
                    {
                        metadata.FocalYRes.RawValueAsString = strVal;
                        metadata.FocalYRes.DisplayValue = BitConverter.ToInt16(propValue, 0).ToString();
                        break;
                    }

                case "a210":
                    {
                        metadata.FocalResUnit.RawValueAsString = strVal;
                        string aa = BitConverter.ToInt16(propValue, 0).ToString();
                        if (aa == "1") metadata.FocalResUnit.DisplayValue = "没有单位";
                        if (aa == "2") metadata.FocalResUnit.DisplayValue = "英尺";
                        if (aa == "3") metadata.FocalResUnit.DisplayValue = "厘米";
                        break;
                    }

                case "a215":
                    {
                        metadata.ExposureIndex.RawValueAsString = strVal;
                        metadata.ExposureIndex.DisplayValue = strCodeVal;
                        break;
                    }

                case "a217":
                    {
                        string aa = BitConverter.ToInt16(propValue, 0).ToString();
                        metadata.SensingMethod.RawValueAsString = strVal;
                        if (aa == "2")
                        {
                            metadata.SensingMethod.DisplayValue = "1 chip color area sensor";
                        }
                        break;
                    }

                case "a301":
                    {
                        metadata.SceneType.RawValueAsString = strVal;
                        metadata.SceneType.DisplayValue = strVal;
                        break;
                    }

                case "a302":
                    {
                        metadata.CfaPattern.RawValueAsString = strVal;
                        metadata.CfaPattern.DisplayValue = strVal;
                        break;
                    }
            }
        }

        // 查找EXIF元素值
        private static string LookupEXIFValue(string Description, string Value)
        {
            string DescriptionValue = null;

            switch (Description)
            {
                case "MeteringMode":

                    #region  MeteringMode
                    {
                        switch (Value)
                        {
                            case "0":
                                DescriptionValue = "Unknown"; break;
                            case "1":
                                DescriptionValue = "Average"; break;
                            case "2":
                                DescriptionValue = "Center Weighted Average"; break;
                            case "3":
                                DescriptionValue = "Spot"; break;
                            case "4":
                                DescriptionValue = "Multi-spot"; break;
                            case "5":
                                DescriptionValue = "Multi-segment"; break;
                            case "6":
                                DescriptionValue = "Partial"; break;
                            case "255":
                                DescriptionValue = "Other"; break;
                        }
                    }
                    #endregion

                    break;
                case "ResolutionUnit":

                    #region ResolutionUnit
                    {
                        switch (Value)
                        {
                            case "1":
                                DescriptionValue = "No Units"; break;
                            case "2":
                                DescriptionValue = "Inch"; break;
                            case "3":
                                DescriptionValue = "Centimeter"; break;
                        }
                    }

                    #endregion

                    break;
                case "Flash":

                    #region Flash
                    {
                        switch (Value)
                        {
                            case "0":
                                DescriptionValue = "未使用"; break;
                            case "1":
                                DescriptionValue = "闪光"; break;
                            case "5":
                                DescriptionValue = "Flash fired but strobe return light not detected"; break;
                            case "7":
                                DescriptionValue = "Flash fired and strobe return light detected"; break;
                        }
                    }
                    #endregion

                    break;
                case "ExposureProg":

                    #region ExposureProg
                    {
                        switch (Value)
                        {
                            case "0":
                                DescriptionValue = "没有定义"; break;
                            case "1":
                                DescriptionValue = "手动控制"; break;
                            case "2":
                                DescriptionValue = "程序控制"; break;
                            case "3":
                                DescriptionValue = "光圈优先"; break;
                            case "4":
                                DescriptionValue = "快门优先"; break;
                            case "5":
                                DescriptionValue = "夜景模式"; break;
                            case "6":
                                DescriptionValue = "运动模式"; break;
                            case "7":
                                DescriptionValue = "肖像模式"; break;
                            case "8":
                                DescriptionValue = "风景模式"; break;
                            case "9":
                                DescriptionValue = "保留的"; break;
                        }
                    }

                    #endregion

                    break;
                case "CompConfig":

                    #region CompConfig
                    {
                        switch (Value)
                        {

                            case "513":
                                DescriptionValue = "YCbCr"; break;
                        }
                    }
                    #endregion

                    break;
                case "Aperture":

                    #region Aperture
                    DescriptionValue = Value;
                    #endregion

                    break;
                case "LightSource":

                    #region LightSource
                    {
                        switch (Value)
                        {
                            case "0":
                                DescriptionValue = "未知"; break;
                            case "1":
                                DescriptionValue = "日光"; break;
                            case "2":
                                DescriptionValue = "荧光灯"; break;
                            case "3":
                                DescriptionValue = "白炽灯"; break;
                            case "10":
                                DescriptionValue = "闪光灯"; break;
                            case "17":
                                DescriptionValue = "标准光A"; break;
                            case "18":
                                DescriptionValue = "标准光B"; break;
                            case "19":
                                DescriptionValue = "标准光C"; break;
                            case "20":
                                DescriptionValue = "标准光D55"; break;
                            case "21":
                                DescriptionValue = "标准光D65"; break;
                            case "22":
                                DescriptionValue = "标准光D75"; break;
                            case "255":
                                DescriptionValue = "其它"; break;
                        }
                    }


                    #endregion
                    break;

            }
            return DescriptionValue;
        }

        private static string _convertBit(byte[] propValue)
        {
            string hexVal = BitConverter.ToString(propValue).Substring(0, 2);
            hexVal = Convert.ToInt32(hexVal, 16).ToString();
            hexVal = hexVal + "00";
            return hexVal.Substring(0, 1) + "." + hexVal.Substring(1, 2);
        }
    }


    #region EXIF元素结构
    /// <summary> 
    /// 结构：存储EXIF元素信息 
    /// </summary> 
    public struct Metadata
    {
        /// <summary>
        /// 是否图片
        /// </summary>
        public bool IsImg;
        /// <summary>
        /// 厂商
        /// </summary>
        public MetadataDetail EquipmentMake;
        /// <summary>
        /// 相机型号
        /// </summary>
        public MetadataDetail CameraModel;
        /// <summary>
        /// 曝光时间,快门速度值
        /// </summary>
        public MetadataDetail ExposureTime;
        /// <summary>
        /// 光圈值
        /// </summary>
        public MetadataDetail Fstop;
        /// <summary>
        /// 拍照日期时间
        /// </summary>
        public MetadataDetail DatePictureTaken;
        /// <summary>
        /// 快门速度
        /// </summary>
        public MetadataDetail ShutterSpeed;
        /// <summary>
        /// 曝光模式
        /// </summary>
        public MetadataDetail MeteringMode;
        /// <summary>
        /// 闪光灯
        /// </summary>
        public MetadataDetail Flash;
        public MetadataDetail XResolution;
        public MetadataDetail YResolution;
        /// <summary>
        /// 照片宽度
        /// </summary>
        public MetadataDetail ImageWidth;
        /// <summary>
        /// 照片高度
        /// </summary>
        public MetadataDetail ImageHeight;
        /// <summary>
        /// f值，光圈数
        /// </summary>
        public MetadataDetail FNumber;
        /// <summary> 
        /// 曝光程序 
        /// </summary> 
        public MetadataDetail ExposureProg;
        public MetadataDetail SpectralSense;
        /// <summary> 
        /// ISO感光度 
        /// </summary> 
        public MetadataDetail ISOSpeed;
        public MetadataDetail OECF;
        /// <summary> 
        /// EXIF版本 
        /// </summary> 
        public MetadataDetail Ver;
        /// <summary> 
        /// 色彩设置 
        /// </summary> 
        public MetadataDetail CompConfig;
        /// <summary> 
        /// 压缩比率 
        /// </summary> 
        public MetadataDetail CompBPP;
        /// <summary> 
        /// 光圈值 
        /// </summary> 
        public MetadataDetail Aperture;
        /// <summary> 
        /// 亮度值Ev 
        /// </summary> 
        public MetadataDetail Brightness;
        /// <summary> 
        /// 曝光补偿 
        /// </summary> 
        public MetadataDetail ExposureBias;
        /// <summary> 
        /// 最大光圈值 
        /// </summary> 
        public MetadataDetail MaxAperture;
        /// <summary> 
        /// 主体距离 
        /// </summary> 
        public MetadataDetail SubjectDist;
        /// <summary> 
        /// 白平衡,光源
        /// </summary> 
        public MetadataDetail LightSource;
        /// <summary> 
        /// 焦距 
        /// </summary> 
        public MetadataDetail FocalLength;
        /// <summary> 
        /// FlashPix版本 
        /// </summary> 
        public MetadataDetail FPXVer;
        /// <summary> 
        /// 色彩空间 
        /// </summary> 
        public MetadataDetail ColorSpace;
        public MetadataDetail Interop;
        public MetadataDetail FlashEnergy;
        public MetadataDetail SpatialFR;
        public MetadataDetail FocalXRes;
        public MetadataDetail FocalYRes;
        public MetadataDetail FocalResUnit;
        /// <summary> 
        /// 曝光指数 
        /// </summary> 
        public MetadataDetail ExposureIndex;
        /// <summary> 
        /// 感应方式 
        /// </summary> 
        public MetadataDetail SensingMethod;
        public MetadataDetail SceneType;
        public MetadataDetail CfaPattern;
    }

    /// <summary> 
    /// 转换数据结构 
    /// </summary> 
    public struct MetadataDetail
    {
        public string Hex;//十六进制字符串 
        public string RawValueAsString;//原始值串 
        public string DisplayValue;//显示值串 
    }
    #endregion
}
