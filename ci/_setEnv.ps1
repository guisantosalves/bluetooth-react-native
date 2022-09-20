# NOTA: Este arquivo é somente para facilitar o teste dos scripts. Ele não é utilizado em Staging/Produção.

$env:buildConfiguration = "Staging"
$env:workDir = "web.documentos"

$env:MSDeployAppPath = "msdeploy-app-path"
$env:MSDeployProjectGuid = "791c9798-524a-4899-a938-92da0ba97c3a"
$env:MSDeployServiceUrl = "https://site-de-deploy.com.br:9999/msdeploy.axd?site=ms-deploy-app-path"
$env:MSDeploySiteToLaunch = "http://www.site-de-deploy.com.br/"

$env:createApiUrl = "0"
$env:createConnString = "0"
$env:createToken = "0"

$env:ConnectionString = "Server=(LocalDB)\\MSSQLLocalDB;Initial Catalog=NOME_DO_BANCO_DE_DADOS;Integrated Security=True;"
$env:TokenAudience = "Alpha.Web.Dev"
$env:TokenIssuer = "Alpha.Api.Dev"
$env:TokenSigningKey = "EztcVaCWaBpF7GeVshBe6Y6fNhMtPQ" # Só exemplo. Não é utilizado em Staging/Produção. Nice try :)

$env:ApiUrl = "https://dev-webapi.alphasoftware.com.br"