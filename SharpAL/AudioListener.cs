using SharpAL.OpenAL;

namespace SharpAL {
    public static class AudioListener {

        public static float Volume {
            get {
                AL.GetListener(ALListenerf.Gain, out var value);
                return value;
            }
            set => AL.Listener(ALListenerf.Gain, value);
        }

        public static (float X, float Y, float Z) GetPosition() {
            AL.GetListener(ALListener3f.Position, out var x, out var y, out var z);
            return (x, y, z);
        }

        public static void SetPosition(float x, float y, float z) {
            AL.Listener(ALListener3f.Position, x, y, z);
        }

        public static (float X, float Y, float Z) GetVelocity() {
            AL.GetListener(ALListener3f.Velocity, out var x, out var y, out var z);
            return (x, y, z);
        }

        public static void SetVelocity(float x, float y, float z) {
            AL.Listener(ALListener3f.Velocity, x, y, z);
        }

        public static (float AtX, float AtY, float AtZ, float UpX, float UpY, float UpZ) GetOrientation() {
            var values = new float[6];
            AL.GetListener(ALListenerfv.Orientation, values);
            return (values[0], values[1], values[2], values[3], values[4], values[5]);
        }

        public static void SetOrientation(float atX, float atY, float atZ, float upX, float upY, float upZ) {
            var values = new[] {
                atX, atY, atZ, upX, upY, upZ
            };
            AL.Listener(ALListenerfv.Orientation, values);
        }

    }
}
