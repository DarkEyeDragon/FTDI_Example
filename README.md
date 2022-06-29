# FTDI_Example
 This project shows how to interface with a FTDI device in C# using a .NET Core Console Application.
 This particular example uses the Enttec DMX USB Pro as example.
 
## Usage
Run the application with the FTDI device connected. It'll show a list of connected devices
then select the right index (usually 0). It'll then send the DMX data over the device for the connected DMX device to interpret

## Why?
I published this since there is little to no updated documentation on FTDI. The most recent stuff dates back to 2008.
I hope it can save someone a few days of having to figure this out themself.
