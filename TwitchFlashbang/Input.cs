using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TwitchFlashbang
{
    public static class Input
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;

        private static IntPtr hookId = IntPtr.Zero;
        public static LowLevelKeyboardProc keyboardProc;
        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public static event Action<Keys> KeyPressed;

        public static void InstallHook()
        {
            keyboardProc = HookCallback;
            hookId = SetHook(keyboardProc);
        }

        public static void UninstallHook()
        {
            WinAPI.UnhookWindowsHookEx(hookId);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return WinAPI.SetWindowsHookEx(WH_KEYBOARD_LL, proc, WinAPI.GetModuleHandle(curModule.ModuleName), 0);
            }
        }


        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Keys key = (Keys)vkCode;

                // Check if any modifiers (CTRL, SHIFT, ALT) are pressed
                bool ctrl = (Control.ModifierKeys & Keys.Control) != 0;
                bool shift = (Control.ModifierKeys & Keys.Shift) != 0;
                bool alt = (Control.ModifierKeys & Keys.Alt) != 0;

                // Combine the modifiers with the pressed key
                key |= ctrl ? Keys.Control : Keys.None;
                key |= shift ? Keys.Shift : Keys.None;
                key |= alt ? Keys.Alt : Keys.None;

                // Raise the KeyPressed event
                KeyPressed?.Invoke(key);
            }

            return WinAPI.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }
    }
}
