<p align="center">
  <a href="https://github.com/akesseler/Plexdata.FileChecksum/blob/master/LICENSE.md" alt="license">
    <img src="https://img.shields.io/github/license/akesseler/Plexdata.FileChecksum.svg" />
  </a>
  <a href="https://github.com/akesseler/Plexdata.FileChecksum/releases/latest" alt="latest">
    <img src="https://img.shields.io/github/release/akesseler/Plexdata.FileChecksum.svg" />
  </a>
  <a href="https://github.com/akesseler/Plexdata.FileChecksum/archive/master.zip" alt="master">
    <img src="https://img.shields.io/github/languages/code-size/akesseler/Plexdata.FileChecksum.svg" />
  </a>
</p>


# Plexdata File Checksum Utility

The project is split into a library responsible to provide all supported methods and 
to perform the analysis as well as executables for user interactions.

## Command Line Interface (CLI)

The command line interface allows any kind of checksum analysis. Such an analysis could 
be the verification of existing file checksums. Another feature is the possibility to 
create file checksums.

### File Checksum Verification

In this section please find some examples of how to perform file checksum verifications.

```
fcc -v -m md5 -f <file-path> -h <expected-checksum>
```

In case of a successful verification the output would look like as shown below.

```
File:   <file-path>
Hash:   <expected-checksum>
Method: MD5
Result: CONFIRMED
```

The command line utility exits with code 0 (0x00000000) in this case.

On the other hand, if the checksum verification fails, the output would look like as 
shown below.

```
File:   <file-path>
Hash:   <expected-checksum>
Method: MD5
Result: UNCONFIRMED
Detect: <detected-checksum>
```

The command line utility exits with code -2147483647 (0x80000001) in this case.


It would also be possible to produce just a short output. For this purpose the argument 
`-s` (spare mode) must be applied.

```
fcc -s -v -m md5 -f <file-path> -h <expected-checksum>
```

In this case and if verification was successful, the output would look like as shown below.

```
CONFIRMED
```

The command line utility exits with code 0 (0x00000000) in this case.

And in case of the verification was unsuccessful, the output would look like as shown below.

```
UNCONFIRMED
```

The command line utility exits with code -2147483647 (0x80000001) in this case.

### File Checksum Creation

In this section please find some examples of how to perform file checksum creation.

```
fcc -c -m md5 <file-1-path>
```

In case of a successful creation the output would look like as shown below.

```
File:   <file-path>
Method: MD5
Result: <created-checksum>
```

The command line utility exits with code 0 (0x00000000) in this case.

It would also be possible to create multiple checksums for more than one file. See 
command line below for such an example.

```
fcc -c -m md5,sha1 <file-1-path> <file-2-path> <file-3-path>
```

In case of a successful creation the output would look like as shown below.

```
File:   <file-1-path>
Method: MD5
Result: <created-checksum>
Method: SHA1
Result: <created-checksum>
File:   <file-2-path>
Method: MD5
Result: <created-checksum>
Method: SHA1
Result: <created-checksum>
File:   <file-3-path>
Method: MD5
Result: <created-checksum>
Method: SHA1
Result: <created-checksum>
```

The command line utility exits with code 0 (0x00000000) in this case.

As for checksum verification, producing just a short output is also possible. For 
this purpose the argument `-s` (spare mode) must be applied as well like shown as 
next.

```
fcc -s -c -m md5,sha1 <file-1-path> <file-2-path> <file-3-path>
```

In case of a successful creation the output would look like as shown below.

```
MD5     <created-checksum>        <file-1-path>
SHA1    <created-checksum>        <file-1-path>
MD5     <created-checksum>        <file-2-path>
SHA1    <created-checksum>        <file-2-path>
MD5     <created-checksum>        <file-3-path>
SHA1    <created-checksum>        <file-3-path>
```

The command line utility exits with code 0 (0x00000000) in this case.

### Application Exit Codes

All above mentioned exit codes are considered as `HRESULT`.

`0 (0x00000000)`  
No error occurred.

`-2147483647 (0x80000001)`  
Verification failed and checksum is not confirmed.

`1 (0x00000001)`  
The program´s help screen or its product version number was shown.

`-2146233029 (0x8013153B)`  
Current operation has been canceled by user. This error code represents the Window 
error code `COR_E_OPERATIONCANCELED`.

Other application exit codes can also occur, for instance in case of an internal 
program exception. In such cases an additional error message is printed out.

### Command Line Arguments

In this section please find all command line arguments that are supported at the 
moment.

`--verify` / `-v`  
Enables the program´s verify mode.

`--create` / `-c`  
Enables the program´s create mode.

`--sparse` / `-s`  
Enables the program´s sparse mode.

`--methods` / `-m`  
Comma separated list of checksum methods. Allowed are `md5`, `sha1`, `sha256`, `sha384`, 
`sha512`, `all` or any combination. But be aware, only the first applied method is used 
in verify mode.

`--hash` / `-h`  
Single hash value to be verified. But note, this argument is only allowed along 
with checksum verification.

`--file` / `-f`  
Fully qualified path of a single file that should be verified. But note, this argument 
is only allowed along with checksum verification.

`--version`  
Shows the program´s product version number and exits.

`--help` / `-?`  
Shows the program´s help screen and exits.

`<files>`  
List of fully qualified file names (separated by spaces) for which checksums should 
be created. But note, this argument is only allowed along with checksum creation.

The usage of wild cards (`*` and `?`) is supported as well. Please keep in mind, files 
with search pattern are expected inside current directory if no path is defined.

**Examples**

_Verify_  
`fcc.exe -v -m <method> -h <hash> -f <path>`

_Create_  
`fcc.exe -c -m <methods> file1 [file2 .. file(n)]`

Finally, please keep in mind that it is strictly recommended to enclose each provided 
file path in double-quotes.

