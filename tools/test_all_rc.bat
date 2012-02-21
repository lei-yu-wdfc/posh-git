msbuild ../run/Wonga.QA.Tests.build /t:Build || pause
msbuild ../run/Wonga.QA.Tests.build /t:Merge || pause
msbuild ../run/Wonga.QA.Tests.build /t:Test /p:SUT=RC /p:AUT=Wb /p:TestDependencies=Config  || pause
msbuild ../run/Wonga.QA.Tests.build /t:Test /p:SUT=RC /p:AUT=Za /p:TestDependencies=Config  || pause
msbuild ../run/Wonga.QA.Tests.build /t:Test /p:SUT=RC /p:AUT=Ca /p:TestDependencies=Config  || pause
msbuild ../run/Wonga.QA.Tests.build /t:Test /p:SUT=RC /p:AUT=Uk /p:TestDependencies=Config  || pause