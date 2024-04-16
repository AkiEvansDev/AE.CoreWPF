using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace AE.CoreWPF;

[StructLayout(LayoutKind.Sequential)]
internal struct SHFILEINFO
{
	public IntPtr hIcon;
	public int iIcon;
	public uint dwAttributes;
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
	public string szDisplayName;
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
	public string szTypeName;
};

public static class WinFileHelper
{
	public static readonly Guid Desktop = new("B4BFCC3A-DB2C-424C-B029-7FE99A87C641");
	public static readonly Guid Downloads = new("374DE290-123F-4565-9164-39C4925E467B");
	public static readonly Guid Documents = new("FDD39AD0-238F-46AF-ADB4-6C85480369C7");
	public static readonly Guid Pictures = new("33E28130-4E1E-4676-835A-98395C3BC3BB");
	public static readonly Guid Videos = new("18989B1D-99B5-455B-841C-AB7C74E4DDFC");

	private const uint SHGFI_ICON = 0x100;
	private const uint SHGFI_LARGEICON = 0x0;

	[DllImport("shell32.dll", CharSet = CharSet.Unicode)]
	private static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, out string pszPath);

	[DllImport("shell32.dll", CharSet = CharSet.Unicode)]
	private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

	[DllImport("user32.dll")]
	private static extern bool DestroyIcon(IntPtr handle);

	public static string GetKnownFolderPath(Guid guid)
	{
		_ = SHGetKnownFolderPath(guid, 0, IntPtr.Zero, out var result);
		return result;
	}

	public static Icon ExtractIconFromPath(string path)
	{
		var shinfo = new SHFILEINFO();
		SHGetFileInfo(path, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_LARGEICON);

		var icon = (Icon)Icon.FromHandle(shinfo.hIcon).Clone();
		DestroyIcon(shinfo.hIcon);

		return icon;
	}
}
