Note From ke9ns:
This code has been upgraded to run under NET 4.5.2
but requires a PowerSDR.exe.config file to travel with the PowerSDR.exe file
this is due to a old Flex1500 DLL file compiled under something less than NET 3.5.
I use VS2015 free community edition (service Pack 1)

Warning: console design complexity can cause delays in viewing console form and may even cause faults to show up on the screen. To avoid this always hit the save button before viewing the console form and be patient to wait for it to reset itself.


From Original Flex GPL source code 2.7.2:
The source tree includes 2 projects which can be compiled 
in Visual Studio .NET 2008.  The solution file (PowerSDR.sln) in 
the Source folder will compile both projects and output the 
files to the bin\Debug or bin\Release folder depending on which
version is being compiled.


WARNING!  Before opening the forms in design view in VS2008, the
PowerSDR project should be compiled.  This is a workaround to
prevent a bug that is documented at the below link with regards
to losing custom control information in the project source code.
Note that it is also necessary to close all design views of
forms before switching between Debug and Release versions.  A
good first step would be to compile in Debug and Release mode to
prevent headaches that result from this issue.

See more at: http://www.kbalertz.com/feedback.aspx?kbnumber=842706