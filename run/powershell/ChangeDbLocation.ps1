[string] $global:configFile = "Wonga.Ops.Configuration.HostEndpoint.dll.config"
[string] $global:DbAddres
[string] $global:wwwroot = "C:\inetpub\wwwroot"
[string] $global:webconfig = "web.config"


Function StartDbChange($IpDbAddres = "")
{
    $global:IpDbAddres = $IpDbAddres
    $services = GetAllService
    ServiceDbConfiguration($services)
    RootDbConfiguration
}

Function ServiceDbConfiguration($services = "")
{
    Foreach ($service in $services)  
    {
       ChangeConfigurationOfServer $service.Name
    }
}

Function RootDbConfiguration()
{
    $configFiles = GetAllConfigFileFromRoot
    ChangeRootDbConfiguration $configFiles
}

Function ChangeRootDbConfiguration($files = "")
{
    Foreach ($file in $files) 
    {
       ChangeConfigurationOfFile $file.fullname
    }
}



#ChangeConfiguration<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
Function ChangeConfigurationOfFile($servic = "")
{
    $Path = Split-Path -parent $servic   
    $FullPath = $servic
    
    $BackUpPath = $Path + '\' + 'backUpConfig'
    $BackUpFulllPath = $BackUpPath +'\' + $global:webconfig
    
    if (CheckPathExist $BackUpPath)
    {
        Write-Host “BackUp already exist”
    }
    else
    {
        CreateBackUpFolder $Path
        CreateBackUpFile $BackUpFulllPath $FullPath $BackUpPath
    }
    
    XmlEditConnectionString $FullPath
}

Function ChangeConfigurationOfServer($servic = "")
{
    $Path = GetPathOfService $servic
    $FullPath = $Path +'\' + $global:configFile
    $BackUpPath = $Path + '\' + 'backUpConfig'
    $BackUpFulllPath = $BackUpPath +'\' + $global:configFile
    
    if (CheckPathExist $BackUpPath)
    {
        Write-Host “BackUp already exist”
    }
    else
    {
        CreateBackUpFolder $Path
        CreateBackUpFile $BackUpFulllPath $FullPath $BackUpPath
    }
    
    XmlEditConnectionString $FullPath
}
#END ChangeConfiguration>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>



#Restore_Function<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
Function RestoreChange()
{
    $services = GetAllService
    RestoreAllSetings($services)
    RestoreConfigFromRoot
}

Function RestoreAllSetings($services = "")
{
    Foreach ($service in $services) 
    {
       RestoreConfigurationOfServer $service.Name
    }
}

Function RestoreConfigFromRoot()
{
    $configFiles = Get-ChildItem -filter $global:webconfig -recurse -path $global:wwwroot
    
    Foreach ($file in $configFiles)  
    {
        $Path = Split-Path -parent $file.fullname 
        $FullPath = $file.fullname
    
        $BackUpPath = $Path + '\' + 'backUpConfig'
        $BackUpFulllPath = $BackUpPath +'\' + $global:webconfig
        
        RestoreBuckUp $BackUpFulllPath $Path
    }
}

Function RestoreConfigurationOfServer($servic = "")
{
    $Path = GetPathOfService $servic
    $BackUpPath = $Path + '\' + 'backUpConfig'
    $BackUpFulllPath = $BackUpPath +'\' + $global:configFile
    
    RestoreBuckUp $BackUpFulllPath $Path
}
#END Restore_Function>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>



#Service_function<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
Function GetAllService()
{
    $ser = Get-Service -name Wonga*
    $ser
}

Function StopAllService()
{
    Stop-Service -name Wonga* -force
}

Function StartAllService()
{
    Start-Service -name Wonga*
}

Function GetAllConfigFileFromRoot()
{
    $allFile = Get-ChildItem -filter $global:webconfig -recurse -path $global:wwwroot
    $allFile
}
#END Service_function>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>





#XML_EDIT<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
Function XmlEditConnectionString($XmlPath = "")
{
    $xmlDoc = [XML](gc $XmlPath) 
    
    $nodes = $xmlDoc.selectNodes("configuration/connectionStrings/add")
    foreach ($nod in $nodes )
    {
        $nod.connectionString = ConnectionStringChange $nod.connectionString
    }
    $xmlDoc.Save($XmlPath)
}


Function ConnectionStringChange($connectionString = "")
{
  $newConnectionString =  $connectionString  -replace "localhost", $global:IpDbAddres -replace "Integrated Security=true;", "User Id=test;Password=test;"
  $newConnectionString
}
#END XML_EDIT>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>