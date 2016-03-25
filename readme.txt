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