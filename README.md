# Rover Engagement Display (RED)
*A telemetry and command interface for operating a remote, embedded system.*

## First clone?
The first time you clone the repo, Nuget will attempt to download any required packages.  One of these packages, SharpDX, is very large for some reason - 250+ mb.  After Nuget is finished installing these packages, remove all folders in `/Packages/SharpDX.2.6.2/Bin/` EXCEPT `DirectX11-Signed-net40` which is the only folder that's actually needed.

## Introduction
RED is structured as a Model-View-View Model (MVVM) application, similar to Model-View-Presenter (MVP). This means the code/XAML that creates a view is seperated from its construction and control logic. https://en.wikipedia.org/wiki/Model–view–viewmodel

The core view controller that handles the contruction of the application is in [ViewModels/ControlCenterViewModel.cs](/RED/ViewModels/ControlCenterViewModel.cs). From there, the individual views are contructed.

Underneath the GUI, there is an implementation of RoveComm in C#, within [Rove Protocol](/RED/RoveProtocol/). This is the core software functionality, as RED's purpose is to send/recieve UDP packets to the rover's systems in a human-convienent manner. The file that defines the data for these packets is defined in [Configurations/MetadataManagerConfig.cs](/RED/Configurations/MetadataManagerConfig.cs).
