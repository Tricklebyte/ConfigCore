$params = @{
  DnsName = 'Localhost'
  FriendlyName = 'Demo_Client'
  Signer = $rootCA #Notice the Signer is the newly created RootCA
  KeyLength = 2048
  KeyAlgorithm = 'RSA'
  HashAlgorithm = 'SHA256'
  KeyExportPolicy = 'Exportable'
  NotAfter = (Get-Date).AddYears(2)
  CertStoreLocation = 'Cert:\LocalMachine\My'
}
$ClientCert = New-SelfSignedCertificate @params
$ClientCert