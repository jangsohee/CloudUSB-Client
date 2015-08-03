using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32.SafeHandles;
using System.ComponentModel;

namespace ContentManager
{
    public class WinAPI
    {
        internal const int
        GENERIC_READ = unchecked((int)0x80000000),
        FILE_FLAG_BACKUP_SEMANTICS = unchecked((int)0x02000000),
        OPEN_EXISTING = unchecked((int)3);

        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern IntPtr NtQueryInformationFile(IntPtr fileHandle, ref IO_STATUS_BLOCK IoStatusBlock, IntPtr pInfoBlock, uint length, FILE_INFORMATION_CLASS fileInformation);

        public struct IO_STATUS_BLOCK
        {
            uint status;
            ulong information;
        }
        public struct _FILE_INTERNAL_INFORMATION
        {
            public ulong IndexNumber;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FILE_OBJECTID_BUFFER
        {
            public struct Union
            {
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
                public byte[] BirthVolumeId;

                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
                public byte[] BirthObjectId;

                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
                public byte[] DomainId;
            }

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] ObjectId;

            public Union BirthInfo;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
            public byte[] ExtendedInfo;
        }

        // Abbreviated, there are more values than shown
        public enum FILE_INFORMATION_CLASS
        {
            FileDirectoryInformation = 1,     // 1
            FileFullDirectoryInformation,     // 2
            FileBothDirectoryInformation,     // 3
            FileBasicInformation,         // 4
            FileStandardInformation,      // 5
            FileInternalInformation      // 6
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool DeviceIoControl(
            SafeFileHandle hDevice,
            uint dwIoControlCode,
            IntPtr lpInBuffer,
            uint nInBufferSize,
            [Out] IntPtr lpOutBuffer,
            int nOutBufferSize,
            ref uint lpBytesReturned,
            IntPtr lpOverlapped
            );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern SafeFileHandle CreateFile(
            String fileName,
            int dwDesiredAccess,
            System.IO.FileShare dwShareMode,
            IntPtr securityAttrs_MustBeZero,
            System.IO.FileMode dwCreationDisposition,
            int dwFlagsAndAttributes,
            IntPtr hTemplateFile_MustBeZero
            );


        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetFileInformationByHandle(IntPtr hFile, out BY_HANDLE_FILE_INFORMATION lpFileInformation);

        public struct BY_HANDLE_FILE_INFORMATION
        {
            public uint FileAttributes;
            public FILETIME CreationTime;
            public FILETIME LastAccessTime;
            public FILETIME LastWriteTime;
            public uint VolumeSerialNumber;
            public uint FileSizeHigh;
            public uint FileSizeLow;
            public uint NumberOfLinks;
            public uint FileIndexHigh;
            public uint FileIndexLow;
        }
    }

    public class FileManager
    {
        const uint FSCTL_GET_OBJECT_ID = 0x0009009c;

        public static String GetFileId(String path)
        {
            using (var fs = File.Open(
                path,
                FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)
                )
            {
                WinAPI.BY_HANDLE_FILE_INFORMATION info;
                WinAPI.GetFileInformationByHandle(fs.Handle, out info);
                return String.Format(
                        "{0:x}", ((info.FileIndexHigh << 32) | info.FileIndexLow));
            }
        }

        public static WinAPI.FILE_OBJECTID_BUFFER GetFolderIdBuffer(String path)
        {
            using (var hFile = WinAPI.CreateFile(
                path,
                WinAPI.GENERIC_READ, FileShare.Read,
                IntPtr.Zero,
                (FileMode)WinAPI.OPEN_EXISTING,
                WinAPI.FILE_FLAG_BACKUP_SEMANTICS,
                IntPtr.Zero
                ))
            {
                if (null == hFile || hFile.IsInvalid) { }
                    //throw new Win32Exception(Marshal.GetLastWin32Error());

                var buffer = default(WinAPI.FILE_OBJECTID_BUFFER);
                var nOutBufferSize = Marshal.SizeOf(buffer);
                var lpOutBuffer = Marshal.AllocHGlobal(nOutBufferSize);
                var lpBytesReturned = default(uint);

                var result =
                    WinAPI.DeviceIoControl(
                        hFile, FSCTL_GET_OBJECT_ID,
                        IntPtr.Zero, 0,
                        lpOutBuffer, nOutBufferSize,
                        ref lpBytesReturned, IntPtr.Zero
                        );

                if (!result)
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                var type = typeof(WinAPI.FILE_OBJECTID_BUFFER);

                buffer = (WinAPI.FILE_OBJECTID_BUFFER)
                    Marshal.PtrToStructure(lpOutBuffer, type);

                Marshal.FreeHGlobal(lpOutBuffer);

                return buffer;
            }
        }

        public static string GetFolderID(string path)
        {
            var buffer = FileManager.GetFolderIdBuffer(path);

            var objectId = buffer.ObjectId
                .Reverse()
                .Select(x => x.ToString("x2"))
                .Aggregate(String.Concat);

            return objectId;
        }

        public static ulong GetFileIDA(string path)
        {
            WinAPI.IO_STATUS_BLOCK iostatus = new WinAPI.IO_STATUS_BLOCK();
            IntPtr memPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(WinAPI._FILE_INTERNAL_INFORMATION)));
            WinAPI._FILE_INTERNAL_INFORMATION objectIDInfo = new WinAPI._FILE_INTERNAL_INFORMATION();

            int structSize = Marshal.SizeOf(objectIDInfo);

            FileInfo fi = new FileInfo(path);
            FileStream fs = fi.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            IntPtr res = WinAPI.NtQueryInformationFile(fs.Handle, ref iostatus, memPtr, (uint)structSize, WinAPI.FILE_INFORMATION_CLASS.FileInternalInformation);

            objectIDInfo = (WinAPI._FILE_INTERNAL_INFORMATION)Marshal.PtrToStructure(memPtr, typeof(WinAPI._FILE_INTERNAL_INFORMATION));

            fs.Close();

            Marshal.FreeHGlobal(memPtr);

            return objectIDInfo.IndexNumber;

        }

        public static ulong GetFileIDB(string path)
        {
            WinAPI.BY_HANDLE_FILE_INFORMATION objectFileInfo = new WinAPI.BY_HANDLE_FILE_INFORMATION();

            FileInfo fi = new FileInfo(path);
            FileStream fs = fi.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            WinAPI.GetFileInformationByHandle(fs.Handle, out objectFileInfo);

            fs.Close();

            ulong fileIndex = ((ulong)objectFileInfo.FileIndexHigh << 32) + (ulong)objectFileInfo.FileIndexLow;
             
            return fileIndex;
        }
    }
}
