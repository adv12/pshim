# pshim
PowerShell Image Manipulation

This is a quickly written and poorly tested PowerShell module for image manipulation, basically just a thin set of Cmdlets wrapping the already very friendly ImageSharp .NET API.  It's primarily for me to learn about ImageSharp and PowerShell and to have something enjoyable to work on in my spare time.  Because ImageSharp is a .NET Core platform-independent library, so is pshim.

It does work, if you want to try it, but it's not packaged in any way; you just have to grab source, build, and publish to some location and use Import-Module to load it (or publish it to somewhere PowerShell is already looking).  The Import-Module command will look something like this:

Import-Module -Name <path_to_publish_directory>/Pshim.dll

Make sure you publish rather than just building, so you get all the dependencies as well.
