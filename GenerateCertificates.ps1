# Run as Administrator

Write-Host ""
Write-Host "=== Generating Root CA, Server, and Client Certificates ==="
Write-Host ""

# Create Root CA
$rootCert = New-SelfSignedCertificate `
  -Subject "CN=MyRootCA" `
  -KeyExportPolicy Exportable `
  -CertStoreLocation "Cert:\CurrentUser\My" `
  -KeyUsage CertSign `
  -KeyUsageProperty Sign `
  -HashAlgorithm sha256 `
  -KeyLength 2048 `
  -NotAfter (Get-Date).AddYears(10)

# Export Root CA to Certs Folder
$rootCAPath = ".\Certs\rootCA.cer"
Export-Certificate -Cert $rootCert -FilePath $rootCAPath
Write-Host "Root CA exported to: $rootCAPath"

# Create Server Certificate
$serverCert = New-SelfSignedCertificate `
  -Subject "CN=localhost" `
  -DnsName "localhost" `
  -KeyExportPolicy Exportable `
  -Signer $rootCert `
  -CertStoreLocation "Cert:\CurrentUser\My" `
  -HashAlgorithm sha256 `
  -TextExtension @("2.5.29.19={text}CA=false")

# Export Server Certificate
$serverPfxPath = ".\Certs\server.pfx"
Export-PfxCertificate -Cert "Cert:\CurrentUser\My\$($serverCert.Thumbprint)" `
  -FilePath $serverPfxPath `
  -Password (ConvertTo-SecureString -String "1234" -Force -AsPlainText)
Write-Host "Server certificate exported to: $serverPfxPath"

# Create Client Certificate
$clientCert = New-SelfSignedCertificate `
  -Subject "CN=client" `
  -DnsName "client" `
  -KeyExportPolicy Exportable `
  -Signer $rootCert `
  -CertStoreLocation "Cert:\CurrentUser\My" `
  -HashAlgorithm sha256 `
  -TextExtension @("2.5.29.19={text}CA=false")

# Export Client Certificate
$clientPfxPath = ".\Certs\client.pfx"
Export-PfxCertificate -Cert "Cert:\CurrentUser\My\$($clientCert.Thumbprint)" `
  -FilePath $clientPfxPath `
  -Password (ConvertTo-SecureString -String "1234" -Force -AsPlainText)
Write-Host "Client certificate exported to: $clientPfxPath"

Write-Host ""
Write-Host "=== Done! Certificates are on your Desktop ==="
Write-Host "Now double-click rootCA.cer -> Install -> Local Machine -> Trusted Root Certification Authorities"
