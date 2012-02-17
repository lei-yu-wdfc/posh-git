msbuild ../run/Wonga.QA.Tests.build /t:Build || pause
msbuild ../run/Wonga.QA.Tests.build /t:Test /p:SUT=RC /p:AUT=Wb /p:TestDependencies=  || pause
msbuild ../run/Wonga.QA.Tests.build /t:Test /p:SUT=RC /p:AUT=Za /p:TestDependencies=  || pause
msbuild ../run/Wonga.QA.Tests.build /t:Test /p:SUT=RC /p:AUT=Ca /p:TestDependencies=  || pause
msbuild ../run/Wonga.QA.Tests.build /t:Test /p:SUT=RC /p:AUT=Uk /p:TestDependencies=  || pause