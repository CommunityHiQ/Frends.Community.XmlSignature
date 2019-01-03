using System;
using System.Collections.Generic;
using System.IO;

using NUnit.Framework;

namespace Frends.Community.XmlSignature.Tests
{
    [TestFixture]
    public class VerifyTaskTest
    {
        private readonly string _certificatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestItems", "certwithpk.pfx");
        private readonly string _privateKeyPassword = "password";

        [Test]
        public void VerifySignedXml_ShouldVerifySignedXmlString()
        {
            var input = new SignXmlInput
            {
                CertificatePath = _certificatePath,
                PrivateKeyPassword = _privateKeyPassword,
                SigningStrategy = SigningStrategyType.PrivateKeyCertificate,
                XmlInputType = XmlParamType.XmlString,
                XmlEnvelopingType = XmlEnvelopingType.XmlEnvelopedSignature,
                Xml = "<root><foo>bar</foo></root>"
            };
            var output = new SignXmlOutput
            {
                OutputType = XmlParamType.XmlString
            };
            var options = new SignXmlOptions
            {
                DigestMethod = DigestMethod.SHA256,
                TransformMethods = new [] { TransformMethod.DsigExcC14 },
                XmlSignatureMethod = XmlSignatureMethod.RSASHA256
            };
            string signedXml = SigningTask.SignXml(input, output, options).Result;
            var verifyInput = new VerifySignatureInput
            {
                XmlInputType = XmlParamType.XmlString,
                Xml = signedXml
            };
            
            var result = VerifyTask.VerifySignedXml(verifyInput, new VerifySignatureOptions());

            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void VerifySignedXml_ShouldVerifySignedXmlDocument()
        {
            var input = new VerifySignatureInput
            {
                XmlInputType = XmlParamType.File,
                XmlFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestItems", "signed.xml")
            };
            var options = new VerifySignatureOptions
            {
                PreserveWhitespace = true
            };

            var result = VerifyTask.VerifySignedXml(input, options);

            Assert.IsTrue(result.IsValid);
        }
    }
}
