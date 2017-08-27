$major = [System.Reflection.Assembly]::LoadFrom("../bin/debug/CommandLine.Parser.dll").GetName().Version.Major

$minor = [System.Reflection.Assembly]::LoadFrom("../bin/debug/CommandLine.Parser.dll").GetName().Version.Minor


$build = [System.Reflection.Assembly]::LoadFrom("../bin/debug/CommandLine.Parser.dll").GetName().Version.Build

$revision = [System.Reflection.Assembly]::LoadFrom("../bin/debug/CommandLine.Parser.dll").GetName().Version.Revision

$infoVersion = [System.Diagnostics.FileVersionInfo]::GetVersionInfo("../bin/debug/commandline.parser.dll").ProductVersion

echo $infoVersion

$preRelease = [System.Text.RegularExpressions.Regex]::Match($infoVersion, "-\w+$").Value

echo $preRelease

$version = "$($major).$($minor).$($build).$($revision)$($preRelease)"

echo Write-host $version

nuget pack "../CommandLineParser.csproj" -Version $version

pause
