﻿using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;

namespace MiniMusic.WinFormsInstaller
{
    //[Designer("System.Windows.Forms.Design.FolderBrowserDialogDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"), DefaultEvent("HelpRequest"), SRDescription("DescriptionFolderBrowserDialog"), DefaultProperty("SelectedPath")]
    public class FolderBrowserDialogEx : CommonDialog
    {
        private static readonly int MAX_PATH = 260;

        // Fields
        private PInvoke.BrowseFolderCallbackProc _callback;

        private string _descriptionText;
        private IntPtr _hwndEdit;
        private Environment.SpecialFolder _rootFolder;
        private IntPtr _rootFolderLocation;
        private string _selectedPath;
        private bool _selectedPathNeedsCheck;
        private int _uiFlags;

        // ctor
        public FolderBrowserDialogEx()
        {
            Reset();
        }

        // Properties
        //[SRDescription("FolderBrowserDialogDescription"), SRCategory("CatFolderBrowsing"), Browsable(true), DefaultValue(""), Localizable(true)]

        /// <summary>
        ///     This description appears near the top of the dialog box, providing direction to the user.
        /// </summary>
        public string Description
        {
            get => _descriptionText;
            set => _descriptionText = value == null ? string.Empty : value;
        }

        //[Localizable(false), SRCategory("CatFolderBrowsing"), SRDescription("FolderBrowserDialogRootFolder"), TypeConverter(typeof(SpecialFolderEnumConverter)), Browsable(true), DefaultValue(0)]
        public Environment.SpecialFolder RootFolder
        {
            get => _rootFolder;
            set
            {
                if (!Enum.IsDefined(typeof(Environment.SpecialFolder), value))
                    throw new InvalidEnumArgumentException("value", (int) value, typeof(Environment.SpecialFolder));
                _rootFolder = value;
            }
        }

        //[Browsable(true), SRDescription("FolderBrowserDialogSelectedPath"), SRCategory("CatFolderBrowsing"), DefaultValue(""), Editor("System.Windows.Forms.Design.SelectedPathEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor)), Localizable(true)]

        /// <summary>
        ///     Set or get the selected path.
        /// </summary>
        public string SelectedPath
        {
            get
            {
                if (_selectedPath != null && _selectedPath.Length != 0 && _selectedPathNeedsCheck)
                {
                    new FileIOPermission(FileIOPermissionAccess.PathDiscovery, _selectedPath).Demand();
                    _selectedPathNeedsCheck = false;
                }
                return _selectedPath;
            }
            set
            {
                _selectedPath = value == null ? string.Empty : value;
                _selectedPathNeedsCheck = true;
            }
        }

        //[SRDescription("FolderBrowserDialogShowNewFolderButton"), Localizable(false), Browsable(true), DefaultValue(true), SRCategory("CatFolderBrowsing")]

        /// <summary>
        ///     Enable or disable the "New Folder" button in the browser dialog.
        /// </summary>
        public bool ShowNewFolderButton { get; set; }

        /// <summary>
        ///     Show an "edit box" in the folder browser.
        /// </summary>
        /// <remarks>
        ///     The "edit box" normally shows the name of the selected folder.
        ///     The user may also type a pathname directly into the edit box.
        /// </remarks>
        /// <seealso cref="ShowFullPathInEditBox" />
        public bool ShowEditBox { get; set; }

        /// <summary>
        ///     Set whether to use the New Folder Browser dialog style.
        /// </summary>
        /// <remarks>
        ///     The new style is resizable and includes a "New Folder" button.
        /// </remarks>
        public bool NewStyle { get; set; } = true;


        public bool DontIncludeNetworkFoldersBelowDomainLevel { get; set; }

        /// <summary>
        ///     Show the full path in the edit box as the user selects it.
        /// </summary>
        /// <remarks>
        ///     This works only if ShowEditBox is also set to true.
        /// </remarks>
        public bool ShowFullPathInEditBox { get; set; } = true;

        public bool ShowBothFilesAndFolders { get; set; }

        // Events
        //[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler HelpRequest
        {
            add => base.HelpRequest += value;
            remove => base.HelpRequest -= value;
        }

        // Factory Methods
        public static FolderBrowserDialogEx PrinterBrowser()
        {
            var x = new FolderBrowserDialogEx();
            // avoid MBRO comppiler warning when passing _rootFolderLocation as a ref:
            x.BecomePrinterBrowser();
            return x;
        }

        public static FolderBrowserDialogEx ComputerBrowser()
        {
            var x = new FolderBrowserDialogEx();
            // avoid MBRO comppiler warning when passing _rootFolderLocation as a ref:
            x.BecomeComputerBrowser();
            return x;
        }


        // Helpers
        private void BecomePrinterBrowser()
        {
            _uiFlags += BrowseFlags.BIF_BROWSEFORPRINTER;
            Description = "Select a printer:";
            PInvoke.Shell32.SHGetSpecialFolderLocation(IntPtr.Zero, CSIDL.PRINTERS, ref _rootFolderLocation);
            ShowNewFolderButton = false;
            ShowEditBox = false;
        }

        private void BecomeComputerBrowser()
        {
            _uiFlags += BrowseFlags.BIF_BROWSEFORCOMPUTER;
            Description = "Select a computer:";
            PInvoke.Shell32.SHGetSpecialFolderLocation(IntPtr.Zero, CSIDL.NETWORK, ref _rootFolderLocation);
            ShowNewFolderButton = false;
            ShowEditBox = false;
        }

        private int FolderBrowserCallback(IntPtr hwnd, int msg, IntPtr lParam, IntPtr lpData)
        {
            switch (msg)
            {
                case BrowseForFolderMessages.BFFM_INITIALIZED:
                    if (_selectedPath.Length != 0)
                    {
                        PInvoke.User32.SendMessage(new HandleRef(null, hwnd),
                            BrowseForFolderMessages.BFFM_SETSELECTIONW, 1, _selectedPath);
                        if (ShowEditBox && ShowFullPathInEditBox)
                        {
                            // get handle to the Edit box inside the Folder Browser Dialog
                            _hwndEdit = PInvoke.User32.FindWindowEx(new HandleRef(null, hwnd), IntPtr.Zero, "Edit",
                                null);
                            PInvoke.User32.SetWindowText(_hwndEdit, _selectedPath);
                        }
                    }
                    break;

                case BrowseForFolderMessages.BFFM_SELCHANGED:
                    var pidl = lParam;
                    if (pidl != IntPtr.Zero)
                        if ((_uiFlags & BrowseFlags.BIF_BROWSEFORPRINTER) == BrowseFlags.BIF_BROWSEFORPRINTER ||
                            (_uiFlags & BrowseFlags.BIF_BROWSEFORCOMPUTER) == BrowseFlags.BIF_BROWSEFORCOMPUTER)
                        {
                            // we're browsing for a printer or computer, enable the OK button unconditionally.
                            PInvoke.User32.SendMessage(new HandleRef(null, hwnd), BrowseForFolderMessages.BFFM_ENABLEOK,
                                0, 1);
                        }
                        else
                        {
                            var pszPath = Marshal.AllocHGlobal(MAX_PATH * Marshal.SystemDefaultCharSize);
                            var haveValidPath = PInvoke.Shell32.SHGetPathFromIDList(pidl, pszPath);
                            var displayedPath = Marshal.PtrToStringAuto(pszPath);
                            Marshal.FreeHGlobal(pszPath);
                            // whether to enable the OK button or not. (if file is valid)
                            PInvoke.User32.SendMessage(new HandleRef(null, hwnd), BrowseForFolderMessages.BFFM_ENABLEOK,
                                0, haveValidPath ? 1 : 0);

                            // Maybe set the Edit Box text to the Full Folder path
                            if (haveValidPath && !string.IsNullOrEmpty(displayedPath))
                            {
                                if (ShowEditBox && ShowFullPathInEditBox)
                                    if (_hwndEdit != IntPtr.Zero)
                                        PInvoke.User32.SetWindowText(_hwndEdit, displayedPath);

                                if ((_uiFlags & BrowseFlags.BIF_STATUSTEXT) == BrowseFlags.BIF_STATUSTEXT)
                                    PInvoke.User32.SendMessage(new HandleRef(null, hwnd),
                                        BrowseForFolderMessages.BFFM_SETSTATUSTEXT, 0, displayedPath);
                            }
                        }
                    break;
            }
            return 0;
        }

        private static PInvoke.IMalloc GetSHMalloc()
        {
            var ppMalloc = new PInvoke.IMalloc[1];
            PInvoke.Shell32.SHGetMalloc(ppMalloc);
            return ppMalloc[0];
        }

        public override void Reset()
        {
            _rootFolder = 0;
            _descriptionText = string.Empty;
            _selectedPath = string.Empty;
            _selectedPathNeedsCheck = false;
            ShowNewFolderButton = true;
            ShowEditBox = true;
            NewStyle = true;
            DontIncludeNetworkFoldersBelowDomainLevel = false;
            _hwndEdit = IntPtr.Zero;
            _rootFolderLocation = IntPtr.Zero;
        }

        protected override bool RunDialog(IntPtr hWndOwner)
        {
            var result = false;
            if (_rootFolderLocation == IntPtr.Zero)
            {
                PInvoke.Shell32.SHGetSpecialFolderLocation(hWndOwner, (int) _rootFolder, ref _rootFolderLocation);
                if (_rootFolderLocation == IntPtr.Zero)
                {
                    PInvoke.Shell32.SHGetSpecialFolderLocation(hWndOwner, 0, ref _rootFolderLocation);
                    if (_rootFolderLocation == IntPtr.Zero)
                        throw new InvalidOperationException("FolderBrowserDialogNoRootFolder");
                }
            }
            _hwndEdit = IntPtr.Zero;
            //_uiFlags = 0;
            if (DontIncludeNetworkFoldersBelowDomainLevel)
                _uiFlags += BrowseFlags.BIF_DONTGOBELOWDOMAIN;
            if (NewStyle)
                _uiFlags += BrowseFlags.BIF_NEWDIALOGSTYLE;
            if (!ShowNewFolderButton)
                _uiFlags += BrowseFlags.BIF_NONEWFOLDERBUTTON;
            if (ShowEditBox)
                _uiFlags += BrowseFlags.BIF_EDITBOX;
            if (ShowBothFilesAndFolders)
                _uiFlags += BrowseFlags.BIF_BROWSEINCLUDEFILES;


            if (Control.CheckForIllegalCrossThreadCalls && Application.OleRequired() != ApartmentState.STA)
                throw new ThreadStateException("DebuggingException: ThreadMustBeSTA");
            var pidl = IntPtr.Zero;
            var hglobal = IntPtr.Zero;
            var pszPath = IntPtr.Zero;
            try
            {
                var browseInfo = new PInvoke.BROWSEINFO();
                hglobal = Marshal.AllocHGlobal(MAX_PATH * Marshal.SystemDefaultCharSize);
                pszPath = Marshal.AllocHGlobal(MAX_PATH * Marshal.SystemDefaultCharSize);
                _callback = FolderBrowserCallback;
                browseInfo.pidlRoot = _rootFolderLocation;
                browseInfo.Owner = hWndOwner;
                browseInfo.pszDisplayName = hglobal;
                browseInfo.Title = _descriptionText;
                browseInfo.Flags = _uiFlags;
                browseInfo.callback = _callback;
                browseInfo.lParam = IntPtr.Zero;
                browseInfo.iImage = 0;
                pidl = PInvoke.Shell32.SHBrowseForFolder(browseInfo);
                if ((_uiFlags & BrowseFlags.BIF_BROWSEFORPRINTER) == BrowseFlags.BIF_BROWSEFORPRINTER ||
                    (_uiFlags & BrowseFlags.BIF_BROWSEFORCOMPUTER) == BrowseFlags.BIF_BROWSEFORCOMPUTER)
                {
                    _selectedPath = Marshal.PtrToStringAuto(browseInfo.pszDisplayName);
                    result = true;
                }
                else
                {
                    if (pidl != IntPtr.Zero)
                    {
                        PInvoke.Shell32.SHGetPathFromIDList(pidl, pszPath);
                        _selectedPathNeedsCheck = true;
                        _selectedPath = Marshal.PtrToStringAuto(pszPath);
                        result = true;
                    }
                }
            }
            finally
            {
                var sHMalloc = GetSHMalloc();
                sHMalloc.Free(_rootFolderLocation);
                _rootFolderLocation = IntPtr.Zero;
                if (pidl != IntPtr.Zero)
                    sHMalloc.Free(pidl);
                if (pszPath != IntPtr.Zero)
                    Marshal.FreeHGlobal(pszPath);
                if (hglobal != IntPtr.Zero)
                    Marshal.FreeHGlobal(hglobal);
                _callback = null;
            }
            return result;
        }


        private class CSIDL
        {
            public const int PRINTERS = 4;
            public const int NETWORK = 0x12;
        }

        private class BrowseFlags
        {
            public const int BIF_DEFAULT = 0x0000;
            public const int BIF_BROWSEFORCOMPUTER = 0x1000;
            public const int BIF_BROWSEFORPRINTER = 0x2000;
            public const int BIF_BROWSEINCLUDEFILES = 0x4000;
            public const int BIF_BROWSEINCLUDEURLS = 0x0080;
            public const int BIF_DONTGOBELOWDOMAIN = 0x0002;
            public const int BIF_EDITBOX = 0x0010;
            public const int BIF_NEWDIALOGSTYLE = 0x0040;
            public const int BIF_NONEWFOLDERBUTTON = 0x0200;
            public const int BIF_RETURNFSANCESTORS = 0x0008;
            public const int BIF_RETURNONLYFSDIRS = 0x0001;
            public const int BIF_SHAREABLE = 0x8000;
            public const int BIF_STATUSTEXT = 0x0004;
            public const int BIF_UAHINT = 0x0100;
            public const int BIF_VALIDATE = 0x0020;
            public const int BIF_NOTRANSLATETARGETS = 0x0400;
        }

        private static class BrowseForFolderMessages
        {
            // messages FROM the folder browser
            public const int BFFM_INITIALIZED = 1;

            public const int BFFM_SELCHANGED = 2;
            public const int BFFM_VALIDATEFAILEDA = 3;
            public const int BFFM_VALIDATEFAILEDW = 4;
            public const int BFFM_IUNKNOWN = 5;

            // messages TO the folder browser
            public const int BFFM_SETSTATUSTEXT = 0x464;

            public const int BFFM_ENABLEOK = 0x465;
            public const int BFFM_SETSELECTIONA = 0x466;
            public const int BFFM_SETSELECTIONW = 0x467;
        }
    }


    internal static class PInvoke
    {
        public delegate int BrowseFolderCallbackProc(IntPtr hwnd, int msg, IntPtr lParam, IntPtr lpData);

        internal static class User32
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, string lParam);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, int lParam);

            [DllImport("user32.dll", SetLastError = true)]
            //public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
            //public static extern IntPtr FindWindowEx(HandleRef hwndParent, HandleRef hwndChildAfter, string lpszClass, string lpszWindow);
            public static extern IntPtr FindWindowEx(HandleRef hwndParent, IntPtr hwndChildAfter, string lpszClass,
                string lpszWindow);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern bool SetWindowText(IntPtr hWnd, string text);
        }

        [ComImport]
        [Guid("00000002-0000-0000-c000-000000000046")]
        [SuppressUnmanagedCodeSecurity]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IMalloc
        {
            [PreserveSig]
            IntPtr Alloc(int cb);

            [PreserveSig]
            IntPtr Realloc(IntPtr pv, int cb);

            [PreserveSig]
            void Free(IntPtr pv);

            [PreserveSig]
            int GetSize(IntPtr pv);

            [PreserveSig]
            int DidAlloc(IntPtr pv);

            [PreserveSig]
            void HeapMinimize();
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class BROWSEINFO
        {
            public BrowseFolderCallbackProc callback;
            public int Flags;
            public int iImage;
            public IntPtr lParam;
            public IntPtr Owner;
            public IntPtr pidlRoot;
            public IntPtr pszDisplayName;
            public string Title;
        }


        [SuppressUnmanagedCodeSecurity]
        internal static class Shell32
        {
            // Methods
            [DllImport("shell32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr SHBrowseForFolder([In] BROWSEINFO lpbi);

            [DllImport("shell32.dll")]
            public static extern int SHGetMalloc([Out] [MarshalAs(UnmanagedType.LPArray)] IMalloc[] ppMalloc);

            [DllImport("shell32.dll", CharSet = CharSet.Auto)]
            public static extern bool SHGetPathFromIDList(IntPtr pidl, IntPtr pszPath);

            [DllImport("shell32.dll")]
            public static extern int SHGetSpecialFolderLocation(IntPtr hwnd, int csidl, ref IntPtr ppidl);
        }
    }
}