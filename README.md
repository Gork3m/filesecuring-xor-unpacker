# filesecuring-xor-unpacker
A static unpacker / deobfuscator for that Lua obfuscator called "filesecuring" / "xor".
This obfuscator was mainly used in FiveM communities, due to the way it detects load=print, it was pretty difficult to dump it by hooking lua functions.
They recently discontinued and opensourced their obfuscator, so I can publicly release my deobfuscator for it.

Usage:
build the project.
run CLI.exe with following arguments:

`CLI.exe input.lua`

It will create an `input_unpacked.lua` in the same directory
⚠️ It might take a while to decode scripts bigger than 10mb, be patient
This obfuscator was written by Federal#9999, feel free to skid it. thats what you do anyways
