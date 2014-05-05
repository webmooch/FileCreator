#File Creator

File Creator is a portable executable that provides the ability to create a single file of almost any size.

It's used for a broad range of testing scenarios (see usage examples below).

The file created may be filled either with null or random data.

File Creator supports a file size of 1 byte through to 8,589,934,591GB and a chunk write size of 1byte through to 2046MB (1MB short of 2GB).

It also displays approximate transfer speed on write completion.

File Creator is still in its infancy but more features and UI options are planned.

It utilises v4.5 of the .NET framework and is written in C# with WPF MVVM as a x64 application to support greater file and chunk size.

##Usage Examples

- Any situation that requires a file of a specific size (file transfer speed tests, parsing or opening of large files, etc.).
- Any situation that requires consumption of disk / storage space.
- Network file share write speed tests.
- Any situation that requires large RAM utilisation (if specifying very large chunk size).
- Determining the fastest chunk write size for any writable disk.

##Screenshots

####Main Window
![Main Window](http://webmooch.net/images/FileCreator_Screenshot.png)


####Size Units
![Size Units](http://webmooch.net/images/FileCreator_SizeUnits.png)


####File Context Menu
![File Context Menu](http://webmooch.net/images/FileCreator_SaveFileAsContextMenu.png)


##Chunk Size Explained

The time taken for data to be written includes the overhead of the storage medium to perform the operation. This is usually measured in single digit milliseconds and is normally insignificant; however this can add considerable delays when many small write operations are requested consecutively. As a result data is generally broken-up into many 'chunks' or 'blocks' and are first written to memory (RAM) before being written (flushed) to disk in 'block' transactions. This is more efficient as the disk is required to perform fewer operations. However, if the chunk size is too large it may slow the system as the amount of RAM required to perform the write operation is at least equal to the chunk size.

File Creator grants you full control over the chunk size (from 1 byte through to 2046MB) which allows you to specify the how much data is first written to memory before it is flushed to disk.

To demonstrate - use File Creator to create a 100MB file with a chunk size of 10MB and record the approximate time taken / write speed. Now try the same operation with a chunk size of 10 bytes and compare.
