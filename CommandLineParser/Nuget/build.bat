REM delete existing nuget packages
del *.nupkg
powershell -noexit -executionpolicy bypass -File "version-no.ps1"
pause

REM  "../CommandLineParser.csproj" "../bin/debug/CommandLine.Parser.dll"