Import-Module ".\bootstrap\psake\psake.psm1";
& ".\logo.ps1"
invoke-psake ".\build.ps1" -Task Release