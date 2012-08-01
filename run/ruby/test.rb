task :meta do
  Rake::Task[:test].invoke('Tests.Meta', '','')
end