ClevoECView
===========

ECView (fan control on Clevo laptops), with additional functionality



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
- ECView 5.5 is required, because changing fan speed requires ecview.dll,
  which requires the 'dpmemio' service.
- ecview.dll is said to be dangerous and could brick EC.

Installation:
  1.Build
  2.Copy out\ClevoECView2.exe, dpmemio.dll, ecview.dll to destination
  3.Copy out\dpmemio.sys to %SystemRoot%\Syswow64\drivers\
  4.Register dpmemio.sys via out\dpmemio.reg
  5.Reboot for dpmemio service to take effect

Verified hardwares:
- Clevo W350SS

TODO:
- Support more fans

