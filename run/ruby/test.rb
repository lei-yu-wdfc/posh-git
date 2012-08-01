task :meta_test => [:config] do 
  test 'Tests.Meta', '',''
end

task :core_test => [:config] do 
  test 'Tests.*', 'Tests.Meta','Category:CoreTest'
end

task :backend_test => [:config, :meta_test] do 
  test 'Tests.*', 'Tests.Meta'
end

task :sanity_test => [:config, :meta_test, :core_test]