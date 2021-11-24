This repository is archived and tasks have been moved to https://github.com/CommunityHiQ/Frends.Community.Xml

# Frends.Community.XmlSignature

This repository contains tasks to sign XML files and verify signed XML files.

[![Actions Status](https://github.com/CommunityHiQ/Frends.Community.XmlSignature/workflows/PackAndPushAfterMerge/badge.svg)](https://github.com/CommunityHiQ/Frends.Community.XmlSignature/actions) ![MyGet](https://img.shields.io/myget/frends-community/v/Frends.Community.XmlSignature) [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT) 

- [Frends.Community.XmlSignature](#Frends.Community.XmlSignature)
  - [Installing](#installing)
  - [Building](#building)
  - [Contributing](#contributing)
  - [Tasks](#tasks)
     - [SignXml](#signxml)
     - [VerifyXml](#verifyxml)
  - [Changelog](#changelog)

## Installing

You can install the task via FRENDS UI Task View or you can find the NuGet package from the following NuGet feed
https://www.myget.org/F/frends-community/api/v3/index.json and in Gallery view in MyGet https://www.myget.org/feed/frends-community/package/nuget/Frends.Community.XmlSignature

You can install the task via FRENDS UI Task View by using `Import Task NuGet` button in Administration > Tasks.

## Building

Clone a copy of the repo

`git clone https://github.com/CommunityHiQ/Frends.Community.XmlSignature.git`

Rebuild the project

`dotnet build`

Run Tests

`dotnet test`

Create a NuGet package

`dotnet pack --configuration Release`

## Contributing

When contributing to this repository, please first discuss the change you wish to make via issue, email, or any other method with the owners of this repository before making a change.

1. Fork the repo on GitHub
2. Clone the project to your own machine
3. Commit changes to your own branch
4. Push your work back up to your fork
5. Submit a Pull request so that we can review your changes

NOTE: Be sure to merge the latest from "upstream" before making a pull request!

## Tasks

### SignXml

Signs a XML document (XMLDSIG). Takes XML input either as a file or as a XML-string and outputs a signed version of it.

#### Input
| Property  | Type  | Description |Example|
|-----------|-------|-------------|-------|
| XmlInputType  | `XmlParamType` | Choose input type | Possible values: `File`, `XML-string` |
| XmlFilePath  | `string` | Path of the XML file to be signed. | `c:\temp\document.xml` |
| Xml  | `string` | File as XML-string | `XML-string` |
| XmlEnvelopingType  | `XmlEnvelopingType` | Choose the type of enveloping | Possible values: `XmlEnvelopedSignature` |
| SigningStrategyType  | `SigningStrategyType` | Choose the type of signing | Possible values: `PrivateKeyCertificate` |
| CertificatePath  | `string` | Path for certificate file | `c:\certificates\signingcertificate.pfx` |
| PrivateKeyPassword  | `string` | Password used for certificate file |  |

#### Output

| Property  | Type  | Description |Example|
|-----------|-------|-------------|-------|
| OutputType  | `XMLParamType` | Output format | Possible values: `File` or `XML-string` |
| OutputFilePath  | `string` | Path for the signed XML file | `c:\temp\signedOutput.xml` |
| OutputEncoding  | `string` | Encoding for output file | `UTF-8` |
| AddSignatureToSourceFile  | `boolean` | If true, add signature to original input file | `true` |

#### Options

| Property  | Type  | Description |Example|
|-----------|-------|-------------|-------|
| IncludeComments  | `boolean` | If true, add additional transform methods | `true` |
| PreserveWhitespace  | `boolean` | Preserve whitespace when loading XML? | `true` |
| XmlSignatureMethod  | `XmlSignatureMethod` | Method for XML signature | Possible values: `RSASHA1`, `RSASHA256`, `RSASHA384`, `RSASHA512` |
| DigestMethod  | `DigestMethod` | Digest method to use | Possible values: `SHA1`, `SHA256`, `SHA384`, `SHA512` |
| TransformMethods  | `TransformMethod` | Transform methods to use | Possible values: `DsigC14`, `DsigC14WithComments`, `DsigExcC14`, `DsigExcC14WithComments`, `DsigBase64` |

#### Result

| Property  | Type  | Description |Example|
|-----------|-------|-------------|-------|
| Result  | `string` | Depending on params OutputType and AddSignatureToSourceFile this contains either XML-string or filepath | |


### VerifyXml

Task for verifying signatures of XML files.

#### Input

| Property  | Type  | Description |Example|
|-----------|-------|-------------|-------|
| XmlInputType  | `XmlParamType` | Choose input type | Possible values: `File`, `XML-string` |
| XmlFilePath  | `string` | Path of the XML file to be signed. | `c:\temp\documentToVerify.xml` |
| Xml  | `string` | File as XML-string | `XML-string` |

#### Options

| Property  | Type  | Description |Example|
|-----------|-------|-------------|-------|
| PreserveWhitespace  | `boolean` | Preserve whitespace when loading XML? | `true` |

#### Result

| Property  | Type  | Description |Example|
|-----------|-------|-------------|-------|
| IsValid  | `boolean` | Is document valid? | `true` |

## Changelog

| Version  | Changes |
|-----------|-------|
| 0.0.5  | Initial version of tasks. |
