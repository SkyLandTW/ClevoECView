ClevoECView
===========

ECView (fan control on Clevo laptops), with additional functionality



License: Public Domain


------------------------------------------------------------------------------

Based on reverse-engineered code from ECView 5.5 on baidu, publisher unknown.

Functionality:
  - Notification icon: displaying fan speed and CPU temperature, via WMI
  - Context menu:
      * fan setting (manual), via ecview.dll
      * fan setting (auto), via ecview.dll
      * opening the original ECView window

Limitation:
  - The notification icon supports single fan only.
  - ECView might damage your laptop EC or cause explosion.

Installation:
  1. Make sure Clevo drivers are installed. While ecview.dll is independent,
     the WMI functionality may rely on the Hotkeys driver (unconfirmed).
  2. Build (msbuild or Visual Studio)
  3. Copy out\ClevoECView2.exe, dpmemio.dll, ecview.dll to destination
  4. Copy out\dpmemio.sys to %SystemRoot%\Syswow64\drivers\
  5. Register dpmemio.sys via out\dpmemio.reg
  6. Reboot for dpmemio service to take effect

Verified hardwares:
  - Clevo W350SS
  - Clevo P170SM-A (CPU fan only)

TODO:
  - Support more fans

Notes:
  - The project is for Visual Studio 2012, but based on .NET Framework 2.0.
    You should be able to change the version in .csproj for 2005/2008/2010.
  - Trace of Clevo WMI is not present in ECView, but inside the latest version
    of Hotkey AP (v2.34.46): In the HotkeyService.exe there is an unused class
    _HotKeyService.Talk2Wmi_. Most of the WMI calls don't work though.
  - UEFI seems to block any changes of settings made to the fan. Reading via
    WMI would still function.
  - Confirmed to work with and without Prema Mod, the unlocked BIOS for Clevo
    laptops.
