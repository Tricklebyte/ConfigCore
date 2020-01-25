$params = @{
  DnsName = 'NoHost'
  FriendlyName = 'No_Trust_Cert'
  KeyLength = 2048
  KeyAlgorithm = 'RSA'
  HashAlgorithm = 'SHA256'
  KeyExportPolicy = 'Exportable'
  NotAfter = (Get-Date)
  CertStoreLocation = 'Cert:\LocalMachine\My'
}
 
$ServerCert = New-SelfSignedCertificate @params
$ServerCert