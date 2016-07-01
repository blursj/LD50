namespace OPCDA
{
    using System;

    public enum qualityBits
    {
        bad = 0,
        badCommFailure = 0x18,
        badConfigurationError = 4,
        badDeviceFailure = 12,
        badLastKnownValue = 20,
        badNotConnected = 8,
        badOutOfService = 0x1c,
        badSensorFailure = 0x10,
        badWaitingForInitialData = 0x20,
        good = 0xc0,
        goodLocalOverride = 0xd8,
        uncertain = 0x40,
        uncertainEUExceeded = 0x54,
        uncertainLastUsableValue = 0x44,
        uncertainSensorNotAccurate = 80,
        uncertainSubNormal = 0x58
    }
}

