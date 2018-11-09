using System.ComponentModel;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Frends.Community.XmlSignature
{
    public static class VerifyTask
    {
        public static VerifySignatureResult VerifySignedXml([PropertyTab] VerifySignatureInput input, [PropertyTab] VerifySignatureOptions options)
        {
            var result = new VerifySignatureResult();
            var xmldoc = new XmlDocument() { PreserveWhitespace = options.PreserveWhitespace };
            StreamReader xmlStream = null;

            if (input.XmlInputType == XmlParamType.File)
            {
                xmlStream = new StreamReader(input.XmlFilePath);
                xmldoc.Load(xmlStream);
            }
            else
            {
                xmldoc.LoadXml(input.Xml);
            }

            // load the signature node
            var signedXml = new SignedXml(xmldoc);
            signedXml.LoadXml((XmlElement)xmldoc.GetElementsByTagName("Signature")[0]);

            X509Certificate2 certificate = null;
            foreach (KeyInfoClause clause in signedXml.KeyInfo)
            {
                if (clause is KeyInfoX509Data)
                {
                    if (((KeyInfoX509Data)clause).Certificates.Count > 0)
                    {
                        certificate = (X509Certificate2)((KeyInfoX509Data)clause).Certificates[0];
                    }
                }
            }

            // Check the signature and return the result.
            result.IsValid = signedXml.CheckSignature(certificate, true);

            // close stream if input was a file
            if (input.XmlInputType == XmlParamType.File)
            {
                xmlStream.Dispose();
            }

            return result;
        }
    }
}
