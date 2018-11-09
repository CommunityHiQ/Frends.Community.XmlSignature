using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.Community.XmlSignature
{
    /// <summary>
    /// Input for verifying signature
    /// </summary>
    public class VerifySignatureInput
    {
        /// <summary>
        /// Either xml string or file path
        /// </summary>
        public XmlParamType XmlInputType { get; set; }

        /// <summary>
        /// Path to the xml document
        /// </summary>
        [DefaultValue("c:\\temp\\documentToVerify.xml")]
        [DisplayFormat(DataFormatString = "Text")]
        [UIHint(nameof(XmlInputType), "", XmlParamType.File)]
        public string XmlFilePath { get; set; }

        /// <summary>
        /// Xml in string format
        /// </summary>
        [DisplayFormat(DataFormatString = "Xml")]
        [UIHint(nameof(XmlInputType), "", XmlParamType.XmlString)]
        public string Xml { get; set; }
    }

    /// <summary>
    /// Options for verifying signatures
    /// </summary>
    public class VerifySignatureOptions
    {
        /// <summary>
        /// Should whitespace be preserved when loading xml?
        /// </summary>
        public bool PreserveWhitespace { get; set; }
    }

    /// <summary>
    /// Verification result
    /// </summary>
    public class VerifySignatureResult
    {
        /// <summary>
        /// True if valid, otherwise false
        /// </summary>
        public bool IsValid { get; set; }
    }
}
