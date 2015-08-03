# RebootTo
Tool for dual/multi-boot Windows users that will prompt to select which OS to reboot to, without having to wait for the boot menu after the reboot.

## What it does
It simplifies the process of rebooting to a non-default Operating System.  Normally in Windows, you'd have to select reboot in the current OS, wait for the machine to reboot and for the boot menu to come up, then select the OS to boot into.  This tool overrides the default boot (for the next reboot only, leaving the default intact).

The screenshot below shows a dual-boot computer with a Home and a Work OS.

![Screenshot](Docs/screenshot.png)

## Supported Operating Systems
This has been tested with Windows 10 and Windows 8.1.  It should also work with Windows 7, but has not yet been tested.

## What's Next
The current implementation requires UAC elevation, which means you'll get a UAC prompt when you launch.  I don't think that can be worked around, but it's worth looking into.  Also, some better integration into the Start Menu would be helpful.
