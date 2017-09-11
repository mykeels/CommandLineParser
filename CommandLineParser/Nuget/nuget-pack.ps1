# A powershell script for packing dotnet projects into nuget packages
# It automatically detects the dotnet project and assembly name
# It automatically detects the assembly information and embeds it in the nuget package

param(
        [parameter(HelpMessage = "Full Path to *.csproj file you intend to build")]
        [string]$projectName,
        [parameter(HelpMessage = "Full Path to *.dll file you intend to build")] 
        [string]$assemblyName
    )

if ([string]::IsNullOrEmpty($projectName) -or [string]::IsNullOrEmpty($assemblyName)) {
    $file = @(gci ../*.csproj)[0] #get the *.csproj file you intend to build from the parent directory

    if ($file) {
        
        $projectName = "$($file.Directory)/$($file.Name)" #get the csproj full path
    
        #search the .csproj file for the <AssemblyName> element and get its value
        $projectContent = [xml](Get-Content $projectName)
        $assemblyName = $($projectContent.Project.PropertyGroup.AssemblyName) #assign the value
    
        if ([string]::IsNullOrEmpty($assemblyName)) {
            # if <AssemblyName> value was NOT found in the csproj file, then infer it from the Name of the .csproj file
            # such as <csproj-file-name>.dll
            # if .csproj file is called ChuckNorris.csproj, then $assemblyName will have value of ChuckNorris.dll
            $assemblyName = "$($file.Directory)/bin/debug/$($file.Name -replace ".csproj", ".dll")"
        }
        else {
            # if <AssemblyName> value was found
            # trim it, and assign it to $assemblyName
            $assemblyName = "$($file.Directory)/bin/debug/$($assemblyName.Trim()).dll"
        }
        
        if (@(gci $projectName).Count -eq 0) {
            Write-Host ("`t... " + "Could not find Project: $projectName") -foreground "yellow"
            Write-Host ("`t... " + "Please specify a project name in the arguments") -foreground "yellow"
            Write-Host ("`t... " + "Exiting Program") -foreground "yellow"
            pause
            Exit-PSSession
        }
        if (@(gci $assemblyName).Count -eq 0) {
            Write-Host ("`t... " + "Could not find Assembly: $assemblyName") -foreground "yellow"
            Write-Host ("`t... " + "Please specify an assembly name in the arguments") -foreground "yellow"
            Write-Host ("`t... " + "Exiting Program") -foreground "yellow"
            pause
            Exit-PSSession
        }
    }
    else {
        Write-Host ("No file found in ../*.csproj") -foreground "yellow"
    }
}

if (![string]::IsNullOrEmpty($projectName)) {
    Write-Output "Project: $projectName"
    Write-Output "Assembly: $assemblyName"

    $major = [System.Reflection.Assembly]::LoadFrom($assemblyName).GetName().Version.Major
    
    $minor = [System.Reflection.Assembly]::LoadFrom($assemblyName).GetName().Version.Minor
    
    $build = [System.Reflection.Assembly]::LoadFrom($assemblyName).GetName().Version.Build
    
    $revision = [System.Reflection.Assembly]::LoadFrom($assemblyName).GetName().Version.Revision
    
    $infoVersion = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($assemblyName).ProductVersion
    
    Write-Output $infoVersion
    
    $preRelease = [System.Text.RegularExpressions.Regex]::Match($infoVersion, "-\w+$").Value
    
    Write-Output $preRelease
    
    $version = "$($major).$($minor).$($build).$($revision)$($preRelease)"
    
    Write-Output Write-host $version
    
    nuget pack "$projectName" -Version $version
}

pause
