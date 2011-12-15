@echo off
echo "Running Music player ..."
java -Djava.library.path=./lib -Djava.class.path=./lib/NativeFmodEx.jar;./MusicPlayerEx.jar org.jouvieje.MusicPlayerEx.MusicPlayerEx
pause