using JetBrains.Annotations;
using SharpAL.OpenAL;

namespace SharpAL.Extensions {
    public static class AudioSourceExtensions {

        public static void Bind([NotNull] this AudioSource source, [CanBeNull] AudioBuffer buffer) {
            if (buffer == null) {
                AL.Source(source.NativeSource, ALSourcei.Buffer, 0);
            } else {
                AL.Source(source.NativeSource, ALSourcei.Buffer, buffer.NativeBuffer);
            }
        }

    }
}
