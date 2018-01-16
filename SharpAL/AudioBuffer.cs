using JetBrains.Annotations;
using SharpAL.OpenAL;

namespace SharpAL {
    public sealed class AudioBuffer : AudioObject {

        public AudioBuffer([NotNull] AudioContext context) {
            Context = context;
            Alc.MakeContextCurrent(context.NativeContext);
            _buffer = AL.GenBuffer();
        }

        public AudioContext Context { get; }

        /// <summary>
        /// Loads audio data to this buffer. The data must be stereo, 16-bit unsigned integer.
        /// </summary>
        /// <param name="data">Data bytes.</param>
        /// <param name="sampleRate">Sample rate.</param>
        public void BufferData([NotNull] byte[] data, int sampleRate) {
            AL.BufferData(NativeBuffer, ALFormat.Stereo16, data, sampleRate);
        }

        public void BufferData([NotNull] byte[] data, ALFormat format, int sampleRate) {
            AL.BufferData(NativeBuffer, format, data, sampleRate);
        }

        internal int NativeBuffer => _buffer;

        protected override void Dispose(bool disposing) {
            AL.DeleteBuffer(_buffer);
            _buffer = 0;
        }

        private int _buffer;

    }
}
