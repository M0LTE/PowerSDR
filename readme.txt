This project was started 1/1/2016 by Darrin Kohn KE9NS and is based upon
the GPL Open Source PowerSDR 2.7.2 SVN: 5123 code provided by Flex Radio Systems, Inc.
With generous help provided by Flex Radio Systems, Inc.


This Source code includes 4 project under the PowerSDR umbrella: DttSP written in C, PowerMate written in C++, PowerSDR written in C#, and Visual Studio Installer (to create the installer)

DttSP and PowerSDR code was provided by Flex Radio Systems, Inc.
PowerMate code was provided by Griffin, Clev R. Munke, and me Darrin ke9ns
PowerSDR modifications beyond 2.7.2 are provided by: me Darrin ke9ns, and dh1tw for the DJConsole code
SWL.csv file from eibispace.de (thank you)
dxloc.txt file initially from HRD (thank you), modified by me Darrin ke9ns
PowerSDR_ke9ns_Installer_RevT9.msi The actual installer program to move the files into the properlocations (including skins) ke9ns
To create the Installer, you must go to VS->Tools->Extensions and updates and download the free "Visual Studio Installer Projects" by Microsoft


Currently compiling under Visual Studio 2015 (VS2015) and NET 4.5.2
After you download and install VS2015, you must then download and install the VS2015 "UPDATE 3" & "Patch" 3
Sometimes the visual design form can become out of sync with its .cs code file.
This can happen if you change some code in the .cs file that VS wants you to change from the design form.
If you get an error message trying to view the "Design" form, close the console (both Design and CS files) and reload.
Its best to always verify that you can properly view the design form, then hit save.


You can run the build from within VS or directly from the VS project release or debug folders.

PowerSDR.exe and PowerSDR.exe.config are the only files needed in the PowerSDR build. I rename them to PowerSDR_T8.
DttSP.dll is the only file needed in the DttSP build.
PowerMate.dll is the only file needed in the PowerMate build.
ALSO: vcruntime140.dll needs to be copied onto the target computer (PSDR program files folder, or download the free VC 2015 runtime installer)

Do a full rebuild the first time you open the project in VS, before doing anything else.

Things I needed to do before the PowerSDR would actually run:
 1) Copy the project into the VS project folder. Make sure the Bin and Source folders are both directly 1 level down 
 from the PowerSDR_v2.7.2 project folder itself, otherwise the paths wont match up to the original project solution information, and it wont compile
 2) Copy over the pal.dll from the Standard Flex PowerSDR program files (x86) folder, into the VS release & debug folders,
  otherwise the PowerSDR messes up and will ask for a firmware update.
 3) Copy over the TNF.dll file from the Standard Flex PowerSDR program files (x86) folder, into the VS release & debug folders.
 4) Go into the VS PowerSDR "references" -> Browse, and add the TNF.DLL to the list. Navigate to the Relase folder you placed it in.
 5) Go into the VS PowerSDR "properties" and remove the NO_TNF reference from the "Build" compile list (you need to do this 2 times: Debug and Release)
 6) Copy over the ATU.dll file from the Standard Flex PowerSDR program files (x86) folder, into the VS release & debug folders.
 7) Go into the VS PowerSDR "references" -> Browse, and add the ATU.DLL to the list. Navigate to the Relase folder you placed it in.
 8) Go into the VS PowerSDR "properties" and remove the NO_NEW_ATU reference from the "Build" compile list (you need to do this 2 times: Debug and Release)
 9) Rebuild the solution: Release or Debug. Now you will have a full working PowerSDR version that you can start your own modding:


The "//ke9ns add" call sign comment to indicate code I added
The "//ke9ns mod" call sign comment to indicate modifications to original 2.7.2 source code


PowerSDR open source code provided by Flex Radio Systems, Inc. is covered under GNU GPL Ver 2
This requires anyone who distributes this code to note its authors.

If you wish to add to this PowerSDR ke9ns project, as opposed to starting with the Flex 2.7.2 version, please contact me Darrin Kohn ke9ns  ke9ns@ke9ns.com
It would be nice to add more features to this PowerSDR ke9ns project based on others source code additions.


73's
Darrin ke9ns.com
