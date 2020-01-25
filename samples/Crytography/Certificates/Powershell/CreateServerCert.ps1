$params = @{
  DnsName = 'LocalHost'
  FriendlyName = 'Demo_Server'
  Signer = $rootCA   #Notice the Signer is the newly created RootCA
  KeyLength = 2048
  KeyAlgorithm = 'RSA'
  HashAlgorithm = 'SHA256'
  KeyExportPolicy = 'Exportable'
  NotAfter = (Get-Date).AddYears(2)
  CertStoreLocation = 'Cert:\LocalMachine\My'
}
 
$ServerCert = New-SelfSignedCertificate @params
$ServerCert