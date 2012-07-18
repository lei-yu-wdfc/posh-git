[Reflection.Assembly]::LoadWithPartialName("System.Messaging")
[string] $global:MsmqTestName = "servicetest"
[string] $global:configFileName = "Wonga.Ops.Configuration.HostEndpoint.dll.config"


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
    
    CheckMsmq
        
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
Function CheckMsmq
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
    $msmqFullname = getMsmqFullName + $MsmqName
    [System.Messaging.MessageQueue]::Create($msmqFullname)
}


Function getMsmqFullName([string]$MsmqName = "")
{
    $msmqFullname = ".\private$\" + $MsmqName
    $msmqFullname
}
#END MSMQ>>>>,>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>