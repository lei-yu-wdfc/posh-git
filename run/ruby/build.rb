require 'albacore' 
require './ruby/common'

#-- build tasks for each solution  
desc 'Builds the framework'
msbuild :framework do |msb|
  msb.solution = File.join(SRC, FRAMEWORK,  "#{FRAMEWORK}.sln")
end
  
 desc 'Builds the backend tests'
msbuild :tests do |msb|
  msb.solution = File.join(SRC, TESTS, "#{TESTS}.sln")
end
  
 desc 'Builds the service tests'
msbuild :service_tests do |msb|
  msb.solution = File.join(SRC, SERVICETESTS, "#{SERVICETESTS}.sln")
end
  
  desc 'Builds the data_tests'
msbuild :data_tests do |msb|
  msb.solution = File.join(SRC, DATATESTS, "#{DATATESTS}.sln")
end

desc 'Builds the ui_tests'
msbuild :ui_tests do |msb|
  msb.solution = File.join(SRC, UITESTS, "#{UITESTS}.sln")
end

desc 'Builds the migration_tests'
msbuild :migration_tests do |msb|
  msb.solution = File.join(SRC, MIGRATIONTESTS, "#{MIGRATIONTESTS}.sln")
end

desc 'Builds the tools'  
msbuild :tools do |msb|
  msb.solution = File.join(SRC, TOOLS, "#{TOOLS}.sln")
end

desc 'Builds Emailer'
msbuild :emailer do |msb|
  msb.solution = File.join(SRC, EMAILER,  "#{EMAILER}.sln")
end

desc 'Builds the performance tests'  
msbuild :performance_tests do |msb|
  msb.solution = File.join(SRC, PERFORMANCETESTS, "#{PERFORMANCETESTS}.sln")
end

desc 'Builds the generators'
msbuild :generators do |msb|
  msb.solution = File.join(SRC, GENERATORS, "#{GENERATORS}.sln")
end
#--

desc 'Install emailer'
task :install_emailer => [:emailer, :uninstall_emailer] do |t|
  install_emailer
end

desc 'Uninstall emailer'
task :uninstall_emailer do |t|
 
  begin
    uninstall_emailer
  rescue 
  end

end

desc 'Builds everything'
task :build => [:framework, :tests, :ui_tests, :migration_tests, :service_tests, :data_tests, :tools, :generators, :pre_generate_serializers]
#--

