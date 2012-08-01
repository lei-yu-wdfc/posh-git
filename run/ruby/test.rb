task :meta do
  test 'Tests.Meta', '',''
end

task :core do
  test 'Tests.*', 'Tests.Meta','Category:CoreTest'
end

task :sanity_test => [:meta, :core]