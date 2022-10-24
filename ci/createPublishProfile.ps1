$currPath = Get-Location
$currPath = ("{0}{1}" -f $currPath, "\Properties\PublishProfiles\")
$currPath
# As informações abaixo devem ser obtidas em <arquivo-de-deploy>.pubSettings
# Ou em <arquivo-de-deploy>.pubXml
$ns = "http://schemas.microsoft.com/developer/msbuild/2003"
$_userName = "alphasoftware-001"

# Criando as pastas do caminho do perfil de publicação
New-Item -Path ".\" -Name "Properties" -ItemType "directory" -Force
New-Item -Path ".\Properties\" -Name "PublishProfiles" -ItemType "directory" -Force


# Criando documento XML e declaração
[xml]$Doc = New-Object System.Xml.XmlDocument
$dec = $Doc.CreateXmlDeclaration('1.0', 'UTF-8', $null)
$Doc.AppendChild($dec)

function CreateXmlElement {
    param (
        $name
    )
    $el = $Doc.CreateElement($name, $ns)
    return $el
}


# Criando nó do projeto e nó de propriedades
$Project = $Doc.CreateNode("element", "Project", $ns)
$Project.SetAttribute("ToolsVersion", "4.0")
$PropertyGroup = $Doc.CreateNode("element", "PropertyGroup", $ns)

# ---- INÍCIO DA CRIAÇÃO DE PROPRIEDADES PARA <env>.pubxml ----
$WebPublishMethod = CreateXmlElement("WebPublishMethod")
$WebPublishMethod.InnerText = "MSDeploy"
$PropertyGroup.AppendChild($WebPublishMethod)

$LastUsedBuildConfiguration = CreateXmlElement("LastUsedBuildConfiguration")
$LastUsedBuildConfiguration.InnerText = $env:buildConfiguration
$PropertyGroup.AppendChild($LastUsedBuildConfiguration)

$LastUsedPlatform = CreateXmlElement("LastUsedPlatform")
$LastUsedPlatform.InnerText = "Any CPU"
$PropertyGroup.AppendChild($LastUsedPlatform)

$SiteUrlToLaunchAfterPublish = CreateXmlElement("SiteUrlToLaunchAfterPublish")
$SiteUrlToLaunchAfterPublish.InnerText = $env:MSDeploySiteToLaunch
$PropertyGroup.AppendChild($SiteUrlToLaunchAfterPublish)

$LaunchSiteAfterPublish = CreateXmlElement("LaunchSiteAfterPublish")
$LaunchSiteAfterPublish.InnerText = "False"
$PropertyGroup.AppendChild($LaunchSiteAfterPublish)

$ExcludeApp_Data = CreateXmlElement("ExcludeApp_Data")
$ExcludeApp_Data.InnerText = "False"
$PropertyGroup.AppendChild($ExcludeApp_Data)

$ProjectGuid = CreateXmlElement("ProjectGuid")
$ProjectGuid.InnerText = $env:MSDeployProjectGuid
$PropertyGroup.AppendChild($ProjectGuid)

$MSDeployServiceURL = CreateXmlElement("MSDeployServiceURL")
$MSDeployServiceURL.InnerText = $env:MSDeployServiceUrl
$PropertyGroup.AppendChild($MSDeployServiceURL)

$DeployIisAppPath = CreateXmlElement("DeployIisAppPath")
$DeployIisAppPath.InnerText = $env:MSDeployAppPath
$PropertyGroup.AppendChild($DeployIisAppPath)

$RemoteSitePhysicalPath = CreateXmlElement("RemoteSitePhysicalPath")
$PropertyGroup.AppendChild($RemoteSitePhysicalPath)

$SkipExtraFilesOnServer = CreateXmlElement("SkipExtraFilesOnServer")
$SkipExtraFilesOnServer.InnerText = "True"
$PropertyGroup.AppendChild($SkipExtraFilesOnServer)

$MSDeployPublishMethod = CreateXmlElement("MSDeployPublishMethod")
$MSDeployPublishMethod.InnerText = "WMSVC"
$PropertyGroup.AppendChild($MSDeployPublishMethod)

$EnableMSDeployBackup = CreateXmlElement("EnableMSDeployBackup")
$EnableMSDeployBackup.InnerText = "True"
$PropertyGroup.AppendChild($EnableMSDeployBackup)

$UserName = CreateXmlElement("UserName")
$UserName.InnerText = $_userName
$PropertyGroup.AppendChild($UserName)

$SavePWD = CreateXmlElement("_SavePWD")
$SavePWD.InnerText = "False"
$PropertyGroup.AppendChild($SavePWD)
# ---- FIM DA CRIAÇÃO DE PROPRIEDADES PARA <env>.pubxml ----


# Inserindo nó de propriedades no nó do projeto,
# nó de projeto no nó de documento,
# e salvando <env>.pubxml
$Project.AppendChild($PropertyGroup)
$Doc.AppendChild($Project)
$Doc.Save(("{0}{1}.pubxml" -f $currPath, $env:buildConfiguration))

# Limpando variáveis para criar <env>.pubxml.user
$Project.RemoveChild($PropertyGroup)
$PropertyGroup = $Doc.CreateNode('element', 'PropertyGroup', $ns)

# ---- INÍCIO DA CRIAÇÃO DE PROPRIEDADES PARA <env>.pubxml.user ----
$TimeStampOfAssociatedLegacyPublishXmlFile = CreateXmlElement("TimeStampOfAssociatedLegacyPublishXmlFile")
$PropertyGroup.AppendChild($TimeStampOfAssociatedLegacyPublishXmlFile)

$EncryptedPassword = CreateXmlElement("EncryptedPassword")
$PropertyGroup.AppendChild($EncryptedPassword)
# ---- FIM DA CRIAÇÃO DE PROPRIEDADES PARA <env>.pubxml.user ----

# Inserindo nó de propriedades no nó do projeto,
# nó de projeto no nó de documento,
# e salvando <env>.pubxml.user
$Project.AppendChild($PropertyGroup)
$Doc.AppendChild($Project)
$Doc.Save(("{0}{1}.pubxml.user" -f $currPath, $env:buildConfiguration))