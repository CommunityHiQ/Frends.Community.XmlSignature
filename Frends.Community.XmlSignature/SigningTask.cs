using System.ComponentModel;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Frends.Community.XmlSignature
{
    public class SigningTask
    {
        /// <summary>
        /// Signs an xml document
        /// </summary>
        /// <param name="input"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static SigningResult SignXml([PropertyTab] SignXmlInput input, [PropertyTab] SignXmlOptions options)
        {
            var result = new SigningResult();
            var xmldoc = new XmlDocument() { PreserveWhitespace = options.PreserveWhitespace };
            StreamReader xmlStream = null;

            if(input.XmlInputType == XmlParamType.File)
            {
                xmlStream = new StreamReader(input.XmlFilePath);
                xmldoc.Load(xmlStream);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(input.Xml))
                    throw new System.ArgumentException("Invalid input xml");
                xmldoc.LoadXml(input.Xml);
            }

            var signedXml = new SignedXml(xmldoc);
            
            // determine signature method
            switch (options.XmlSignatureMethod)
            {
                case XmlSignatureMethod.RSASHA1:
                    signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA1Url;
                    break;
                case XmlSignatureMethod.RSASHA256:
                    signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA256Url;
                    break;
                case XmlSignatureMethod.RSASHA384:
                    signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA384Url;
                    break;
                case XmlSignatureMethod.RSASHA512:
                    signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA512Url;
                    break;
            }

            // determine how to sign
            switch (input.SigningStrategy)
            {
                case SigningStrategyType.PrivateKeyCertificate:
                    var cert = new X509Certificate2(input.CertificatePath, input.PrivateKeyPassword);
                    signedXml.SigningKey = cert.GetRSAPrivateKey();

                    // public key certificate is submitted with the xml document
                    var keyInfo = new KeyInfo();
                    keyInfo.AddClause(new KeyInfoX509Data(cert));
                    signedXml.KeyInfo = keyInfo;
                    break;
            }

            var reference = new Reference();
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform(options.IncludeComments));

            // add different transforms
            foreach (var transform in options.TransformMethods)
            {
                switch (transform)
                {
                    case TransformMethod.DsigBase64:
                        reference.AddTransform(new XmlDsigBase64Transform());
                        break;
                    case TransformMethod.DsigC14:
                        reference.AddTransform(new XmlDsigC14NTransform());
                        break;
                    case TransformMethod.DsigC14WithComments:
                        reference.AddTransform(new XmlDsigC14NWithCommentsTransform());
                        break;
                    case TransformMethod.DsigExcC14:
                        reference.AddTransform(new XmlDsigExcC14NTransform());
                        break;
                    case TransformMethod.DsigExcC14WithComments:
                        reference.AddTransform(new XmlDsigExcC14NWithCommentsTransform());
                        break;
                }
            }

            // target the whole xml document
            reference.Uri = "";

            // add digest method
            switch (options.DigestMethod)
            {
                case DigestMethod.SHA1:
                    reference.DigestMethod = SignedXml.XmlDsigSHA1Url;
                    break;
                case DigestMethod.SHA256:
                    reference.DigestMethod = SignedXml.XmlDsigSHA256Url;
                    break;
                case DigestMethod.SHA384:
                    reference.DigestMethod = SignedXml.XmlDsigSHA384Url;
                    break;
                case DigestMethod.SHA512:
                    reference.DigestMethod = SignedXml.XmlDsigSHA512Url;
                    break;
            }

            // add references to signed xml
            signedXml.AddReference(reference);

            // compute the signature
            signedXml.ComputeSignature();

            // as this is Xml Enveloped Signature,
            // add the signature element to the original xml as the last child of the root element
            xmldoc.DocumentElement.AppendChild(xmldoc.ImportNode(signedXml.GetXml(), true));

            // output results either to a file or result object
            if(options.OutputType == XmlParamType.File)
            {
                // signed xml document is written in target destination
                using (var writer = new XmlTextWriter(options.OutputFilePath, Encoding.GetEncoding(options.OutputEncoding)))
                {
                    xmldoc.WriteTo(writer);
                }

                // and result will indicate the document path
                result.Result = options.OutputFilePath;
            }
            else
            {
                // signed xml document is returned from task
                result.Result = xmldoc.OuterXml;
            }

            // close stream if input was a file
            if(input.XmlInputType == XmlParamType.File)
            {
                xmlStream.Dispose();
            }

            return result;
        }
    }
}
