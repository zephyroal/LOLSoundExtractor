/*===============================================================================================
 OUTPUT_MP3.DLL
 Copyright (c), Firelight Technologies Pty, Ltd 2004-2005.

 Shows how to write an FMOD output plugin that writes the output to an mp3 file using 
 lame_enc.dll.

 Most of this source code that does the encoding is taken from the LAME encoder example.
 An FMOD Ex output plugin is created by declaring the FMODGetOutputDescription function,
 then filling out a FMOD_OUTPUT_DESCRIPTION and returning a pointer to it.
 FMOD will then call those functions when appropriate.

 To get the output from FMOD so that you can write it to your sound device (or LAME encoder
 function in this case), FMOD_OUTPUT_STATE::readfrommixer is called to run the mixer.

 We acknowledge that we are using LAME, which originates from www.mp3dev.org.
 LAME is under the LGPL and as an external FMOD plugin with full source code for the interface
 it is allowable under the LGPL to be distributed in this fashion.

===============================================================================================*/


This is not the 'official' version of output_mp3.dll provided in FMOD Ex distribution.
I've modified the library to pass more parameter to the plugin (conversion settings).

USE THIS LIBRARY AT OUR OWN RISK !

Modification's author
 Jérôme JOUVIE (Jouvieje)
 jerome.jouvie@gmail.com
 http://jerome.jouvie.free.fr/
