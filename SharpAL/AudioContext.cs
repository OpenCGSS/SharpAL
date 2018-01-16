using System;
using JetBrains.Annotations;
using SharpAL.OpenAL;

namespace SharpAL {
    public sealed class AudioContext : AudioObject {

        public AudioContext([NotNull] AudioDevice device, [CanBeNull] int[] attribList) {
            _context = Alc.CreateContext(device.NativeDevice, attribList);
            Device = device;
        }

        public AudioContext([NotNull] AudioDevice device, [CanBeNull] AlcContextAttributes[] attribList) {
            _context = Alc.CreateContext(device.NativeDevice, attribList);
            Device = device;
        }

        public AudioContext([NotNull] AudioDevice device)
            : this(device, (int[])null) {
        }

        public AudioDevice Device { get; }

        public bool MakeCurrent() {
            return Alc.MakeContextCurrent(NativeContext);
        }

        public static bool Reset() {
            return Alc.MakeContextCurrent(IntPtr.Zero);
        }

        internal IntPtr NativeContext => _context;

        protected override void Dispose(bool disposing) {
            if (_context != IntPtr.Zero) {
                var currentContext = Alc.GetCurrentContext();
                if (currentContext == _context) {
                    Alc.MakeContextCurrent(IntPtr.Zero);
                }

                Alc.DestroyContext(_context);
            }

            _context = IntPtr.Zero;
        }

        private IntPtr _context;

    }
}
