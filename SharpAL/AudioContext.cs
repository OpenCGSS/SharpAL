using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using SharpAL.OpenAL;

namespace SharpAL {
    public sealed partial class AudioContext : AudioObject {

        public AudioContext([NotNull] AudioDevice device, [CanBeNull] int[] attribList) {
            _context = Alc.CreateContext(device.NativeDevice, attribList);
            Device = device;
            _watchThread = new WatchThread(this);
            _watchThread.Start();
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

        internal void ManageSource([NotNull] AudioSource audioSource) {
            _createdSources.Add(audioSource);
        }

        internal void UnmanageSource([NotNull] AudioSource audioSource) {
            _createdSources.Remove(audioSource);
        }

        protected override void Dispose(bool disposing) {
            _watchThread.Terminate();

            if (_context != IntPtr.Zero) {
                var currentContext = Alc.GetCurrentContext();
                if (currentContext == _context) {
                    Alc.MakeContextCurrent(IntPtr.Zero);
                }

                Alc.DestroyContext(_context);
            }

            _context = IntPtr.Zero;
        }

        private readonly HashSet<AudioSource> _createdSources = new HashSet<AudioSource>();

        private IntPtr _context;
        private readonly WatchThread _watchThread;

    }
}
