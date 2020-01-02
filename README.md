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

The command line utility exits with code: _0 (0x00000000)_

On the other hand, if the checksum verification fails, the output would look like as 
shown below.

```
File:   <file-path>
Hash:   <expected-checksum>
Method: MD5
Result: UNCONFIRMED
Detect: <detected-checksum>
```

The command line utility exits with code: _-2147483647 (0x80000001)_


It would also be possible to produce just a short output. For this purpose the argument 
`-s` must be applied.

```
fcc -s -v -m md5 -f <file-path> -h <expected-checksum>
```

In this case and if verification was successful, the output would look like as shown below.

```
CONFIRMED
```

The command line utility exits with code: _0 (0x00000000)_

And in case of the verification was unsuccessful, the output would look like as shown below.

```
UNCONFIRMED
```

The command line utility exits with code: _-2147483647 (0x80000001)_

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

The command line utility exits with code: _0 (0x00000000)_

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

The command line utility exits with code: _0 (0x00000000)_

As for checksum verification, producing just a short output is also possible. For 
this purpose the argument `-s` must be applied as well like shown as next.

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

The command line utility exits with code: _0 (0x00000000)_



