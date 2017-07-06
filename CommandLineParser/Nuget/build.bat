REM delete existing nuget packages
del *.nupkg
powershell -noexit -executionpolicy bypass -File "version-no.ps1"
pause