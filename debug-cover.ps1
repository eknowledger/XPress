Import-Module ".\bootstrap\psake\psake.psm1";
invoke-psake ".\build.ps1" -Task Debug-Cover