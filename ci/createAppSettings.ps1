param(
    $createConnString = 0,
    $createToken = 0,
    $createApiUrl = 0
)

# Objeto principal - appSettings
$fileFullPath = ''
$appSettings = New-Object -TypeName psobject

# Criando ConnectionStrings - !!!! NÃO ESQUECER DE SETAR A VARIÁVEL NO PIPELINE !!!!
if ($createConnString -eq 1) {
    $connStrings = New-Object -TypeName psobject
    $connStrings | Add-Member -MemberType NoteProperty -Name 'Default' -Value $env:CONNSTRING
    
    $appSettings | Add-Member -MemberType NoteProperty -Name 'ConnectionStrings' -Value $connStrings
}

# Criando token - !!!! NÃO ESQUECER DE SETAR A VARIÁVEL NO PIPELINE !!!!
if ($createToken -eq 1) {
    $token = New-Object -TypeName psobject
    $token | Add-Member -MemberType NoteProperty -Name 'Issuer' -Value $env:TokenIssuer
    $token | Add-Member -MemberType NoteProperty -Name 'Audience' -Value $env:TokenAudience
    $token | Add-Member -MemberType NoteProperty -Name 'SigningKey' -Value $env:TOKENSIGNINGKEY
    
    $appSettings | Add-Member -MemberType NoteProperty -Name 'Token' -Value $token
}

# Criando ApiUrl - !!!! NÃO ESQUECER DE SETAR A VARIÁVEL NO PIPELINE !!!!
if ($createApiUrl -eq 1) {
    $appSettings | Add-Member -MemberType NoteProperty -Name 'ApiUrl' -Value $env:ApiUrl
}


$logLevel = New-Object -TypeName psobject
$logLevel | Add-Member -MemberType NoteProperty -Name 'Default' -Value 'Warning'


$logging = New-Object -TypeName psobject
$logging | Add-Member -MemberType NoteProperty -Name 'LogLevel' -Value $logLevel


$appSettings | Add-Member -MemberType NoteProperty -Name 'Logging' -Value $logging
#$appSettings | Add-Member -MemberType NoteProperty -Name 'AllowedHosts' -Value '*'

$appSettings | ConvertTo-Json -Depth 20 -Compress

# Gera o caminho completo do arquivo, dependendo da Configuration recebida
if ($env:buildConfiguration -eq 'Release') {
    # $fileFullPath = ('{0}appSettings.json' -f $env:ProjectFolder)
    $fileFullPath = 'appSettings.json'
}
else {
    # $fileFullPath = ('{0}appSettings.{1}.json' -f $env:ProjectFolder, $env:buildConfiguration)
    $fileFullPath = ('appSettings.{0}.json' -f $env:buildConfiguration)
}

ConvertTo-Json -InputObject $appSettings -Depth 20 | Out-File -FilePath $fileFullPath