task :meta do [:config]
  test 'Tests.Meta', '',''
end

task :core do [:config]
  test 'Tests.*', 'Tests.Meta','Category:CoreTest'
end

task :sanity_test => [:meta, :core]