using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using JetBrains.Annotations;

namespace SharpAL.OpenAL {
    public static class Alc {

        [DllImport(LibraryName, EntryPoint = "alcCreateContext", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        private static extern unsafe IntPtr CreateContext([In] IntPtr device, [In, CanBeNull] int* attrlist);

        public static unsafe IntPtr CreateContext(IntPtr device, [CanBeNull] int[] attribList) {
            fixed (int* attribListPtr = attribList) {
                return CreateContext(device, attribListPtr);
            }
        }

        public static unsafe IntPtr CreateContext(IntPtr device, [CanBeNull] AlcContextAttributes[] attribList) {
            fixed (AlcContextAttributes* attribListPtr = attribList) {
                return CreateContext(device, (int*)attribListPtr);
            }
        }

        [DllImport(LibraryName, EntryPoint = "alcMakeContextCurrent", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        public static extern bool MakeContextCurrent(IntPtr context);

        [DllImport(LibraryName, EntryPoint = "alcProcessContext", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        public static extern void ProcessContext(IntPtr context);

        [DllImport(LibraryName, EntryPoint = "alcSuspendContext", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        public static extern void SuspendContext(IntPtr context);

        [DllImport(LibraryName, EntryPoint = "alcDestroyContext", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        public static extern void DestroyContext(IntPtr context);

        [DllImport(LibraryName, EntryPoint = "alcGetCurrentContext", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetCurrentContext();

        [DllImport(LibraryName, EntryPoint = "alcGetContextsDevice", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetContextsDevice(IntPtr context);

        [DllImport(LibraryName, EntryPoint = "alcOpenDevice", ExactSpelling = true, CallingConvention = DefaultCallingConvention, CharSet = CharSet.Ansi), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr OpenDevice([In, CanBeNull] string devicename);

        [DllImport(LibraryName, EntryPoint = "alcCloseDevice", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        public static extern bool CloseDevice([In] IntPtr device);

        [DllImport(LibraryName, EntryPoint = "alcGetError", ExactSpelling = true, CallingConvention = DefaultCallingConvention), SuppressUnmanagedCodeSecurity]
        public static extern AlcError GetError([In] IntPtr device);

        [DllImport(LibraryName, EntryPoint = "alcIsExtensionPresent", ExactSpelling = true, CallingConvention = DefaultCallingConvention, CharSet = CharSet.Ansi), SuppressUnmanagedCodeSecurity]
        public static extern bool IsExtensionPresent([In] IntPtr device, [In, NotNull] string extensionName);

        [DllImport(LibraryName, EntryPoint = "alcGetProcAddress", ExactSpelling = true, CallingConvention = DefaultCallingConvention, CharSet = CharSet.Ansi), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetProcAddress([In] IntPtr device, [In, NotNull] string funcname);

        [DllImport(LibraryName, EntryPoint = "alcGetEnumValue", ExactSpelling = true, CallingConvention = DefaultCallingConvention, CharSet = CharSet.Ansi), SuppressUnmanagedCodeSecurity]
        public static extern int GetEnumValue([In] IntPtr device, [In, NotNull] string enumname);

        [DllImport(LibraryName, EntryPoint = "alcGetString", ExactSpelling = true, CallingConvention = DefaultCallingConvention, CharSet = CharSet.Ansi), SuppressUnmanagedCodeSecurity]
        private static extern IntPtr GetStringPrivate([In] IntPtr device, [In] AlcGetString param);

        [NotNull]
        public static string GetString(IntPtr device, AlcGetString param) {
            var pstr = GetStringPrivate(device, param);
            string str;

            if (pstr != IntPtr.Zero) {
                str = Marshal.PtrToStringAnsi(pstr);
            } else {
                str = string.Empty;
            }

            Debug.Assert(str != null, nameof(str) + " != null");

            return str;
        }

        [NotNull, ItemNotNull]
        public static string[] GetString(IntPtr device, AlcGetStringList param) {
            var strPtr = GetStringPrivate(device, (AlcGetString)param);

            if (strPtr == IntPtr.Zero) {
                return EmptyStringArray;
            }

            var result = new List<string>();
            var sb = new StringBuilder();
            var offset = 0;

            while (true) {
                var b = Marshal.ReadByte(strPtr, offset++);
                if (b != '\0') {
                    sb.Append((char)b);
                } else {
                    result.Add(sb.ToString());

                    if (Marshal.ReadByte(strPtr, offset) == 0) {
                        break;
                    }

                    sb.Clear();
                }
            }

            return result.ToArray();
        }

        [DllImport(LibraryName, EntryPoint = "alcGetIntegerv", ExactSpelling = true, CallingConvention = DefaultCallingConvention, CharSet = CharSet.Ansi), SuppressUnmanagedCodeSecurity]
        private static extern unsafe void GetInteger(IntPtr device, AlcGetInteger param, int size, int* data);

        public static unsafe void GetInteger(IntPtr device, AlcGetInteger param, int size, int[] data) {
            fixed (int* dataPtr = data) {
                GetInteger(device, param, size, dataPtr);
            }
        }

        private const string LibraryName = "openal32.dll";
        private const CallingConvention DefaultCallingConvention = CallingConvention.Cdecl;

        private static readonly string[] EmptyStringArray = new string[0];

    }
}
