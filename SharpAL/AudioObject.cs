using System;
using SharpAL.OpenAL;

namespace SharpAL {
    public abstract class AudioObject : DisposeBase {

        public static string Version => AL.Get(ALGetString.Version);

        public static string Vendor => AL.Get(ALGetString.Vendor);

        public static string Renderer => AL.Get(ALGetString.Renderer);

        public static string[] GetExtensions() {
            var str = AL.Get(ALGetString.Extensions);

            if (str == null) {
                return EmptyStrings;
            }

            return str.Split(ExtensionSeparator, StringSplitOptions.RemoveEmptyEntries);
        }

        private static readonly string[] EmptyStrings = new string[0];
        private static readonly char[] ExtensionSeparator = {' '};

    }
}
