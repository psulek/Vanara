﻿using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Runtime.InteropServices;
using static Vanara.PInvoke.ComDlg32;
using static Vanara.PInvoke.User32;

namespace Vanara.PInvoke.Tests
{
	[TestFixture]
	public class ComDlg32Tests
	{
		[OneTimeSetUp]
		public void _Setup()
		{
		}

		[OneTimeTearDown]
		public void _TearDown()
		{
		}

		[Test]
		public void ChooseColorTest()
		{
			var cc = new CHOOSECOLOR
			{
				lStructSize = (uint)Marshal.SizeOf(typeof(CHOOSECOLOR)),
				rgbResult = System.Drawing.Color.Red,
				Flags = CC.CC_RGBINIT,
				hwndOwner = GetDesktopWindow()
			};
			Assert.That(ChooseColor(ref cc), Is.True);
		}

		[Test]
		public void GetOpenFileNameTest()
		{
			var fnch = new char[261];
			var fn = new string(fnch);
			var ofn = new OPENFILENAME
			{
				lStructSize = (uint)Marshal.SizeOf(typeof(OPENFILENAME)),
				lpstrFile = fn,
				nMaxFile = (uint)fnch.Length,
				lpstrFilter = "All\0*.*\0Text\0*.txt\0",
				nFilterIndex = 1,
				Flags = OFN.OFN_PATHMUSTEXIST | OFN.OFN_FILEMUSTEXIST
			};
			Assert.That(GetOpenFileName(ref ofn), Is.True);
			Assert.That(ofn.lpstrFilter.Length, Is.GreaterThan(0));
		}

		[Test]
		public void FindTextTest()
		{
			using var wnd = new DlgWin(RegisterWindowMessage(FINDMSGSTRING));
			var fwch = new char[261];
			var fw = new string(fwch);
			var fr = new FINDREPLACE
			{
				lStructSize = (uint)Marshal.SizeOf(typeof(FINDREPLACE)),
				hwndOwner = wnd.MessageWindowHandle,
				lpstrFindWhat = fw,
				wFindWhatLen = (ushort)fwch.Length,
			};
			Assert.That(wnd.hdlg = FindText(ref fr), Is.True);
		}

		class DlgWin : SystemEventHandler
		{
			uint rmsg;
			internal HWND hdlg = default;

			public DlgWin(uint m)
			{
				rmsg = m;
			}

			protected override bool PreprocessMessage(in MSG msg) => IsDialogMessage(hdlg, msg);

			protected override bool MessageFilter(HWND hwnd, uint msg, IntPtr wParam, IntPtr lParam, out IntPtr lReturn)
			{
				lReturn = default;
				if (msg == rmsg)
					return true;
				return false;
			}
		}
	}
}