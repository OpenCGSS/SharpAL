namespace SharpAL.OpenAL {
    // ReSharper disable once InconsistentNaming
    public enum ALSourcef {

        ReferenceDistance = 0x1020,
        MaxDistance = 0x1023,
        RolloffFactor = 0x1021,
        Pitch = 0x1003,
        Gain = 0x100A,
        MinGain = 0x100D,
        MaxGain = 0x100E,
        ConeInnerAngle = 0x1001,
        ConeOuterAngle = 0x1002,
        ConeOuterGain = 0x1022,
        SecOffset = 0x1024, // AL_EXT_OFFSET extension.
        EfxAirAbsorptionFactor = 0x20007,
        EfxRoomRolloffFactor = 0x20008,
        EfxConeOuterGainHighFrequency = 0x20009,

    }
}
