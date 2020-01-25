#Create the CA
$rootCAparams = @{
  DnsName = 'LocalHost Root Cert'
  FriendlyName = 'Demo_Root'
  KeyLength = 2048
  KeyAlgorithm = 'RSA'
  HashAlgorithm = 'SHA256'
  KeyExportPolicy = 'Exportable'
  NotAfter = (Get-Date).AddYears(5)
  CertStoreLocation = 'Cert:\LocalMachine\My'
  KeyUsage = 'CertSign','CRLSign' #fixes invalid certificate error
}
$rootCA = New-SelfSignedCertificate @rootCAparams
$rootCA

#Add/Import the new Root CA to the Root Certificate Store
$CertStore = New-Object -TypeName `
  System.Security.Cryptography.X509Certificates.X509Store(
  [System.Security.Cryptography.X509Certificates.StoreName]::Root,
  'LocalMachine')
$CertStore.open('MaxAllowed')
$CertStore.add($rootCA)
$CertStore.close()