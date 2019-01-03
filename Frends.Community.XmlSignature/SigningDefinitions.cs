using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Frends.Community.XmlSignature
{
    /// <summary>
    /// Signing input
    /// </summary>
    public class SignXmlInput
    {
        public XmlParamType XmlInputType { get; set; }

        /// <summary>
        /// Path to xml document to sign
        /// </summary>
        [DefaultValue("c:\\temp\\document.xml")]
        [DisplayFormat(DataFormatString = "Text")]
        [UIHint(nameof(XmlInputType), "", XmlParamType.File)]
        public string XmlFilePath { get; set; }

        /// <summary>
        /// Xml to sign
        /// </summary>
        [DefaultValue("<root><value>123</value></root>")]
        [DisplayFormat(DataFormatString = "Xml")]
        [UIHint(nameof(XmlInputType), "", XmlParamType.XmlString)]
        public string Xml { get; set; }

        /// <summary>
        /// Xml signing technique to use
        /// </summary>
        public XmlEnvelopingType XmlEnvelopingType { get; set; }

        /// <summary>
        /// How to sign the document
        /// </summary>
        public SigningStrategyType SigningStrategy { get; set; }

        /// <summary>
        /// Path to certificate with private key
        /// </summary>
        [DefaultValue("c:\\certificates\\signingcertificate.pfx")]
        [DisplayFormat(DataFormatString = "Text")]
        [UIHint(nameof(SigningStrategy), "", SigningStrategyType.PrivateKeyCertificate)]
        public string CertificatePath { get; set; }

        /// <summary>
        /// Private key password
        /// </summary>
        [PasswordPropertyText]
        [UIHint(nameof(SigningStrategy), "", SigningStrategyType.PrivateKeyCertificate)]
        public string PrivateKeyPassword { get; set; }
    }

    public class SignXmlOutput
    {
        /// <summary>
        /// Output to file or xml string?
        /// </summary>
        public XmlParamType OutputType { get; set; }

        /// <summary>
        /// Output file path
        /// </summary>
        [DefaultValue("c:\\temp\\signedOutput.xml")]
        [DisplayFormat(DataFormatString = "Text")]
        [UIHint(nameof(OutputType), "", XmlParamType.File)]
        public string OutputFilePath { get; set; }

        /// <summary>
        /// Output file encoding
        /// </summary>
        [DefaultValue("UTF-8")]
        [DisplayFormat(DataFormatString = "Text")]
        [UIHint(nameof(OutputType), "", XmlParamType.File)]
        public string OutputEncoding { get; set; }

        /// <summary>
        /// If source is file, then you can add signature to it
        /// </summary>
        [UIHint(nameof(OutputType), "", XmlParamType.File)]
        public bool AddSignatureToSourceFile { get; set; }
    }

    /// <summary>
    /// Signing options
    /// </summary>
    public class SignXmlOptions
    {
        public bool IncludeComments { get; set; }

        /// <summary>
        /// Should whitespace be preserved when loading xml?
        /// </summary>
        public bool PreserveWhitespace { get; set; }

        /// <summary>
        /// Which signature method to use with signing
        /// </summary>
        public XmlSignatureMethod XmlSignatureMethod { get; set; }

        /// <summary>
        /// Which digest method to use
        /// </summary>
        public DigestMethod DigestMethod { get; set; }

        /// <summary>
        /// Which transform methods to use
        /// </summary>
        public TransformMethod[] TransformMethods { get; set; }
    }

    public class SigningResult
    {
        /// <summary>
        /// If output type is File, then this will indicated filePath, otherwise will contain signed xml as string
        /// </summary>
        public string Result { get; set; }
    }

    /// <summary>
    /// Can be either a file or xml string
    /// </summary>
    public enum XmlParamType
    {
        File,
        XmlString
    }

    public enum SigningStrategyType
    {
        PrivateKeyCertificate
    }

    public enum XmlEnvelopingType
    {
        XmlEnvelopedSignature
        //XmlEnvelopingSignature // not supported
        //XmlDetachedSignature // not supported
    }

    /// <summary>
    /// Signature method for XMLDSIG
    /// </summary>
    public enum XmlSignatureMethod
    {
        RSASHA1,
        RSASHA256,
        RSASHA384,
        RSASHA512
    }

    /// <summary>
    /// Transform method to add
    /// </summary>
    public enum TransformMethod
    {
        DsigC14,
        DsigC14WithComments,
        DsigExcC14,
        DsigExcC14WithComments,
        DsigBase64
    }

    /// <summary>
    /// Digest method
    /// </summary>
    public enum DigestMethod
    {
        SHA1,
        SHA256,
        SHA384,
        SHA512
    }
}
