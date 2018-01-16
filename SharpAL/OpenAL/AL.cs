using System;
using System.Runtime.InteropServices;
using System.Security;
using JetBrains.Annotations;

namespace SharpAL.OpenAL {
    // ReSharper disable once InconsistentNaming
    public static class AL {

        [DllImport(LibraryName, EntryPoint = "alEnable", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        public static extern void Enable(ALCapability capability);

        [DllImport(LibraryName, EntryPoint = "alDisable", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        public static extern void Disable(ALCapability capability);

        [DllImport(LibraryName, EntryPoint = "alIsEnabled", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        public static extern bool IsEnabled(ALCapability capability);

        [DllImport(LibraryName, EntryPoint = "alGetString", ExactSpelling = true, CallingConvention = DefaultCallingConvention, CharSet = CharSet.Ansi), SuppressUnmanagedCodeSecurity]
        private static extern IntPtr GetString(ALGetString param);

        [DllImport(LibraryName, EntryPoint = "alGetString", ExactSpelling = true, CallingConvention = DefaultCallingConvention, CharSet = CharSet.Ansi), SuppressUnmanagedCodeSecurity]
        private static extern IntPtr GetString(ALError param);

        [CanBeNull]
        public static string Get(ALGetString param) {
            return Marshal.PtrToStringAnsi(GetString(param));
        }

        [CanBeNull]
        public static string GetErrorString(ALError param) {
            return Marshal.PtrToStringAnsi(GetString(param));
        }

        [DllImport(LibraryName, EntryPoint = "alGetInteger", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        public static extern int Get(ALGetInteger param);

        [DllImport(LibraryName, EntryPoint = "alGetFloat", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        public static extern float Get(ALGetFloat param);

        [DllImport(LibraryName, EntryPoint = "alGetError", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        public static extern ALError GetError();

        [DllImport(LibraryName, EntryPoint = "alIsExtensionPresent", ExactSpelling = true, CallingConvention = DefaultCallingConvention, CharSet = CharSet.Ansi), SuppressUnmanagedCodeSecurity]
        public static extern bool IsExtensionPresent([In, NotNull] string extname);

        [DllImport(LibraryName, EntryPoint = "alGetProcAddress", ExactSpelling = true, CallingConvention = DefaultCallingConvention, CharSet = CharSet.Ansi), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetProcAddress([In, NotNull] string fname);

        [DllImport(LibraryName, EntryPoint = "alGetEnumValue", ExactSpelling = true, CallingConvention = DefaultCallingConvention, CharSet = CharSet.Ansi), SuppressUnmanagedCodeSecurity]
        public static extern int GetEnumValue([In, NotNull] string ename);

        [DllImport(LibraryName, EntryPoint = "alListenerf", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        public static extern void Listener(ALListenerf param, float value);

        [DllImport(LibraryName, EntryPoint = "alListener3f", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        public static extern void Listener(ALListener3f param, float value1, float value2, float value3);

        [DllImport(LibraryName, EntryPoint = "alListenerfv", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void Listener(ALListenerfv param, [NotNull] float* values);

        public static unsafe void Listener(ALListenerfv param, [NotNull] float[] values) {
            fixed (float* ptr = &values[0]) {
                Listener(param, ptr);
            }
        }

        [DllImport(LibraryName, EntryPoint = "alGetListenerf", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        public static extern void GetListener(ALListenerf param, out float value);

        [DllImport(LibraryName, EntryPoint = "alGetListener3f", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        public static extern void GetListener(ALListener3f param, out float value1, out float value2, out float value3);

        [DllImport(LibraryName, EntryPoint = "alGetListenerfv", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void GetListener(ALListenerfv param, [NotNull] float* values);

        public static unsafe void GetListener(ALListenerfv param, [NotNull] float[] values) {
            fixed (float* p = values) {
                GetListener(param, p);
            }
        }

        [DllImport(LibraryName, EntryPoint = "alGenSources", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void GenSources(int n, [NotNull] uint* sources);

        public static unsafe int[] GenSources(int n) {
            var sources = new int[n];
            fixed (int* sourcesPtr = sources) {
                GenSources(sources.Length, (uint*)sourcesPtr);
            }

            return sources;
        }

        public static unsafe int GenSource() {
            int source;
            GenSources(1, (uint*)&source);
            return source;
        }

        [DllImport(LibraryName, EntryPoint = "alDeleteSources", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void DeleteSources(int n, [NotNull] uint* sources);

        public static unsafe void DeleteSources([NotNull] int[] sources) {
            fixed (int* sourcesPtr = sources) {
                DeleteSources(sources.Length, (uint*)sourcesPtr);
            }
        }

        public static unsafe void DeleteSource(int sid) {
            DeleteSources(1, (uint*)&sid);
        }

        [DllImport(LibraryName, EntryPoint = "alIsSource", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern bool IsSource(uint sid);

        public static bool IsSource(int sid) {
            return IsSource(unchecked ((uint)sid));
        }

        [DllImport(LibraryName, EntryPoint = "alSourcef", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void Source(uint sid, ALSourcef param, float value);

        public static void Source(int sid, ALSourcef param, float value) {
            Source(unchecked ((uint)sid), param, value);
        }

        [CLSCompliant(false)]
        [DllImport(LibraryName, EntryPoint = "alSource3f", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void Source(uint sid, ALSource3f param, float value1, float value2, float value3);

        public static void Source(int sid, ALSource3f param, float value1, float value2, float value3) {
            Source(unchecked ((uint)sid), param, value1, value2, value3);
        }

        [DllImport(LibraryName, EntryPoint = "alSourcei", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void Source(uint sid, ALSourcei param, int value);

        public static void Source(int sid, ALSourcei param, int value) {
            Source(unchecked ((uint)sid), param, value);
        }

        public static void Source(int sid, ALSourceb param, bool value) {
            Source(unchecked ((uint)sid), (ALSourcei)param, value ? 1 : 0);
        }

        [DllImport(LibraryName, EntryPoint = "alSource3i", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void Source(uint sid, ALSource3i param, int value1, int value2, int value3);

        public static void Source(int sid, ALSource3i param, int value1, int value2, int value3) {
            Source(unchecked ((uint)sid), param, value1, value2, value3);
        }

        [DllImport(LibraryName, EntryPoint = "alGetSourcef", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void GetSource(uint sid, ALSourcef param, out float value);

        public static void GetSource(int sid, ALSourcef param, out float value) {
            GetSource(unchecked ((uint)sid), param, out value);
        }

        [DllImport(LibraryName, EntryPoint = "alGetSource3f", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void GetSource(uint sid, ALSource3f param, out float value1, out float value2, out float value3);

        public static void GetSource(int sid, ALSource3f param, out float value1, out float value2, out float value3) {
            GetSource(unchecked ((uint)sid), param, out value1, out value2, out value3);
        }

        [DllImport(LibraryName, EntryPoint = "alGetSourcei", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void GetSource(uint sid, ALGetSourcei param, [Out] out int value);

        public static void GetSource(int sid, ALGetSourcei param, out int value) {
            GetSource(unchecked ((uint)sid), param, out value);
        }

        public static void GetSource(int sid, ALSourceb param, out bool value) {
            GetSource(unchecked ((uint)sid), (ALGetSourcei)param, out var result);
            value = result != 0;
        }

        [DllImport(LibraryName, EntryPoint = "alSourcePlayv"), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void SourcePlay(int ns, [NotNull] uint* sids);

        public static unsafe void SourcePlay([NotNull] int[] sids) {
            fixed (int* sidsPtr = sids) {
                SourcePlay(sids.Length, (uint*)sidsPtr);
            }
        }

        [DllImport(LibraryName, EntryPoint = "alSourceStopv"), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void SourceStop(int ns, [NotNull] uint* sids);

        public static unsafe void SourceStop([NotNull] int[] sids) {
            fixed (int* sidsPtr = sids) {
                SourceStop(sids.Length, (uint*)sidsPtr);
            }
        }

        [DllImport(LibraryName, EntryPoint = "alSourceRewindv"), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void SourceRewind(int ns, [NotNull] uint* sids);

        public static unsafe void SourceRewind(int[] sids) {
            fixed (int* sidsPtr = sids) {
                SourceRewind(sids.Length, (uint*)sidsPtr);
            }
        }

        [DllImport(LibraryName, EntryPoint = "alSourcePausev"), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void SourcePause(int ns, [NotNull] uint* sids);

        public static unsafe void SourcePause([NotNull] int[] sids) {
            fixed (int* sidsPtr = sids) {
                SourcePause(sids.Length, (uint*)sidsPtr);
            }
        }

        [DllImport(LibraryName, EntryPoint = "alSourcePlay", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void SourcePlay(uint sid);

        public static void SourcePlay(int sid) {
            SourcePlay(unchecked ((uint)sid));
        }

        [DllImport(LibraryName, EntryPoint = "alSourceStop", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void SourceStop(uint sid);

        public static void SourceStop(int sid) {
            SourceStop(unchecked ((uint)sid));
        }

        [DllImport(LibraryName, EntryPoint = "alSourceRewind", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void SourceRewind(uint sid);

        public static void SourceRewind(int sid) {
            SourceRewind(unchecked ((uint)sid));
        }

        [DllImport(LibraryName, EntryPoint = "alSourcePause", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void SourcePause(uint sid);

        public static void SourcePause(int sid) {
            SourcePause(unchecked ((uint)sid));
        }

        [DllImport(LibraryName, EntryPoint = "alSourceQueueBuffers"), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void SourceQueueBuffers(uint sid, int numEntries, [NotNull] uint* bids);


        public static unsafe void SourceQueueBuffers(int sid, [NotNull] int[] bids) {
            fixed (int* bidsPtr = bids) {
                SourceQueueBuffers(unchecked ((uint)sid), bids.Length, (uint*)bidsPtr);
            }
        }

        public static unsafe void SourceQueueBuffer(int source, int buffer) {
            SourceQueueBuffers(unchecked ((uint)source), 1, (uint*)&buffer);
        }

        [DllImport(LibraryName, EntryPoint = "alSourceUnqueueBuffers"), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void SourceUnqueueBuffers(uint sid, int numEntries, [In] uint* bids);

        public static unsafe void SourceUnqueueBuffers(int sid, [NotNull] int[] bids) {
            fixed (int* bidsPtr = bids) {
                SourceUnqueueBuffers(unchecked ((uint)sid), bids.Length, (uint*)bidsPtr);
            }
        }

        public static int SourceUnqueueBuffer(int sid) {
            int buf;
            unsafe {
                SourceUnqueueBuffers(unchecked ((uint)sid), 1, (uint*)&buf);
            }

            return buf;
        }

        public static int[] SourceUnqueueBuffers(int sid, int numEntries) {
            if (numEntries < 0) {
                throw new ArgumentOutOfRangeException(nameof(numEntries), numEntries, null);
            }

            var result = new int[numEntries];
            SourceUnqueueBuffers(sid, result);

            return result;
        }

        [DllImport(LibraryName, EntryPoint = "alGenBuffers", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void GenBuffers(int n, [NotNull] uint* buffers);

        public static unsafe int[] GenBuffers(int n) {
            var buffers = new int[n];

            fixed (int* buffersPtr = buffers) {
                GenBuffers(n, (uint*)buffersPtr);
            }

            return buffers;
        }

        public static unsafe int GenBuffer() {
            int buffer;

            GenBuffers(1, (uint*)&buffer);

            return buffer;
        }

        [DllImport(LibraryName, EntryPoint = "alDeleteBuffers", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void DeleteBuffers(int n, [In] uint* buffers);

        public static unsafe void DeleteBuffers([NotNull] int[] buffers) {
            fixed (int* buffersPtr = buffers) {
                DeleteBuffers(buffers.Length, (uint*)buffersPtr);
            }
        }

        public static unsafe void DeleteBuffer(int buffer) {
            DeleteBuffers(1, (uint*)&buffer);
        }

        [DllImport(LibraryName, EntryPoint = "alIsBuffer", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern bool IsBuffer(uint bid);

        public static bool IsBuffer(int bid) {
            return IsBuffer(unchecked ((uint)bid));
        }

        [DllImport(LibraryName, EntryPoint = "alBufferData", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void BufferData(uint bid, ALFormat format, IntPtr buffer, int size, int freq);

        [DllImport(LibraryName, EntryPoint = "alBufferData", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void BufferData(uint bid, ALFormat format, byte* buffer, int size, int freq);

        public static void BufferData(int bid, ALFormat format, IntPtr buffer, int size, int freq) {
            BufferData(unchecked ((uint)bid), format, buffer, size, freq);
        }

        public static void BufferData<TBuffer>(int bid, ALFormat format, [NotNull] TBuffer[] buffer, int freq)
            where TBuffer : struct {
            if (buffer.Length == 0) {
                return;
            }

            var structSize = Marshal.SizeOf(typeof(TBuffer));
            var bufferSizeInBytes = structSize * buffer.Length;

            var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

            try {
                BufferData(unchecked ((uint)bid), format, handle.AddrOfPinnedObject(), bufferSizeInBytes, freq);
            } finally {
                handle.Free();
            }
        }

        public static unsafe void BufferData(int bid, ALFormat format, [NotNull] byte[] buffer, int freq) {
            if (buffer.Length == 0) {
                return;
            }

            fixed (byte* bufferPtr = buffer) {
                BufferData(unchecked ((uint)bid), format, bufferPtr, buffer.Length, freq);
            }
        }

        public static unsafe void BufferData(int bid, ALFormat format, [NotNull] byte[] buffer, int startOffset, int count, int freq) {
            if (buffer.Length == 0) {
                return;
            }

            if (startOffset < 0) {
                startOffset = 0;
            }

            if (startOffset + count > buffer.Length) {
                count = buffer.Length - startOffset;
            }

            fixed (byte* bufferPtr = buffer) {
                BufferData(unchecked ((uint)bid), format, bufferPtr + startOffset, count, freq);
            }
        }

        [DllImport(LibraryName, EntryPoint = "alGetBufferi", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern void GetBuffer(uint bid, ALGetBufferi param, out int value);

        public static void GetBuffer(int bid, ALGetBufferi param, out int value) {
            GetBuffer(unchecked ((uint)bid), param, out value);
        }

        [DllImport(LibraryName, EntryPoint = "alDopplerFactor", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        public static extern void DopplerFactor(float value);

        [DllImport(LibraryName, EntryPoint = "alDopplerVelocity", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        public static extern void DopplerVelocity(float value);

        [DllImport(LibraryName, EntryPoint = "alSpeedOfSound", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        public static extern void SpeedOfSound(float value);

        [DllImport(LibraryName, EntryPoint = "alDistanceModel", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        public static extern void DistanceModel(ALDistanceModel distancemodel);

        private const string LibraryName = "openal32.dll";
        private const CallingConvention DefaultCallingConvention = CallingConvention.Cdecl;

    }
}
