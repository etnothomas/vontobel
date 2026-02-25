To run please modify the watchedFolder and partnersDirectory paths in Program.cs, also modify the partner files to make sure the paths match something on your file system.
Once done, run the program and move an IBT.xml file to the watched directory to trigger the parsing and sending. 

When the programme initializes it will try to read the partnersDirectory and parse the jsons there (in this repo they are in the partners folder).

It it using blocking queues as the main way to pass data between async processes. 

points for improvement:
- error handling -> dead letter queues and retries.
- async behavior -> running in just a few thread now (just learned about csharps async model, so not super familiar with it yet).
- better typed formatting -> now it just takes jsons and formats those.
- better typing in general.
- configuration -> blocking queues are instantiated with default settings, and much more could be configured in a config file.

much more to say, but am to tired right now :)