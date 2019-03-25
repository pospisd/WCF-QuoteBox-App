====================================================================

InstallUtil.exe

====================================================================

INSTALL:
To install the project into Windows Services launch Command Prompt 
in Administrator mode and navigate to the installutil.exe directory.

I found this locally in:
C:\Windows\Microsoft.NET\Framework\v4.0.30319

Then enter: 'installutil C:<Path to project ...bin\Debug>\QuoteBoxWinServiceHost.exe'

UNISTALL:
enter: 'installutil /u C:<Path to project ...bin\Debug>\QuoteBoxWinServiceHost.exe'
