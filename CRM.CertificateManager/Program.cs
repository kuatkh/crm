using CertificateManager;
using CertificateManager.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace CRM.CertificateManager
{
    class Program
    {
        static CreateCertificates _cc;

        public static X509Certificate2 CreateRsaCertificate(string dnsName, int validityPeriodInDays)
        {
            var basicConstraints = new BasicConstraints
            {
                CertificateAuthority = false,
                HasPathLengthConstraint = false,
                PathLengthConstraint = 0,
                Critical = false
            };

            var subjectAlternativeName = new SubjectAlternativeName
            {
                DnsName = new List<string> { dnsName }
            };

            var x509KeyUsageFlags = X509KeyUsageFlags.DigitalSignature;

            // only if certification authentication is used
            var enhancedKeyUsages = new OidCollection
            {
                new Oid("1.3.6.1.5.5.7.3.1"),  // TLS Server auth
                new Oid("1.3.6.1.5.5.7.3.2"),  // TLS Client auth
            };

            var certificate = _cc.NewRsaSelfSignedCertificate(
                new DistinguishedName { CommonName = dnsName },
                basicConstraints,
                new ValidityPeriod
                {
                    ValidFrom = DateTimeOffset.UtcNow,
                    ValidTo = DateTimeOffset.UtcNow.AddDays(validityPeriodInDays)
                },
                subjectAlternativeName,
                enhancedKeyUsages,
                x509KeyUsageFlags,
                new RsaConfiguration { KeySize = 2048 }
            );

            return certificate;
        }

        public static X509Certificate2 CreateECDsaCertificate(string dnsName, int validityPeriodInDays)
        {
            var basicConstraints = new BasicConstraints
            {
                CertificateAuthority = false,
                HasPathLengthConstraint = false,
                PathLengthConstraint = 0,
                Critical = false
            };

            var san = new SubjectAlternativeName
            {
                DnsName = new List<string> { dnsName }
            };

            var x509KeyUsageFlags = X509KeyUsageFlags.DigitalSignature;

            // only if certification authentication is used
            var enhancedKeyUsages = new OidCollection {
                new Oid("1.3.6.1.5.5.7.3.1"),  // TLS Server auth
                new Oid("1.3.6.1.5.5.7.3.2"),  // TLS Client auth
            };

            var certificate = _cc.NewECDsaSelfSignedCertificate(
                new DistinguishedName { CommonName = dnsName },
                basicConstraints,
                new ValidityPeriod
                {
                    ValidFrom = DateTimeOffset.UtcNow,
                    ValidTo = DateTimeOffset.UtcNow.AddDays(validityPeriodInDays)
                },
                san,
                enhancedKeyUsages,
                x509KeyUsageFlags,
                new ECDsaConfiguration());

            return certificate;
        }

        static void Main(string[] args)
        {
            var sp = new ServiceCollection()
                .AddCertificateManager()
                .BuildServiceProvider();

            _cc = sp.GetService<CreateCertificates>();

            var rsaCert = CreateRsaCertificate("localhost", 30);
            var ecdsaCert = CreateECDsaCertificate("localhost", 30);

            string password = "1q@W3e$R";
            var iec = sp.GetService<ImportExportCertificate>();

            var rsaCertPfxBytes =
                iec.ExportSelfSignedCertificatePfx(password, rsaCert);
            File.WriteAllBytes("/config/crmRsaCert.pfx", rsaCertPfxBytes);

            var ecdsaCertPfxBytes =
                iec.ExportSelfSignedCertificatePfx(password, ecdsaCert);
            File.WriteAllBytes("/config/crmEcdsaCert.pfx", ecdsaCertPfxBytes);
        }
    }
}
