desc 'Runs the meta tests'
task :meta_test => [:config] do 
  test 'Tests.Meta', '',''
end

desc 'Runs the backend core tests'
task :core_test => [:config] do 
  test 'Tests.*', 'Tests.Meta','Category:CoreTest'
end

desc 'Runs the backend tests'
task :backend_test => [:config, :meta_test] do 
  test 'Tests.*', 'Tests.Meta'
end

desc 'Runs core system tests'
task :full_system_core_test => [:config] do
  test 'UiTests.Web', '','Category:CoreTest'
end

desc 'Runs all full system tests'
task :full_system_test => [:config] do
  test 'UiTests.Web:UiTests.Mobile', '',''
end

desc 'Runs Meta + Core backend tests'
task :sanity_test => [:config, :meta_test, :core_test]