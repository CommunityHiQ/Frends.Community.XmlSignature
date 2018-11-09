using System;
using System.Collections.Generic;
using System.IO;

using NUnit.Framework;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Frends.Community.XmlSignature.Tests
{
    [TestFixture]
    public class SigningTaskTest
    {
        private readonly string _certificatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestItems", "certwithpk.pfx");
        private readonly string _privateKeyPassword = "password";

        [Test]
        public void SignXml_ShouldSignXmlStringWithPrivateKeyCertificate()
        {
            var input = new SignXmlInput
            {
                CertificatePath = _certificatePath,
                PrivateKeyPassword = _privateKeyPassword,
                SigningStrategy = SigningStrategyType.PrivateKeyCertificate,
                XmlInputType = XmlParamType.XmlString,
                XmlEnvelopingType = XmlEnvelopingType.XmlEnvelopedSignature,
                Xml = "<root><value>foo</value></root>"
            };
            var options = new SignXmlOptions
            {
                DigestMethod = DigestMethod.SHA256,
                OutputType = XmlParamType.XmlString,
                TransformMethods = new [] { TransformMethod.DsigExcC14 },
                XmlSignatureMethod = XmlSignatureMethod.RSASHA256
            };

            SigningResult result = SigningTask.SignXml(input, options);

            StringAssert.Contains("<Signature", result.Result);
        }

        [Test]
        public void SignXml_ShouldSignXmlFileWithPrivateKeyCertificate()
        {
            // create file
            string xmlFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestItems", Guid.NewGuid().ToString() + ".xml");
            File.WriteAllText(xmlFilePath, @"<root>
    <value>foo</value>
</root>");
            var input = new SignXmlInput
            {
                CertificatePath = _certificatePath,
                PrivateKeyPassword = _privateKeyPassword,
                SigningStrategy = SigningStrategyType.PrivateKeyCertificate,
                XmlEnvelopingType = XmlEnvelopingType.XmlEnvelopedSignature,
                XmlInputType = XmlParamType.File,
                XmlFilePath = xmlFilePath
            };
            var options = new SignXmlOptions
            {
                DigestMethod = DigestMethod.SHA256,
                TransformMethods = new [] { TransformMethod.DsigExcC14 },
                XmlSignatureMethod = XmlSignatureMethod.RSASHA256,
                OutputType = XmlParamType.File,
                OutputFilePath = xmlFilePath.Replace(".xml", "_signed.xml"),
                OutputEncoding = "utf-8",
                PreserveWhitespace = true
            };

            SigningResult result = SigningTask.SignXml(input, options);
            var signedXml = File.ReadAllText(result.Result);

            StringAssert.Contains("<Signature", signedXml);

            // cleanup
            File.Delete(xmlFilePath);
            File.Delete(result.Result);
        }
    }
}
