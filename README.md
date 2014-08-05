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

Compatible hardwares:
- Clevo W350SS (my laptop)

TODO:
- Register dpmemio automatically without ECView installer
- Support more fans

