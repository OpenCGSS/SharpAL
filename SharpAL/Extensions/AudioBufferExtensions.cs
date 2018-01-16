using JetBrains.Annotations;

namespace SharpAL.Extensions {
    public static class AudioBufferExtensions {

        public static void BindTo([CanBeNull] this AudioBuffer buffer, [NotNull] AudioSource source) {
            AudioSourceExtensions.Bind(source, buffer);
        }

    }
}
