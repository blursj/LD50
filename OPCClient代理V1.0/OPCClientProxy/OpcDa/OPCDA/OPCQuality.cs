namespace OPCDA
{
    using System;

    public class OPCQuality
    {
        public limitBits LimitField;
        public qualityBits QualityField;
        public byte VendorField;

        public OPCQuality(qualityBits qstat)
        {
            this.QualityField = qualityBits.good;
            this.LimitField = limitBits.none;
            this.VendorField = 0;
            this.QualityField = qstat;
        }

        public OPCQuality(short qVal)
        {
            this.QualityField = qualityBits.good;
            this.LimitField = limitBits.none;
            this.VendorField = 0;
            this.QualityField = ((qualityBits) qVal) & (qualityBits.goodLocalOverride | qualityBits.badWaitingForInitialData | qualityBits.badConfigurationError);
            this.LimitField = ((limitBits) qVal) & limitBits.constant;
            this.VendorField = (byte) (this.VendorField >> 8);
        }

        public short GetCode()
        {
            ushort num = 0;
            num = (ushort) (num | ((ushort) this.QualityField));
            num = (ushort) (num | ((ushort) this.LimitField));
            num = (ushort) (num | ((ushort) (this.VendorField << 8)));
            if (num > 0x7fff)
            {
                return (short) -(0x10000 - num);
            }
            return (short) num;
        }

        public static qualityBits ParseQualityBits(string qs)
        {
            qs = qs.ToLower();
            if (qualityBits.good.ToString() == qs)
            {
                return qualityBits.good;
            }
            if (qualityBits.goodLocalOverride.ToString() == qs)
            {
                return qualityBits.goodLocalOverride;
            }
            if (qualityBits.bad.ToString() != qs)
            {
                if (qualityBits.badConfigurationError.ToString() == qs)
                {
                    return qualityBits.badConfigurationError;
                }
                if (qualityBits.badNotConnected.ToString() == qs)
                {
                    return qualityBits.badNotConnected;
                }
                if (qualityBits.badDeviceFailure.ToString() == qs)
                {
                    return qualityBits.badDeviceFailure;
                }
                if (qualityBits.badSensorFailure.ToString() == qs)
                {
                    return qualityBits.badSensorFailure;
                }
                if (qualityBits.badLastKnownValue.ToString() == qs)
                {
                    return qualityBits.badLastKnownValue;
                }
                if (qualityBits.badCommFailure.ToString() == qs)
                {
                    return qualityBits.badCommFailure;
                }
                if (qualityBits.badOutOfService.ToString() == qs)
                {
                    return qualityBits.badOutOfService;
                }
                if (qualityBits.badWaitingForInitialData.ToString() == qs)
                {
                    return qualityBits.badWaitingForInitialData;
                }
                if (qualityBits.uncertain.ToString() == qs)
                {
                    return qualityBits.uncertain;
                }
                if (qualityBits.uncertainLastUsableValue.ToString() == qs)
                {
                    return qualityBits.uncertainLastUsableValue;
                }
                if (qualityBits.uncertainSensorNotAccurate.ToString() == qs)
                {
                    return qualityBits.uncertainSensorNotAccurate;
                }
                if (qualityBits.uncertainEUExceeded.ToString() == qs)
                {
                    return qualityBits.uncertainEUExceeded;
                }
                if (qualityBits.uncertainSubNormal.ToString() == qs)
                {
                    return qualityBits.uncertainSubNormal;
                }
            }
            return qualityBits.bad;
        }
    }
}

