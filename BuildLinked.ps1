# Set Working Directory
Split-Path $MyInvocation.MyCommand.Path | Push-Location
[Environment]::CurrentDirectory = $PWD

Remove-Item "Build/Linked" -Force -Recurse
dotnet publish "./GlazeWM.Bootstrapper/GlazeWM.Bootstrapper.csproj" -c Release -o "Build/Linked" /p:OutputPath="./bin/Release" /p:ReloadedILLink="true"

# Restore Working Directory
Pop-Location