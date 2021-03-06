[Reflection.Assembly]::LoadWithPartialName("System.Messaging")
[string] $global:MsmqTestName = "servicetest"
[string] $global:configFileName = "Wonga.Ops.Configuration.HostEndpoint.dll.config"


[string] $global:DbAddres
[string] $global:configFile = "Wonga.Ops.Configuration.HostEndpoint.dll.config"
[string] $global:wwwroot = "C:\inetpub\wwwroot"
[string] $global:webconfig = "web.config"



Function configservice ([string]$ServiceName = "")
{
    $servName = "Wonga.$ServiceName.Handlers"
    $serviceLocation = GetPathOfService $servName
    $backUpLocation = $serviceLocation + '\backUpConfig'
    $backUpConfigLocation = $backUpLocation + '\' + $global:configFileName
    $orginalConfigLocation = $serviceLocation + '\' + $global:configFileName
    
    if (CheckPathExist $backUpLocation)
    {
        Write-Host “folder already exist”
    }
    else
    {
        CreateBackUpFolder $serviceLocation
    }
    
    CheckMsmq $global:MsmqTestName
        
    CreateBackUpFile $backUpConfigLocation $orginalConfigLocation $backUpLocation
    XmlEdit $orginalConfigLocation $ServiceName
    
    restart-service $servName $backUpConfigLocation $serviceLocation
}

Function configservice_undo ([string]$ServiceName = "")
{
    $servName = "Wonga.$ServiceName.Handlers"
    $serviceLocation = GetPathOfService $servName
    $backUpLocation = $serviceLocation + '\backUpConfig'
    $backUpConfigLocation = $backUpLocation + '\' + $global:configFileName
    $orginalConfigLocation = $serviceLocation + '\' + $global:configFileName
    
    RestoreBuckUp $backUpConfigLocation $serviceLocation
    restart-service $servName $backUpConfigLocation $serviceLocation
}


Function restart-service([string]$srvName = "",[string]$backUpConfigLocation = "",[string]$serviceLocation = "")
{
    stop-Service $srvName
    $service = get-service $srvName

    if($service.status -ne “Stopped”)
    {
        write($srvName + ” has failed to stop, please manually start.”)
        RestoreBuckUp $backUpConfigLocation $serviceLocation
    }

    start-Service $srvname
    $service = get-service $srvName

    if($service.status -ne “Running”)
    {
        write($srvName + ” has failed to start, please manually start.”)
        RestoreBuckUp $backUpConfigLocation $serviceLocation
    }
    else
    {
        write($srvName + ” has started.”)
    }
}



Function GetPathOfService ([string]$ServiceName = "")
{
    $partpath = (Get-WmiObject -query "SELECT PathName FROM Win32_Service WHERE Name = '$ServiceName'").PathName
    
    $path = $partpath.split('"')
    $path1 = (Split-Path $path[1])
    $path1
}



#BackUp<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
Function CreateBackUpFolder([string]$PathToService ="")
{
    $pathBackUpDirect = $PathToService + '\backUpConfig'
    New-Item -Path $pathBackUpDirect -Type Directory
}

Function CreateBuckUp([string]$FileLocation="",[string]$destination="")
{
    Copy-Item $FileLocation $destination
}

Function RestoreBuckUp([string]$FileLocation="",[string]$destination="")
{
    if (CheckPathExist $FileLocation)
    {
        Copy-Item $FileLocation $destination
        Remove-Item $FileLocation 
        $Directory = Split-Path -parent $FileLocation
        Remove-Item -Force $Directory
    }
}

Function CreateBackUpFile([string]$backUpConfigLocation = "", [string]$orginalConfigLocation= "", [string]$backUpLocation= "")
{   
    if(CheckPathExist $backUpConfigLocation)
    {
        Write-Host "Config file restore $backUpConfigLocation"
    }
    else
    {
        CreateBuckUp $orginalConfigLocation $backUpLocation
    }
}

Function CheckPathExist([string]$PathToCheck ="")
{
    #Write-Host “location to check $PathToService”
    if ((Test-Path -path $PathToCheck) -eq $True)
    {
        $true
    }   
    else 
    {
        $false
    }
}
#END BackUp>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>




#XML_EDIT<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
Function XmlEdit([string]$XmlPath = "", [string]$serviceName= "")
{
    $xmlDoc = [XML](gc $XmlPath) 
    $orginalServiceName = $serviceName + "service@localhost"
    
    $xmlDoc.selectNodes("configuration/UnicastBusConfig/MessageEndpointMappings/add") | 
    foreach {
        if( $_.Endpoint -ne $orginalServiceName)
        {
            $_.Endpoint = $global:MsmqTestName + "@localhost"
        }
    }
    $xmlDoc.Save($XmlPath)
}
#END XML_EDIT>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>





#MSMQ<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
Function CheckMsmq ([string]$MsmqName = "")
{
    if(checkMsmqExist  $global:MsmqTestName){
        #write-host "Msmq exist"
    }
    else
    {
        createMsmq $global:MsmqTestName
    }
}

Function checkMsmqExist([string]$MsmqName = "")
{
    $msmqFullname = getMsmqFullName $MsmqName
   
    if ([System.Messaging.MessageQueue]::Exists($msmqFullname))
    {
        $true
    }
    else
    {
        $false
    }
}

Function createMsmq([string]$MsmqName = "")
{
    $msmqFullname = getMsmqFullName $MsmqName
    [System.Messaging.MessageQueue]::Create($msmqFullname,$true)
}


Function getMsmqFullName([string]$MsmqName = "")
{
    $msmqFullname = ".\private$\" + $MsmqName
    $msmqFullname
}
#END MSMQ>>>>,>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>



#Change DB locatins<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
Function StartDbChange($IpDbAddres = "")
{
    $global:IpDbAddres = $IpDbAddres
    $services = GetAllService
	StopAllService
    ServiceDbConfiguration($services)
    RootDbConfiguration
	StartAllService
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
#END Change DB locatins>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

#ChangeConfigurationFile<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
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
#END ChangeConfigurationFile>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>



#Restore_Function<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
Function RestoreChange()
{
    $services = GetAllService
	StopAllService
    RestoreAllSetings($services)
    RestoreConfigFromRoot
	StartAllService
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