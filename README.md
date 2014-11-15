# Rover Engagement Display (RED)
*A telemetry and command interface for operating a remote, embedded system.*

## Important Notes

### First clone?
The first time you clone the repo, Nuget will attempt to download any required packages.  One of these packages, SharpDX, is very large for some reason - 250+ mb.  After Nuget is finished installing these packages, remove all folders in `/Packages/SharpDX.2.6.2/Bin/` EXCEPT `DirectX11-Signed-net40` which is the only folder that's actually needed
